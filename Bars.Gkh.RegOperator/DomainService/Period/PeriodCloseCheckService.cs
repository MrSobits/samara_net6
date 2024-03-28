namespace Bars.Gkh.RegOperator.DomainService.Period
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Period;
    using Bars.Gkh.RegOperator.Tasks.Period.Providers;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    public class PeriodCloseCheckService : IPeriodCloseCheckService
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Возвращает список зарегистрированных чекеров
        /// </summary>
        /// <returns></returns>
        public IDataResult ListCheckers(BaseParams baseParams)
        {
            var showAll = baseParams.Params.GetAs("showAll", false);

            var checkers = this.Container.ResolveAll<IPeriodCloseChecker>();
            var checkDomain = this.Container.ResolveDomain<PeriodCloseCheck>();
            using (this.Container.Using(checkers, checkDomain))
            {
                var added = checkDomain.GetAll().Select(y => y.Impl).ToList();
                var data = checkers.Select(
                    x => new
                    {
                        Impl = x.GetType().FullName,
                        x.Code,
                        x.Name
                    })
                    .WhereIf(!showAll, x => !added.Contains(x.Impl))
                    .ToList();

                return new ListDataResult(data, data.Count);
            }
        }

        /// <summary>
        /// Запускает проверку
        /// </summary>
        /// <returns></returns>
        public IDataResult RunCheck()
        {
            return this.RunCheckInternal(false);
        }

        /// <summary>
        /// Запускает проверку и закрытие периода в случае успеха
        /// </summary>
        /// <returns></returns>
        public IDataResult RunCheckAndClosePeriod(BaseParams baseParams)
        {
            var periodRepo = this.Container.Resolve<IChargePeriodRepository>();

            using (this.Container.Using(periodRepo))
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
            }

            return this.RunCheckInternal(true);
        }

        private IDataResult RunCheckInternal(bool closeAfterCheck)
        {
            var taskMan = this.Container.Resolve<ITaskManager>();

            try
            {
                return taskMan.CreateTasks(new PeriodCloseCheckTaskProvider(this.Container, closeAfterCheck), null);
            }
            finally
            {
                this.Container.Release(taskMan);
            }
        }

        private IDataResult ValidatePeriodClose(ChargePeriod period, BaseParams @params)
        {
            if (period.IsClosing)
            {
                return new BaseDataResult(false, "Закрытие периода в процессе");
            }

            var filterService = this.Container.Resolve<IPersonalAccountFilterService>();
            var chargeDomain = this.Container.ResolveDomain<PersonalAccountCharge>();

            using (this.Container.Using(filterService, chargeDomain))
            {
                var totalAccounts = this.Container.ResolveRepository<BasePersonalAccount>().GetAll()
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

                var producedChargeCount = chargeDomain.GetAll()
                    .Count(x => x.ChargePeriod.Id == period.Id && x.IsActive);

                if (producedChargeCount < totalAccounts)
                {
                    return new BaseDataResult(false, "Не по всем лицевым счетам проведены начисления");
                }
            }

            return new BaseDataResult(true);
        }
    }
}