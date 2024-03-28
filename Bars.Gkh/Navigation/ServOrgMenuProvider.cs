namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class ServOrgMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "ServOrg";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки поставщика жилищных услуг";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "serviceorganizationedit/{0}/edit").AddRequiredPermission("Gkh.Orgs.Serv.View").WithIcon("icon-shield-rainbow");
            root.Add("Муниципальные образования", "serviceorganizationedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.Serv.View");
            root.Add("Жилые дома", "serviceorganizationedit/{0}/realobj").AddRequiredPermission("Gkh.Orgs.Serv.RealtyObject.View").WithIcon("icon-folder-home");
            root.Add("Договора с жилыми домами", "serviceorganizationedit/{0}/realobjcontract").AddRequiredPermission("Gkh.Orgs.Serv.RealityObjectContract.View").WithIcon("icon-building-key");
            root.Add("Документы", "serviceorganizationedit/{0}/document").AddRequiredPermission("Gkh.Orgs.Serv.View").WithIcon("icon-paste-plain");
            root.Add("Услуги", "serviceorganizationedit/{0}/service").AddRequiredPermission("Gkh.Orgs.Serv.View").WithIcon("icon-rgb");
            root.Add("Прекращение деятельности", "serviceorganizationedit/{0}/activity").AddRequiredPermission("Gkh.Orgs.Serv.View").WithIcon("icon-cross");
        }
    }
}