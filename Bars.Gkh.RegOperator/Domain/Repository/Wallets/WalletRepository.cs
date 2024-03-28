namespace Bars.Gkh.RegOperator.Domain.Repository.Wallets
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;

    using Entities.ValueObjects;
    using Entities.Wallet;
    using Repositories;

    /// <summary>
    /// Репозиторий для Кошелек оплат
    /// </summary>
    public class WalletRepository : BaseDomainRepository<Wallet>, IWalletRepository
    {
        private readonly ISessionProvider sessions;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="sessions">Провайдер сессий NHibernate</param>
        public WalletRepository(ISessionProvider sessions)
        {
            this.sessions = sessions;
        }

        /// <summary>
        /// Получить кошельки, к которых уходили деньги
        /// </summary>
        /// <param name="transfers">Трансферы</param>
        /// <returns>Запрос с кошельками</returns>
        public IQueryable<IWallet> GetSourceWalletsFor(IEnumerable<Transfer> transfers)
        {
            var sourcesGuids = transfers.Select(x => x.SourceGuid).ToList();
            return this.DomainService.GetAll().Where(w => sourcesGuids.Contains(w.TransferGuid));
        }

        /// <summary>
        /// Получить кошельки, на которые уходили деньги
        /// </summary>
        /// <param name="transfers">Трансферы</param>
        /// <returns>Запрос с кошельками</returns>
        public IQueryable<IWallet> GetTargetWalletsFor(IEnumerable<Transfer> transfers)
        {
            var targetsGuids = transfers.Select(x => x.TargetGuid).ToList();
            return this.DomainService.GetAll().Where(w => targetsGuids.Contains(w.TransferGuid));
        }

        /// <summary>
        /// Обновить баланс кошельков
        /// </summary>
        /// <param name="walletGuids">Идентификаторы кошельков</param>
        /// <param name="realityObject">Учитывать ли копии трансферов</param>
        public void UpdateWalletBalance(List<string> walletGuids, bool realityObject = false)
        {
            walletGuids = walletGuids.Distinct().ToList();

            using (var session = this.sessions.OpenStatelessSession())
            using (var tr = session.BeginTransaction())
            {
                var tableName = realityObject ? "regop_reality_transfer" : "regop_transfer";

                session.CreateSQLQuery(string.Format(@"
                                        DROP TABLE IF EXISTS tmpTargetSum;
                                        DROP TABLE IF EXISTS tmpSourceSum;
                                        DROP TABLE IF EXISTS tmpMoneyLockSum;

                                        CREATE TEMP TABLE tmpTargetSum
                                        (
                                            wallet_guid varchar(40),
                                            amount_sum numeric(19, 5)
                                        );

                                        CREATE TEMP TABLE tmpSourceSum
                                        (
                                            wallet_guid varchar(40),
                                            amount_sum numeric(19, 5)
                                        );

                                        CREATE TEMP TABLE tmpMoneyLockSum
                                        (
                                            wallet_guid varchar(40),
                                            amount_sum numeric(19, 5)
                                        );
                                        
                                        --- Подсчет итоговой суммы по target_guid ---
                                        INSERT INTO tmpTargetSum
                                        SELECT t.target_guid, sum(t.amount*target_coef)
                                        FROM {0} t
                                        WHERE t.target_guid in (:guids)
                                            AND t.is_affect
                                        GROUP BY t.target_guid;

                                        --- Подсчет итоговой суммы по source_guid ---
                                        INSERT INTO tmpSourceSum
                                        SELECT t.source_guid, sum(t.amount*target_coef)
                                        FROM {0} t
                                        WHERE t.source_guid in (:guids) 
                                            AND t.is_affect
                                        GROUP BY t.source_guid;

                                        --- Подсчет итоговой суммы в regop_money_lock ---
                                        INSERT INTO tmpMoneyLockSum
                                        SELECT w.wallet_guid, sum(amount)
                                        FROM regop_money_lock ml
                                        JOIN regop_wallet w ON w.id = ml.wallet_id
                                                            AND w.wallet_guid in (:guids)
                                        WHERE ml.is_active
                                        GROUP BY w.wallet_guid;

                                        --- Обновление кошельков ---
                                        --- Баланс = (по target'у) - (по source'у) - (в money_lock) ---
                                        UPDATE regop_wallet w
			                            SET balance = round((COALESCE ((SELECT tmpTarget.amount_sum 
                                                                        FROM tmpTargetSum tmpTarget 
                                                                        WHERE tmpTarget.wallet_guid = w.wallet_guid), 0) -
						                                     COALESCE ((SELECT tmpSource.amount_sum 
                                                                        FROM tmpSourceSum tmpSource 
                                                                        WHERE tmpSource.wallet_guid = w.wallet_guid), 0) -
					                                         COALESCE ((SELECT tmpLock.amount_sum 
                                                                        FROM tmpMoneyLockSum tmpLock 
                                                                        WHERE tmpLock.wallet_guid = w.wallet_guid), 0)), 2),
			                                has_new_ops = false
			                            WHERE
				                            w.wallet_guid in (:guids);", tableName))
                        .SetParameterList("guids", walletGuids)
                        .ExecuteUpdate();

                try
                {
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
        }
    }
}
