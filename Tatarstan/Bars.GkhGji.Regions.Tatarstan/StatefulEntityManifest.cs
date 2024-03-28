namespace Bars.GkhGji.Regions.Tatarstan
{
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanResolutionGji;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gji_document_taskdisp", "Документы ГЖИ - Задание", typeof(TaskDisposal)),
                new StatefulEntityInfo("gji_document_protocol_gji_rt", "Документы ГЖИ - Протокол ГЖИ РТ", typeof(TatarstanProtocolGjiContragent)),
                new StatefulEntityInfo("gji_document_resolution_gji_rt", "Документы ГЖИ - Постановление суда", typeof(TatarstanResolutionGji)),
                new StatefulEntityInfo("gji_document_task_actionisolated", "Документы ГЖИ - Задание КНМ без взаимодействия с контролируемыми лицами", typeof(TaskActionIsolated)),
                new StatefulEntityInfo("gji_document_preventive_action", "Документы ГЖИ - Профилактическое мероприятия", typeof(PreventiveAction)),
                new StatefulEntityInfo("gji_document_act_actionisolated", "Документы ГЖИ - Акт по КНМ без взаимодействия с контролируемыми лицами", typeof(ActActionIsolated)),
                new StatefulEntityInfo("gji_document_preventive_action_task", "Документы ГЖИ - Задание профилактического мероприятия", typeof(PreventiveActionTask)),
                new StatefulEntityInfo("gji_document_motivatedpresentation", "Документы ГЖИ - Мотивированное представление", typeof(MotivatedPresentation)),
                new StatefulEntityInfo("gji_document_visit_sheet", "Документы ГЖИ - Лист визита", typeof(VisitSheet)),
                new StatefulEntityInfo("gji_document_motivatedpresentation_appealcits", "Документы ГЖИ - Мотивированное представление по обращению гражданина", typeof(MotivatedPresentationAppealCits)),
                new StatefulEntityInfo("gji_rapid_response_system_appeal_details", "обращение в СОПР", typeof(RapidResponseSystemAppealDetails)),
                new StatefulEntityInfo("gji_tat_protocol", "Документы ГЖИ - Протокол для Татарстана", typeof(TatProtocol))
            };
        }
    }
}