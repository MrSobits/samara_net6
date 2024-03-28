namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для списка документов по экспорту в РИС</summary>
    public class SSTUExportedAppealMap : BaseEntityMap<SSTUExportedAppeal>
    {

        public SSTUExportedAppealMap() :
                base("Обращение экспортированное в ССТУ", "GJI_EXPORTED_APPEAL")
        {
        }

        protected override void Map()
        {
            Reference(x => x.AppealCits, "Обращение").Column("GJI_APPEAL_ID").NotNull().Fetch();
            Property(x => x.ExportDate, "Дата выгрузки").Column("EXPORT_DATE");
        }
    }
}
