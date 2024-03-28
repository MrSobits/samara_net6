using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions
{
    /// <summary>
    /// Базовый класс для связи сущности с Действием в рамках КНМ 
    /// </summary>
    public class KnmActionBundle : BaseEntity
    {
        /// <summary>
        /// Действие в рамках КНМ
        /// </summary>
        public virtual KnmAction KnmAction { get; set; }

        /// <summary>
        /// Вспомогательная сущность, для использования в обобщенном методе
        /// </summary>
        public virtual BaseEntity GkhGjiEntity { get; set; }
    }
}
