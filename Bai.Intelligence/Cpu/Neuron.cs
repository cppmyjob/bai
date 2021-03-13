﻿using System.Collections.Generic;
using Bai.Intelligence.Function;

namespace Bai.Intelligence.Cpu
{

    public class Neuron
    {
        public int Index { get; set; }
        public List<NeuronInput> Inputs { get; } = new List<NeuronInput>();
        public INeuronFunction Function { get; set; }
        public NeuronSource Source { get; set; }
    }
}
