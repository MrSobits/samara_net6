namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Сведения по блокам жилого дома"</summary>
    public class RealityObjectBlockMap : BaseImportableEntityMap<RealityObjectBlock>
    {
        /// <summary>
        /// Маппинг
        /// </summary>
        public RealityObjectBlockMap() : 
                base("Сведения по помещениям жилого дома", "GKH_OBJ_BLOCK")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Number, "Number").Column("NUMBER");
            this.Property(x => x.AreaLiving, "Жилая площадь").Column("AREA_LIVING");
            this.Property(x => x.AreaTotal, "Общая площадь").Column("AREA_TOTAL");
            this.Property(x => x.CadastralNumber, "Кадастровый номер").Column("CADASTRAL_NUMBER");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
