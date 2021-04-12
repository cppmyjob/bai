using System;
using System.Collections.Generic;
using System.Text;
using Bai.Intelligence.Models.Layers;

namespace Bai.Intelligence.Models
{
    public class SequentialContext
    {
        private readonly int _firstInputCount;
        private readonly int _finalOutputCount;
        private int _layersNumber;
        private int _currentLayer;
        private int _lastOutputOffset;
        private int _regularOutputOffset;
        private int _previousOutputOffset;

        public SequentialContext(int firstInputCount, int finalOutputCount, int layersNumber)
        {
            _firstInputCount = firstInputCount;
            _finalOutputCount = finalOutputCount;
            _layersNumber = layersNumber;
            _regularOutputOffset = _firstInputCount + _finalOutputCount;
            _lastOutputOffset = _firstInputCount;
        }


        public int PreviousInputCount { get; private set; }

        public int InputOffset { get; set; }


        public int OutputOffset
        {
            get => _currentLayer == _layersNumber - 1 ? _lastOutputOffset : _regularOutputOffset;
            set
            {
                if (_currentLayer == _layersNumber - 1)
                    _lastOutputOffset = value;
                else
                    _regularOutputOffset = value;
            }
        }

        public void Init()
        {
            _currentLayer = 0;
            PreviousInputCount = _firstInputCount;
            InputOffset = 0;
            _previousOutputOffset = OutputOffset;
        }

        public void NextLayer(int previousInputCount)
        {
            PreviousInputCount = previousInputCount;
            InputOffset = _previousOutputOffset;
            _previousOutputOffset = OutputOffset;
            ++_currentLayer;
        }

        public void InsertLayer()
        {
            ++_layersNumber;
            _previousOutputOffset = OutputOffset;
        }
    }
}
