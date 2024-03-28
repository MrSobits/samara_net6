namespace Bars.GkhGji.Navigation
{
    using B4;
    using Bars.B4.Application;
    using Bars.GkhGji.Contracts;

    public class DocumentsGjiRegisterMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "DocumentsGjiRegister";
            }
        }

        public string Description
        {
            get
            {
                return "Меню для документов ГЖИ";
            }
        }

        public void Init(MenuItem root)
        {

            var disposalsText = ApplicationContext.Current.Container.Resolve<IDisposalText>().SubjectiveManyCase;

            root.Add(disposalsText, "B4.controller.documentsgjiregister.Disposal").AddRequiredPermission("GkhGji.DocumentsGji.Disposal.View");
            root.Add("Решения", "B4.controller.documentsgjiregister.Decision").AddRequiredPermission("GkhGji.DocumentsGji.Decision.View");
            root.Add("Акты проверок", "B4.controller.documentsgjiregister.ActCheck").AddRequiredPermission("GkhGji.DocumentsGji.ActCheck.View");
            root.Add("Акты обследований", "B4.controller.documentsgjiregister.ActSurvey").AddRequiredPermission("GkhGji.DocumentsGji.ActSurvey.View");
            root.Add("Акты проверки предписаний", "B4.controller.documentsgjiregister.ActRemoval").AddRequiredPermission("GkhGji.DocumentsGji.ActRemoval.View");
            root.Add("Акты профилактического визита", "B4.controller.documentsgjiregister.PreventiveVisit").AddRequiredPermission("GkhGji.DocumentsGji.PreventiveVisit.View");
            root.Add("Предписания", "B4.controller.documentsgjiregister.Prescription").AddRequiredPermission("GkhGji.DocumentsGji.Prescription.View");
            root.Add("Протоколы", "B4.controller.documentsgjiregister.ProtocolGji").AddRequiredPermission("GkhGji.DocumentsGji.Protocol.View");
            root.Add("Постановления", "B4.controller.documentsgjiregister.Resolution").AddRequiredPermission("GkhGji.DocumentsGji.Resolution.View");
            root.Add("Представления", "B4.controller.documentsgjiregister.Presentation").AddRequiredPermission("GkhGji.DocumentsGji.Presentation.View");
            root.Add("Постановления Роспотребнадзора", "B4.controller.documentsgjiregister.ResolutionRospotrebnadzor").AddRequiredPermission("GkhGji.DocumentsGji.ResolutionRospotrebnadzor.View");
            root.Add(string.Format("{0} вне инспекторской деятельности", disposalsText), "B4.controller.documentsgjiregister.DisposalNullInspection").AddRequiredPermission("GkhGji.DocumentsGji.DisposalNullInspection.View");
        }
    }
}