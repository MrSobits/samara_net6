namespace Bars.Gkh.Domain
{
    using System.Collections.Generic;

    using Bars.B4;

    public class EmptyMenuModificator : IMenuModificator
    {
        public string Key
        {
            get { return string.Empty; }
        }

        public IEnumerable<MenuItem> Modify(IEnumerable<MenuItem> menuItems)
        {
            return menuItems;
        }
    }
}