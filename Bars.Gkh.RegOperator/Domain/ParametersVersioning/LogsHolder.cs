namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Authentification;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;

    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;
    using NHibernate;
    using Repository;

    public class LogsHolder
    {
        private readonly IWindsorContainer _container;
        private IDictionary<string, LogRecord> _logs;
        private IGkhUserManager _userManager;
        private IChargePeriodRepository _chargePeriodRepo;

        private readonly object _lock = new object();
        private IPeriod _curPeriod;
        private IPeriod _fPeriod;

        private IGkhUserManager UserManager
        {
            get { return _userManager ?? (_userManager = _container.Resolve<IGkhUserManager>()); }
        }

        private IChargePeriodRepository PeriodRepo
        {
            get { return _chargePeriodRepo ?? (_chargePeriodRepo = _container.Resolve<IChargePeriodRepository>()); }
        }

        private IPeriod CurrentPeriod
        {
            get { return _curPeriod ?? (_curPeriod = PeriodRepo.GetCurrentPeriod()); }
        }

        private IPeriod FirstPeriod
        {
            get { return _fPeriod ?? (_fPeriod = PeriodRepo.GetFirstPeriod()); }
        }

        public LogsHolder(IWindsorContainer container)
        {
            _container = container;
            _logs = new ConcurrentDictionary<string, LogRecord>();
        }

        public void Add(EntityLogLight log, object entity)
        {
            _logs[CreateKey(log)] = new LogRecord() { Log = log, Entity = entity };
        }

        public void Remove(params string[] keys)
        {
            keys.ForEach(key => _logs.Remove(key));
        }

        public void Clear()
        {
            _logs.Clear();
        }

        public void Flush()
        {
            lock (_lock)
            {
                var sessions = _container.Resolve<ISessionProvider>();
                var session = sessions.GetCurrentSession();
                var flush = session.FlushMode;
                session.FlushMode = FlushMode.Never;

                try
                {
                    var login = UserManager.GetActiveUser().Return(x => x.Login);

                    _logs.Values.ForEach(x =>
                    {
                        x.Log.ObjectCreateDate = DateTime.Now;
                        x.Log.ObjectEditDate = DateTime.Now;
                        x.Log.DateApplied = DateTime.UtcNow;
                        x.Log.DateActualChange = GetDateActualChange(x.Entity, x.Log.ClassName);
                        x.Log.User = login.IsEmpty() ? "anonymous" : login;
                    });
                }
                finally
                {
                    session.FlushMode = flush;
                }

                using (_container.Using(sessions))
                {
                    using (var ss = sessions.OpenStatelessSession())
                    {
                        using (var tr = ss.BeginTransaction())
                        {
                            try
                            {
                                _logs.Values.ForEach(x => ss.Insert(x.Log));
                                tr.Commit();
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                            finally
                            {
                                Clear();
                            }
                        }
                    }
                }
            }
        }

        private string CreateKey(EntityLogLight log)
        {
            return "{0}|{1}".FormatUsing(log.EntityId, log.ParameterName);
        }

        private DateTime GetNow()
        {
            var now = DateTime.UtcNow;

            var currentPeriodStartDate = CurrentPeriod.Return(x => x.StartDate);

            if (now < currentPeriodStartDate)
            {
                now = currentPeriodStartDate.AddDays(1);
            }

            return now;
        }

        private DateTime GetDateActualChange(object entity, string className)
        {
            var now = GetNow();

            var hasDate = entity as IHasDateActualChange;

            DateTime dateActualChange;

            if (hasDate != null)
            {
                dateActualChange = hasDate.ActualChangeDate;
            }
            else
            {
                dateActualChange =
                    new DateTime(
                        now.Year,
                        now.Month,
                        1, //day
                        now.Hour,
                        now.Minute,
                        now.Second,
                        now.Millisecond);
            }

            switch (className)
            {
                case "Room":
                    return FirstPeriod.Return(x => x.StartDate);
                default:
                    return dateActualChange;
            }
        }

        private class LogRecord
        {
            public EntityLogLight Log { get; set; }
            public object Entity { get; set; }
        }
    }
}