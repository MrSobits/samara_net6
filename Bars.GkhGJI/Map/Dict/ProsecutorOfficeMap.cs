namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;


    /// <summary>
    /// Маппинг для сущности "Отдел прокуратуры"
    /// </summary>
    public class ProsecutorOfficeMap : BaseEntityMap<ProsecutorOffice>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ProsecutorOfficeMap() :
                base("Отдел прокуратуры", "GJI_DICT_PROSECUTOR_OFFICE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE").Length(15);
            this.Property(x => x.ERKNMCode, "Код").Column("ERKNM_CODE").Length(15);
            this.Property(x => x.FederalCentersCode, "Код ФЦ").Column("FC_CODE").Length(5);
            this.Property(x => x.Name, "Полное наименование").Column("NAME").Length(500);
            this.Property(x => x.LocalAreasCode, "Код НП").Column("LA_CODE").Length(10);
            this.Property(x => x.RegionsCode, "Код регона").Column("REGION").Length(5);
            this.Reference(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY_ID").Fetch();

        }
    }
}