namespace Bars.Gkh.Domain.DatabaseMutex.ForTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Bars.B4.DataAccess;

    using Castle.Windsor;

    using NHibernate;

    public class DatabaseMutexTests
    {
        private readonly ISession session;
        private readonly ISessionFactory sessionFactory;
        private readonly IDatabaseMutexManager databaseMutexManager;

        public DatabaseMutexTests(IWindsorContainer container)
        {
            this.session = container.Resolve<ISessionProvider>().GetCurrentSession();
            this.sessionFactory = container.Resolve<ISessionFactory>();
            this.databaseMutexManager = container.Resolve<IDatabaseMutexManager>();
        }

        public void ExceptionIsThrownWhenUsedOutsideTransaction()
        {
            var mutex = new DatabaseLockedMutexHandle(this.session, Guid.NewGuid().ToString(), "lock", null);
            var thrownException = false;
            try
            {
                mutex.Lock();
            }
            catch (DatabaseMutexWithoutTransactionException ex)
            {
                thrownException = true;
            }

            if (!thrownException)
            {
                throw new Exception("Should throw exception when lock is called outside transactions");
            }
        }

        public void SingleThreadedLocksAndUnlocks()
        {
            using (var tx = this.session.BeginTransaction())
            {
                using (this.databaseMutexManager.Lock(Guid.NewGuid().ToString(), "lock"))
                {
                }

                tx.Commit();
            }
        }

        public void TwoThreadsBasicInterleaved()
        {
            string mutexName = Guid.NewGuid().ToString();
            using (var th1 = new PuppetThread("puppet1"))
            using (var th2 = new PuppetThread("puppet2"))
            {
                ISession s1 = null, s2 = null;
                ITransaction tx1 = null, tx2 = null;
                DatabaseLockedMutexHandle m1 = null, m2 = null;
                var dataLock = new object();
                var th2Acquired = false;
                try
                {
                    th1.Do(() => { s1 = this.sessionFactory.OpenSession(); tx1 = s1.BeginTransaction(); m1 = new DatabaseLockedMutexHandle(s1, mutexName, "lock", null); });
                    th2.Do(() => { s2 = this.sessionFactory.OpenSession(); tx2 = s2.BeginTransaction(); m2 = new DatabaseLockedMutexHandle(s2, mutexName, "lock", null); });

                    th1.Do(() => m1.Lock(), timeout: TimeSpan.FromSeconds(10));
                    th2.Do(() =>
                    {
                        m2.Lock();
                        lock (dataLock)
                        {
                            th2Acquired = true;
                        }
                    }, PuppetResult.NoWait);
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    if (th2.IsIdle)
                    {
                        throw new Exception("While thread1 holds mutex, thread2 should block while trying acquire it");
                    }
                    
                    th1.Do(() => m1.Close());
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    if (th2.IsIdle)
                    {
                        throw new Exception("When thread2 closed mutex and not yet commited transaction, thread2 should block while trying to acquire it");
                    }
                    
                    th1.Do(() => tx1.Commit());
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    if (!(th2.IsIdle && th2Acquired))
                    {
                        throw new Exception("When thread2 closed mutex and commited transaction, thread2 should get lock it");
                    }
                }
                finally
                {
                    if (tx1 != null) { th1.Do(() => tx1.Dispose(), PuppetResult.Wait); };
                    if (tx2 != null) { th2.Do(() => tx2.Dispose(), PuppetResult.Wait); };
                    if (s1 != null) { th1.Do(() => s1.Dispose(), PuppetResult.Wait); };
                    if (s2 != null) { th1.Do(() => s2.Dispose(), PuppetResult.Wait); };
                }
            }
        }

        public void SeveralContendedThreadExcludeEachOther()
        {
            var random = new Random(31234);
            var mutexName = Guid.NewGuid().ToString();
            var threadCount = 10;
            var dataLock = new object();
            long inMutex = 0;
            var mutexEntryCount = new List<long>();
            var puppets = Enumerable.Range(1, threadCount).Select(x => new PuppetThread("puppet_" + x)).ToList();
            var exceptions = new List<Exception>();
            try
            {
                foreach (var puppet in puppets)
                {
                    long sleepMs = random.Next(1000);
                    puppet.Do(() =>
                    {
                        using (var puppetSession = this.sessionFactory.OpenSession())
                        using (var tx = puppetSession.BeginTransaction())
                        {
                            using (var mutex = new DatabaseLockedMutexHandle(puppetSession, mutexName, "lock", null))
                            {
                                mutex.Lock();
                                lock (dataLock)
                                {
                                    ++inMutex;
                                    mutexEntryCount.Add(inMutex);
                                }

                                Thread.Sleep(TimeSpan.FromMilliseconds(2 * sleepMs));
                            }
                            
                            lock (dataLock)
                            {
                                --inMutex;
                                mutexEntryCount.Add(inMutex);
                            }

                            tx.Commit();
                        }
                    },
                    PuppetResult.NoWait);
                }

                foreach (var puppet in puppets)
                {
                    var ex = puppet.WaitForWorkDone();
                    if (ex != null)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
            finally
            {
                foreach (var puppet in puppets)
                {
                    puppet.Dispose();
                }
            }

            var invalidMutexEntryCounts = mutexEntryCount.Where(x => x > 1).ToList();
            if (invalidMutexEntryCounts.Count > 0)
            {
                throw new Exception("Более 1-го паралельного вхождения в блокировку");
            }

            if (exceptions.Count > 0)
            {
                throw new Exception("Есть ошибки");
            }
        }

        public void TryEnter()
        {
            var random = new Random(31234);
            var mutexName = Guid.NewGuid().ToString();
            var threadCount = 10;
            var dataLock = new object();
            long inMutex = 0;
            var mutexEntryCount = new List<long>();
            var puppets = Enumerable.Range(1, threadCount).Select(x => new PuppetThread("puppet_" + x)).ToList();
            var exceptions = new List<Exception>();
            try
            {
                foreach (var puppet in puppets)
                {
                    inMutex++;
                    var id = inMutex;
                    long sleepMs = random.Next(1000);
                    puppet.Do(() =>
                    {
                        using (var puppetSession = this.sessionFactory.OpenSession())
                        using (var tx = puppetSession.BeginTransaction())
                        {
                            using (var mutex = new DatabaseLockedMutexHandle(puppetSession, mutexName, "try_lock" + id, null))
                            {
                                if (mutex.TryLock())
                                {
                                    lock (dataLock)
                                    {
                                        mutexEntryCount.Add(inMutex);
                                    }
                                }

                                Thread.Sleep(TimeSpan.FromMilliseconds(2 * sleepMs));
                            }

                            tx.Commit();
                        }
                    },
                    PuppetResult.NoWait);
                }

                foreach (var puppet in puppets)
                {
                    var ex = puppet.WaitForWorkDone();
                    if (ex != null)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
            finally
            {
                foreach (var puppet in puppets)
                {
                    puppet.Dispose();
                }
            }

            if (mutexEntryCount.Count > 1)
            {
                throw new Exception("Более 1-го паралельного вхождения в блокировку");
            }

            if (exceptions.Count > 0)
            {
                throw new Exception("Есть ошибки");
            }
        }
    }
}