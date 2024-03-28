namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Domain.Repository.Wallets;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Пересчёт баланса кошелька по базовому тарифу по всем кошелькам
    /// </summary>
    public class RoBaseTarifWalltetBalanceFix : BaseExecutionAction
    {
        

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description => "Пересчёт баланса кошелька по базовому тарифу по всем кошелькам";

        /// <summary>
        /// Наименование действияя
        /// </summary>
        public override string Name => "Пересчёт баланса кошельков по базовому тарифу";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;



        private BaseDataResult Execute()
        {
            var walletRepo = this.Container.Resolve<IWalletRepository>();
            var paymentAccountRepo = this.Container.ResolveRepository<RealityObjectPaymentAccount>();

            var walletsGuids = paymentAccountRepo.GetAll()
                .Where(x => x.BaseTariffPaymentWallet != null)
                .Select(x => x.BaseTariffPaymentWallet.WalletGuid)
                .ToList();

            try
            {
                foreach (var wallets in walletsGuids.Section(100))
                {
                    walletRepo.UpdateWalletBalance(wallets.ToList(), realityObject: true);
                }
            }
            catch (Exception ex)
            {
                return BaseDataResult.Error(ex.Message);
            }
            finally
            {
                this.Container.Release(walletRepo);
                this.Container.Release(paymentAccountRepo);
            }

            return new BaseDataResult {Success = true};
        }
    }
}