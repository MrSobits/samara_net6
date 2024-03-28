namespace Bars.Gkh.Overhaul.Nso.PriorityParams
{
    using System;

    public interface IQualitPriorityParam
    {
        string Id { get; }

        Type EnumType { get; }
    }
}