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

    /// <summary>
    /// Пересчёт баланса кошельков субсидий
    /// </summary>
    public class RoSubsideyWalltetBalanceFix : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Пересчёт баланса кошельков субсидий по всем домам.\n" +
            "Кошелки: субсидии фонда, региональные субсидии, стимулирующие субсидии, целевые субсидии";

        /// <summary>
        /// Наименование действия
        /// </summary>
        public override string Name => "Пересчёт баланса кошельков субсидий";

        /// <summary>
        /// Выполняемое действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var walletRepo = this.Container.Resolve<IWalletRepository>();
            var paymentAccountRepo = this.Container.ResolveDomain<RealityObjectPaymentAccount>();

            var walletsGuidsGrups = paymentAccountRepo.GetAll()
                .Select(
                    x => new
                    {
                        FundSubsidyWallet = x.FundSubsidyWallet.WalletGuid,
                        RegionalSubsidyWallet = x.RegionalSubsidyWallet.WalletGuid,
                        StimulateSubsidyWallet = x.StimulateSubsidyWallet.WalletGuid,
                        TargetSubsidyWallet = x.TargetSubsidyWallet.WalletGuid
                    })
                .ToList();

            var walletsGuids =
                walletsGuidsGrups.SelectMany(
                    x => new[] {x.FundSubsidyWallet, x.RegionalSubsidyWallet, x.StimulateSubsidyWallet, x.TargetSubsidyWallet});

            try
            {
                foreach (var wallets in walletsGuids.Section(1000))
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

            return new BaseDataResult();
        }
    }
}