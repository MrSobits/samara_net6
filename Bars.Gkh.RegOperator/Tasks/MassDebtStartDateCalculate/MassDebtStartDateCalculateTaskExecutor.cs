namespace Bars.Gkh.RegOperator.Tasks.UnacceptedPayment
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using B4;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Domain;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class MassDebtStartDateCalculateTaskExecutor : ITaskExecutor
    {
        public static readonly string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        public string ExecutorCode { get; private set; }

        private ILawsuitOwnerInfoService _LawsuitOwnerInfoService;
        public IDomainService<Lawsuit> LawsuitDomain { get; set; }

        public MassDebtStartDateCalculateTaskExecutor(ILawsuitOwnerInfoService lawsuitOwnerInfoService)
        {
            _LawsuitOwnerInfoService = lawsuitOwnerInfoService;
        }

        #region Implementation of ITaskExecutor

        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
             var clwIds = @params.Params.GetAs<long[]>("clwIds", ignoreCase: true);

            StringBuilder log = new StringBuilder();
            try
            {
                if (clwIds.IsEmpty())
                {
                    return new BaseDataResult(false, "Не выбраны дела");
                }

                long[] lawsuitIds = LawsuitDomain.GetAll().Where(x => clwIds.Contains(x.ClaimWork.Id)).Select(x => x.Id).ToArray();
                foreach (var lawsuitId in lawsuitIds)
                {
                    var baseParams = new BaseParams();
                    baseParams.Params.Add("docId", lawsuitId);
                    try
                    {
                        var result = _LawsuitOwnerInfoService.DebtStartDateCalculate(baseParams);
                        if (!result.Success)
                        {
                            log.AppendLine($"lawsuitId: {lawsuitId}, message: {result.Message};");
                        };
                    }
                    catch (Exception e)
                    {
                        log.AppendLine($"lawsuitId: {lawsuitId}, message: {e.Message}; stacktrace: {e.StackTrace};");
                    }
                }
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(log + " message: {0} {1}\r\n stacktrace: {2}".FormatUsing(e.Message, e.InnerException, e.StackTrace));
            }
            if (log.Length == 0)
            {
                return new BaseDataResult();
            }
            return BaseDataResult.Error(log.ToString());
        }

        #endregion
    }
}