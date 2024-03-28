namespace Bars.Gkh.Overhaul.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Entities;

    public interface IRealityObjectTariffService
    {
        Dictionary<long, decimal> FillRealityObjectTariffDictionary();

        Dictionary<long, TarifInfoProxy> FillRealityObjectTariffInfoDictionary(IQueryable<RealityObject> roQuery, Dictionary<long, List<long>> newRoTypes = null);
    }

    public class TarifInfoProxy
    {
        public decimal Tarif { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public bool IsFromDecision { get; set; }
    }
}