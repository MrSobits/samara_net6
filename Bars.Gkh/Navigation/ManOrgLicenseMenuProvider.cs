namespace Bars.Gkh.Navigation
{
    using B4;

    public class ManOrgLicenseMenuProvider : INavigationProvider
    {
        public static string Key = "ManOrgLicense";

        string INavigationProvider.Key
        {
            get
            {
                return Key;
            }
        }

        public string Description
        {
            get
            {
                return "Меню карточки лицензии УО";
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Обращение", "manorglicense/{0}/{1}/editrequest").AddRequiredPermission("Gkh.ManOrgLicense.Request.View").WithIcon("icon-outline");
            root.Add("Лицензия", "manorglicense/{0}/{1}/editlicense").AddRequiredPermission("Gkh.ManOrgLicense.License.View");
        }
    }
}