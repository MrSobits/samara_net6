namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    /// <inheritdoc />
    public class TaskActionIsolatedRealityObjectService : ITaskActionIsolatedRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            try
            {
                var taskActionId = baseParams.Params.GetAsId("documentId");
                var objectIds = baseParams.Params.GetAs<long[]>("roIds");
                
                IDomainService<RealityObject> realityObjectDomain;
                IDomainService<TaskActionIsolated> taskActionDomain;
                IDomainService<TaskActionIsolatedRealityObject> taskActionRoService;

                using (this.Container.Using(
                    taskActionDomain = this.Container.ResolveDomain<TaskActionIsolated>(),
                    realityObjectDomain = this.Container.ResolveDomain<RealityObject>(),
                    taskActionRoService = this.Container.Resolve<IDomainService<TaskActionIsolatedRealityObject>>()))
                {
                    var taskAction = taskActionDomain.Get(taskActionId);

                    if (taskAction.IsNotNull())
                    {
                        var realityObjects = realityObjectDomain.GetAll()
                            .Where(x => objectIds.Contains(x.Id));

                        var existsRealityObjects = taskActionRoService.GetAll()
                            .Where(x => x.Task == taskAction && realityObjects.Contains(x.RealityObject))
                            .Select(x => x.RealityObject);

                        foreach (var ro in realityObjects.Where(x => !existsRealityObjects.Contains(x)))
                        {
                            var taskActionRo = new TaskActionIsolatedRealityObject
                            {
                                Task = taskAction,
                                RealityObject = ro
                            };

                            taskActionRoService.Save(taskActionRo);
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}