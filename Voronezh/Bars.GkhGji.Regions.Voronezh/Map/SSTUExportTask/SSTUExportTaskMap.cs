namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для задач по экспорту в ССТУ</summary>
    public class SSTUExportTaskMap : BaseEntityMap<SSTUExportTask>
    {

        public SSTUExportTaskMap() :
                base("Экспорт в ССТУ", "GJI_SSTU_EXPORT_TASK")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Operator, "Оператор").Column("OPERATOR_ID").NotNull().Fetch();
            Reference(x => x.FileInfo, "Файл").Column("FILE_ID").Fetch();
            Property(x => x.SSTUExportState, "Статус").Column("SSTU_EXPORT_STATE").NotNull();
            Property(x => x.SSTUSource, "Источник").Column("SSTU_SOURCE").NotNull();
            this.Property(x => x.ExportExported, "Экспортировать только экспортированные Да/Нет").Column("EXPORTEXPORTED").NotNull();
        }
    }
}
