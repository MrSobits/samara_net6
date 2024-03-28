namespace Bars.Gkh.Overhaul.Tat.PriorityParams
{
    using System;

    public interface IMultiPriorityParam
    {
        string Id { get; }

        Type Type { get; }
    }
}