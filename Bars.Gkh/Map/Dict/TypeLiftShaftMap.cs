namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности "Тип шахты лифта"
    /// </summary>
    public class TypeLiftShaftMap : BaseImportableEntityMap<TypeLiftShaft>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TypeLiftShaftMap()
            : base("Тип шахты лифта", "GKH_DICT_LIFT_SHAFT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}