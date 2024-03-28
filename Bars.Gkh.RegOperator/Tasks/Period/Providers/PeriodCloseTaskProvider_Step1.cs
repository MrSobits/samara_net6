namespace Bars.Gkh.RegOperator.Tasks.Period.Providers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Repository;
    using Bars.Gkh.RegOperator.Tasks.Charges;
    using Bars.Gkh.RegOperator.Tasks.Period.Callbacks;
    using Bars.Gkh.RegOperator.Tasks.Period.Executors;
    using Bars.Gkh.RegOperator.Utils;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Провайдер задачи закрытия периода (Этап 1)
    /// </summary>
    public class PeriodCloseTaskProvider_Step1 : ITaskProvider
    {
        private readonly IWindsorContainer container;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public PeriodCloseTaskProvider_Step1(IWindsorContainer container)
        {
            this.container = container;
        }

        #region Implementation of ITaskProvider
        
        /// <summary>
        /// Создать задачи
        /// </summary>
        /// <param name="params">Базовые параметры</param>
        /// <returns>Результат</returns>
        public CreateTasksResult CreateTasks(BaseParams @params)
        {
            var chargeDomain = this.container.ResolveDomain<PersonalAccountCharge>();
            var periodRepo = this.container.Resolve<IChargePeriodRepository>();
            var chargeRepo = this.container.Resolve<IPersonalAccountChargeRepository>();
            var operationLock = this.container.GetGkhConfig<RegOperatorConfig>().GeneralConfig.OperationLock.ToDto();

            using (this.container.Using(chargeDomain, periodRepo))
            {
                var period = periodRepo.GetCurrentPeriod();

                var needToFix = chargeRepo.GetNeedToBeFixedForPeriod(period, false).OrderBy(x => x.Id).Select(x => x.Id);
                var countCharges = needToFix.Count();

                var descrs = new List<TaskDescriptor>();

                var take = 10000;
                ActionPartitioner.ProcessByPortion(
                    done =>
                    {
                        var args = DynamicDictionary.Create();
                        args.SetValue("operationLock", operationLock);
                        args.SetValue("periodId", period.Id);
                        args.SetValue("ids", needToFix.Skip(done).Take(take).ToArray());

                        descrs.Add(
                            new TaskDescriptor(
                                "Закрытие периода",
                                PeriodCloseTaskExecutor_Step1.Id,
                                new BaseParams {Params = args})
                            {
                                SuccessCallback = PeriodCloseSuccessCallback_Step1.Id,
                                FailCallback = PeriodCloseFailCallback.Id
                            });
                    },
                    countCharges,
                    take);

                if (descrs.IsEmpty())
                {
                    var args = DynamicDictionary.Create();
                    args.SetValue("periodId", period.Id);
                    args.SetValue("ids", new long[0]);

                    descrs.Add(
                        new TaskDescriptor(
                            "Закрытие периода",
                            PeriodCloseTaskExecutor_Step1.Id,
                            new BaseParams {Params = args})
                        {
                            SuccessCallback = PeriodCloseSuccessCallback_Step1.Id,
                            FailCallback = PeriodCloseFailCallback.Id
                        });
                }

                var result = new CreateTasksResult(descrs.ToArray());

                if (operationLock.Enabled)
                {
                    if (operationLock.PreserveLockAfterCalc)
                    {
                        PersonalAccountChargeTableLocker.Unlock(this.container);
                    }

                    PeriodCloseTableLocker.Lock(this.container);
                }

                return result;
            }
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode => "PeriodClose";

        #endregion
    }
}