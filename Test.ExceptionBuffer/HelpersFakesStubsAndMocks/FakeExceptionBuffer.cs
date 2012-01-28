using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AUIM.ExceptionHandling;

namespace ExceptionBufferTest.HelpersFakesStubsAndMocks
{
    internal class FakeExceptionBuffer: ExceptionBuffer
    {
        internal Dictionary<Guid, ExceptionBufferingCriteria> ExceptionCriteriaSets
        {
            get { return _exceptionCriteriaSets; }
        }
    }
}
