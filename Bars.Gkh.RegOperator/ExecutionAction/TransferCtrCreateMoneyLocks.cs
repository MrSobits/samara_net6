namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Entities.Wallet;
    using Bars.Gkh.RegOperator.Extenstions;

    /// <summary>
    /// Действие: "Создание блокировок денег для Заявок на перечисление средств подрядчикам"
    /// </summary>
    public class TransferCtrCreateMoneyLocks : BaseExecutionAction
    {
        /// <summary>
        /// Описание задачи
        /// </summary>
        public override string Description
            => "Создание отсутствующих блокировок денег для Заявок на перечеслиние средств подрядчикам, которые не были созданы при массовом импорте.\n" +
                "А также, обнуление суммы \"Оплачено\" у заявок, которые были неверно оплачены";

        /// <summary>
        /// Название задачи
        /// </summary>
        public override string Name => "Создание блокировок денег и правка неверных оплат для Заявок на перечисление средств подрядчикам";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Сервис детализации оплат заявки на перечисление средств подрядчикам
        /// </summary>
        public IDomainService<TransferCtrPaymentDetail> TransferCtrPaymentDetailDomain { get; set; }

        /// <summary>
        /// Домен-сервис Счёт оплат дома
        /// </summary>
        public IDomainService<RealityObjectPaymentAccount> RoPaymentAccountDomainService { get; set; }

        /// <summary>
        /// Сервис кошельков оплат
        /// </summary>
        public IDomainService<Wallet> WalletDomain { get; set; }

        private BaseDataResult Execute()
        {
            var transferCtrService = this.Container.Resolve<ITransferCtrService>();
            var transferCtrDomain = this.Container.ResolveDomain<TransferCtr>();
            var moneyLockDomain = this.Container.ResolveDomain<MoneyLock>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var errorTransfers = new List<string>();

            using (var session = sessionProvider.OpenStatelessSession())
            {
                using (var tr = session.BeginTransaction())
                {
                    //все заявки, которые якобы "оплачены" но у них нет трансферов денег - обнуляем у них сумму "оплачено"
                    //в следствии они попадут под следующий запрос, чтобы создать блокировки денег
                    var query =
                        @"UPDATE RF_TRANSFER_CTR set paid_sum = 0 where id in (SELECT ctr.id FROM RF_TRANSFER_CTR ctr 
                        left join REGOP_TRANSFER tr on ctr.TRANSFER_GUID = tr.TARGET_GUID
                        where csum > 0 and paid_sum = csum and tr.id is null)";
                    try
                    {
                        session.CreateSQLQuery(query).ExecuteUpdate();
                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                    }
                }
            }

            try
            {
                var moneyLockQuery = moneyLockDomain.GetAll().Where(y => y.IsActive);
                var transferCtrs = transferCtrDomain.GetAll()
                    .Where(x => x.PaidSum == 0 && x.Sum > 0)
                    .Where(x => !moneyLockQuery.Any(y => x.TransferGuid == y.TargetGuid))
                    .AsEnumerable();

                foreach (var transferCtr in transferCtrs)
                {
                    var roId = transferCtr.Return(x => x.ObjectCr).Return(x => x.RealityObject).Return(x => x.Id);

                    var account = this.RoPaymentAccountDomainService.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);
                    var walletIds = account.GetWallets().Select(x => x.Id);

                    var paymentDetails = this.TransferCtrPaymentDetailDomain.GetAll()
                        .Where(x => walletIds.Contains(x.Wallet.Id))
                        .Where(x => x.TransferCtr.Id == transferCtr.Id)
                        .ToArray();

                    try
                    {
                        transferCtrService.SaveWithDetails(transferCtr, paymentDetails);
                    }
                    catch
                    {
                        errorTransfers.Add(transferCtr.DocumentNum);
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message);
            }
            finally
            {
                this.Container.Release(transferCtrService);
                this.Container.Release(transferCtrDomain);
                this.Container.Release(moneyLockDomain);
                this.Container.Release(sessionProvider);
            }

            if (errorTransfers.Any())
            {
                var errorMsg = string.Format(
                    "Операция завершена с ошибками, указанные заявки не могут быть обновлены: {0}",
                    string.Join(", ", errorTransfers));
                return new BaseDataResult(true, errorMsg);
            }

            return new BaseDataResult();
        }
    }
}