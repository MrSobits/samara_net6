namespace Bars.Gkh.Navigation
{
    using System.Collections.Generic;

    public interface ITreeMenuProvider
    {
        string Type { get; }

        IList<TreeMenuItem> GetMenuItems();
    }
}