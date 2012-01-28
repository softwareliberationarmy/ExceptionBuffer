using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AUIM.ExceptionHandling
{
    public class ExceptionBufferingCriteria
    {
        public Predicate<Exception> ExceptionCriteria { get; set; }
        public Frequency FrequencyAllowed { get; set; }
    }
}
