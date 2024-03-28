namespace Bars.Gkh.Regions.Tyumen.Navigation
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Navigation;

    /// <summary>
    /// Замена провайдера меню для жилого дома в Тюмени
    /// </summary>
    public class RealityObjMenuProvider : INavigationProvider
    {
        public string Key
        {
            get { return RealityObjMenuKey.Key; }
        }

        public string Description
        {
            get { return RealityObjMenuKey.Description; }
        }

        public void Init(MenuItem root)
        {
            var structElementItem = root.Items.FirstOrDefault(x => x.Caption == "Конструктивные характеристики");
            if (structElementItem != null)
            {
                var index = root.Items.IndexOf(structElementItem);
                root.Items.Insert(
                    index,
                    new MenuItem("Лифты", "realityobjectedit/{0}/lift")
                    {
                        Icon = "icon-gkh-arrow-ns",
                        RequiredPermissions = new List<string> {"Gkh.RealityObject.Register.Lift.View"}
                    });
            }
        }
    }
}