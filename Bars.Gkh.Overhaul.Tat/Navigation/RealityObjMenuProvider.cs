namespace Bars.Gkh.Overhaul.Tat.Navigation
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
            root.Add("Протоколы и решения собственников", "realityobjectedit/{0}/ownerprotocol").AddRequiredPermission("Gkh.RealityObject.Register.OwnerProtocol.View");
        }
    }
}