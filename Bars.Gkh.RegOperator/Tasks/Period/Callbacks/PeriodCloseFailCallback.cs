namespace Bars.Gkh.RegOperator.Tasks.Period.Callbacks
{
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;

    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Tasks.Charges;

    using Castle.Windsor;
    using Domain.ValueObjects;
    using Entities;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class PeriodCloseFailCallback : ITaskCallback
    {
        private readonly IWindsorContainer _container;
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public PeriodCloseFailCallback(IWindsorContainer container)
        {
            _container = container;
        }

        #region Implementation of ITaskCallback

        public CallbackResult Call(long taskId,
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            IPeriod periodDto = @params.Params.GetAs<PeriodDto>("period");
            if (periodDto != null)
            {
                var repo = _container.ResolveRepository<ChargePeriod>();
                using (_container.Using(repo))
                {
                   var  period = repo.Get(periodDto.Id);
                    period.IsClosing = false;

                    repo.Update(period);
                }
            }

            var cfg = @params.Params.GetAs<OperationLockConfig>("operationLock");
            if (cfg != null && cfg.Enabled)
            {
                PeriodCloseTableLocker.Unlock(_container);
                if (cfg.PreserveLockAfterCalc)
                {
                    PersonalAccountChargeTableLocker.Lock(_container);
                }
            }

            return new CallbackResult(true);
        }

        #endregion
    }
}