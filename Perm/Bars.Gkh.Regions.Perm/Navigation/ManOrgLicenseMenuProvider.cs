namespace Bars.Gkh.Regions.Perm.Navigation
{
    using Bars.B4;

    public class ManOrgLicenseMenuProvider : INavigationProvider
    {
        public static string Key = "ManOrgLicense";

        string INavigationProvider.Key => ManOrgLicenseMenuProvider.Key;

        public string Description => "Меню карточки лицензии УО";

        public void Init(MenuItem root)
        {
            root.Add("Заявления", "manorglicense/{0}/{1}/requestlist").AddRequiredPermission("Gkh.ManOrgLicense.Request.View").WithIcon("icon-outline");
            root.Add("Лицензия", "manorglicense/{0}/{1}/editlicense").AddRequiredPermission("Gkh.ManOrgLicense.License.View");
        }
    }
}