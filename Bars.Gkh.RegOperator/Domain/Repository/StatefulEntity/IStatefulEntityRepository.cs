namespace Bars.Gkh.RegOperator.Domain.Repository.StatefulEntity
{
    using B4.Modules.States;

    /// <summary>
    /// Репозиторий для получения статусов для объектоа со статусом
    /// </summary>
    public interface IStatefulEntityRepository
    {
        /// <summary>
        /// Получить статус по имени
        /// </summary>
        /// <typeparam name="TEntity">Тип объекта со статусом</typeparam>
        /// <param name="stateName">Имя статуса</param>
        /// <returns><see cref="State"/></returns>
        State GetStateByName<TEntity>(string stateName) where TEntity : IStatefulEntity;
    }
}