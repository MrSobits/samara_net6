namespace Bars.Gkh.Reforma.Navigation
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
            root.Add("Реформа ЖКХ", "managingorganizationedit/{0}/reformamanorg").AddRequiredPermission("Gkh.Orgs.Managing.Register.Reforma.View");
        }
    }
}