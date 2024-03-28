namespace Sobits.RosReg.Tasks.ExtractParse
{
    using System;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Modules.Tasks.Common.Service;

    using Sobits.RosReg.Controllers;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class ExtractParseTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IDomainService<TaskEntry> TaskEntryDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; set; }

        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var xParseController = new ExtractParseController { Container = ApplicationContext.Current.Container };
                xParseController.AssignTypes();
                xParseController.Parse();
                xParseController.SetActive();

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}