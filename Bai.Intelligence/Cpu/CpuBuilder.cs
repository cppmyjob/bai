using System;
using System.Collections.Generic;
using System.Data;
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
                if (inputs.Count == 0)
                    // TODO correct exception
                    throw new Exception("Infinitive loop");

                var neuronInputs = GetNeuronInputs(buildRuntimeContext, inputs, neuronDict);

                // TODO add capacity
                source = new List<int>();

                if (neuronInputs.OneToOne.Count > 0)
                {
                    var dotCycles = AddDotCycle(buildRuntimeContext, neuronInputs.OneToOne);
                    AddFunctionOneToOneCycle(buildRuntimeContext, source, neuronDict, dotCycles);
                }

                if (neuronInputs.ManyToMany.Count > 0)
                {
                    var multiCycle = AddMultiCycle(buildRuntimeContext, neuronInputs.ManyToMany);
                    AddFunctionManyToManyCycle(buildRuntimeContext, source, neuronDict, multiCycle);
                }

                foreach (var input in inputs.SelectMany(t => t.Inputs)) 
                    input.Processed = true;
                ClearMap(inputMap);

            } while (inputMap.Keys.Count > 0);
        }

        private List<DotCycle> AddDotCycle(BuildRuntimeContext buildRuntimeContext, List<InputItem> neuronInputsOneToOne)
        {
            var cycles = new Dictionary<string, DotCycle>();

            var neuronGroups = neuronInputsOneToOne.GroupBy(t => t.Neuron.Index);
            foreach (var neuronGroup in neuronGroups)
            {
                var orderedInputs = neuronGroup.OrderBy(t => t.SourceIndex);
                var sourceIndexes = orderedInputs.Select(t => t.SourceIndex).ToArray();

                var sourceIndexesKey = string.Join(",", sourceIndexes);
                if (!cycles.TryGetValue(sourceIndexesKey, out var cycle))
                {
                    var inputData = new DotCycle.InputData();
                    inputData.SourceIndexes = sourceIndexes;
                    // TODO add capacity
                    inputData.DotProducts = new List<DotCycle.DotProduct>();
                    cycle = new DotCycle(inputData);
                    cycles.Add(sourceIndexesKey, cycle);
                }

                cycle.Inputs.DotProducts.Add(new DotCycle.DotProduct
                    {
                        NeuronIndex = neuronGroup.Key,
                        OutputIndex = buildRuntimeContext.TempMemoryIndex++,
                        Weights = orderedInputs.Select(t => t.Weight).ToArray()
                    });

            }

            var result = cycles.Values.ToList();
            buildRuntimeContext.RuntimeCycles.AddRange(result);
            return result;
        }

        private (List<InputItem> OneToOne, List<InputItem> ManyToMany) GetNeuronInputs(
            BuildRuntimeContext buildRuntimeContext, List<MapInput> inputs, Dictionary<int, Neuron> neuronDict)
        {
            List<InputItem> oneToOne = null;
            List<InputItem> manyToMany = null;

            var groups = inputs.SelectMany(t => t.Inputs).GroupBy(t => t.Neuron.Function.FunctionIoType);
            foreach (var group in groups)
            {
                switch (group.Key)
                {
                    case FunctionIoType.OneToOne:
                        oneToOne = group.ToList();
                        continue;
                    case FunctionIoType.ManyToMany:
                        manyToMany = group.ToList();
                        continue;
                    default:
                        throw new NotSupportedException();
                }
            }

            oneToOne ??= new List<InputItem>(0);
            manyToMany ??= new List<InputItem>(0);

            return (oneToOne, manyToMany);
        }

        private static Dictionary<int, MapInput> CreateInputMap(List<Neuron> neurons)
        {
            var map = new Dictionary<int, MapInput>();
            for (var index = 0; index < neurons.Count; index++)
            {
                var neuron = neurons[index];
                for (var i = 0; i < neuron.Inputs.Count; i++)
                {
                    var input = neuron.Inputs[i];
                    if (!map.TryGetValue(input.SourceIndex, out var mapItem))
                    {
                        mapItem = new MapInput { SourceIndex = input.SourceIndex };
                        map.Add(input.SourceIndex, mapItem);
                    }

                    mapItem.Inputs.Add(new InputItem
                    {
                        Neuron = neuron,
                        SourceIndex = input.SourceIndex,
                        Weight = input.Weight
                    });
                }
            }

            //foreach (var neuron in neurons)
            //{
            //    var groups = neuron.Inputs.GroupBy(t => t.SourceIndex);
            //    foreach (var group in groups)
            //    {
            //        if (!map.TryGetValue(group.Key, out var mapItem))
            //        {
            //            mapItem = new MapInput { SourceIndex = group.Key };
            //            map.Add(group.Key, mapItem);
            //        }

            //        var inputs = group.Select(t => new InputItem
            //        {
            //            Neuron = neuron,
            //            SourceIndex = t.SourceIndex,
            //            Weight = t.Weight
            //        });
            //        mapItem.Inputs.AddRange(inputs);
            //    }
            //}

            return map;
        }

        private void ClearMap(Dictionary<int, MapInput> map)
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
            Dictionary<int, Neuron> neuronDict, List<DotCycle> dotCycles)
        {
            var dotProducts = dotCycles.SelectMany(t => t.Inputs.DotProducts).ToList();
            var cycle = new FunctionOneToOneCycle(dotProducts.Count);
            foreach (var dotProduct in dotProducts)
            {
                var neuron = neuronDict[dotProduct.NeuronIndex];
                var outputIndex = neuron.Outputs[0];
                cycle.Items.Add(new FunctionOneToOneCycle.Item {
                    Function = (INeuronFunctionOneToOne)neuron.Function,
                    InputValueIndex = dotProduct.OutputIndex,
                    TempOutputIndex = outputIndex
                });
                source.Add(outputIndex);
            }
            buildRuntimeContext.RuntimeCycles.Add(cycle);
        }

        private void AddFunctionManyToManyCycle(BuildRuntimeContext buildRuntimeContext, List<int> source, 
            Dictionary<int, Neuron> neuronDict, MultiCycle multiCycle)
        {
            var cycle = new FunctionManyToManyCycle(multiCycle.Items.Count);
            foreach (var group in multiCycle.Items.GroupBy(t => t.NeuronIndex))
            {
                var neuron = neuronDict[group.Key];
                
                cycle.Items.Add(new FunctionManyToManyCycle.Item
                {
                    Function = (INeuronFunctionManyToMany)neuron.Function,
                    InputValueIndexes = group.Select(t => t.OutputIndex).ToArray(),
                    TempOutputIndexes = neuron.Outputs
                });
                source.AddRange(neuron.Outputs);
            }
            buildRuntimeContext.RuntimeCycles.Add(cycle);
        }

        private MultiCycle AddMultiCycle(BuildRuntimeContext buildRuntimeContext, List<InputItem> inputs)
        {
            var cycle = new MultiCycle(inputs.Count);
            foreach (var input in inputs)
            {
                var item = new MultiCycle.Item
                           {
                               Weight = input.Weight,
                               SourceIndex = input.SourceIndex,
                               NeuronIndex = input.Neuron.Index,
                               OutputIndex = buildRuntimeContext.TempMemoryIndex++
                           };
                cycle.Items.Add(item);
            }

            buildRuntimeContext.RuntimeCycles.Add(cycle);
            return cycle;
        }

        private List<MapInput> GetMapItemsForCycle(Dictionary<int, MapInput> map, List<int> sourcesIndex)
        {
            var result = new List<MapInput>();
            foreach (var index in sourcesIndex)
            {
                if (!map.TryGetValue(index, out var mapItem))
                    continue;
                result.Add(mapItem);
            }

            return result;
        }

        private class InputItem
        {
            public float Weight;
            public int SourceIndex;
            public bool Processed;
            public Neuron Neuron;
        }

        private class MapInput
        {
            public int SourceIndex;
            public readonly List<InputItem> Inputs = new List<InputItem>();
        }

        private class BuildRuntimeContext
        {
            public List<Cycle> RuntimeCycles;
            public int TempMemoryIndex;
        }
    }
}