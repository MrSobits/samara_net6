using Bars.Gkh.Navigation;

namespace Bars.Gkh1468.Navigation
{
    using Bars.B4;

    public class RealityObj1468MenuProvider : INavigationProvider
    {
        public string Key { get { return RealityObjMenuKey.Key; } }
        public string Description { get { return RealityObjMenuKey.Description; } }

        public void Init(MenuItem root)
        {
			root.Add("Поставщики ресурсов", "realityobjectedit/{0}/publicservorg").AddRequiredPermission("Gkh.RealityObject.Register.PublicServOrg.View");
        }
    }
}