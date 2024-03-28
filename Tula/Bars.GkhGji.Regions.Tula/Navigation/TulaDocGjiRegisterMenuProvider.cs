namespace Bars.GkhGji.Navigation
{
    using B4;

    public class TulaDocGjiRegisterMenuProvider : INavigationProvider
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
            root.Add("Акты осмотра", "B4.controller.documentsgjiregister.ActView").AddRequiredPermission("GkhGji.DocumentsGji.ActView.View");
        }
    }
}