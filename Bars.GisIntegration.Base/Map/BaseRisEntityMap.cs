namespace Bars.GisIntegration.Base.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Базовый маппинг для BaseRisEntity-сущностей
    /// </summary>
    public abstract class BaseRisEntityMap<TBaseEntity> : BaseEntityMap<TBaseEntity> where TBaseEntity : BaseRisEntity
    {
        /// <summary>
        /// Конструктор типа BaseRisEntityMap
        /// </summary>
        /// <param name="entityName">Название сущности</param>
        /// <param name="tableName">Имя таблицы</param>
        protected BaseRisEntityMap(string entityName, string tableName)
            : base(entityName, tableName)
        {
        }

        public override void InitMap()
        {
            base.InitMap();

            this.Property(x => x.ExternalSystemEntityId, "Id объекта в системе, из которой он был перемещен").Column("EXTERNAL_ID").NotNull();
            this.Property(x => x.ExternalSystemName, "Наименование системы").Column("EXTERNAL_SYSTEM_NAME").Length(50);          
            this.Reference(x => x.Contragent, "Поставщик данных").Column("GI_CONTRAGENT_ID").Fetch();
            this.Property(x => x.Guid, "Гуид, присвоенный объекту при загрузке в ГИС").Column("GUID").Length(50);
            this.Property(x => x.Operation, "Операция, которую необходимо выполнить с записью в ГИС").Column("OPERATION");
        }
    }
}