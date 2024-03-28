namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Маппинг сущности <see cref="PersAccGroupRelation"/>
    /// </summary>
    public class PersAccGroupRelationMap : BaseEntityMap<PersAccGroupRelation>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public PersAccGroupRelationMap()
            : base("Сущность связи лицевого счета с группой лицевых счетов", "REGOP_PA_GROUP_RELATION")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Group, "Группа лицевых счетов").Column("GROUP_ID").NotNull().Fetch();
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("PA_ID").NotNull().Fetch();
        }
    }
}
