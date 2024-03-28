namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг сущности ContentRepairMkdWork
    /// </summary>
    public class ContentRepairMkdWorkMap : BaseImportableEntityMap<ContentRepairMkdWork>
    {
        /// <summary>
        /// Конструктор маппинга сущности ContentRepairMkdWork
        /// </summary>
        public ContentRepairMkdWorkMap()
            : base("Работы по содержанию и ремонту МКД", "GKH_DICT_CONTENT_REPAIR_MKD_WORK")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            Reference(x => x.Work, "Вид работы").Column("WORK_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Property(x => x.Code, "Код").Column("CODE").Length(10);          
        }
    }
}
