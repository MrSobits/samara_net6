namespace Bars.Gkh.Regions.Nso.Navigation
{
    using Bars.B4;
    using Bars.Gkh.Navigation;

    public class RealityObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get
            {
                return RealityObjMenuKey.Key;
            }
        }

        public string Description
        {
            get
            {
                return RealityObjMenuKey.Description;
            }
        }

        public void Init(MenuItem root)
        {
            root.Add("Документы", "realityobjectedit/{0}/document").AddRequiredPermission("Gkh.RealityObject.Register.Document.View").WithIcon("icon-page-white-paste-table");
        }
    }
}