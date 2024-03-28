namespace Bars.GkhGji.Entities
{
    using B4.DataAccess;
    using Enums;

    public class EntityOldValueHistory : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual long EntityId { get; set; }

        /// <summary>
        /// Код вида проверки
        /// </summary>
        public virtual TypeEntityLogging TypeEntityLogging { get; set; }

        /// <summary>
        /// Код правила
        /// </summary>
        public virtual byte[] OldValue { get; set; }
    }
}