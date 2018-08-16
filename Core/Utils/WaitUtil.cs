using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Utils
{
    public class WaitUtil
    {
        private int _timeout = 10000;
        private int _pollingEvery = 1000;
        private Exception _ex = null;

        public static WaitUtil Init()
        {
            return new WaitUtil();
        }

        public WaitUtil WithTimeout(int timeoutInMs)
        {
            _timeout = timeoutInMs;
            return this;
        }

        public WaitUtil PollingEvery(int intervalInMs)
        {
            _pollingEvery = intervalInMs;
            return this;
        }

        public WaitUtil WithException(Exception e)
        {
            _ex = e;
            return this;
        }

        public bool WaitFor<T>(Predicate<T> predicate, T arg)
        {
            var t0 = DateTime.Now;

            while (!predicate.Invoke(arg) && !IsTimeoutExpired(t0, _timeout))
            {
                Thread.Sleep(_pollingEvery);
            }

            return !IsTimeoutExpired(t0, _timeout);
        }

        private bool IsTimeoutExpired(DateTime start, int timeout) => (DateTime.Now - start).TotalSeconds >= timeout / _pollingEvery;
    }
}
