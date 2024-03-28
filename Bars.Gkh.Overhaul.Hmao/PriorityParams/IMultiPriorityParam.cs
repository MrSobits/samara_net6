namespace Bars.Gkh.Overhaul.Hmao.PriorityParams
{
    using System;
    using System.Collections.Generic;
    using Entities;

    public interface IMultiPriorityParam
    {
        string Id { get; }

        Type Type { get; }

        bool CheckContains(IStage3Entity obj, IEnumerable<StoredMultiValue> value);
    }
}