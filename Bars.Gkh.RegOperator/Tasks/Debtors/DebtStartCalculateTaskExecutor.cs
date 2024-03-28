namespace Bars.Gkh.RegOperator.Tasks.Debtors
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;

    using Bars.B4.Application;
    using Bars.Gkh.RegOperator.Domain;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Выполняет задачу рассчета даты начала долга и суммы лога
    /// </summary>
    public class DebtStartCalculateTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }

        public IDataResult Execute(BaseParams baseParams,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            var container = ApplicationContext.Current.Container;
                ct.ThrowIfCancellationRequested();

            var lawsuitService = container.Resolve<ILawsuitOwnerInfoService>();
            
            indicator.Report(null, 5, "Начат расчет");
            var docId = baseParams.Params.GetAs<long>("docId");
            var recIds = baseParams.Params.GetAs<string>("recIds");
            var transfers = recIds.Split(',').Select(id => id.ToLong()).ToList();
            lawsuitService.CalcLegalWithReferenceCalc(docId, transfers);

            return new BaseDataResult();
        }
    }
}