namespace Bars.Gkh.RegOperator.Tasks.Period.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Tasks.Period.Callbacks;
    using Bars.Gkh.RegOperator.Tasks.Period.Executors;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    public class PeriodCloseCheckTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        private readonly bool closeAfterCheck;

        public PeriodCloseCheckTaskProvider(IWindsorContainer container, bool closeAfterCheck)
        {
            this.container = container;
            this.closeAfterCheck = closeAfterCheck;
        }

        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            var checkDomain = this.container.ResolveDomain<PeriodCloseCheck>();
            var periodRepo = this.container.Resolve<IChargePeriodRepository>();
            var checkResultDomain = this.container.ResolveDomain<PeriodCloseCheckResult>();
            var userManager = this.container.Resolve<IGkhUserManager>();

            try
            {
                var descrs = new List<TaskDescriptor>();

                this.container.InTransaction(
                    () =>
                        {
                            var currentPeriod = periodRepo.GetCurrentPeriod();
                            var checks = checkDomain.GetAll().ToList();

                            checkResultDomain.GetAll()
                                             .Where(x => x.Period.Id == currentPeriod.Id)
                                             .Select(x => x.Id)
                                             .ForEach(id => checkResultDomain.Delete(id));

                            if (checks.Count == 0)
                            {
                                var closeService = this.container.Resolve<IChargePeriodCloseService>();
                                try
                                {
                                    var res = closeService.CloseCurrentPeriod(new BaseParams { Params = new DynamicDictionary() });
                                }
                                finally
                                {
                                    this.container.Release(closeService);
                                }
                            }
                            else
                            {
                                foreach (var check in checks)
                                {
                                    var param = new DynamicDictionary();

                                    var checkResult = new PeriodCloseCheckResult(check, currentPeriod, userManager.GetActiveUser());
                                    checkResultDomain.Save(checkResult);

                                    param.SetValue("periodId", currentPeriod.Id);
                                    param.SetValue("resultId", checkResult.Id);
                                    param.SetValue("closeAfterCheck", this.closeAfterCheck);

                                    descrs.Add(
                                        new TaskDescriptor(check.Name, PeriodCloseCheckTaskExecutor.Id, new BaseParams { Params = param })
                                        {
                                            SuccessCallback
                                                    =
                                                    PeriodCloseCheckSuccessCallback
                                                    .Id
                                        });
                                }
                            }
                        });

                return new CreateTasksResult(descrs.ToArray());
            }
            finally
            {
                this.container.Release(checkDomain);
                this.container.Release(periodRepo);
                this.container.Release(checkResultDomain);
                this.container.Release(userManager);
            }
        }

        public string TaskCode => "PeriodCloseCheck";
    }
}