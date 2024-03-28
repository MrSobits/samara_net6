namespace Bars.GkhGji.DomainService.SurveyPlan
{
    using System;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.Dict;

    public class SurveyPlanCandidateProxy
    {
        public virtual AuditPurposeGji AuditPurpose { get; set; }

        public virtual Contragent Contragent { get; set; }

        public virtual int PlanYear { get; set; }

        public virtual Month PlanMonth { get; set; }

        public virtual string Reason { get; set; }

        public virtual bool FromLastAuditDate { get; set; }
    }
}