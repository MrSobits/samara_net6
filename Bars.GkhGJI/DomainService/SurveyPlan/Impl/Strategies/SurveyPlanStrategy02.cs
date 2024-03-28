namespace Bars.GkhGji.DomainService.SurveyPlan.Impl.Strategies
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    ///     В выборку по второй цели попадают только Контрагенты со следующими ролями:
    ///     1. Органы местного самоуправления
    /// </summary>
    public sealed class SurveyPlanStrategy02 : AbstractSurveyPlanStrategy
    {
        private readonly IDomainService<LocalGovernment> _localGovernmentDomain;

        public SurveyPlanStrategy02(
            IDomainService<Contragent> contragentDomain,
            IDomainService<ContragentAuditPurpose> contragentAuditPurposeDomain,
            IDomainService<AuditPurposeGji> auditPurposeDomain,
            IDomainService<LocalGovernment> localGovernmentDomain)
            : base(contragentDomain, contragentAuditPurposeDomain, auditPurposeDomain)
        {
            _localGovernmentDomain = localGovernmentDomain;
        }

        public override string Code
        {
            get
            {
                return "02";
            }
        }

        protected override string LastAuditDateReason
        {
            get
            {
                return
                    "Истечение одного года со дня окончания проведения последней плановой проверки юридического лица, индивидуального предпринимателя";
            }
        }

        protected override int LastAuditDateYears
        {
            get
            {
                return 1;
            }
        }

        protected override string OtherwiseDateReason
        {
            get
            {
                return
                    "Истечение одного года со дня постановки на учет в муниципальном реестре наемных домов социального использования первого наемного"
                    + " дома социального использования, наймодателем жилых помещений в котором является лицо, деятельность которого подлежит проверке";
            }
        }

        protected override int OtherwiseDateYears
        {
            get
            {
                return 1;
            }
        }

        protected override IQueryable<Contragent> ApplyFilter(IQueryable<Contragent> query)
        {
            return
                query.Where(
                    x =>
                    _localGovernmentDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
        }

        protected override DateTime? GetOtherwiseDate(Contragent c)
        {
            return c.RegDateInSocialUse;
        }
    }
}