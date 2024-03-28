namespace Bars.GkhGji.DomainService.SurveyPlan.Impl.Strategies
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    ///     В выборку по пятой цели попадают только Контрагенты со следующими ролями:
    ///     1. Управляющие организации, у которых Тип управления = УК
    /// </summary>
    public sealed class SurveyPlanStrategy05 : AbstractSurveyPlanStrategy
    {
        private readonly IDomainService<ManagingOrganization> _manOrgDomain;

        public SurveyPlanStrategy05(
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
                return "05";
            }
        }

        protected override string LastAuditDateReason
        {
            get
            {
                return
                    "Истечение 3-х лет со дня окончания проведения последней плановой проверки юридического лица, индивидуального предпринимателя";
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
                    "Истечение одного года со дня получения лицензии юридического лица, индивидуального предпринимателя";
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
                    _manOrgDomain.GetAll()
                                 .Any(y => y.Contragent.Id == x.Id && y.TypeManagement == TypeManagementManOrg.UK));
        }

        protected override DateTime? GetOtherwiseDate(Contragent c)
        {
            return c.LicenseDateReceipt;
        }
    }
}