namespace Bars.GkhGji.DomainService.SurveyPlan.Impl.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public abstract class AbstractSurveyPlanStrategy : ISurveyPlanStrategy
    {
        private readonly IDomainService<AuditPurposeGji> _auditPurposeDomain;

        private readonly IDomainService<ContragentAuditPurpose> _contragentAuditPurposeDomain;

        private readonly IDomainService<Contragent> _contragentDomain;

        private AuditPurposeGji _purpose;

        protected AbstractSurveyPlanStrategy(
            IDomainService<Contragent> contragentDomain,
            IDomainService<ContragentAuditPurpose> contragentAuditPurposeDomain,
            IDomainService<AuditPurposeGji> auditPurposeDomain)
        {
            _contragentDomain = contragentDomain;
            _contragentAuditPurposeDomain = contragentAuditPurposeDomain;
            _auditPurposeDomain = auditPurposeDomain;
        }

        /// <summary>
        ///     Причина при включении в план на основании даты прошлой проверки
        /// </summary>
        protected abstract string LastAuditDateReason { get; }

        /// <summary>
        ///     Количество лет с предыдущей проверки
        /// </summary>
        protected abstract int LastAuditDateYears { get; }

        /// <summary>
        ///     Причина при включении в план на основании другой даты
        /// </summary>
        protected abstract string OtherwiseDateReason { get; }

        /// <summary>
        ///     Количество лет с другой даты
        /// </summary>
        protected abstract int OtherwiseDateYears { get; }

        public abstract string Code { get; }

        public AuditPurposeGji Purpose
        {
            get
            {
                return _purpose ?? (_purpose = _auditPurposeDomain.GetAll().FirstOrDefault(x => x.Code == Code));
            }
        }

        public virtual IEnumerable<SurveyPlanCandidateProxy> CreatePlanItems()
        {
            if (Purpose == null)
            {
                return new SurveyPlanCandidateProxy[0];
            }

            var contragents = ApplyFilter(_contragentDomain.GetAll()).ToArray();
            var items = new List<SurveyPlanCandidateProxy>();

            foreach (var contragent in contragents)
            {
                var nextAudit = GetNextAudit(contragent);
                if (nextAudit == null)
                {
                    continue;
                }

                items.Add(
                    new SurveyPlanCandidateProxy
                    {
                            AuditPurpose = Purpose,
                            Contragent = contragent,
                            PlanYear = nextAudit.Value.PlanYear,
                            PlanMonth = nextAudit.Value.PlanMonth,
                            Reason = nextAudit.Value.Reason,
                            FromLastAuditDate = nextAudit.Value.FromLastAuditDate
                        });
            }

            return items;
        }

        /// <summary>
        ///     Применение фильтрации к контрагентам (в зависимости от цели)
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        protected abstract IQueryable<Contragent> ApplyFilter(IQueryable<Contragent> query);

        /// <summary>
        ///     Получение следующей даты проверки с причиной
        /// </summary>
        /// <param name="contragent"></param>
        /// <returns></returns>
        protected virtual NextAudit? GetNextAudit(Contragent contragent)
        {
            var lastAudit =
                _contragentAuditPurposeDomain.GetAll()
                                             .Where(
                                                 x => x.Contragent.Id == contragent.Id && x.AuditPurpose.Code == Code)
                                             .Select(x => x.LastInspDate)
                                             .OrderByDescending(x => x)
                                             .FirstOrDefault();
            DateTime planDate;
            string reason;
            if (lastAudit != null)
            {
                reason = LastAuditDateReason;
                planDate = lastAudit.Value.AddYears(LastAuditDateYears).AddMonths(1);
            }
            else
            {
                var otherwiseDate = GetOtherwiseDate(contragent);
                if (otherwiseDate == null)
                {
                    return null;
                }
                reason = OtherwiseDateReason;
                planDate = otherwiseDate.Value.AddYears(OtherwiseDateYears).AddMonths(1);
            }

            return new NextAudit(lastAudit != null, reason, planDate.Year, (Month)planDate.Month);
        }

        /// <summary>
        ///     Получение другой даты, на которую следует опираться, если предыдущих проверок не было
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        protected abstract DateTime? GetOtherwiseDate(Contragent c);

        /// <summary>
        ///     Информация о следующей проверке (дата + причина)
        /// </summary>
        protected struct NextAudit
        {
            public NextAudit(bool fromLastAuditDate, string reason, int planYear, Month planMonth)
                : this()
            {
                FromLastAuditDate = fromLastAuditDate;
                Reason = reason;
                PlanYear = planYear;
                PlanMonth = planMonth;
            }

            /// <summary>
            ///     Плановый месяц
            /// </summary>
            public Month PlanMonth { get; private set; }

            /// <summary>
            ///     Плановый год
            /// </summary>
            public int PlanYear { get; private set; }

            /// <summary>
            ///     Причина
            /// </summary>
            public string Reason { get; private set; }

            /// <summary>
            ///     Признак расчета на основе даты прошлой проверки
            /// </summary>
            public bool FromLastAuditDate { get; private set; }
        }
    }
}