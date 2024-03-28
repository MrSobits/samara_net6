namespace Bars.Gkh.Utils.Caching
{
    using Bars.B4;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Интерфейс кэшируемого представления
    /// </summary>
    /// <typeparam name="TEntity">Тип кэшируемой сущности</typeparam>
    public interface ICacheableViewModel<TEntity> : IViewModel<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Сделать кэш для невалидным
        /// </summary>
        void InvalidateCache();
    }
}