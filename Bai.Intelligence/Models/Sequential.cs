using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Definition;
using Bai.Intelligence.Models.Layers;

namespace Bai.Intelligence.Models
{
    public class Sequential
    {
        public List<Layer> Layers { get; } = new List<Layer>();

        public NetworkDefinition NetworkDefinition { get; private set; }

        public void Compile()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                var layer = Layers[i];
                if (i == 0)
                {

                }

                var genes = layer.Compile();
            }
        }

    }
}
