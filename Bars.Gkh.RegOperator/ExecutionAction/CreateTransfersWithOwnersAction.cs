namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.Utils;

    using Dapper;

    /// <summary>
    /// Рефакторинг - Разделение трансферов
    /// </summary>
    public class CreateTransfersWithOwnersAction : BaseExecutionAction
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private readonly int CommandTimeOut = 3600 * 3;

        /// <inheritdoc />
        public override string Code => nameof(CreateTransfersWithOwnersAction);

        /// <inheritdoc />
        public override string Name => "Рефакторинг - Разделение трансферов";

        /// <inheritdoc />
        public override string Description
            => "Действие разделяет таблицу трансферов на три: Трансферы счёта оплат дома, трансферы оплат ЛС, трансферы начислений ЛС";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Поставщик сессий
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ChargePeriod" />
        /// </summary>
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        private BaseDataResult Execute()
        {
            ExecutionActionHelper.EnsureSuccess<CreatePeriodRefenenceInMoneyOperation>();
            ExecutionActionHelper.EnsureSuccess<SetWalletOwnerInfoAction>();

            this.SessionProvider.InStatelessConnectionTransaction(this.ExecuteSqls);

            return new BaseDataResult();
        }

        private void ExecuteSqls(IDbConnection connection, IDbTransaction transaction)
        {
            var isManuallyMerge = this.ExecutionParams.Params.GetAs("IsManuallyMerge", false);
            var setOwners = !this.ExecutionParams.Params.GetAs("DontSetOwners", false);

            var periodIds = this.ChargePeriodDomain.GetAll().OrderBy(x => x.StartDate).Select(x => x.Id).ToArray();
            string sql;

            if (!isManuallyMerge)
            {
                // разделяем трансферы слияния на 2
                sql = @"DROP TABLE IF EXISTS __merges;
                    CREATE temp table __merges (id BIGINT);";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                foreach (var periodId in periodIds)
                {
                    var tableName = $"regop_transfer_period_{periodId}";
                    var existsTable = connection.ExecuteScalar<bool>($"select count(*) > 0 from pg_tables where tablename = '{tableName}'");

                    if (!existsTable)
                    {
                        continue;
                    }

                    // 1. создаём таблицу с идентификаторами трансферов слияния
                    sql = $@"INSERT INTO __merges
                        select id from regop_transfer_period_{periodId} where reason is null";
                    connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                    // 2. создаём целевые трансферы (из источника к целевому ЛС)
                    sql =
                        $@"insert INTO regop_transfer_period_{periodId}
                (object_version, object_create_date, object_edit_date, amount, reason,
                    source_guid, target_guid, originator_id, payment_date, operation_date,
                    op_id, is_indirect, is_copy_for_source, target_coef, is_affect, is_loan,
                    is_return_loan, originator_name, period_id)
                    select
                        t.object_version,
                        t.object_create_date,
                        t.object_edit_date,
                        t.amount,
                        t.reason,
                        '0CFE40F2-184E-4F4B-A721-AED27C51C13C' as source_guid,
                        t.target_guid,
                        t.originator_id,
                        t.payment_date,
                        t.operation_date,
                        t.op_id,
                        t.is_indirect,
                        t.is_copy_for_source,
                        t.target_coef,
                        case when t.amount < 0 then true else false end as is_affect,
                        t.is_loan,
                        t.is_return_loan,
                        t.originator_name,
                        t.period_id
                    from regop_transfer_period_{periodId} t where t.id in (select id from __merges);";
                    connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                    // 3. обновляем целевой GUID у трансферов ЛС, с которых сливаем
                    sql =
                        $@"update regop_transfer_period_{periodId} t
                        set target_guid = '0CFE40F2-184E-4F4B-A721-AED27C51C13C',
                            is_affect = (case when t.amount < 0 then true else false end)
                        where id in (select id from __merges);";
                    connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                    sql = "DELETE FROM __merges";
                    connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);
                }
            }

            var triggerList = new List<string>();

            if (setOwners)
            {
                sql = "ALTER TABLE regop_transfer ADD COLUMN owner_id BIGINT;";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                foreach (var periodId in periodIds)
                {
                    var tableName = $"regop_transfer_period_{periodId}";
                    var existsTable = connection.ExecuteScalar<bool>($"select count(*) > 0 from pg_tables where tablename = '{tableName}'");

                    // алгоритм проставление владельца:
                    // Для трансферов на ЛС:
                    // 1. Оплата, расход, начисление и слияния приходят/уходят в никуда
                    // Для трансферов на дом:
                    // 1. Копии на дом
                    // 2. Все остальные операции
                    if (existsTable)
                    {
                        sql = $"select indexname, indexdef from pg_indexes where tablename = '{tableName}' and lower(indexdef) !~ 'unique'";
                        var indexDef = new List<Tuple<string, string>>();

                        using (var reader = connection.ExecuteReader(sql, transaction: transaction))
                        {
                            while (reader.Read())
                            {
                                indexDef.Add(Tuple.Create(reader.GetString(0), reader.GetString(1)));
                            }
                        }

                        foreach (var tuple in indexDef)
                        {
                            connection.Execute($"DROP INDEX {tuple.Item1};", transaction: transaction);
                        }

                        // трансферы, пришедшие на дом с ЛС
                        sql =
                            $@"UPDATE {tableName} t 
                          SET 
                              owner_id = w.owner_id,
                              is_affect = true,
                              is_copy_for_source = true -- на челябе были случаи, когда тут false 
                          FROM regop_wallet w 
                          WHERE w.owner_type = {(
                                int) WalletOwnerType.RealityObjectPaymentAccount} and (w.wallet_guid = t.target_guid)
                            and exists (select null from regop_wallet w2 where w2.wallet_guid = t.source_guid and w2.owner_type = {(
                                    int) WalletOwnerType.BasePersonalAccount})";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut * 3);

                        // трансферы на ЛС
                        sql =
                            $@"UPDATE {tableName} t 
                          SET owner_id = w.owner_id
                          FROM regop_wallet w 
                          WHERE w.owner_type = {(
                                int) WalletOwnerType.BasePersonalAccount} and t.owner_id is null and 
                            ((w.wallet_guid = t.source_guid and not t.is_copy_for_source) -- если присоединились по source_guid, то это не должна быть копия на дом
                              or w.wallet_guid = t.target_guid);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut * 3);

                        // трансферы на дом, которым не проставили владельца
                        sql =
                            $@"UPDATE {tableName} t 
                          SET owner_id = w.owner_id, is_affect = true
                          FROM regop_wallet w 
                          WHERE t.owner_id is null 
                                and w.owner_type = {(
                                int) WalletOwnerType.RealityObjectPaymentAccount} 
                                and (w.wallet_guid = t.source_guid or w.wallet_guid = t.target_guid);";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut * 3);

                        foreach (var tuple in indexDef)
                        {
                            connection.Execute(tuple.Item2, transaction: transaction);
                        }

                        // добавляем индексы на владельца
                        sql = $"CREATE INDEX ON {tableName} (owner_id)";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                        sql = $"drop trigger if exists delete_{tableName}_tr on {tableName};";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                        sql = $"drop trigger if exists delete_transfer_tr on {tableName};";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                        // удаляем "мертвые трансферы"
                        sql = $"delete from {tableName} where owner_id is null";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                        // удаляем копии "мертвых трансферов"
                        sql =
                            $@"delete FROM {tableName} tt
                            WHERE originator_id is not null and NOT exists(SELECT NULL
                                          FROM regop_transfer t
                                             WHERE tt.originator_id = t.id and t.period_id <= {periodId});";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                        triggerList.Add(
                            $@"CREATE TRIGGER delete_{tableName}_tr
                           BEFORE DELETE ON public.{tableName}
                           FOR EACH ROW EXECUTE PROCEDURE public.delete_from_transfer();");
                    }
                }

                // добавляем индексы на владельца
                sql = "CREATE INDEX ON regop_transfer (owner_id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);
            }

            sql = @"ALTER TABLE public.regop_transfer ALTER COLUMN owner_id SET NOT NULL;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            sql = "ANALYZE regop_transfer";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            // количество трансферов до разделения
            var countBefore = connection.ExecuteScalar<long>("SELECT count(*) from regop_transfer", transaction: transaction);

            // создаём таблицу трансферов на дом
            sql = @"CREATE SEQUENCE public.regop_reality_transfer_id_seq
                  INCREMENT 1
                  MINVALUE 1
                  MAXVALUE 9223372036854775807
                  START 1070148
                  CACHE 1;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            sql = @"CREATE TABLE regop_reality_transfer
                (
                    id INTEGER DEFAULT nextval('regop_reality_transfer_id_seq'::regclass) PRIMARY KEY NOT NULL,
                    object_version BIGINT NOT NULL,
                    object_create_date TIMESTAMP NOT NULL,
                    object_edit_date TIMESTAMP NOT NULL,
                    amount NUMERIC(19,5) NOT NULL,
                    reason VARCHAR(1000),
                    source_guid VARCHAR(40) NOT NULL,
                    target_guid VARCHAR(40) NOT NULL,
                    originator_id BIGINT,
                    payment_date TIMESTAMP NOT NULL,
                    operation_date TIMESTAMP NOT NULL,
                    op_id BIGINT NOT NULL,
                    is_indirect BOOLEAN DEFAULT false NOT NULL,
                    target_coef SMALLINT DEFAULT 1 NOT NULL,
                    is_affect BOOLEAN DEFAULT false NOT NULL,
                    is_loan BOOLEAN DEFAULT false NOT NULL,
                    is_return_loan BOOLEAN DEFAULT false NOT NULL,
                    originator_name VARCHAR(150),
                    period_id BIGINT DEFAULT 0 NOT NULL,
                    owner_id BIGINT DEFAULT 0 NOT NULL,
                    copy_transfer_id BIGINT,
                    CONSTRAINT fk_regop_ro_tr_op FOREIGN KEY (op_id) REFERENCES regop_money_operation (id),
                    CONSTRAINT fk_regop_ro_tr_period_id FOREIGN KEY (period_id) REFERENCES regop_period (id),
                    CONSTRAINT fk_regop_ro_tr_owner_id FOREIGN KEY (owner_id) REFERENCES regop_ro_payment_account (id)
                );

                CREATE INDEX ind_regop_ro_transfer_target ON regop_reality_transfer (target_guid);
                CREATE INDEX ind_regop_ro_transfer_source ON regop_reality_transfer (source_guid);
                CREATE INDEX ind_regop_ro_transfer_originator_id ON regop_reality_transfer (originator_id);
                CREATE INDEX ind_regop_ro_transfer_cpy_tr_id ON regop_reality_transfer (copy_transfer_id);
                CREATE INDEX ind_regop_ro_transfer_op_id ON regop_reality_transfer (op_id);
                CREATE INDEX ind_regop_ro_transfer_op_date_date ON regop_reality_transfer (date(operation_date));
                CREATE INDEX ON regop_reality_transfer (target_guid) WHERE is_affect;
                CREATE INDEX ON regop_reality_transfer (source_guid) WHERE is_affect;
                CREATE INDEX ON regop_reality_transfer (source_guid) WHERE reason LIKE 'Возврат%';
                CREATE INDEX ind_regop_ro_transfer_owner_id ON regop_reality_transfer (owner_id);";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            foreach (var periodId in periodIds)
            {
                var tableName = $"regop_reality_transfer_period_{periodId}";

                // создаем партиции и вставляем данные
                sql =
                    $@"CREATE TABLE IF NOT EXISTS {tableName}
                            (LIKE regop_reality_transfer INCLUDING ALL, 
                            		CONSTRAINT  {tableName}_constrain 
                            	CHECK (period_id = {periodId} ))
                            	INHERITS (regop_reality_transfer)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                sql =
                    $@"INSERT INTO {tableName}
                    SELECT 
                        t.id,
                        t.object_version,
                        t.object_create_date,
                        t.object_edit_date,
                        t.amount,
                        t.reason,
                        t.source_guid,
                        t.target_guid,
                        (case when not t.is_copy_for_source then t.originator_id else null end) as originator_id,
                        t.payment_date,
                        t.operation_date,
                        t.op_id,
                        t.is_indirect,
                        t.target_coef,
                        true, -- у дома всегда is_affect = true
                        t.is_loan,
                        t.is_return_loan,
                        t.originator_name,
                        t.period_id,
                        t.owner_id,
                        (case when t.is_copy_for_source then t.originator_id else null end) as copy_transfer_id
                    FROM regop_transfer_period_{periodId} t
                    JOIN regop_wallet w on w.owner_type = 20 -- трансферы дома
                      and w.owner_id = t.owner_id -- для оптимизации Join'а
                      and (w.wallet_guid = t.source_guid or w.wallet_guid = t.target_guid);";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                // внешний ключ на таблицу трансферов для копий (чтобы триггер не создавать :))
                sql =
                    $@"ALTER TABLE {tableName} ADD CONSTRAINT fk_regop_ro_tr_{periodId}_copy_transfer_id 
                    FOREIGN KEY (copy_transfer_id) REFERENCES regop_transfer_period_{periodId} (id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                sql =
                    $@"ALTER TABLE {tableName} ADD CONSTRAINT fk_regop_ro_tr_{periodId}_owner_id
                    FOREIGN KEY (owner_id) REFERENCES REGOP_RO_PAYMENT_ACCOUNT (id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                sql = $"CREATE UNIQUE INDEX ON {tableName}(copy_transfer_id) where copy_transfer_id is not null";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                // лечение трансферов, где is_affect = false
                sql =
                    $@"select tr.id from regop_transfer_period_{periodId} tr
                            join {tableName} tr1 on tr1.copy_transfer_id = tr.id 
                            where not tr.is_affect";

                var invalidIds = connection.Query<long>(sql, transaction: transaction, commandTimeout: this.CommandTimeOut).ToArray();
                if (invalidIds.Any())
                {
                    foreach (var ids in invalidIds.Section(10000))
                    {
                        sql =
                            $@"update regop_transfer_period_{periodId} t set is_affect = true
                                    where id in ({string
                                .Join(",", ids)})";
                        connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);
                    }
                }

                // удаляем трансферы, которые перенесли
                sql =
                    $@"delete from regop_transfer_period_{periodId} tr 
                         where exists (select null from regop_reality_transfer_period_{periodId} rto where rto.id = tr.id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);
            }

            sql = "ANALYZE regop_reality_transfer;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            // пересобираем секвенцию
            var maxId = connection.ExecuteScalar<long>("SELECT max(id) from regop_reality_transfer", transaction: transaction) + 1;
            sql = $"ALTER SEQUENCE regop_reality_transfer_id_seq RESTART WITH {maxId}";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            // создаём таблицу трансферов начислений
            sql = @"CREATE SEQUENCE public.regop_charge_transfer_id_seq
                  INCREMENT 1
                  MINVALUE 1
                  MAXVALUE 9223372036854775807
                  START 1070148
                  CACHE 1;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            sql = @"CREATE TABLE regop_charge_transfer
                (
                    id INTEGER DEFAULT nextval('regop_charge_transfer_id_seq'::regclass) PRIMARY KEY NOT NULL,
                    object_version BIGINT NOT NULL,
                    object_create_date TIMESTAMP NOT NULL,
                    object_edit_date TIMESTAMP NOT NULL,
                    amount NUMERIC(19,5) NOT NULL,
                    reason VARCHAR(1000),
                    source_guid VARCHAR(40) NOT NULL,
                    target_guid VARCHAR(40) NOT NULL,
                    originator_id BIGINT,
                    payment_date TIMESTAMP NOT NULL,
                    operation_date TIMESTAMP NOT NULL,
                    op_id BIGINT NOT NULL,
                    is_indirect BOOLEAN DEFAULT false NOT NULL,
                    target_coef SMALLINT DEFAULT 1 NOT NULL,
                    is_affect BOOLEAN DEFAULT false NOT NULL,
                    is_loan BOOLEAN DEFAULT false NOT NULL,
                    is_return_loan BOOLEAN DEFAULT false NOT NULL,
                    originator_name VARCHAR(150),
                    period_id BIGINT DEFAULT 0 NOT NULL,
                    owner_id BIGINT DEFAULT 0 NOT NULL,
                    CONSTRAINT fk_regop_ro_tr_op FOREIGN KEY (op_id) REFERENCES regop_money_operation (id),
                    CONSTRAINT fk_regop_ro_tr_period_id FOREIGN KEY (period_id) REFERENCES regop_period (id),
                    CONSTRAINT fk_regop_ro_tr_owner_id FOREIGN KEY (owner_id) REFERENCES regop_pers_acc (id)
                );

                CREATE INDEX ind_regop_ch_transfer_target ON regop_charge_transfer (target_guid);
                CREATE INDEX ind_regop_ch_transfer_source ON regop_charge_transfer (source_guid);
                CREATE INDEX ind_regop_ch_transfer_originator_id ON regop_charge_transfer (originator_id);
                CREATE INDEX ind_regop_ch_transfer_op_id ON regop_charge_transfer (op_id);
                CREATE INDEX ind_regop_ch_transfer_op_date_date ON regop_charge_transfer (date(operation_date));
                CREATE INDEX ON regop_charge_transfer (source_guid) WHERE reason IS null;
                CREATE INDEX ind_regop_ch_transfer_owner_id ON regop_reality_transfer (owner_id);";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            // создаем партиции
            foreach (var periodId in periodIds)
            {
                var tableName = $"regop_charge_transfer_period_{periodId}";

                // создаем партиции и вставляем данные
                sql =
                    $@"CREATE TABLE IF NOT EXISTS {tableName}
                            (LIKE regop_charge_transfer INCLUDING ALL, 
                            		CONSTRAINT  {tableName}_constrain 
                            	CHECK (period_id = {periodId}))
                            	INHERITS (regop_charge_transfer)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                sql =
                    $@"INSERT INTO {tableName}
                    SELECT 
                        t.id,
                        t.object_version,
                        t.object_create_date,
                        t.object_edit_date,
                        t.amount,
                        t.reason,
                        t.source_guid,
                        t.target_guid,
                        t.originator_id,
                        t.payment_date,
                        t.operation_date,
                        t.op_id,
                        t.is_indirect,
                        t.target_coef,
                        t.is_affect,
                        t.is_loan,
                        t.is_return_loan,
                        t.originator_name,
                        t.period_id,
                        t.owner_id
                    FROM regop_transfer_period_{periodId} t
                    JOIN regop_wallet w on w.owner_type = 10 -- трансферы ЛС
                      and w.owner_id = t.owner_id -- для оптимизации Join'а
                      and (w.wallet_guid = t.source_guid or w.wallet_guid = t.target_guid)
                    where not t.is_affect and (not t.is_indirect or t.reason is null);";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                // внешний ключ на ЛС для трансферов начислений
                sql =
                    $@"ALTER TABLE {tableName} ADD CONSTRAINT fk_regop_tr_{periodId}_owner_id
                    FOREIGN KEY (owner_id) REFERENCES REGOP_PERS_ACC (id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                // удаляем трансферы, которые перенесли
                sql =
                    $@"delete from regop_transfer_period_{periodId} tr 
                         where exists (select null from regop_charge_transfer_period_{periodId} rto where rto.id = tr.id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                // внешний ключ на ЛС для трансферов оплаты
                sql =
                    $@"ALTER TABLE regop_transfer_period_{periodId} ADD CONSTRAINT fk_regop_ch_tr_{periodId}_owner_id
                    FOREIGN KEY (owner_id) REFERENCES REGOP_PERS_ACC (id)";
                connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

                // проверка, что не осталось трансферов с начислениями
                sql =
                    $@"SELECT count(*) > 0 from regop_transfer_period_{periodId} tr 
                    where tr.reason in (
                        'Начисление пени',
                        'Начисление по базовому тарифу',
                        'Начисление по тарифу решения',
                        'Перерасчет пени',
                        'Перерасчет по тарифу решения',
                        'Перерасчет по базовому тарифу',
                        'Отмена начислений по базовому тарифу',
                        'Отмена начисления пени',
                        'Установка/изменение пени',
                        'Установка/изменение сальдо',
                        'Зачет средств за выполненные работы',
                        'Начисление по тарифу решения',
                        'Отмена начислений по тарифу решений',
                        'Отмена начислений по тарифу решения',
                        'Установка/изменение сальдо по пени',
                        'Отмена ручной корректировки пени',
                        'Отмена ручной корректировки по базовому тарифу',
                        'Отмена ручной корректировки по тарифу решения',
                        'Установка/изменение сальдо по тарифу решения',
                        'Установка/изменение сальдо по базовому тарифу')
                      or tr.reason is null and not is_affect and tr.amount > 0";

                if (connection.ExecuteScalar<bool>(sql, transaction: transaction, commandTimeout: this.CommandTimeOut))
                {
                    throw new ValidationException($"В таблице regop_transfer_period_{periodId} имеются неверные трансферы начислений");
                }

                // проверка, что не импортировали трансферы с оплатами
                sql =
                    $@"SELECT id from regop_charge_transfer_period_{periodId} tr 
                    where tr.reason in (
                        'Возврат взносов на КР по базовому тарифу',
                        'Возврат взносов на КР по тарифу решения',
                        'Возврат взносов на КР',
                        'Возврат МСП',
                        'Возврат оплаты по базовому тарифу',
                        'Возврат оплаты по тарифу решения',
                        'Возврат пени',
                        'Возврат средств по базовому тарифу',
                        'Возврат средств',
                        'Зачисление по базовому тарифу в счет отмены возврата средств',
                        'Зачисление по пеням в счет отмены возврата',
                        'Зачисление по тарифу решения в счет отмены возврата средств',
                        'Корректировка оплат по базовому тарифу',
                        'Корректировка оплат по пени',
                        'Корректировка оплат по тарифу решения',
                        'Оплата пени',
                        'Оплата по базовому тарифу',
                        'Оплата по тарифу решения',
                        'Отмена оплата пени',
                        'Отмена оплаты пени',
                        'Отмена оплаты по базовому тарифу',
                        'Отмена оплаты по тарифу решения',
                        'Отмена оплаты',
                        'Отмена поступления за аренду',
                        'Отмена поступления по соц. поддержке',
                        'Отмена поступления средств за ранее выполененные работы'
                        'Отмена поступления средств за ранее выполненные работы',
                        'Отмены оплаты по мировому соглашению'
                        'Отмены поступления ранее накопленных средств',
                        'Поступление за проделанные работы',
                        'Региональная субсидия',
                        'Субсидия фонда')
                      or tr.reason is null and tr.amount < 0";

                var dataList = connection.Query<long>(sql, transaction: transaction, commandTimeout: this.CommandTimeOut).ToList();
                if (dataList.Any())
                {
                    throw new ValidationException(
                        $"В таблице regop_charge_transfer_period_{periodId} имеются неверные трансферы оплат:\r\n" +
                            $"{dataList.AggregateWithSeparator(x => x.ToString(), "\r\n")}");
                }
            }

            sql = "ANALYZE regop_charge_transfer;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            // пересобираем секвенцию
            maxId = connection.ExecuteScalar<long>("SELECT max(id) from regop_charge_transfer", transaction: transaction) + 1;
            sql = $"ALTER SEQUENCE regop_charge_transfer_id_seq RESTART WITH {maxId}";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            var countRo = connection.ExecuteScalar<long>("SELECT count(*) from regop_reality_transfer", transaction: transaction);
            var countPa = connection.ExecuteScalar<long>("SELECT count(*) from regop_transfer", transaction: transaction);
            var countPaCharge = connection.ExecuteScalar<long>("SELECT count(*) from regop_charge_transfer", transaction: transaction);

            // проверяем, что количество совпало. УРА!
            if (countRo + countPa + countPaCharge != countBefore)
            {
                throw new ValidationException("Количество трансферов не совпадает");
            }

            // удаляем столбец is_copy_for_source
            sql = "alter table regop_transfer drop column is_copy_for_source;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            var listCommands = triggerList;

            sql =
                "SELECT 'ALTER TABLE ' || tablename || ' DROP COLUMN IF EXISTS is_copy_for_source' as command from pg_tables where tablename like 'regop_transfer_period_%'";
            using (var reader = connection.ExecuteReader(sql, transaction: transaction))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            // добавляем foreign key в основные трансферы
            sql =
                "SELECT 'ALTER TABLE ' || tablename || '  ADD CONSTRAINT fk_regop_' || tablename ||'_owner_id FOREIGN KEY (owner_id) REFERENCES regop_pers_acc (id)' as command from pg_tables where tablename like 'regop_transfer_period_%'";
            using (var reader = connection.ExecuteReader(sql, transaction: transaction))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            foreach (var command in listCommands)
            {
                connection.Execute(command, transaction: transaction);
            }

            // создаем BEFORE триггер на вставку в мастер-таблицу
            this.CreateTriggersAndConstraints(connection, transaction, "regop_reality_transfer", "ro");
            this.CreateTriggersAndConstraints(connection, transaction, "regop_charge_transfer", "ch");

            connection.Execute("alter table regop_wallet drop column owner_id;", transaction: transaction);
        }

        private void CreateTriggersAndConstraints(
            IDbConnection connection,
            IDbTransaction transaction,
            string tableName,
            string alias)
        {
            // триггер вставки в партицию
            var sql =
                $@"CREATE TRIGGER partitioning_trigger_regop_{alias}_transfer
                            BEFORE INSERT ON public.{tableName}
                            FOR EACH ROW
                            EXECUTE PROCEDURE public.insert_into_partition_by_period_id(); ";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            //создаем AFTER триггер на вставку в мастер-таблицу - костыль для работы с NH (он требует ожидаемое значение affected_rows)
            sql =
                $@"CREATE TRIGGER partitioning_trigger_regop_{alias}_transfer_after
                            AFTER INSERT
                            ON public.{tableName}
                            FOR EACH STATEMENT
                            EXECUTE PROCEDURE public.delete_from_master_by_period_id();";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            // ограничение
            sql =
                $@"CREATE OR REPLACE FUNCTION public.delete_from_{alias}_transfer()
                             RETURNS TRIGGER AS $BODY$  
                            BEGIN  
                                IF  (SELECT COUNT(1)>0 FROM public.{tableName} WHERE originator_id=OLD.id)
                                THEN 
                                RAISE EXCEPTION 'Операция удаления нарушает ограничение, originator_id=%', OLD.id
                                    USING HINT = 'Прежде чем удалить трансфер удалите трансферы от которых он зависит ({tableName}.originator_id->{tableName}.id)';
                                END IF;
                              RETURN OLD;
                            END ;
                             $BODY$ 
                            LANGUAGE plpgsql VOLATILE COST 1;";
            connection.Execute(sql, transaction: transaction, commandTimeout: this.CommandTimeOut);

            var listCommands = new List<string>();

            sql =
                $@"select concat('CREATE TRIGGER delete_',tablename,'_tr 
                             BEFORE DELETE ON public.',tablename,' 
                             FOR EACH ROW EXECUTE PROCEDURE public.delete_from_{alias}_transfer();') as command
                             from pg_tables where schemaname='public' AND tablename like '{tableName}_period_%'";

            using (var reader = connection.ExecuteReader(sql, transaction: transaction))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            sql =
                $@"SELECT CONCAT('ALTER TABLE ',schemaname,'.',tablename,
                           ' ADD CONSTRAINT fk_',tablename,'_op FOREIGN KEY (op_id)
                           REFERENCES ',schemaname,'.','regop_money_operation ', '(id)',';') as command
                           FROM pg_tables WHERE schemaname='public' AND tablename LIKE '{tableName}_period_%' ";

            using (var reader = connection.ExecuteReader(sql, transaction: transaction))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            sql = $"SELECT 'CREATE INDEX ON ' || tablename || ' (owner_id)' as command from pg_tables where tablename like '{tableName}_period_%'";
            using (var reader = connection.ExecuteReader(sql, transaction: transaction))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            sql = $"SELECT 'CREATE INDEX ON ' || tablename || ' (originator_id)' as command from pg_tables where tablename like '{tableName}_period_%'";
            using (var reader = connection.ExecuteReader(sql, transaction: transaction))
            {
                while (reader.Read())
                {
                    listCommands.Add(reader["command"].ToString());
                }
            }

            foreach (var command in listCommands)
            {
                connection.Execute(command, transaction: transaction);
            }
        }
    }
}