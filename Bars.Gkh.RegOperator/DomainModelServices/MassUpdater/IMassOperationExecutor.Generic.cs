namespace Bars.Gkh.RegOperator.DomainModelServices.MassUpdater
{
    using Bars.B4.DataAccess;
    /// <summary>
    /// Интерфейс для сервиса, поддерживающего массовую обработку
    /// </summary>
    public interface IMassOperationExecutor<in TEntity> : IMassOperationExecutor where TEntity : PersistentObject
    {
        /// <summary>
        /// Добавить сущность для изменения
        /// </summary>
        void AddEntity(TEntity entity);
    }
}