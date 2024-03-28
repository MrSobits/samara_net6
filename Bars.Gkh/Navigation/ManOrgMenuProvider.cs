namespace Bars.Gkh.Navigation
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
            root.Add("Общие сведения", "managingorganizationedit/{0}/edit").AddRequiredPermission("Gkh.Orgs.Managing.View").WithIcon("icon-shield-rainbow");
            root.Add("Муниципальные образования", "managingorganizationedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.Managing.View");
            root.Add("Жилые дома", "managingorganizationedit/{0}/realityObject").AddRequiredPermission("Gkh.Orgs.Managing.Register.RealityObject.View").WithIcon("icon-folder-home");
            root.Add("Управление домами", "managingorganizationedit/{0}/contract").AddRequiredPermission("Gkh.Orgs.Managing.Register.Contract.View").WithIcon("icon-building-key");
            root.Add("Документы", "managingorganizationedit/{0}/documentation").AddRequiredPermission("Gkh.Orgs.Managing.Register.Documentation.View").WithIcon("icon-paste-plain");
            root.Add("Режимы работы", "managingorganizationedit/{0}/workmode").AddRequiredPermission("Gkh.Orgs.Managing.Register.WorkMode.View").WithIcon("icon-time-green");
            root.Add("Членство в объединениях", "managingorganizationedit/{0}/membership").AddRequiredPermission("Gkh.Orgs.Managing.Register.Membership.View").WithIcon("icon-rosette-blue");
            root.Add("Прекращение деятельности", "managingorganizationedit/{0}/activity").AddRequiredPermission("Gkh.Orgs.Managing.Register.Activity.View").WithIcon("icon-cross");
        }
    }
}