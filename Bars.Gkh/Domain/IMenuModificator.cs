namespace Bars.Gkh.Domain
{
    using System.Collections.Generic;

    using Bars.B4;

    public interface IMenuModificator
    {
        string Key { get; }

        IEnumerable<MenuItem> Modify(IEnumerable<MenuItem> menuItems);
    }
}