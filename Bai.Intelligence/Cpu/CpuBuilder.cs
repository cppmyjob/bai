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
            var memorySize = definition.InputCount + definition.OutputCount;

            var runtime = new CpuRuntime();

            runtime.Memory = new float[memorySize];

            var buildRuntimeContext = new BuildRuntimeContext();
            buildRuntimeContext.RuntimeCycles = runtime.Cycles;
            AddCycles(definition, buildRuntimeContext);

            runtime.TempMemory = new float[buildRuntimeContext.TempMemoryIndex];

            return runtime;
        }

        private void AddCycles(NetworkDefinition definition, BuildRuntimeContext buildRuntimeContext)
        {
            var neurons = CreateNeurons(definition);
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


            var source = Enumerable.Range(0, definition.InputCount).ToArray();
            var inputs = GetMapItemsForCycle(map, source);

            var multiCycle = AddMultiCycle(buildRuntimeContext, inputs);
            var sumCycle = AddSumCycle(buildRuntimeContext, multiCycle);
            AddFunctionCycle(buildRuntimeContext, neurons, sumCycle);

            foreach (var input in inputs.SelectMany(t => t.Inputs)) input.Processed = true;
        }

        private void AddFunctionCycle(BuildRuntimeContext buildRuntimeContext, List<Neuron> neurons, SumCycle sumCycle)
        {
            var neuronDict = neurons.ToDictionary(t => t.Index, t => t);
            var cycle = new FunctionCycle(sumCycle.Items.Count);
            foreach (var sumCycleItem in sumCycle.Items)
            {
                var neuron = neuronDict[sumCycleItem.NeuronIndex];
                cycle.Items.Add(new FunctionCycle.Item {
                                                             Function = neuron.Function,
                                                             InputValueIndex = sumCycleItem.ResultIndex,
                                                             OutputIndex = neuron.Source.Index
                                                      });
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

        private List<MapItem> GetMapItemsForCycle(Dictionary<int, MapItem> map, int[] sourcesIndex)
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