namespace Bars.Gkh.Decisions.Nso
{
    using System.Collections.Generic;
    using B4.Modules.States;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gkh_decision_notification", "Уведомление о принятии решения", typeof (DecisionNotification)),
                new StatefulEntityInfo("gkh_real_obj_dec_protocol", "Протокол решений", typeof(RealityObjectDecisionProtocol)),
                new StatefulEntityInfo("gkh_real_obj_gov_dec", "Протокол решений органа гос. власти", typeof(GovDecision)),

                new StatefulEntityInfo("gkh_real_obj_crfund_dec", "Протокол о формировании фонда капитального ремонта", typeof(CrFundDecisionProtocol)),
                new StatefulEntityInfo("gkh_real_obj_mkdorg_dec", "Протокол о выборе управляющей компании для дома", typeof(ManagementOrganizationDecisionProtocol)),
                new StatefulEntityInfo("gkh_real_obj_mkdmanage_type_dec", "Протокол о выборе формы управления многоквартирным домом", typeof(MkdManagementTypeDecisionProtocol)),
                new StatefulEntityInfo("gkh_real_obj_ooi_manage_dec", "Протокол по вопросам, связанных с эксплуатацией и управлением общим имуществом здания", typeof(OoiManagementDecisionProtocol)),
                new StatefulEntityInfo("gkh_real_obj_tariff_approval_dec", "Протокол об утверждение тарифа на содержание и ремонт жилья", typeof(TariffApprovalDecisionProtocol))
            };
        }
    }
}