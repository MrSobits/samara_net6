namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Enums;

    using Dapper;

    /// <summary>
    /// Действие выполняет добавление информации о владельцах кошельков
    /// </summary>
    public class SetWalletOwnerInfoAction : BaseExecutionAction
    {
        private static readonly int Timeout = 3600 * 10;

        /// <inheritdoc />
        public override string Code => nameof(SetWalletOwnerInfoAction);

        /// <inheritdoc />
        public override string Description => "Действие выполняет добавление информации о владельцах кошельков";

        /// <inheritdoc />
        public override string Name => "Рефакторинг - Рефакторинг кошельков";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Поставщик сессии
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        private BaseDataResult Execute()
        {
            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                var connection = session.Connection;
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var sql =
                            $@"drop table if exists _wallet_list;
                            create temp table _wallet_list as
                            -- дома  
                            select  unnest(array[
                                {(
                                int) WalletType.AccumulatedFundWallet},
                                {(int) WalletType.RestructAmicableAgreementWallet},
                                {(
                                    int) WalletType.BankPercentWallet},
                                {(int) WalletType.BaseTariffWallet},
                                {(
                                        int) WalletType.DecisionTariffWallet},
                                {(int) WalletType.FundSubsidyWallet},
                                {(
                                            int) WalletType.OtherSourcesWallet},
                                {(int) WalletType.PenaltyWallet},
                                {(
                                                int) WalletType.PreviosWorkPaymentWallet},
                                {(int)
                                                    WalletType.RegionalSubsidyWallet},
                                {(int) WalletType.RentWallet},
                                {(
                                                        int) WalletType.SocialSupportWallet},
                                {(int)
                                                            WalletType.StimulateSubsidyWallet},
                                {(int)
                                                                WalletType.TargetSubsidyWallet}]) as wallet_type,
		                        unnest(array[
                                AF_WALLET_ID,
                                RAA_WALLET_ID,
                                BP_WALLET_ID,
                                BT_WALLET_ID,
		                        DT_WALLET_ID,
                                FSU_WALLET_ID,
                                OS_WALLET_ID,
		                        P_WALLET_ID,
                                PWP_WALLET_ID,
                                RSU_WALLET_ID,
		                        R_WALLET_ID,
                                SS_WALLET_ID,
                                SSU_WALLET_ID,
		                        TSU_WALLET_ID]) as wallet_id, 
                                id as owner_id,
                                {(
                                                                    int) WalletOwnerType.RealityObjectPaymentAccount} as owner_type
                                from public.regop_ro_payment_account";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql =
                            $@"-- лицевые счета 
                            Insert into _wallet_list (wallet_type, wallet_id, owner_id, owner_type)                            
                            select  unnest(array[
							            {(
                                int) WalletType.AccumulatedFundWallet},
							            {(int) WalletType.BaseTariffWallet},
							            {(int)
                                    WalletType.DecisionTariffWallet},
							            {(int) WalletType.PenaltyWallet},
							            {(int)
                                        WalletType.PreviosWorkPaymentWallet},
							            {(int) WalletType.RentWallet},
							            {(int)
                                            WalletType.SocialSupportWallet},
                                        {(int)
                                                WalletType.RestructAmicableAgreementWallet}]) as wallet_type,

		                            unnest(array[
							            AF_WALLET_ID,
							            BT_WALLET_ID,
                                        DT_WALLET_ID,
                                        P_WALLET_ID,
                                        PWP_WALLET_ID,
                                        R_WALLET_ID,
                                        SS_WALLET_ID,
                                        RAA_WALLET_ID]) as wallet_id,

		                           id as owner_id,

                                   {(
                                                    int) WalletOwnerType.BasePersonalAccount} as owner_type
                            from public.regop_pers_acc";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = @"CREATE INDEX on _wallet_list (wallet_id);
                                ANALYSE _wallet_list;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = @"ALTER TABLE regop_wallet ADD COLUMN owner_type INTEGER;
                                ALTER TABLE regop_wallet ADD COLUMN owner_id BIGINT;
                                ALTER TABLE regop_wallet ADD COLUMN wallet_type INTEGER;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = "DROP INDEX idx_regop_wallet_wg;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = @"UPDATE regop_wallet w
                              set
                                wallet_type = wl.wallet_type,
                                owner_id = wl.owner_id,
                                owner_type = wl.owner_type
                            from _wallet_list wl
                             where wl.wallet_id = w.id;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = "CREATE UNIQUE INDEX idx_regop_wallet_wg  ON public.regop_wallet(wallet_guid);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);
                        sql = @"CREATE INDEX ind_regop_wallet_owner_type ON regop_wallet(owner_type);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = "CREATE INDEX ind_regop_wallet_owner_id ON regop_wallet(owner_id);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = "CREATE INDEX ind_regop_wallet_type ON regop_wallet(wallet_type);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = @"delete from regop_wallet where owner_id is null;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = @"ALTER TABLE regop_wallet ALTER COLUMN owner_type SET NOT NULL;
                                ALTER TABLE regop_wallet ALTER COLUMN owner_id SET NOT NULL;
                                ALTER TABLE regop_wallet ALTER COLUMN wallet_type SET NOT NULL;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        sql = "analyze regop_wallet;";
                        connection.Execute(sql, transaction: transaction, commandTimeout: SetWalletOwnerInfoAction.Timeout);

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return new BaseDataResult();
        }
    }
}