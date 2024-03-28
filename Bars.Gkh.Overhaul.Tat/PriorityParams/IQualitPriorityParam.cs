namespace Bars.Gkh.Overhaul.Tat.PriorityParams
{
    using System;

    public interface IQualitPriorityParam
    {
        string Id { get; }

        Type EnumType { get; }
    }
}