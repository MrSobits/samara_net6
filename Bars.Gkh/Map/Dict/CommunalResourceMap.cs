namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Класс-маппинг для сущности "Коммунальный ресурс"
    /// </summary>
    public class CommunalResourceMap : BaseImportableEntityMap<CommunalResource>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public CommunalResourceMap()
            : base("Bars.Gkh.Entities.Dicts.CommunalResource", "GKH_DICT_COMM_RESOURCE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255);
            this.Property(x => x.Code, "Код").Column("CODE").Length(255);
        }
    }
}
