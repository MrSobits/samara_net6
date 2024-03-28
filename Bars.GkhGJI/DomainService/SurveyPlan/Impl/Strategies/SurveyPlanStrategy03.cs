namespace Bars.GkhGji.DomainService.SurveyPlan.Impl.Strategies
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    ///     Третья цель может применяться только в отношении следующих типов юридических лиц:
    ///     1. Управляющие организации
    /// </summary>
    public sealed class SurveyPlanStrategy03 : AbstractSurveyPlanStrategy
    {
        private readonly IDomainService<ManagingOrganization> _manOrgDomain;

        public SurveyPlanStrategy03(
            IDomainService<Contragent> contragentDomain,
            IDomainService<ContragentAuditPurpose> contragentAuditPurposeDomain,
            IDomainService<AuditPurposeGji> auditPurposeDomain,
            IDomainService<ManagingOrganization> manOrgDomain)
            : base(contragentDomain, contragentAuditPurposeDomain, auditPurposeDomain)
        {
            _manOrgDomain = manOrgDomain;
        }

        public override string Code
        {
            get
            {
                return "03";
            }
        }

        protected override string LastAuditDateReason
        {
            get
            {
                return "Истечение 3-х лет со дня окончания проведения последней плановой проверки юридического лица";
            }
        }

        protected override int LastAuditDateYears
        {
            get
            {
                return 3;
            }
        }

        protected override string OtherwiseDateReason
        {
            get
            {
                return
                    "Истечение 3-х лет со дня государственной регистрации юридического лица, индивидуального предпринимателя";
            }
        }

        protected override int OtherwiseDateYears
        {
            get
            {
                return 3;
            }
        }

        protected override IQueryable<Contragent> ApplyFilter(IQueryable<Contragent> query)
        {
            return query.Where(x => _manOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
        }

        protected override DateTime? GetOtherwiseDate(Contragent c)
        {
            return c.DateRegistration;
        }
    }
}