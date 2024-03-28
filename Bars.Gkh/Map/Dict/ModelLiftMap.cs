namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для сущности "Модель лифта"
    /// </summary>
    public class ModelLiftMap : BaseImportableEntityMap<ModelLift>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ModelLiftMap()
            : base("Модель лифта", "GKH_DICT_MODEL_LIFT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300);
        }
    }
}