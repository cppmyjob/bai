using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Models.Layers;

namespace Bai.Intelligence.Models
{
    public class SequentialContext
    {
        public int PreviousInputCount { get; set; }
        public int InputOffset { get; set; }
        public int OutputOffset { get; set; }
    }
}
