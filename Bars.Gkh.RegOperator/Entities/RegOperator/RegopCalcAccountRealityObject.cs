namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Связка между расчетным счетом рег оператора и МКД
    /// </summary>
    public class RegopCalcAccountRealityObject : BaseImportableEntity
    {
        public virtual RegOpCalcAccount RegOpCalcAccount { get; set; }

        public virtual RealityObject RealityObject { get; set; }
    }
}