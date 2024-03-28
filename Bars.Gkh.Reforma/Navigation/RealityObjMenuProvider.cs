namespace Bars.Gkh.Reforma.Navigation
{
    using Bars.B4;
    using Bars.Gkh.Navigation;

    public class RealityObjMenuProvider : INavigationProvider
    {
        #region Public Properties

        public string Description
        {
            get
            {
                return RealityObjMenuKey.Description;
            }
        }

        public string Key
        {
            get
            {
                return RealityObjMenuKey.Key;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void Init(MenuItem root)
        {
            root.Add("Реформа ЖКХ", "realityobjectedit/{0}/reforma").AddRequiredPermission("Gkh.RealityObject.Register.Reforma.View");
        }

        #endregion
    }
}