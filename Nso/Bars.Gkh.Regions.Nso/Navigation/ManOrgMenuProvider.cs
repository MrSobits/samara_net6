namespace Bars.Gkh.Regions.Nso.Navigation
{
    using Bars.B4;

    public class ManOrgMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "ManOrg";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки управляющей организации";
            }
        }
        public void Init(MenuItem root)
        {
            root.Add("Реестры", "B4.controller.manorg.Registry").AddRequiredPermission("Gkh.Orgs.Managing.Register.Registry.View");
        }
    }
}
