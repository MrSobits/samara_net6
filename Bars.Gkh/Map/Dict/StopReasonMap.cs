namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Класс-маппинг для сущности "Причина расторжения договора"
    /// </summary>
    public class StopReasonMap : BaseImportableEntityMap<StopReason>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public StopReasonMap()
            : base("Bars.Gkh.Entities.Dicts.CommunalResource", "GKH_DICT_STOP_REASON")
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
