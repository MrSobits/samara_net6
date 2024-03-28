namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Доп. Работы"</summary>
    public class AdditWorkMap : BaseImportableEntityMap<AdditWork>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public AdditWorkMap() :
                base("Работы", "GKH_DICT_ADDIT_WORK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.Code, "Код").Column("CODE").Length(10);
            this.Property(x => x.Queue, "Очередность").Column("QUEUE").Length(2);
            this.Property(x => x.Percentage, "Проценты").Column("PERCENTAGE");
            this.Reference(x => x.Work, "Работа").Column("WORK_ID").NotNull();
        }
    }
}
