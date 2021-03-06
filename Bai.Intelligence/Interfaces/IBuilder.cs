﻿using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Organism.Definition;

namespace Bai.Intelligence.Interfaces
{
    public interface IBuilder
    {
        IRuntime Build(NetworkDefinition definition);
    }
}
