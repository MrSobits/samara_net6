namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с действиями в рамках КНМ
    /// </summary>
    public class KnmActionService : IKnmActionService
    {
        private readonly IWindsorContainer container;

        public KnmActionService(IWindsorContainer container)
        {
            this.container = container;
        }
        
        /// <inheritdoc />
        public IDataResult AddDecisionPlannedActions(BaseParams baseParams) =>
            this.SaveKnmActionsForEntity<DecisionKnmAction, TatarstanDecision>(baseParams, "documentId");

        /// <inheritdoc />
        public IDataResult AddTaskActionIsolatedPlannedActions(BaseParams baseParams) =>
            this.SaveKnmActionsForEntity<TaskActionIsolatedKnmAction, TaskActionIsolated>(baseParams, "documentId");

        /// <summary>
        /// Сохранить сущности ссылающиеся на действие КНМ
        /// </summary>
        private IDataResult SaveKnmActionsForEntity<TEntity, TMainEntity>(BaseParams baseParams, string entityIdKey)
            where TEntity : BaseKnmActionMainEntityRef<TMainEntity>, new()
            where TMainEntity : BaseEntity, new()
        {
            var domainService = this.container.ResolveDomain<TEntity>();

            using (var transaction = this.container.Resolve<IDataTransaction>())
            using (this.container.Using(domainService))
            {
                try
                {
                    var entityId = baseParams.Params.GetAsId(entityIdKey);
                    var actionIds = baseParams.Params.GetAs<string>("actionId").ToLongArray();

                    var existsActions = domainService.GetAll()
                        .Where(x => x.MainEntity.Id == entityId)
                        .ToList();

                    existsActions
                        .Where(x => !actionIds.Contains(x.KnmAction.Id))
                        .ToList()
                        .ForEach(x => domainService.Delete(x.Id));

                    var saveActions = actionIds.Except(existsActions.Select(x => x.KnmAction.Id)).ToList();
                    saveActions.ForEach(x =>
                    {
                        var newTaskAction = new TEntity
                        {
                            MainEntity = new TMainEntity { Id = entityId },
                            KnmAction = new KnmAction { Id = x }
                        };

                        domainService.Save(newTaskAction);
                    });

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(false, e.Message);
                }

                return new BaseDataResult();
            }
        }
    }
}