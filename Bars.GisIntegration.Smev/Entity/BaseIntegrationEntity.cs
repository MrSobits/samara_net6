namespace Bars.GisIntegration.Smev.Entity
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;


    /// <summary>
    /// Базовая модель хранения информации для интеграции
    /// </summary>
    public class BaseIntegrationEntity: BaseEntity
    {
        /// <summary>
        /// Идентификатор сущности
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        public virtual string EntityType { get; set; }

        /// <summary>
        /// Сборка сущности
        /// </summary>
        public virtual string AssemblyType { get; set; }

        /// <summary>
        /// Guid сущности
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Получен ли ответ от реестра
        /// </summary>
        public virtual bool IsAnswered { get; set; }

        /// <summary>
        /// Задача
        /// </summary>
        public virtual RisTask Task { get; set; }
        
        /// <summary>
        /// Поле сущности
        /// </summary>
        public virtual string FieldName { get; set; }
    }
}