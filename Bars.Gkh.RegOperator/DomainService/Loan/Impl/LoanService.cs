namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System.IO;
    using System.Linq;

    using B4.Modules.Tasks.Common.Service;
    using B4;
    using B4.IoC;
    using B4.Modules.Analytics.Reports.Enums;
    using B4.Modules.Analytics.Reports.Generators;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent;
    using Bars.Gkh.RegOperator.DomainEvent.Events.RealityObjectLoan;
    using Bars.Gkh.RegOperator.Entities.Loan;

    using Castle.Windsor;
    using CodedReports.PayDoc;
    using DomainModelServices;
    using Tasks.Loans;

    /// <summary>
    /// Сервис займов
    /// </summary>
    public class LoanService : ILoanService
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        public LoanService(ITaskManager taskManager, ILoanReserver loanReserver, IWindsorContainer container)
        {
            this.taskManager = taskManager;
            this.loanReserver = loanReserver;
            this.container = container;
        }
        
        /// <summary>
        /// ручное взятие займа
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult TakeLoanManually(BaseParams baseParams)
        {
            return this.loanReserver.ReserveLoan(baseParams);
        }

        /// <summary>
        /// Автоматическое взятие займа
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult TakeLoanAutomatically(BaseParams baseParams)
        {
            var roIds = baseParams.Params.GetAs<long[]>("roIds");

            var realityObjectLoanTaskDomain = this.container.ResolveDomain<RealityObjectLoanTask>();
            using (this.container.Using(realityObjectLoanTaskDomain))
            {
                if (realityObjectLoanTaskDomain.GetAll().WhereContains(x => x.RealityObject.Id, roIds).Any())
                {
                    return BaseDataResult.Error("Процесс взятия займа по дому уже запущен. Повторное взятие займа запрещено.");
                }
            }

            var taskResult = this.taskManager.CreateTasks(new LoanTakerTaskProvider(), baseParams);

            var taskId = taskResult.Data.Descriptors.FirstOrDefault()?.TaskId ?? 0;
            if (taskId > 0)
            {
                foreach (var roId in roIds)
                {
                    DomainEvents.Raise(new RealityObjectLoanTaskStartEvent(new RealityObject { Id = roId }, new TaskEntry { Id = taskId }));
                }
            }

            return taskResult;
        }

        public Stream DownloadDisposal(BaseParams baseParams)
        {
            var loanDisposalReport = new LoanDisposal(baseParams.Params.GetAs<long>("Id"));
            var generator = this.container.Resolve<ICodedReportManager>();

            using (this.container.Using(generator))
            {
                return generator.GenerateReport(loanDisposalReport, null, ReportPrintFormat.docx); 
            }
        }

        private readonly ILoanReserver loanReserver;
        private readonly ITaskManager taskManager;
    }
}