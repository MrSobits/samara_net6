namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Переведенные названия логируемых полей сущности
    /// </summary>
    public class EntityHistoryTranslatedProperties : PersistentObject
    {
        /// <summary>
        /// Тип сущности
        /// </summary>
        public virtual string EntityType { get; set; }

        /// <summary>
        /// Английское название(PropertyCode)
        /// </summary>
        public virtual string EnglishName { get; set; }

        /// <summary>
        /// Русское название
        /// </summary>
        public virtual string RussianName { get; set; }
    }
}