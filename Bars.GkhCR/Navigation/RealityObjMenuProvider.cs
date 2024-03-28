namespace Bars.GkhCR.Navigation
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
            root.Add("Программы КР", "realityobjectedit/{0}/programcr").AddRequiredPermission("Gkh.RealityObject.Register.ProgramCr.View").WithIcon("icon-table-gear");
        }
    }
}