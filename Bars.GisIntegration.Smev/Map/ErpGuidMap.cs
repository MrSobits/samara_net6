namespace Bars.GisIntegration.Smev.Map.Inspection
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Smev.Entity;

    /// <summary>
    /// Маппинг для <see cref="ErpGuid"/>
    /// </summary>
    public class ErpGuidMap : BaseEntityMap<ErpGuid>
    {
        public ErpGuidMap() : base("Bars.GkhGji.Entities", "GI_ERP_GUID") { }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.EntityId, "Идентификатор сущности").Column("ENTITY_ID");
            Property(x => x.EntityType, "Тип сущности").Column("ENTITY_TYPE");
            Property(x => x.AssemblyType, "Сборка сущности").Column("ASSEMBLY_TYPE");
            Property(x => x.Guid, "Гуид сущности").Column("GUID");
            Property(x => x.IsAnswered, "Получен ли ответ от ЕРП").Column("IS_ANSWERED");
            Property(x => x.FieldName, "Поле сущности").Column("FIELD_NAME");

            Reference(x => x.Task, "Задача").Column("TASK_ID").Fetch();
        }
    }
}