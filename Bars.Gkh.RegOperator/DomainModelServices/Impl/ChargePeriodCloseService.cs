namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Tasks.Common.Service;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using Domain.Extensions;
    using Domain.Repository;
    using Entities;
    using Tasks.Period.Providers;

    /// <summary>
    /// Сервис закрытия периода
    /// </summary>
    public class ChargePeriodCloseService : IChargePeriodCloseService
    {
        #region Injections

        private readonly IWindsorContainer container;
        private readonly ITaskManager taskManager;

        /// <summary>
        /// .ctor
        /// </summary>
        public ChargePeriodCloseService(
            IWindsorContainer container,
            ITaskManager taskManager)
        {
            this.container = container;
            this.taskManager = taskManager;
        }

        #endregion Injections

        /// <summary>
        /// Закрытие текущего периода. При закрытии периода начисление недоступно
        /// 1) Проверяем, что для всех ЛС были проведены начисления
        /// 2) "Фиксируем" начисления
        /// 3) Обновляем данные по ЛС за текущий период
        /// 4.1) Группируем начисления по домам и аггрегируем сумму начислений в одну операцию
        /// 4.2) Сумму по операции пробрасываем в счет дома
        /// 5) Закрываем текущий период и создаем новый период c датой начала = конец текущего + 1 день
        /// </summary>
        public IDataResult CloseCurrentPeriod(BaseParams baseParams)
        {
            var periodRepo = this.container.Resolve<IChargePeriodRepository>();

            using (this.container.Using(periodRepo))
            {
                var period = periodRepo.GetCurrentPeriod(false);

                if (period == null)
                {
                    return new BaseDataResult(false, "Нет данных о периоде!");
                }

                var validation = this.ValidatePeriodClose(period, baseParams);

                if (!validation.Success)
                {
                    return validation;
                }

                IDataResult result;
                try
                {
                    this.SetPeriodIsClosing(period);

                    result = this.taskManager.CreateTasks(new PeriodCloseTaskProvider_Step1(this.container), baseParams);
                }
                catch (Exception e)
                {
                    this.SetPeriodIsNotClosing(period);

                    result = new BaseDataResult(false, e.Message);
                }

                return result;
            }
        }

        private void SetPeriodIsNotClosing(ChargePeriod period)
        {
            period.IsClosing = false;
            this.UpdatePeriod(period);
        }

        private void SetPeriodIsClosing(ChargePeriod period)
        {
            period.IsClosing = true;
            this.UpdatePeriod(period);
        }

        private void UpdatePeriod(ChargePeriod period)
        {
            var repo = this.container.ResolveRepository<ChargePeriod>();
            using (this.container.Using(repo))
            {
                repo.Update(period);
            }
        }

        private IDataResult ValidatePeriodClose(ChargePeriod period, BaseParams @params)
        {
            if (period.IsClosing)
            {
                return new BaseDataResult(false, "Закрытие периода в процессе");
            }

            var filterService = this.container.Resolve<IPersonalAccountFilterService>();
            var chargeDomain = this.container.ResolveDomain<PersonalAccountCharge>();

            using (this.container.Using(filterService, chargeDomain))
            {
                var totalAccounts = this.container.ResolveRepository<BasePersonalAccount>().GetAll()
                    .ToDto()
                    .FilterCalculable(period, filterService)
                    .FilterByRegFondSetting(filterService)
                    .FilterByBaseParams(@params, filterService)
                    .Count(
                        x =>
                            x.CloseDate <= DateTime.MinValue 
                            || !x.CloseDate.HasValue 
                            || x.CloseDate.Value.Year >= 3000
                            || period.StartDate < x.CloseDate);

                var producedChargeCount = chargeDomain.GetAll().Count(x => x.ChargePeriod.Id == period.Id && x.IsActive);

                if (producedChargeCount < totalAccounts)
                {
                    return new BaseDataResult(false, "Не по всем лицевым счетам проведены начисления");
                }
            }

            return new BaseDataResult(true);
        }
    }
}