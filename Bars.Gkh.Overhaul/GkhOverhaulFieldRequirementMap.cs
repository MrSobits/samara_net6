namespace Bars.Gkh.Overhaul
{
    using Bars.Gkh.DomainService;

    internal class GkhOverhaulFieldRequirementMap : FieldRequirementMap
    {
        public GkhOverhaulFieldRequirementMap()
        {
            this.Namespace("Gkh.RealityObject.StructElem", "Конструктивные характеристик");
            this.Namespace("Gkh.RealityObject.StructElem.Field", "Поля");

            this.Requirement("Gkh.RealityObject.StructElem.Field.LastOverhaulYear_Rqrd", "Год установки или последнего кап. ремонта");
            this.Requirement("Gkh.RealityObject.StructElem.Field.Wearout_Rqrd", "Износ (%)");
            this.Requirement("Gkh.RealityObject.StructElem.Field.Volume_Rqrd", "Объем");
        }
    }
}
