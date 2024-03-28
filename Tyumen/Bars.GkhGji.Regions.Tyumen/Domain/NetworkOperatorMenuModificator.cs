namespace Bars.GkhGji.Regions.Tyumen.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Navigation;

    public class NetworkOperatorMenuModificator : IMenuModificator
    {
        public string Key
        {
            get { return RealityObjMenuKey.Key; }
        }

        public IEnumerable<MenuItem> Modify(IEnumerable<MenuItem> menuItems)
        {
            var items = menuItems.ToList();
            var keIndex = items.FindIndex(x => x.Caption.ToLower() == "конструктивные характеристики");
            var noIndex = items.FindIndex(x => x.Caption.ToLower() == "операторы связи");
            if (keIndex < 0 || noIndex < 0)
            {
                return menuItems;
            }

            var noItem = items[noIndex];
            items.RemoveAt(noIndex);
            items.Insert(keIndex + 1, noItem);

            return items;
        }
    }
}