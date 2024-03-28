namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Extensions;

    public class ActCheckActionDomainService : BaseDomainService<ActCheckAction>
    {
        /// <summary>
        /// Провайдер сессии Hibernate
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Сервис прокси Hibernate
        /// </summary>
        public IUnProxy UnProxy { get; set; }

        /// <inheritdoc />
        protected override void SaveEntityInternal(ActCheckAction entity)
        {
            var actionType = entity.ActionType.GetEntityClassType();
            ActCheckAction action = null;

            if (actionType == null)
            {
                throw new Exception($"Сохранение для действия с типом '{entity.ActionType.GetDisplayName()}' не реализовано");
            }

            if (entity.PrototypeId > 0 && entity.PrototypeActionType.HasValue)
            {
                var prototypeType = entity.PrototypeActionType.Value.GetEntityClassType();

                if (prototypeType == null)
                {
                    throw new Exception($"Сохранение на основе действия с типом '{entity.PrototypeActionType.GetDisplayName()}' не реализовано");
                }
                
                var prototypeRepository = this.GetGenericRepository(prototypeType);

                using (this.Container.Using(prototypeRepository))
                {
                    var prototype = prototypeRepository.Get(entity.PrototypeId);
                    action = (ActCheckAction)Activator.CreateInstance(actionType, prototype);

                    if (action.ExecutionPlace != null)
                    {
                        action.ExecutionPlace = (FiasAddress)this.UnProxy.GetUnProxyObject(action.ExecutionPlace);
                        SessionProvider.GetCurrentSession().Evict(action.ExecutionPlace);
                        action.ExecutionPlace.Id = 0;
                        Utils.SaveFiasAddress(this.Container, action.ExecutionPlace);
                    }

                    action.Date = entity.Date;
                    action.CreationPlace = entity.CreationPlace;
                    action.ActionType = entity.ActionType;
                }
            }
            else
            {
                action = (ActCheckAction)Activator.CreateInstance(actionType, entity);
            }

            var actionRepository = this.GetGenericRepository(actionType);

            using (this.Container.Using(actionRepository))
            {
                actionRepository.Save(action);
                entity.Id = action.Id;
                this.AttachPrototypeInspectors(entity);
            }
        }

        /// <summary>
        /// Получить обобщенный репозиторий
        /// </summary>
        /// <param name="entityType">Тип действия</param>
        /// <returns></returns>
        private IRepository GetGenericRepository(Type entityType)
        {
            var resolveMethod = this.Container.GetType().GetMethod(nameof(this.Container.Resolve), Type.EmptyTypes);
            var genericMethod = resolveMethod.MakeGenericMethod(typeof(IRepository<>).MakeGenericType(entityType));
            return (IRepository)genericMethod.Invoke(this.Container, null);
        }
        
        /// <summary>
        /// Привязка инспекторов прототипа
        /// </summary>
        /// <param name="entity">Действие</param>
        private void AttachPrototypeInspectors(ActCheckAction entity)
        {
            if (entity.PrototypeId == 0)
            {
                return;
            }
            
            var actionInspectorDomain = this.Container.ResolveDomain<ActCheckActionInspector>();

            using (this.Container.Using(actionInspectorDomain))
            {
                actionInspectorDomain.GetAll()
                    .Where(x => x.ActCheckAction.Id == entity.PrototypeId)
                    .Select(x => x.Inspector.Id)
                    .AsEnumerable()
                    .ForEach(x => actionInspectorDomain.Save(new ActCheckActionInspector()
                    {
                        ActCheckAction = entity,
                        Inspector = new Inspector(){Id = x}
                    }));
            }
        }
    }
}