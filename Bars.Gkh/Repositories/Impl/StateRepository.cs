namespace Bars.Gkh.Repositories.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;

    public class StateRepository : IStateRepository
    {
        private readonly IRepository<State> _stateRepo;
        private readonly IStateProvider _stateInfoProvider;

        public StateRepository(
            IRepository<State> stateRepo,
            IStateProvider stateInfoProvider)
        {
            _stateRepo = stateRepo;
            _stateInfoProvider = stateInfoProvider;
        }

        #region Implementation of IStateRepository

        /// <summary>
        /// Получить все статусы для опредленного типа
        /// </summary>
        /// <param name="predicate">Доп фильтр на выборку</param>
        public List<State> GetAllStates<TStateful>(Expression<Func<State, bool>> predicate = null) where TStateful : IStatefulEntity
        {
            var typeInfo = _stateInfoProvider.GetStatefulEntityInfo(typeof (TStateful));

            var result = _stateRepo.GetAll()
                .Where(x => x.TypeId == typeInfo.TypeId);

            if (predicate.IsNotNull())
            {
                result = result.Where(predicate);
            }

            return result.ToList();
        }

        /// <summary>
        /// Существуют ли статусы для переданного типа
        /// </summary>
        /// <param name="predicate">Доп фильтр на выборку</param>
        /// <typeparam name="TStateful">Тип объекта</typeparam>
        public bool AnyState<TStateful>(Expression<Func<State, bool>> predicate = null) where TStateful : IStatefulEntity
        {
            var typeInfo = _stateInfoProvider.GetStatefulEntityInfo(typeof(TStateful));

            var result = _stateRepo.GetAll()
                .Where(x => x.TypeId == typeInfo.TypeId);

            if (predicate.IsNotNull())
            {
                result = result.Where(predicate);
            }

            return result.Any();
        }

        /// <summary>
        /// Сохранить статус в БД
        /// </summary>
        /// <param name="state">Статус</param>
        public void Save(State state)
        {
            if (state.Id > 0)
            {
                _stateRepo.Update(state);
            }
            else
            {
                _stateRepo.Save(state);
            }
        }

        #endregion
    }
}