namespace Bars.GkhGji.Regions.Tyumen.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Regions.Tyumen.Entities.Dicts;

    /// <summary>
    /// Связка оператор связи - дом - тех решение
    /// </summary>
    public class NetworkOperatorRealityObjectTechDecision : BaseEntity
    {
        public virtual NetworkOperatorRealityObject NetworkOperatorRealityObject { get; set; }

        public virtual TechDecision TechDecision { get; set; }
    }
}