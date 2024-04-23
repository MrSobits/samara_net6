namespace Bars.GkhGji
{
    using System.Collections.Generic;

    using Bars.B4.Application;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.SurveyPlan;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            var dispText = ApplicationContext.Current.Container.Resolve<IDisposalText>();

            return new[]
            {
                new StatefulEntityInfo("gji_document_disp", string.Format("Документы ГЖИ - {0}", dispText.SubjectiveCase), typeof(Disposal)),
                new StatefulEntityInfo("gji_document_decision", "Документы ГЖИ - Решение", typeof(Decision)),
                new StatefulEntityInfo("gji_document_profvizit", "Документы ГЖИ - Профвизит", typeof(PreventiveVisit)),
                new StatefulEntityInfo("gji_document_actcheck", "Документы ГЖИ - Акт проверки", typeof(ActCheck)),
                new StatefulEntityInfo("gji_document_actisolated", "Документы ГЖИ - Акт без взаимодействия", typeof(ActIsolated)),
                new StatefulEntityInfo("gji_document_actrem", "Документы ГЖИ - Акт устранения нарушений", typeof(ActRemoval)),
                new StatefulEntityInfo("gji_document_actsur", "Документы ГЖИ - Акт обследования", typeof(ActSurvey)),
                new StatefulEntityInfo("gji_document_prescr", "Документы ГЖИ - Предписание", typeof(Prescription)),
                new StatefulEntityInfo("prescription_official_report", "Предписание - Служебная записка", typeof(PrescriptionOfficialReport)),
                new StatefulEntityInfo("gji_document_prot", "Документы ГЖИ - Протокол", typeof(Protocol)),
                new StatefulEntityInfo("gji_document_presen", "Документы ГЖИ - Представление", typeof(Presentation)),
                new StatefulEntityInfo("gji_document_resol", "Документы ГЖИ - Постановление", typeof(Resolution)),
                new StatefulEntityInfo("gji_document_resolpros", "Документы ГЖИ - Постановление прокуратуры", typeof(ResolPros)),
                new StatefulEntityInfo("gji_document_resol_rosp", "Документы ГЖИ - Постановление Роспотребнадзора", typeof(ResolutionRospotrebnadzor)),
                new StatefulEntityInfo("gji_document_protocolmvd", "Документы ГЖИ - Протокол МВД", typeof(ProtocolMvd)),
                new StatefulEntityInfo("gji_document_protocolmhc", "Документы ГЖИ - Протокол МЖК", typeof(ProtocolMhc)),
                new StatefulEntityInfo("gji_document_protocolrso", "Документы ГЖИ - Протокол РСО", typeof(ProtocolRSO)),
                new StatefulEntityInfo("gji_document_warning", "Документы ГЖИ - Предостережение", typeof(WarningDoc)),
                new StatefulEntityInfo("gji_document_motivationconclusion", "Документы ГЖИ - Мотивировочное заключение", typeof(MotivationConclusion)),
                new StatefulEntityInfo("gji_heatseason_document", "Документ подготовки к отопительному сезону", typeof(HeatSeasonDoc)),
                new StatefulEntityInfo("gji_activity_tsj_statute", "Деятельность ТСЖ - Устав", typeof(ActivityTsjStatute)),
                new StatefulEntityInfo("gji_activity_tsj_member", "Деятельность ТСЖ - Реестр членов ТСЖ", typeof(ActivityTsjMember)),
                new StatefulEntityInfo("gji_appeal_citizens", "Обращение граждан", typeof(AppealCits)),
                new StatefulEntityInfo("mkdlicrequest", "Заявка на внесение изменений в реестр лицензий", typeof(MKDLicRequest)),
                new StatefulEntityInfo("gji_business_activity", "Уведомление о начале предпринимательской деятельности", typeof(BusinessActivity)),
                new StatefulEntityInfo("gji_inspection_planaction", "Проверка план мероприятий", typeof(BasePlanAction)),
                new StatefulEntityInfo("gji_appeal_cits_answer", "Ответ (обращение граждан)", typeof(AppealCitsAnswer)),

                // Базовые типы намеренно перенесены в самый низ, для корректного определения типа статуса для таких случаев как
                // наследование конкретного типа, например, OffspringDisposal : Disposal, чтобы OffspringDisposal смог получить 
                // начальный статус Disposal, а не начальный статус DocumentGji
             
                new StatefulEntityInfo("gji_document", "Документы ГЖИ", typeof(DocumentGji)),
                new StatefulEntityInfo("gji_inspection", "Основания проверок", typeof(InspectionGji)),

                new StatefulEntityInfo("gji_survey_plan", "План проверки ГЖИ", typeof(SurveyPlan))
            };
        }
    }
}