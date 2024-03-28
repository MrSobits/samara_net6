namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Базовый класс ссылки сущности на действие КНМ
    /// </summary>
    /// <typeparam name="TMainEntity">Тип ссылающейся сущности</typeparam>
    public class BaseKnmActionMainEntityRef<TMainEntity> : BaseEntity
        where TMainEntity : BaseEntity
    {
        /// <summary>
        /// Действие в рамках КНМ
        /// </summary>
        public virtual KnmAction KnmAction { get; set; }

        /// <summary>
        /// Основная сущность
        /// </summary>
        public virtual TMainEntity MainEntity { get; set; }
    }
}