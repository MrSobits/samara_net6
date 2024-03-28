namespace Bars.Gkh.RegOperator.Interceptors.Decision
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Decisions.Nso.Entities;
    using B4.Modules.NHibernateChangeLog;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;

    using Decisions.Nso.Domain;

    using NHibernate.Linq;

    public class MonthlyFeeAmountDecisionInterceptor : EmptyDomainInterceptor<MonthlyFeeAmountDecision>
    {
        public ILogMonthlyFeeAmountDecisionService LogMonthlyFeeAmountDecisionService { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public ISessionProvider SessionProvider { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<MonthlyFeeAmountDecision> service, MonthlyFeeAmountDecision entity)
        {
            return this.SaveHistory(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<MonthlyFeeAmountDecision> service, MonthlyFeeAmountDecision entity)
        {
            var result = this.SaveHistory(entity);

            if (result.Success)
            {
                this.LogMonthlyFeeAmountDecisionService.Log(entity, entity.Protocol.RealityObject.Id, ActionKind.Insert);
            }

            return result;
        }

        public override IDataResult AfterCreateAction(IDomainService<MonthlyFeeAmountDecision> service, MonthlyFeeAmountDecision entity)
        {
            // Логирования сущности в место NHibernateChangeLog
            this.LogMonthlyFeeAmountDecisionService.Log(entity, entity.Protocol.RealityObject.Id, ActionKind.Insert);
            return new BaseDataResult(true, string.Empty);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<MonthlyFeeAmountDecision> service, MonthlyFeeAmountDecision entity)
        {
            var historyService = this.Container.Resolve<IDomainService<MonthlyFeeAmountDecHistory>>();

            var roId = entity.Protocol.RealityObject.Id;
            var history = historyService.GetAll().FirstOrDefault(x => x.Protocol.RealityObject.Id == roId);

            if (history == null)
            {
                return new BaseDataResult(true, string.Empty);
            }

            var toDelete = entity.Decision;

            var newList = new List<PeriodMonthlyFee>();

            foreach (var periodMonthlyFee in history.Decision)
            {
                if (toDelete.FirstOrDefault(x => x.Guid != periodMonthlyFee.Guid) != null)
                {
                    newList.Add(periodMonthlyFee);
                }
            }

            history.Decision = newList;

            if (history.Id > 0)
            {
                historyService.Update(history);
            }
            else
            {
                historyService.Save(history);
            }

            // Логирования сущности в место NHibernateChangeLog
            this.LogMonthlyFeeAmountDecisionService.Log(entity, roId, ActionKind.Delete);

            return new BaseDataResult(true, string.Empty);
        }

        private IDataResult SaveHistory(MonthlyFeeAmountDecision entity)
        {
            var roId = entity.Protocol.RealityObject.Id;
            var user = this.UserManager.GetActiveOperator();

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                var history = session.Query<MonthlyFeeAmountDecHistory>()
                        .FirstOrDefault(x => x.Protocol.RealityObject.Id == roId)
                    ?? new MonthlyFeeAmountDecHistory
                    {
                        Protocol = entity.Protocol,
                        UserName = user != null ? user.Name : "Администратор",
                        Decision = new List<PeriodMonthlyFee>()
                    };

                foreach (var periodMonthlyFee in entity.Decision)
                {
                    if (string.IsNullOrEmpty(periodMonthlyFee.Guid) || periodMonthlyFee.Guid == "00000000-0000-0000-0000-000000000000")
                    {
                        periodMonthlyFee.Guid = Guid.NewGuid().ToString();
                        history.Decision.Add(periodMonthlyFee);
                    }
                    else
                    {
                        var h = history.Decision.FirstOrDefault(x => x.Guid == periodMonthlyFee.Guid);
                        if (h == null)
                        {
                            h = new PeriodMonthlyFee();
                            history.Decision.Add(h);
                        }

                        h.Value = periodMonthlyFee.Value;
                        h.Guid = periodMonthlyFee.Guid;
                        h.To = periodMonthlyFee.To;
                        h.From = periodMonthlyFee.From;
                    }
                }

                var guids = session.Query<MonthlyFeeAmountDecision>()
                    .Where(x => x.Id != entity.Id)
                    .Where(x => x.Decision != null)
                    .Where(x => x.Protocol.RealityObject.Id == roId)
                    .Select(x => x.Decision)
                    .AsEnumerable()
                    .SelectMany(x => x)
                    .ToList();

                guids.AddRange(entity.Decision);

                var newList = new List<PeriodMonthlyFee>();

                foreach (var periodMonthlyFee in history.Decision)
                {
                    if (guids.FirstOrDefault(x => x.Guid == periodMonthlyFee.Guid) != null)
                    {
                        newList.Add(periodMonthlyFee);
                    }
                }

                history.Decision = newList;

                if (history.Id > 0)
                {
                    session.Update(history);
                }
                else
                {
                    session.Insert(history);
                }

                return new BaseDataResult(true, string.Empty);
            }

        }
    }
}