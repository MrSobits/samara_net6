namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.Gkh.RegOperator.Entities.Refactor;
    using Bars.Gkh.RegOperator.Extenstions;

    using Domain.Extensions;
    using Entities;
    using Gkh.Domain;

    /// <summary>
    /// ViewModel для детализации оплат заявки на перечисление средств подрядчикам. (См. <see cref="TransferCtrPaymentDetail"/>)
    /// </summary>
    public class TransferCtrPaymentDetailViewModel : BaseViewModel<TransferCtrPaymentDetail>
    {
        /// <summary>
        /// Получить список кошельков
        /// </summary>
        /// <param name="domainService">Домен-сервис для <see cref="TransferCtrPaymentDetail"/> (Детализация заявки на перечисление средств подрядчикам)</param>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        public override IDataResult List(IDomainService<TransferCtrPaymentDetail> domainService, BaseParams baseParams)
        {
            var transferCtrId = baseParams.Params.GetAsId("transferCtrId");
            var transferCtrDomain = this.Container.ResolveDomain<TransferCtr>();
            var ropayaccDomain = this.Container.ResolveDomain<RealityObjectPaymentAccount>();

            using (this.Container.Using(transferCtrDomain, ropayaccDomain))
            {
                var transferCtr = transferCtrDomain.Get(transferCtrId);
                var roId = transferCtr.Return(x => x.ObjectCr).Return(x => x.RealityObject).Return(x => x.Id);

                var account = ropayaccDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == roId);

                if (account == null)
                {
                    return BaseDataResult.Error("Не найден счет оплат дома");
                }
                
                var exists = domainService.GetAll()
                    .Where(x => x.TransferCtr.Id == transferCtrId)
                    .ToArray()
                    .Select(x => new PaymentDetailProxy
                    {
                        WalletId = x.Wallet.Id,
                        WalletName = x.Wallet.GetWalletFullName(this.Container, account),
                        Balance = x.Wallet.Balance,
                        Amount = x.Amount,
                        RefundSum = x.RefundSum
                    })
                    .ToArray();

                var result = exists
                    .Union(account.GetWallets()
                        .Where(x => exists.All(z => z.WalletId != x.Id))
                        .Select(x => new PaymentDetailProxy
                        {
                            WalletId = x.Id,
                            WalletName = x.GetWalletFullName(this.Container, account),
                            Balance = x.Balance
                        }))
                    .Where(x => x.Balance > 0 || x.Amount > 0)
                    .ToArray();

                return new ListDataResult(result, result.Length);
            }
        }

        private class PaymentDetailProxy
        {
            public long WalletId { get; set; }

            public string WalletName { get; set; }

            public decimal Balance { get; set; }

            public decimal Amount { get; set; }

            public decimal RefundSum { get; set; }
        }
    }
}