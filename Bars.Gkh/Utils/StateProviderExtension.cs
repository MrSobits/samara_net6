namespace Bars.Gkh.Utils
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;

    public static class StateProviderExtension
    {
        /// <summary>
        /// Получить начальный статус для типа сущности
        /// </summary>
        public static State GetDefaultState(this IStateProvider provider, Type entityType)
        {
            var stateProvider = provider as StateProvider;
            if (stateProvider == null)
            {
                throw new NotImplementedException();
            }

            var repository = stateProvider.Container.Resolve<IRepository<State>>();
            using (stateProvider.Container.Using(repository))
            {
                if (!stateProvider.IsExistAnyState(entityType))
                {
                    var baseType = stateProvider.GetStatefulEntityInfoBase(entityType);
                    if (baseType != null)
                    {
                        return repository.GetAll()
                            .FirstOrDefault(x => x.TypeId == baseType.TypeId && x.StartState);
                    }
                }
                var entityInfo = stateProvider.GetStatefulEntityInfo(entityType);
                return repository.GetAll()
                    .FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.StartState);
            }
        }
    }
}