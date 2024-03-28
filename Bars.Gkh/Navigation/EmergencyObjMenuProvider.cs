namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class EmergencyObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "EmergencyObj";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки аварийного дома";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Сведения о доме", "B4.controller.emergencyobj.Edit").AddRequiredPermission("Gkh.EmergencyObject.View");
            root.Add("Разрезы финансирования", "B4.controller.emergencyobj.ResettlementProgram").AddRequiredPermission("Gkh.EmergencyObject.View");
        }
    }
}