namespace Bars.GkhGji.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Entities;

    public class InspectionBaseContragentMap : GkhBaseEntityMap<InspectionBaseContragent>
    {
        public InspectionBaseContragentMap()
            : base("GJI_INSPECTION_BASE_CONTRAGENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.InspectionGji, "Органы совместной проверки").Column("INSPECTION_ID").NotNull();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull();
        }
    }
}