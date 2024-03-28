using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
using Castle.Windsor;
using System;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl
{
    /// <inheritdoc />
    public class PreventiveActionTaskService : IPreventiveActionTaskService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult List(BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var isExport = baseParams.Params.GetAs<bool>("isExport");
            var loadParams = baseParams.GetLoadParam();

            var preventiveActionTaskDomain = this.Container.Resolve<IDomainService<PreventiveActionTask>>();
            var preventiveActionDomain = this.Container.Resolve<IDomainService<Entities.PreventiveAction.PreventiveAction>>();
            var linkDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();

            using (this.Container.Using(preventiveActionTaskDomain, preventiveActionDomain, linkDomain))
            {
                var data = preventiveActionTaskDomain.GetAll()
                    .Join(linkDomain.GetAll(),
                        x => x.Id,
                        y => y.Children.Id,
                        (x, y) => new
                        {
                            PreventiveAction = y.Parent,
                            Task = x
                        })
                    .Join(preventiveActionDomain.GetAll(),
                        x => x.PreventiveAction.Id,
                        y => y.Id,
                        (x, y) => new
                        {
                            x.Task,
                            PreventiveAction = y
                        })
                    .WhereIf(dateStart.HasValue, x => x.Task.ActionStartDate >= dateStart)
                    .WhereIf(dateEnd.HasValue, x => x.Task.ActionStartDate <= dateEnd)
                    .Select(x => new
                    {
                        x.Task.Id,
                        x.Task.State,
                        Municipality = x.PreventiveAction.Municipality != null
                            ? x.PreventiveAction.Municipality.Name
                            : null,
                        x.Task.ActionType,
                        x.Task.VisitType,
                        x.Task.DocumentNumber,
                        x.Task.DocumentDate,
                        x.Task.ActionStartDate,
                        x.Task.Inspection.TypeBase,
                        x.Task.TypeDocumentGji,
                        InspectionId = x.Task.Inspection.Id,
                        Executor = x.Task.Executor != null
                            ? x.Task.Executor.Fio
                            : null
                    })
                .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                var orderField = loadParams.Order.FirstOrDefault(x => x.Name == "State");

                data = orderField != null
                    ? orderField.Asc
                        ? data.OrderBy(x => x.State.Code)
                        : data.OrderByDescending(x => x.State.Code)
                    : data.Order(loadParams);

                var resultData = isExport ? data : data.Paging(loadParams);

                return new ListDataResult(resultData.ToList(), totalCount);
            }
        }
    }
}