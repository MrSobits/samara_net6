namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности "Тип приводов дверей кабины"
    /// </summary>
    public class TypeLiftDriveDoorsMap : BaseImportableEntityMap<TypeLiftDriveDoors>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TypeLiftDriveDoorsMap()
            : base("Типы приводов дверей кабины лифта", "GKH_DICT_DRIVE_DOORS")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}