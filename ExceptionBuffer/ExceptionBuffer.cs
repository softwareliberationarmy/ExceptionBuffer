using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AUIM.ExceptionHandling
{
    public class ExceptionBuffer
    {
        protected Dictionary<Guid, ExceptionBufferingCriteria> _exceptionCriteriaSets =
            new Dictionary<Guid, ExceptionBufferingCriteria>();

        protected Dictionary<Guid, Queue<DateTime>> _exceptionsSoFar =
            new Dictionary<Guid, Queue<DateTime>>();

        public void Add(ExceptionBufferingCriteria Criteria)
        {
            _exceptionCriteriaSets.Add(Guid.NewGuid(), Criteria);
        }

        public void HandleException(Exception ExceptionToHandle)
        {
            int matches = 0;
            bool overOurLimit = false;

            foreach (KeyValuePair<Guid, ExceptionBufferingCriteria> kvp in _exceptionCriteriaSets)
            {
                if (kvp.Value.ExceptionCriteria(ExceptionToHandle))
                {
                    matches++;

                    if (!_exceptionsSoFar.ContainsKey(kvp.Key))
                    {
                        _exceptionsSoFar.Add(kvp.Key, new Queue<DateTime>());
                    }
                    _exceptionsSoFar[kvp.Key].Enqueue(DateTime.Now);
                    if (_exceptionsSoFar[kvp.Key].Count > kvp.Value.FrequencyAllowed.NumberOfTimes)
                    {
                        DateTime dt = _exceptionsSoFar[kvp.Key].Dequeue();
                        if ((DateTime.Now - dt) < kvp.Value.FrequencyAllowed.Duration 
                            || kvp.Value.FrequencyAllowed.Duration == TimeSpan.Zero)
                        {
                            _exceptionsSoFar[kvp.Key].Clear();
                            overOurLimit = true;
                        }
                    }                    
                }
            }

            if(matches == 0 || overOurLimit)
                throw ExceptionToHandle;
        }

    }
}
