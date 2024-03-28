namespace Bars.GisIntegration.Base.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Базовая сущность для хранения объектов для интеграции с ГИС
    /// </summary>
    public class BaseRisEntity : BaseEntity
    {
        /// <summary>
        /// Id объекта в системе, из которой он был перемещен
        /// </summary>
        public virtual long ExternalSystemEntityId { get; set; }

        /// <summary>
        /// Наименование системы
        /// </summary>
        public virtual string ExternalSystemName { get; set; }

        /// <summary>
        /// Поставщик данных
        /// </summary>
        public virtual RisContragent Contragent { get; set; }

        /// <summary>
        /// Гуид, присвоенный объекту при загрузке в ГИС
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Операция, которую необходимо выполнить с записью в ГИС
        /// </summary>
        public virtual RisEntityOperation Operation  { get; set; }
}
}