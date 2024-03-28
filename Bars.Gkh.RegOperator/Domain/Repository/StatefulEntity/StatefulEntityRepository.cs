namespace Bars.Gkh.RegOperator.Domain.Repository.StatefulEntity
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;

    public class StatefulEntityRepository : IStatefulEntityRepository
    {
        private readonly IStateProvider _stateProvider;
        private readonly IRepository<State> _stateRepo;

        public StatefulEntityRepository(
            IStateProvider stateProvider,
            IRepository<State> stateRepo)
        {
            _stateProvider = stateProvider;
            _stateRepo = stateRepo;
        }

        /// <summary>
        /// Получить статус по имени
        /// </summary>
        /// <typeparam name="TEntity">Тип объекта со статусом</typeparam>
        /// <param name="stateName">Имя статуса</param>
        /// <returns><see cref="State"/></returns>
        public State GetStateByName<TEntity>(string stateName) where TEntity : IStatefulEntity
        {
            var typeInfo = _stateProvider.GetStatefulEntityInfo(typeof (TEntity));

            return _stateRepo.GetAll()
                .Where(x => x.TypeId == typeInfo.TypeId)
                .FirstOrDefault(x => x.Name.ToLower() == stateName.ToLower());
        }
    }
}