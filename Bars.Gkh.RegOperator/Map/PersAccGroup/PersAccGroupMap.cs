namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Маппинг сущности <see cref="PersAccGroup"/>
    /// </summary>
    public class PersAccGroupMap : BaseEntityMap<PersAccGroup>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PersAccGroupMap()
            : base("Группа лицевых счетов", "REGOP_PA_GROUP")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование группы").Column("NAME").NotNull().Length(100);
            this.Property(x => x.IsSystem, "Признак системности группы").Column("IS_SYSTEM").DefaultValue(20).NotNull().DefaultValue(YesNo.No);
        }
    }
}
