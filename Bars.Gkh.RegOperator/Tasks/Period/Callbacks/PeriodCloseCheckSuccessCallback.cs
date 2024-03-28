namespace Bars.Gkh.RegOperator.Tasks.Period.Callbacks
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class PeriodCloseCheckSuccessCallback : ITaskCallback
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;

        public PeriodCloseCheckSuccessCallback(IWindsorContainer container)
        {
            this.container = container;
        }

        public CallbackResult Call(long taskId, BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            var closeAfterCheck = @params.Params.GetAs<bool>("closeAfterCheck");
            if (closeAfterCheck)
            {
                var periodId = @params.Params.GetAsId("periodId");
                bool checkFailure;
                var resultDomain = this.container.ResolveDomain<PeriodCloseCheckResult>();
                try
                {
                    checkFailure = resultDomain.GetAll()
                        .Where(x => x.Period.Id == periodId)
                        .Where(x => x.IsCritical)
                        .Any(x => x.CheckState != PeriodCloseCheckStateType.Success);
                }
                finally
                {
                    this.container.Release(resultDomain);
                }

                if (!checkFailure)
                {
                    indicator.Report(null, 100, "Проверки пройдены. Запуск закрытия периода");

                    var closeService = this.container.Resolve<IChargePeriodCloseService>();
                    try
                    {
                        var res = closeService.CloseCurrentPeriod(new BaseParams { Params = new DynamicDictionary() });
                        indicator.Report(null, 100, !res.Success ? res.Message : "Проверки пройдены. Запущено закрытие периода");
                    }
                    finally
                    {
                        this.container.Release(closeService);
                    }
                }
            }

            return new CallbackResult(true);
        }
    }
}