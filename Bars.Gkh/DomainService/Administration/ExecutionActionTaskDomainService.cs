namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;

    public class ExecutionActionTaskDomainService : BaseDomainService<ExecutionActionTask>
    {
        public IEnumerable<IDomainServiceInterceptor<ExecutionActionTask>> DomainServiceInterceptors { get; set; }
        public IExecutionActionService ExecutionActionService { get; set; }

        public override IDataResult Save(BaseParams baseParams)
        {
            var saveResult = base.Save(baseParams) as SaveDataResult;

            if (saveResult.Success)
            {
                var entityList = saveResult.Data as List<ExecutionActionTask>;
                var executionActionTask = entityList.First(x => x != null);
                try
                {
                    var createResult = this.ExecutionActionService.CreateTaskAction(executionActionTask);
                    if (!createResult.Success)
                    {
                        throw new Exception(createResult.Message);
                    }
                }
                catch
                {
                    executionActionTask.IsDelete = true;
                    this.Repository.Update(executionActionTask);

                    throw;
                }
            }

            return saveResult;
        }

        public override void Save(ExecutionActionTask task)
        {
            base.Save(task);

            this.ExecutionActionService.CreateTaskAction(task);
        }

        /// <inheritdoc />
        public override IDataResult Delete(BaseParams baseParams)
        {
            var idList = baseParams.Params.GetAs("records", new List<long>(), true);
            this.InTransaction(() =>
            {
                foreach (var id in idList)
                {
                    var task = this.Repository.Get(id);

                    if (task == null)
                    {
                        throw new ArgumentNullException(nameof(baseParams),
                            $@"Не найдена запись с идентификатором {id}");
                    }

                    IDataResult result = new BaseDataResult();

                    this.CallBeforeDeleteInterceptors(task, ref result, this.DomainServiceInterceptors);

                    task.IsDelete = true;

                    this.Repository.Update(task);

                    this.CallAfterDeleteInterceptors(task, ref result, this.DomainServiceInterceptors);
                }
            });

            return new BaseDataResult(idList);
        }
    }
}