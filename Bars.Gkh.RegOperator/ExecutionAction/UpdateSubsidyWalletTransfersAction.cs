namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    /// <summary>
    /// </summary>
    public class UpdateSubsidyWalletTransfersAction : BaseExecutionAction
    {
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => @"Данное действие исправляет проблему с трансферами, " +
            "которые были созданы с неверными параметрами, вследствие чего не влияли на баланс кошелька.\r\n" +
            "Действие проставляет трансферам IsAffected = true.\r\n" +
            "После этого действия необходимо провести действие \"Пересчёт баланса кошельков субсидий\" для обновления баланса кошельков.";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Исправление тансферов по кошелькам субсидий домов";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var paymentAccountRepo = this.Container.ResolveDomain<RealityObjectPaymentAccount>();
            var transferDomain = this.Container.ResolveDomain<Transfer>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

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

            var walletsGuids = walletsGuidsGrups.SelectMany(
                x => new[] {x.FundSubsidyWallet, x.RegionalSubsidyWallet, x.StimulateSubsidyWallet, x.TargetSubsidyWallet});

            var query = "UPDATE REGOP_TRANSFER set IS_AFFECT = true where IS_AFFECT = false and SOURCE_GUID in (:ids)";

            using (var session = sessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var wallets in walletsGuids.Section(1000))
                        {
                            session.CreateSQLQuery(query).SetParameterList("ids", wallets).ExecuteUpdate();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                    finally
                    {
                        this.Container.Release(paymentAccountRepo);
                        this.Container.Release(transferDomain);
                        this.Container.Release(sessionProvider);
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}