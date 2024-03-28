namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections.Generic;

    public interface IRealityObjectCollectionService
    {
        Dictionary<long, decimal> FillRealityObjectCollectionDictionary();
    }
}