namespace Bars.GkhGji.Regions.Tatarstan
{
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Enums;

    public class DocumentsGjiRegisterMenuProvider : INavigationProvider
    {
        public string Key => "DocumentsGjiRegister";

        public string Description => "Меню для документов ГЖИ"; 

        public void Init(MenuItem root)
        {
            var disposalsText = ApplicationContext.Current.Container.Resolve<IDisposalText>().SubjectiveManyCase;

            var disposalMenuItem = root.Add(disposalsText, "B4.controller.documentsgjiregister.TatDisposal").AddRequiredPermission("GkhGji.DocumentsGji.Disposal.View");
            disposalMenuItem.Options.Add("typeDocumentGji", TypeDocumentGji.Disposal);

            var decisionMenuItem = root.Add("Решения", "B4.controller.documentsgjiregister.Decision").AddRequiredPermission("GkhGji.DocumentsGji.Disposal.View");
            decisionMenuItem.Options.Add("typeDocumentGji", TypeDocumentGji.Decision);

            root.Add("Акты проверок", "B4.controller.documentsgjiregister.ActCheck").AddRequiredPermission("GkhGji.DocumentsGji.ActCheck.View");
            root.Add("Акты обследований", "B4.controller.documentsgjiregister.ActSurvey").AddRequiredPermission("GkhGji.DocumentsGji.ActSurvey.View");
            root.Add("Акты проверки предписаний", "B4.controller.documentsgjiregister.ActRemoval").AddRequiredPermission("GkhGji.DocumentsGji.ActRemoval.View");
            root.Add("Предписания", "B4.controller.documentsgjiregister.Prescription").AddRequiredPermission("GkhGji.DocumentsGji.Prescription.View");
            root.Add("Протоколы", "B4.controller.documentsgjiregister.ProtocolGji").AddRequiredPermission("GkhGji.DocumentsGji.Protocol.View");
            root.Add("Постановления", "B4.controller.documentsgjiregister.Resolution").AddRequiredPermission("GkhGji.DocumentsGji.Resolution.View");
            root.Add("Представления", "B4.controller.documentsgjiregister.Presentation").AddRequiredPermission("GkhGji.DocumentsGji.Presentation.View");
            root.Add("Постановления Роспотребнадзора", "B4.controller.documentsgjiregister.ResolutionRospotrebnadzor").AddRequiredPermission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.View");
            root.Add($"{disposalsText} вне инспекторской деятельности", "B4.controller.documentsgjiregister.DisposalNullInspection").AddRequiredPermission("GkhGji.DocumentsGji.DisposalNullInspection.View");
            root.Add("Предостережения", "B4.controller.documentsgjiregister.WarningDoc").AddRequiredPermission("GkhGji.DocumentsGji.WarningInspection.View");
            root.Add("Мотивировочное заключение", "B4.controller.documentsgjiregister.MotivationConclusion").AddRequiredPermission("GkhGji.DocumentsGji.MotivationConclusion.View");
            root.Add("Акты без взаимодействия", "B4.controller.documentsgjiregister.ActIsolated").AddRequiredPermission("GkhGji.DocumentsGji.ActIsolated.View");
            root.Add("Акты по КНМ без взаимодействия", "B4.controller.documentsgjiregister.ActKnmIsolated");
            root.Add("Задания по КНМ", "B4.controller.documentsgjiregister.TaskAction");
            root.Add("Профилактические мероприятия", "B4.controller.documentsgjiregister.PreventiveAction");
            root.Add("Задания по профилактическим мероприятиям", "B4.controller.documentsgjiregister.PreventiveActionTask");
            root.Add("Лист визита", "B4.controller.documentsgjiregister.VisitSheet");
            root.Add("Мотивированное представление по обращениям граждан", "B4.controller.documentsgjiregister.MotivatedPresentationAppealCits");
            root.Add("Мотивированные представления", "B4.controller.documentsgjiregister.MotivatedPresentation");
        }
    }
}