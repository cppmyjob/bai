using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Bai.Intelligence.Interfaces;

namespace Bai.Intelligence.Utils
{
    public class TimeMeter
    {
        private readonly ILogger _logger;
        private readonly string _label;
        private readonly Stopwatch _stopWatch = new Stopwatch();
        public TimeMeter(ILogger logger, string label)
        {
            _logger = logger;
            _label = label;
        }

        public void Start()
        {
            _stopWatch.Start();
        }

        public void Stop(string suffix = "")
        {
            _stopWatch.Stop();

            var ts = _stopWatch.Elapsed;
            _logger.Debug($"{_label} RunTime {ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds:000} {suffix}");
        }

    }
}
