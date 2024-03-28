namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности "Кабина лифта"
    /// </summary>
    public class CabinLiftMap : BaseImportableEntityMap<CabinLift>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CabinLiftMap()
            : base("Лифт (кабина)", "GKH_DICT_CABIN_LIFT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}