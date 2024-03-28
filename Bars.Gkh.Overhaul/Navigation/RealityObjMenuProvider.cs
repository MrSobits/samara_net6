namespace Bars.Gkh.Overhaul.Navigation
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
            root.Add("Конструктивные характеристики", "realityobjectedit/{0}/structelement").AddRequiredPermission("Gkh.RealityObject.Register.StructElem.View").WithIcon("icon-table-gear");
        }
    }
}