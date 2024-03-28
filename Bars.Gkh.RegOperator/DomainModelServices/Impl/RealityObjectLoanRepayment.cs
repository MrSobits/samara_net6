namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.DatabaseMutex;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Exceptions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using NHibernate.Linq;

    /// <summary>
    /// Сервис возврата займа
    /// </summary>
    public class RealityObjectLoanRepayment : IRealityObjectLoanRepayment
    {
        private const int Take = 1000;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер мьютексов
        /// </summary>
        public IDatabaseMutexManager DatabaseMutexManager { get; set; }

        /// <summary>
        /// Сервис фильтрации займов
        /// </summary>
        public IRealityObjectLoanViewService RealityObjectLoanViewService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectPaymentAccount"/>
        /// </summary>
        public IDomainService<RealityObjectPaymentAccount> DomainServiceRealityObjectPaymentAccount { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLoan"/>
        /// </summary>
        public IDomainService<RealityObjectLoan> DomainServiceRealityObjectLoan { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObjectLoanWallet"/>
        /// </summary>
        public IDomainService<RealityObjectLoanWallet> DomainServiceRealityObjectLoanWallet { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="MoneyOperation"/>
        /// </summary>
        public IDomainService<MoneyOperation> MoneyOperationDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Transfer"/>
        /// </summary>
        public IDomainService<Transfer> DomainServiceTransfer { get; set; }

        /// <summary>
        /// Репозиторий <see cref="ChargePeriod"/>
        /// </summary>
        public IChargePeriodRepository ChargeRepo { get; set; }

        /// <summary>
        /// Сессия счетов дома
        /// </summary>
        public IRealtyObjectPaymentSession Session { get; set; }

        /// <summary>
        /// Возврат всех займов
        /// </summary>
        /// <returns>Результат</returns>
        public IDataResult RepaymentAll(BaseParams baseParams)
        {
            var results = new List<ReturnLoanResult>();

            try
            {
                var repaymentInfos = this.GetRepaymentInfos(baseParams);
                foreach (var repaymentInfo in repaymentInfos)
                {
                    var info1 = repaymentInfo;
                    this.Container.InTransaction(
                        () =>
                        {
                            IDatabaseLockedMutexHandle handler;
                            if (!this.TryEnter(info1.Account, info1.RealityObject.Address, out handler))
                            {
                                var result = new ReturnLoanResult(info1.Loan.LoanTaker.RealityObject)
                                {
                                    Message = $"По адресу {info1.RealityObject.Address} уже идет возврат займов"
                                };

                                results.Add(result);
                                return;
                            }

                            try
                            {
                                var info = info1;
#warning должно быть ExecutionMode.Bulk, но времено отключено. Необходимо доработать контроль баланса для bulk операций.
                                this.Container.InTransaction(() => results.Add(this.Repayment(info)));
                            }
                            finally
                            {
                                handler.Dispose();
                            }
                        });
                }

                this.Session.Complete();

                var resultData = results.Count == 0
                    ? new BaseDataResult(false, "Возврат займа не осуществлен. Не выбрано ни одного займа в статусе \"Утверждено\"")
                    : results.Count > 0 && results.All(x => x.Message.IsEmpty())
                        ? new BaseDataResult(false, "Недостаточно средств для возврата займа")
                        : new ListDataResult(results, results.Count) as IDataResult;

                return resultData;
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        /// <summary>Возврат займов дома</summary>
        public IDataResult Repayment(BaseParams baseParams)
        {
            var roId = baseParams.Params.GetAsId("roId");

            var ropayacc = this.DomainServiceRealityObjectPaymentAccount.GetAll()
                .FirstOrDefault(x => x.RealityObject.Id == roId);

            if (ropayacc == null)
            {
                return BaseDataResult.Error("Не удалось получить счет оплат жилого дома");
            }
            
            IDataResult result = null;

            try
            {
                this.Container.InTransaction(() =>
                {
                    using (new DatabaseMutexContext(ropayacc, "Возврат займа"))
                    {
                        result = this.RepaymentInternal(ropayacc, baseParams, true);
                    }
                });
            }
            catch (DatabaseMutexException)
            {
                return BaseDataResult.Error("Возврат займа по данному дому уже запущен");
            }
            catch (ValidationException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
            catch (GkhException exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error("Ошибка возврата займа: " + exception.Message);
            }
            return result ?? new BaseDataResult();
        }

        /// <summary>
        /// Возврат займов дома
        /// </summary>
        /// <param name="repaymentParams">
        /// Параметры возврата займов дома
        /// </param>
        /// <returns>
        /// Экземпляр <see cref="IDataResult"/>.
        /// </returns>
        private IDataResult RepaymentInternal(RealityObjectPaymentAccount realityObjectPaymentAccount, BaseParams baseParams, bool fetchWallets)
        {
            IDatabaseLockedMutexHandle handler;
            if (!this.TryEnter(realityObjectPaymentAccount, realityObjectPaymentAccount.RealityObject.Address, out handler))
            {
                return new BaseDataResult("Возврат займа по данному дому уже запущен");
            }

            var results = new List<ReturnLoanResult>();

            try
            {
                var infos = this.GetRepaymentInfos(baseParams, fetchWallets);

                results.AddRange(infos.Select(info => this.Repayment(info)));

                this.Session.Complete();
            }
            finally
            {
                handler.Dispose();
            }

            var result = results.Count == 0
                ? new BaseDataResult(false, "Возврат займа не осуществлен. Не выбрано ни одного займа в статусе \"Утверждено\"")
                : results.Count > 0 && results.All(x => x.Message.IsEmpty())
                    ? new BaseDataResult(false, "Недостаточно средств для возврата займа")
                    : new BaseDataResult(true, "Погашение займов выполнено успешно");

            return result;
        }

        /// <summary>
        /// Выполнение задачи
        /// </summary>
        /// <param name="params">
        /// Параметры исполнения задачи.
        ///             При вызове из планировщика передаются параметры из JobDataMap
        ///             и контекст исполнения в параметре JobContext
        /// </param>
        public void Execute(DynamicDictionary @params)
        {
            this.RepaymentAll(new BaseParams { Params = @params });
        }

        /// <summary>Погасить займ</summary>
        /// <param name="repaymentInfo">Информация о займе</param>
        /// <param name="mode">Тип запуска</param>
        protected ReturnLoanResult Repayment(RepaymentInfo repaymentInfo, ExecutionMode mode = ExecutionMode.Sequential)
        {
            var result = new ReturnLoanResult(repaymentInfo.Loan.LoanTaker.RealityObject);

            #warning Нужны приоритеты оплаты по субсидиям и отдельно по остальным кошелькам. Кому первому платить?! Пока сортировка по дате создания(не учитываются статусы). Раньше взял - раньше гасишь
            if (repaymentInfo.Account.GetBalance() <= decimal.Zero)
            {
                // Баланс нулевой или отрицательный. Погашать займ не из чего.
                result.Message = "Недостаточно средств для погашения займа";
                return result;
            }

            var transfers = new List<Transfer>();
            var loanWallets = new List<RealityObjectLoanWallet>();

            var period = this.ChargeRepo.GetCurrentPeriod();

            // Первыми гасим займы по субсидиям
            var subsidyWallets = repaymentInfo.Wallets.Where(x => this.IsSubsidy(x.TypeSourceLoan)).OrderBy(x => x.ObjectCreateDate).ToArray();
            if (subsidyWallets.Length > 0)
            {
                foreach (var loanWallet in subsidyWallets)
                {
                    // В первую учередь списываем с кошельков субсидий
                    var success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.FundSubsidyWallet, repaymentInfo.Account, transfers, period);
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.RegionalSubsidyWallet, repaymentInfo.Account, transfers, period) || success;
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.StimulateSubsidyWallet, repaymentInfo.Account, transfers, period) || success;
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.TargetSubsidyWallet, repaymentInfo.Account, transfers, period) || success;

                    // Потом с остальных
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.BaseTariffPaymentWallet, repaymentInfo.Account, transfers, period) || success;
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.DecisionPaymentWallet, repaymentInfo.Account, transfers, period) || success;
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.OtherSourcesWallet, repaymentInfo.Account, transfers, period) || success;
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.BankPercentWallet, repaymentInfo.Account, transfers, period) || success;
                    success = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.PenaltyPaymentWallet, repaymentInfo.Account, transfers, period) || success;

                    if (success)
                    {
                        loanWallets.Add(loanWallet);
                    }
                }
            }

            // Обрабатываем займы по не субсидиям.
            var otherWallets = repaymentInfo.Wallets.Where(x => !this.IsSubsidy(x.TypeSourceLoan)).OrderBy(x => x.ObjectCreateDate).ToArray();
            foreach (var loanWallet in otherWallets)
            {
                var sucess = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.BaseTariffPaymentWallet, repaymentInfo.Account, transfers, period);
                sucess = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.DecisionPaymentWallet, repaymentInfo.Account, transfers, period) || sucess;
                sucess = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.OtherSourcesWallet, repaymentInfo.Account, transfers, period) || sucess;
                sucess = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.BankPercentWallet, repaymentInfo.Account, transfers, period) || sucess;
                sucess = this.TryRollBack(loanWallet, loanWallet.Loan.LoanTaker.PenaltyPaymentWallet, repaymentInfo.Account, transfers, period) || sucess;

                if (sucess)
                {
                    loanWallets.Add(loanWallet);
                }
            }

            if (transfers.Count > 0)
            {
                // Обновляем информацию о размере погашеного займа
                loanWallets.ForEach(this.DomainServiceRealityObjectLoanWallet.Update);
                repaymentInfo.Loan.LoanReturnedSum = repaymentInfo.Wallets.Sum(y => y.ReturnedSum);
                this.DomainServiceRealityObjectLoan.Update(repaymentInfo.Loan);
                repaymentInfo.Loan.Operations.ForEach(this.MoneyOperationDomain.SaveOrUpdate);

                this.TryChangeState(repaymentInfo.Loan);

                // Сохраняем переводы
                transfers.ForEach(this.DomainServiceTransfer.Save);

                result.ReturnedSum = transfers.Sum(x => x.Amount);
                result.Message = repaymentInfo.Loan.LoanReturnedSum == repaymentInfo.Loan.LoanSum
                    ? "Погашен"
                    : "Погашен частично. Для полного погашения недостаточно средств";
            }

            return result;
        }

        /// <summary>Попробовать выполнить возврат</summary>
        /// <param name="loanWallet">Займ из кошелька</param>
        /// <param name="account"></param>
        /// <param name="transfers">Список трансферов</param>
        /// <param name="period"></param>
        /// <param name="mode">Тип запуска</param>
        /// <param name="walletForRepayment"></param>
        /// <returns>Произвелось ли погашение</returns>
        protected bool TryRollBack(
            RealityObjectLoanWallet loanWallet,
            Wallet walletForRepayment,
            RealityObjectPaymentAccount account,
            List<Transfer> transfers,
            ChargePeriod period,
            ExecutionMode mode = ExecutionMode.Sequential)
        {
            var transfer = loanWallet.Repayment(walletForRepayment, period);
            if (transfer == null)
            {
                return false;
            }

            transfers.Add(transfer);
            return true;
        }

        /// <summary>Зарезервировать возврат</summary>
        /// <param name="account">Счет жилого дома</param>
        /// <param name="address">Адрес дома</param>
        /// <param name="handler">Блокировка</param>
        /// <returns>true or false</returns>
        protected bool TryEnter(RealityObjectPaymentAccount account, string address, out IDatabaseLockedMutexHandle handler)
        {
            return this.DatabaseMutexManager.TryLock(
                account.GetMutexName(),
                string.Format("Возврат займов по дому \"{0}\"", address),
                out handler);
        }

        /// <summary>Является субсидией</summary>
        /// <param name="typeSourceLoan">Источник займа</param>
        /// <returns>true or false</returns>
        protected bool IsSubsidy(TypeSourceLoan typeSourceLoan)
        {
            bool result;
            switch (typeSourceLoan)
            {
                case TypeSourceLoan.FundSubsidy:
                case TypeSourceLoan.RegionalSubsidy:
                case TypeSourceLoan.StimulateSubsidy:
                case TypeSourceLoan.TargetSubsidy:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        /// <summary>Получить список займов</summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список займов</returns>
        protected RepaymentInfo[] GetRepaymentInfos(BaseParams baseParams, bool fetchWallets = false)
        {
#warning Необходимо будет добавить пропуск займов которые "перезаняли" - когда будет готов функционал

            var code = "002"; // Утверждено
            var codeSmol = "2"; // Утверждено

            var roId = baseParams.Params.GetAsId("roId");
            if (roId == 0)
            {
                // Так пишу потому что в клиентской части в займах переделал отправку параметра на filter чтобы не упало 
                // если есть еще места которые работают с этим параметром как с Params
                roId = baseParams.GetLoadParam().Filter.GetAsId("roId");
            }

            var filterQuery = this.RealityObjectLoanViewService.List(baseParams, fetchWallets);

            var data = this.DomainServiceRealityObjectLoanWallet.GetAll()
                .WhereIf(!fetchWallets, x => filterQuery.Any(y => y.Id == x.Loan.Id))
                .WhereIf(fetchWallets, x => x.Loan.LoanTaker.RealityObject.Id == roId)
                .Where(x => x.Loan.State.Code == code || x.Loan.State.Code == codeSmol)
                .Where(x => x.Sum != x.ReturnedSum)
                .Fetch(x => x.Loan)
                .ThenFetch(x => x.LoanTaker)
                .ThenFetch(x => x.RealityObject)
                .ToArray()
                .WhereIf(fetchWallets, x => filterQuery.Select(y => y.Id).Contains(x.Loan.Id))
                .GroupBy(x => x.Loan)
                .Select(
                    x => new RepaymentInfo
                    {
                        RealityObject = x.Key.LoanTaker.RealityObject,
                        Loan = x.Key,
                        Wallets = x.ToArray()
                    })
                .OrderByDescending(x => x.Loan.ObjectCreateDate)
                .ToArray();

            var total = data.Length;
            var skip = 0;

            while (skip < total)
            {
                var values = data.Skip(skip).Take(RealityObjectLoanRepayment.Take).ToArray();
                var roIds = values.Select(x => x.RealityObject.Id).ToArray();
                var accounts = this.DomainServiceRealityObjectPaymentAccount.GetAll()
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .ToArray();

                values.ForEach(x => { x.Account = accounts.Single(y => y.RealityObject.Id == x.RealityObject.Id); });

                skip += RealityObjectLoanRepayment.Take;
            }

            return data;
        }

        private void TryChangeState(RealityObjectLoan loan)
        {
            if (loan.LoanReturnedSum != loan.LoanSum)
            {
                return;
            }

            var appliedCode = "003";
            var appliedCodeSmolensk = "3";

            var stateProvider = this.Container.Resolve<IStateProvider>();
            var stateRepo = this.Container.ResolveRepository<State>();
            using (this.Container.Using(stateProvider, stateRepo))
            {
                var typeInfo = stateProvider.GetStatefulEntityInfo(typeof(RealityObjectLoan));

                var newState = stateRepo.GetAll()
                    .FirstOrDefault(x => x.TypeId == typeInfo.TypeId && (x.Code == appliedCode || x.Code == appliedCodeSmolensk));

                if (newState.IsNull())
                {
                    throw new StateException("Не найден статус с кодом {0} для объекта с типом {1}".FormatUsing(appliedCode, typeInfo.Name));
                }

                stateProvider.ChangeState(loan.Id, typeInfo.TypeId, newState, "Исполнение займа", true, false);
            }
        }

        /// <summary>Информация о займе</summary>
        protected class RepaymentInfo
        {
            public RealityObject RealityObject { get; set; }

            public RealityObjectPaymentAccount Account { get; set; }

            public RealityObjectLoan Loan { get; set; }

            public RealityObjectLoanWallet[] Wallets { get; set; }
        }

        protected class ReturnLoanResult
        {
            public ReturnLoanResult()
            {
            }

            public ReturnLoanResult(RealityObject taker)
            {
                this.Taker = taker.Address;
            }

            public string Taker { get; set; }

            public decimal ReturnedSum { get; set; }

            public string Message { get; set; }
        }
    }
}