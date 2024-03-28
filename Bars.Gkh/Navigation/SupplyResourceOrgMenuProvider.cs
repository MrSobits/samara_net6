namespace Bars.Gkh.Navigation
{
    using Bars.B4;

    public class SupplyResourceOrgMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return "SupplyResourceOrg";
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки поставщика коммунальных услуг";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Общие сведения", "supplyresorgedit/{0}/edit").AddRequiredPermission("Gkh.Orgs.SupplyResource.View").WithIcon("icon-shield-rainbow");
            root.Add("Муниципальные образования", "supplyresorgedit/{0}/municipality").AddRequiredPermission("Gkh.Orgs.SupplyResource.View");
            root.Add("Жилые дома", "supplyresorgedit/{0}/realobj").AddRequiredPermission("Gkh.Orgs.SupplyResource.RealtyObject.View").WithIcon("icon-folder-home");
            root.Add("Договора с жилыми домами", "supplyresorgedit/{0}/realobjcontract").AddRequiredPermission("Gkh.Orgs.SupplyResource.ContractsWithRealObj.View").WithIcon("icon-building-key");
            root.Add("Документы", "supplyresorgedit/{0}/document").AddRequiredPermission("Gkh.Orgs.SupplyResource.View").WithIcon("icon-paste-plain");
            root.Add("Услуги", "supplyresorgedit/{0}/service").AddRequiredPermission("Gkh.Orgs.SupplyResource.View").WithIcon("icon-rgb");
            root.Add("Прекращение деятельности", "supplyresorgedit/{0}/activity").AddRequiredPermission("Gkh.Orgs.SupplyResource.View").WithIcon("icon-cross"); 
        }
    }
}