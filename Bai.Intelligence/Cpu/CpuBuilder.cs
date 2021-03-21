using System;
using System.Collections.Generic;
using System.Linq;
using Bai.Intelligence.Cpu.Runtime;
using Bai.Intelligence.Definition;
using Bai.Intelligence.Definition.Dna.Genes;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Cpu
{
    public class CpuBuilder : IBuilder
    {
        public IRuntime Build(NetworkDefinition definition)
        {
            var runtime = new CpuRuntime(definition.InputCount, definition.OutputCount);

            var buildRuntimeContext = new BuildRuntimeContext {
                RuntimeCycles = runtime.Cycles,
                TempMemoryIndex = definition.InputCount
            };
            AddCycles(definition, buildRuntimeContext);

            runtime.TempMemory = new float[buildRuntimeContext.TempMemoryIndex];

            return runtime;
        }

        private void AddCycles(NetworkDefinition definition, BuildRuntimeContext buildRuntimeContext)
        {
            var neurons = CreateNeurons(definition);
            
            // add space to neuron outputs
            buildRuntimeContext.TempMemoryIndex += neurons.Count;

            ConfigureCycles(definition, neurons, buildRuntimeContext);
        }

        private List<Neuron> CreateNeurons(NetworkDefinition definition)
        {
            var neurons = new List<Neuron>();
            var context = new BuilderContext();
            var neuronIndex = 0;
            foreach (var chromosome in definition.Chromosomes)
            {
                var woman = chromosome.Woman;
                var man = chromosome.Man;
                for (var j = 0; j < woman.Genes.Length; j++)
                {
                    var womanGene = woman.Genes[j];
                    var manGene = man.Genes[j];
                    var gene = GetFinalGene(womanGene, manGene);

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

        private void ConfigureCycles(NetworkDefinition definition, List<Neuron> neurons,
            BuildRuntimeContext buildRuntimeContext)
        {
            var inputMap = CreateInputMap(neurons);
            var source = Enumerable.Range(0, definition.InputCount).ToList();

            do
            {
                var inputs = GetMapItemsForCycle(inputMap, source);
                var multiCycle = AddMultiCycle(buildRuntimeContext, inputs);
                var sumCycle = AddSumCycle(buildRuntimeContext, multiCycle);
                source = AddFunctionCycle(buildRuntimeContext, neurons, sumCycle);
                foreach (var input in inputs.SelectMany(t => t.Inputs)) input.Processed = true;
                ClearMap(inputMap);
            } while (inputMap.Keys.Count > 0);
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

        private List<int> AddFunctionCycle(BuildRuntimeContext buildRuntimeContext, List<Neuron> neurons, SumCycle sumCycle)
        {
            var neuronDict = neurons.ToDictionary(t => t.Index, t => t);
            var cycle = new FunctionCycle(sumCycle.Items.Count);
            var result = new List<int>(sumCycle.Items.Count);
            foreach (var sumCycleItem in sumCycle.Items)
            {
                var neuron = neuronDict[sumCycleItem.NeuronIndex];
                var outputIndex = neuron.Source.Index;
                cycle.Items.Add(new FunctionCycle.Item {
                                                             Function = neuron.Function,
                                                             InputValueIndex = sumCycleItem.ResultIndex,
                                                             TempOutputIndex = neuron.Source.Index
                                                      });
                result.Add(outputIndex);
            }
            buildRuntimeContext.RuntimeCycles.Add(cycle);
            return result;
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

        private SumCycle AddSumCycle(BuildRuntimeContext buildRuntimeContext, MultiCycle multiCycle)
        {
            var cycle = new SumCycle(10);

            var groups = multiCycle.Items.GroupBy(t => t.NeuronIndex, item => item);

            foreach (var group in groups)
            {
                var indexes = group.Select(t => t.OutputIndex).ToArray();
                cycle.Items.Add(new SumCycle.Item
                                {
                                    Indexes = indexes,
                                    NeuronIndex = group.Key,
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

        private BaseGene GetFinalGene(BaseGene womanGene, BaseGene manGene)
        {
            return womanGene.Dominant ? womanGene : manGene;
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