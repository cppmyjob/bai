using System;
using System.Collections.Generic;
using System.Linq;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Genetic;
using Bai.Intelligence.Interfaces;
using Bai.Intelligence.Organism;
using Bai.Intelligence.Organism.Definition;
using Bai.Intelligence.Organism.Definition.Dna;
using Bai.Intelligence.Organism.Definition.Dna.Genes;
using Bai.Intelligence.Organism.Functions;
using Bai.Intelligence.Utils.Random;

namespace Bai.Intelligence.Cpu
{
    public class CpuBuilder : IBuilder
    {
        private class CyclePreviousCycleData
        {
            public int[] OutputIndexes;
            public int NeuronIndex;
        }

        public IRuntime Build(NetworkDefinition definition)
        {
            using var random = RandomFactory.Instance.Create();

            var runtime = new CpuRuntime(definition.InputCount, definition.OutputCount);

            var buildRuntimeContext = new BuildRuntimeContext
                                      {
                                          RuntimeCycles = runtime.Cycles,
                                          TempMemoryIndex = definition.InputCount
                                      };
            AddCycles(definition, buildRuntimeContext, random);

            runtime.TempMemory = new float[buildRuntimeContext.TempMemoryIndex];

            return runtime;
        }

        private void AddCycles(NetworkDefinition definition, BuildRuntimeContext buildRuntimeContext,
            IRandom random)
        {
            var phenotypeCreator = new PhenotypeCreator(random);

            var phenotype = phenotypeCreator.Execute(definition);

            var neurons = CreateNeurons(phenotype);
            
            // add space to neuron outputs
            buildRuntimeContext.TempMemoryIndex += neurons.Sum(t => t.Outputs.Length);

            ConfigureCycles(definition.InputCount, neurons, buildRuntimeContext);
        }

        private List<Neuron> CreateNeurons(List<NeuronDna> phenotype)
        {
            var neurons = new List<Neuron>();
            var context = new BuilderContext();
            var neuronIndex = 0;
            foreach (var chromosome in phenotype)
            {
                foreach (var gene in chromosome.Genes)
                {
                    if (gene is CreateNeuronGene)
                    {
                        context.Neuron = new Neuron
                                         {
                                             Index = neuronIndex++
                                         };
                        neurons.Add(context.Neuron);
                        continue;
                    }
                    gene.Build(context);
                }
            }
            return neurons;
        }

        private void ConfigureCycles(int inputCount, List<Neuron> neurons,
            BuildRuntimeContext buildRuntimeContext)
        {
            var neuronDict = neurons.ToDictionary(t => t.Index, t => t);
            var inputMap = CreateInputMap(neurons);
            var source = Enumerable.Range(0, inputCount).ToList();

            do
            {
                var inputs = GetMapItemsForCycle(inputMap, source);
                var multiCycle = AddMultiCycle(buildRuntimeContext, inputs);

                var inputFunctionData = SeparateByFunctionType(multiCycle, neuronDict);

                source = new List<int>(inputFunctionData.ManyToMany.Count + inputFunctionData.OneToOne.Count);

                if (inputFunctionData.OneToOne.Count > 0)
                {
                    var sumCycle = AddSumCycle(buildRuntimeContext, inputFunctionData.OneToOne);
                    AddFunctionOneToOneCycle(buildRuntimeContext, source, neuronDict, sumCycle);
                }

                if (inputFunctionData.ManyToMany.Count > 0)
                {
                    AddFunctionManyToManyCycle(buildRuntimeContext, source, neuronDict, inputFunctionData.ManyToMany);
                }

                foreach (var input in inputs.SelectMany(t => t.Inputs)) 
                    input.Processed = true;
                ClearMap(inputMap);

            } while (inputMap.Keys.Count > 0);
        }

        private (List<CyclePreviousCycleData> OneToOne, List<CyclePreviousCycleData> ManyToMany) SeparateByFunctionType(
            MultiCycle multiCycle, Dictionary<int, Neuron> neuronDict)
        {
            var oneToOne = new List<CyclePreviousCycleData>();
            var manyToMany = new List<CyclePreviousCycleData>();
            var groups = multiCycle.Items.GroupBy(t => t.NeuronIndex, item => item);
            foreach (var group in groups)
            {
                var indexes = group.Select(t => t.OutputIndex).ToArray();
                var data = new CyclePreviousCycleData
                           {
                               NeuronIndex = group.Key,
                               OutputIndexes = indexes
                           };
                var function = neuronDict[group.Key].Function;
                if (function is INeuronFunctionManyToMany)
                {
                    manyToMany.Add(data);
                }
                else
                {
                    oneToOne.Add(data);
                }
            }
            return (oneToOne, manyToMany);
        }

        private static Dictionary<int, MapItem> CreateInputMap(List<Neuron> neurons)
        {
            var map = new Dictionary<int, MapItem>();
            foreach (var neuron in neurons)
            foreach (var input in neuron.Inputs)
            {
                if (!map.TryGetValue(input.SourceIndex, out var mapItem))
                {
                    mapItem = new MapItem();
                    map.Add(input.SourceIndex, mapItem);
                }

                mapItem.Inputs.Add(new MapInputItem
                                   {
                                       NeuronIndex = neuron.Index,
                                       SourceIndex = input.SourceIndex,
                                       Weight = input.Weight
                                   });
            }

            return map;
        }

        private void ClearMap(Dictionary<int, MapItem> map)
        {
            var toDelete = new List<int>();
            foreach (var mapItem in map)
            {
                mapItem.Value.Inputs.RemoveAll(t => t.Processed);
                if (mapItem.Value.Inputs.Count == 0)
                    toDelete.Add(mapItem.Key);
            }

            foreach (var index in toDelete)
            {
                map.Remove(index);
            }
        }

        private void AddFunctionOneToOneCycle(BuildRuntimeContext buildRuntimeContext, List<int> source,
            Dictionary<int, Neuron> neuronDict, SumCycle sumCycle)
        {
            
            var cycle = new FunctionOneToOneCycle(sumCycle.Items.Count);
            foreach (var sumCycleItem in sumCycle.Items)
            {
                var neuron = neuronDict[sumCycleItem.NeuronIndex];
                var outputIndex = neuron.Outputs[0];
                cycle.Items.Add(new FunctionOneToOneCycle.Item {
                    Function = (INeuronFunctionOneToOne)neuron.Function,
                    InputValueIndex = sumCycleItem.ResultIndex,
                    TempOutputIndex = outputIndex
                });
                source.Add(outputIndex);
            }
            buildRuntimeContext.RuntimeCycles.Add(cycle);
        }

        private void AddFunctionManyToManyCycle(BuildRuntimeContext buildRuntimeContext, List<int> source, 
            Dictionary<int, Neuron> neuronDict, List<CyclePreviousCycleData> inputCycleData)
        {
            if (inputCycleData.Count == 0)
                return;

            var cycle = new FunctionManyToManyCycle(inputCycleData.Count);
            foreach (var cycleData in inputCycleData)
            {
                var neuron = neuronDict[cycleData.NeuronIndex];
                
                cycle.Items.Add(new FunctionManyToManyCycle.Item
                {
                    Function = (INeuronFunctionManyToMany)neuron.Function,
                    InputValueIndexes = cycleData.OutputIndexes,
                    TempOutputIndexes = neuron.Outputs
                });
                source.AddRange(neuron.Outputs);
            }
            buildRuntimeContext.RuntimeCycles.Add(cycle);
        }

        private MultiCycle AddMultiCycle(BuildRuntimeContext buildRuntimeContext, List<MapItem> inputs)
        {
            var cycle = new MultiCycle(inputs.Count);
            foreach (var input in inputs.SelectMany(t => t.Inputs))
            {
                var item = new MultiCycle.Item
                           {
                               Weight = input.Weight,
                               SourceIndex = input.SourceIndex,
                               NeuronIndex = input.NeuronIndex,
                               OutputIndex = buildRuntimeContext.TempMemoryIndex++
                           };
                cycle.Items.Add(item);
            }

            buildRuntimeContext.RuntimeCycles.Add(cycle);
            return cycle;
        }

        //private SumCycle AddSumCycle(BuildRuntimeContext buildRuntimeContext, MultiCycle multiCycle)
        //{
        //    var cycle = new SumCycle(10);

        //    var groups = multiCycle.Items.GroupBy(t => t.NeuronIndex, item => item);

        //    foreach (var group in groups)
        //    {
        //        var indexes = group.Select(t => t.OutputIndex).ToArray();
        //        cycle.Items.Add(new SumCycle.Item
        //                        {
        //                            Indexes = indexes,
        //                            NeuronIndex = group.Key,
        //                            ResultIndex = buildRuntimeContext.TempMemoryIndex++
        //                        });
        //    }

        //    buildRuntimeContext.RuntimeCycles.Add(cycle);
        //    return cycle;
        //}

        private SumCycle AddSumCycle(BuildRuntimeContext buildRuntimeContext, List<CyclePreviousCycleData> inputCycleData)
        {
            var cycle = new SumCycle(10);

            foreach (var cycleData in inputCycleData)
            {
                cycle.Items.Add(new SumCycle.Item
                {
                    Indexes = cycleData.OutputIndexes,
                    NeuronIndex = cycleData.NeuronIndex,
                    ResultIndex = buildRuntimeContext.TempMemoryIndex++
                });
            }

            buildRuntimeContext.RuntimeCycles.Add(cycle);
            return cycle;
        }

        private List<MapItem> GetMapItemsForCycle(Dictionary<int, MapItem> map, List<int> sourcesIndex)
        {
            var result = new List<MapItem>();
            foreach (var index in sourcesIndex)
            {
                if (!map.TryGetValue(index, out var mapItem))
                    continue;
                result.Add(mapItem);
            }

            return result;
        }

        private class MapInputItem
        {
            public float Weight { get; set; }
            public int SourceIndex { get; set; }
            public bool Processed { get; set; }
            public int NeuronIndex { get; set; }
        }

        private class MapItem
        {
            public List<MapInputItem> Inputs { get; } = new List<MapInputItem>();
        }

        private class BuildRuntimeContext
        {
            public List<Cycle> RuntimeCycles { get; set; }
            public int TempMemoryIndex { get; set; }
        }
    }
}