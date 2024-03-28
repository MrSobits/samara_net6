namespace Bars.Gkh.Domain.DatabaseMutex.ForTest
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class PuppetThreadWorkerException : Exception
    {
        public PuppetThreadWorkerException() : base() { }
        public PuppetThreadWorkerException(string msg) : base(msg) { }
        public PuppetThreadWorkerException(string msg, Exception innerException) : base(msg, innerException) { }
    }

    public enum PuppetResult
    {
        NoWait,
        Wait,
        WaitAndThrowError
    }

    public class PuppetThread : IDisposable
    {
        Thread thread;

        object dataLock = new object();
        bool workerIdle = true;
        bool shouldQuit = false;
        bool workDone = true;
        Action nextWork = null;
        Exception lastException = null;
        List<Exception> exceptions = new List<Exception>();

        public PuppetThread(string threadName = null)
        {
            this.thread = new Thread(this.ThreadFunction);
            if (threadName != null) {
                this.thread.Name = threadName;
            }
            this.thread.Start();
        }

        public bool IsIdle { get { lock (this.dataLock) { return this.workerIdle; } } }

        public void Close()
        {
            lock (this.dataLock) {
                this.shouldQuit = true;
                Monitor.PulseAll(this.dataLock);
            }
            if (!this.thread.Join(TimeSpan.FromSeconds(2))) {
                this.thread.Abort();
            }
        }

        public void Dispose()
        {
            this.Close();
        }

        public Exception WaitForWorkDone(TimeSpan? timeout = null)
        {
            lock (this.dataLock) {
                if (timeout == null) {
                    while (!this.workDone) {
                        Monitor.Wait(this.dataLock);
                    }
                } else {
                    DateTime deadline = DateTime.Now + timeout.Value;
                    while (!this.workDone && DateTime.Now < deadline) {
                        Monitor.Wait(this.dataLock, deadline - DateTime.Now);
                    }
                    if (DateTime.Now > deadline) {
                        throw new PuppetThreadWorkerException("Timeout in puppet thread '" + this.thread.Name + "'");
                    }
                }
                return this.lastException;
            }
        }

        public void Do(Action work, PuppetResult resultHandling = PuppetResult.WaitAndThrowError, TimeSpan? timeout = null)
        {
            lock (this.dataLock) {
                this.nextWork = work;
                this.workDone = false;
                this.lastException = null;
                Monitor.PulseAll(this.dataLock);
            }
            if (resultHandling != PuppetResult.NoWait) {
                Exception exception = this.WaitForWorkDone(timeout);
                if (this.lastException != null && resultHandling == PuppetResult.WaitAndThrowError) {
                    throw new PuppetThreadWorkerException("Exception in puppet thread '" + this.thread.Name + "'", this.lastException);
                }
            }
        }

        bool GetWork(out Action work)
        {
            lock (this.dataLock) {
                while (this.nextWork == null && !this.shouldQuit) {
                    Monitor.Wait(this.dataLock);
                }
                if (this.shouldQuit) {
                    work = null;
                    return false;
                } else {
                    work = this.nextWork;
                    this.nextWork = null;
                    return true;
                }
            }
        }

        void ThreadFunction()
        {
            while (true) {
                Action work;
                if (!this.GetWork(out work)) {
                    lock (this.dataLock) {
                        this.workDone = true;
                        Monitor.PulseAll(this.dataLock);
                    }
                    break;
                }
                lock (this.dataLock) {
                    this.workerIdle = false;
                    Monitor.PulseAll(this.dataLock);
                }
                try {
                    work();
                } catch (Exception ex) {
                    ex = new Exception("Exception in supplied work functon", ex);
                    lock (this.dataLock) {
                        this.exceptions.Add(ex);
                        this.lastException = ex;
                    }
                }
                lock (this.dataLock) {
                    this.workerIdle = true;
                    Monitor.PulseAll(this.dataLock);
                }
                lock (this.dataLock) {
                    this.workDone = true;
                    Monitor.PulseAll(this.dataLock);
                }
            }
        }
    }
}
