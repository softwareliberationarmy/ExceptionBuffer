using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AUIM.ExceptionHandling
{
    public class Frequency
    {
        public Frequency(int NumberOfTimes, TimeSpan Duration)
        {
            if (NumberOfTimes < 1)
                throw new ArgumentOutOfRangeException("NumberOfTimes", "Must be greater than zero");

            this.NumberOfTimes = NumberOfTimes;
            this.Duration = Duration;
        }

        public int NumberOfTimes { get; private set; }
        public TimeSpan Duration { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} times per {1}", NumberOfTimes, Duration);
        }

    }
}
