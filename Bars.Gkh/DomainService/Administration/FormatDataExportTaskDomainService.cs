namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.FormatDataExport.Scheduler;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;

    using global::Quartz;

    public class FormatDataExportTaskDomainService : BaseDomainService<FormatDataExportTask>
    {
        public IEnumerable<IDomainServiceInterceptor<FormatDataExportTask>> DomainServiceInterceptors { get; set; }
        public IFormatDataExportSchedulerService FormatDataExportScheduler { get; set; }

        /// <inheritdoc />
        public override IDataResult Delete(BaseParams baseParams)
        {
            var idList = baseParams.Params.GetAs("records", new List<long>(), true);
            foreach (var id in idList)
            {
                var task = this.Repository.Get(id);

                if (task == null)
                {
                    throw new ArgumentNullException(nameof(baseParams),
                        $@"Не найдена запись с идентификатором {id}");
                }

                task.IsDelete = true;
                this.Repository.Update(task);
                IDataResult result =new BaseDataResult();

                this.CallAfterDeleteInterceptors(task, ref result, this.DomainServiceInterceptors);
            }

            return new BaseDataResult(idList);
        }

        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var result = base.Save(baseParams) as SaveDataResult;

                var savedTasks = result.Data as List<FormatDataExportTask>;

                try
                {
                    savedTasks.ForEach(x =>
                    {
                        this.FormatDataExportScheduler.ScheduleJob(new FormatDataExportJobInstance(x));
                    });
                    
                    return result;
                }
                catch (ObjectAlreadyExistsException e)
                {
                    return new SaveDataResult() { Success = false, Message = "Действие уже поставлено в очередь" };
                }
                catch (Exception e)
                {
                    return new SaveDataResult() { Success = false, Message = e.Message };
                }
        }
    }
}