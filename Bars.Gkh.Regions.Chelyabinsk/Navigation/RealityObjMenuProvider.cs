namespace Bars.Gkh.Regions.Chelyabinsk.Navigation
{
    using Bars.B4;
    
    public class RealityObjMenuKey
    {
        public static string Key
        {
            get { return "RealityObj"; }
        }

        public static string Description
        {
            get
            {
                return "Меню карточки жилого дома";
            }
        }
    } 

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
            //root.Add("Сведения о квартирах", "realityobjectedit/{0}/apartinfo").AddRequiredPermission("Gkh.RealityObject.Register.ApartInfo.View").WithIcon("icon-key");
        }
    }
}
