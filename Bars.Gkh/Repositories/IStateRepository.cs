namespace Bars.Gkh.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Репозиторий для получения различных данных по статусам
    /// </summary>
    public interface IStateRepository
    {
        /// <summary>
        /// Получить все статусы для опредленного типа
        /// </summary>
        /// <param name="predicate">Доп фильтр на выборку</param>
        List<State> GetAllStates<TStateful>(Expression<Func<State, bool>> predicate = null) where TStateful : IStatefulEntity;

        /// <summary>
        /// Существуют ли статусы для переданного типа
        /// </summary>
        /// <param name="predicate">Доп фильтр на выборку</param>
        /// <typeparam name="TStateful">Тип объекта</typeparam>
        bool AnyState<TStateful>(Expression<Func<State, bool>> predicate = null) where TStateful : IStatefulEntity;

        /// <summary>
        /// Сохранить статус в БД
        /// </summary>
        /// <param name="state">Статус</param>
        void Save(State state);
    }
}