namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности "Тип лифта"
    /// </summary>
    public class TypeLiftMap : BaseImportableEntityMap<TypeLift>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TypeLiftMap()
            : base("Тип лифта", "GKH_DICT_TYPE_LIFT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}