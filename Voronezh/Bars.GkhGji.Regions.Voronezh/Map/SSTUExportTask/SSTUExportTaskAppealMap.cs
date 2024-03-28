namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для списка документов по экспорту в РИС</summary>
    public class SSTUExportTaskAppealMap : BaseEntityMap<SSTUExportTaskAppeal>
    {

        public SSTUExportTaskAppealMap() :
                base("Обращение в задаче экспорта в ССТУ", "GJI_SSTU_EXPORT_TASK_APPEAL")
        {
        }

        protected override void Map()
        {
            Reference(x => x.AppealCits, "Обращение").Column("GJI_APPEAL_ID").NotNull().Fetch();
            Reference(x => x.SSTUExportTask, "Задача").Column("GJI_SSTU_EXPORT_TASK_ID").NotNull().Fetch();
        }
    }
}
