namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Extensions;

    using Dapper;

    /// <summary>
    /// Действие предварительной выверки перед разделением трансферов
    /// </summary>
    public class ValidateTransfersBeforeSplittingAction : BaseExecutionAction
    {
        private const string sql = @"-- Сохдать схему если ее нет
                do
                $$
                declare ""__sql"" varchar;
	                ""__r"" record;
                begin
                        if not exists (select * from pg_namespace where nspname = 'service')
                        then
                        ""__sql"" = 'create schema service;';
                        execute format(""__sql"");
                        end if;
                end;
                $$ language plpgsql;

                -- бекапы на всякий случай
                --select * into service.dump_regop_transfer_14822 from regop_transfer;
                --select * into service.dump_regop_wallet_14822 from regop_wallet;

                -- Настройка причин трансферов
                drop table if exists service.reasons_for_fix;

                create table service.reasons_for_fix (id int, reason varchar(1000), reason_group varchar(50));

                -- Список причин трансферов (только ЛС), для добавления нового просто добавить в конец с новым кодом и на какое поле влияет
                insert into service.reasons_for_fix (id, reason, reason_group) values
                (1,'Возврат взносов на КР','tariff_desicion_payment'),
                (1,'Возврат взносов на КР','tariff_payment'),
                (2,'Возврат взносов на КР по базовому тарифу','tariff_payment'),
                (3,'Возврат пени','penalty_payment'),
                (4,'Возврат средств','tariff_desicion_payment'),
                (4,'Возврат средств','tariff_payment'),
                (4,'Возврат средств','penalty_payment'),
                (5,'Возврат средств по базовому тарифу','tariff_payment'),
                (6,'Зачисление по базовому тарифу в счет отмены возврата средств','tariff_payment'),
                (7,'Корректировка оплат по базовому тарифу','tariff_payment'),
                (8,'Корректировка оплат по пени','penalty_payment'),
                (9,'Начисление пени','penalty'),
                (10,'Начисление по базовому тарифу','charge_base_tariff'),
                (10,'Начисление по базовому тарифу','charge_tariff'),
                (11,'Оплата пени','penalty_payment'),
                (12,'Оплата по базовому тарифу','tariff_payment'),
                (13,'Отмена начислений по базовому тарифу','charge_tariff'),
                (13,'Отмена начислений по базовому тарифу','charge_base_tariff'),
                (14,'Отмена начисления пени','penalty'),
                (15,'Отмена оплаты пени','penalty_payment'),
                (16,'Отмена оплаты по базовому тарифу','tariff_payment'),
                (17,'Перерасчет пени','recalc_penalty'),
                (18,'Перерасчет по базовому тарифу','recalc'),
                (21,'Установка/изменение пени','penalty_balance_change'),
                (22,'Установка/изменение сальдо','balance_change'),
                (23,'','charge_tariff'),
                (23,'','tariff_payment'),
                (23,'','tariff_desicion_payment'),
                (23,'','penalty'),
                (23,'','charge_base_tariff'),
                (23,'','penalty_payment'),
                (24,'Перенос долга при слиянии','penalty'),
                (24,'Перенос долга при слиянии','tariff_payment'),
                (24,'Перенос долга при слиянии','tariff_desicion_payment'),
                (24,'Перенос долга при слиянии','penalty_payment'),
                (24,'Перенос долга при слиянии','charge_base_tariff'),
                (24,'Перенос долга при слиянии','charge_tariff'),
                (25,'Зачет средств за выполненные работы','perf_work_charge'),
                (25,'Зачет средств за выполненные работы','balance_change'),
                (25,'Зачет средств за выполненные работы','dec_balance_change'),
                (26,'Оплата по тарифу решения','tariff_desicion_payment'),
                (27,'Отмена оплата пени','penalty_payment'),
                (28,'Отмена оплаты','penalty_payment'),
                (28,'Отмена оплаты','tariff_desicion_payment'),
                (28,'Отмена оплаты','tariff_payment'),
                (29,'Перерасчет','recalc'),
                (29,'Перерасчет','recalc_penalty'),
                (29,'Перерасчет','recalc_decision'),
                (30,'Зачисление по пеням в счет отмены возврата','penalty_payment'),
                (31,'Зачисление по тарифу решения в счет отмены возврата средств','tariff_desicion_payment'),
                (32,'Корректировка оплат по тарифу решения','tariff_desicion_payment'),
                (33,'Начисление по тарифу решения','charge_tariff'),
                (34,'Отмена начислений по тарифу решений','charge_tariff'),
                (35,'Отмена начислений по тарифу решения','charge_tariff'),
                (36,'Отмена оплаты по тарифу решения','tariff_desicion_payment'),
                (37,'Перерасчет по тарифу решения','recalc_decision'),
                (38,'Установка/изменение сальдо по пени','penalty_balance_change'),
                (39,'Установка/изменение сальдо по тарифу решения','dec_balance_change'),
                (40,'Установка/изменение сальдо по базовому тарифу','balance_change'),
                (41,'Поступление за проделанные работы','tariff_payment'),
                (42,'Отмена поступления средств за ранее выполененные работы','tariff_payment'),
                (43,'Отмена ручной корректировки пени','penalty_balance_change'),
                (44,'Возврат оплаты пени','penalty_payment'),
                (45,'Возврат оплаты по базовому тарифу','tariff_payment'),
                (46,'Возврат оплаты по тарифу решения','tariff_desicion_payment'),
                (47,'Оплачено пени','penalty_payment'),
                (48,'Оплачено по базовому тарифу','tariff_payment'),
                (49,'Оплачено по тарифу решения','tariff_desicion_payment'),
                (50,'Отмена ручной корректировки по базовому тарифу','charge_base_tariff'),
                (50,'Отмена ручной корректировки по базовому тарифу','charge_tariff'),
                -- Добавлен из test_rt
                (51, 'Начислено по базовому тарифу', 'charge_tariff'),
                (51, 'Начислено по базовому тарифу', 'charge_base_tariff')
                ;

                -- Выберем guid и id всех кошельков
                drop table if exists service.all_wallet_for_test;

                create table service.all_wallet_for_test (wallet_id int, wallet_guid varchar(40), w_ls boolean, w_house boolean, w_type varchar(10));

                insert into service.all_wallet_for_test (wallet_id, wallet_guid)
                select w.id, w.wallet_guid
                    from regop_wallet w;

                -- Индекс на wallet_id
                CREATE INDEX ix_w_for_test_wallet_id ON service.all_wallet_for_test (wallet_id);

                -- Проставляем признак ЛС, Дом, Тип
                -- ЛС
                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'bt'
                from regop_pers_acc a
                where a.bt_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'dt'
                from regop_pers_acc a
                where a.dt_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'p'
                from regop_pers_acc a
                where a.p_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'r'
                from regop_pers_acc a
                where a.r_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'ss'
                from regop_pers_acc a
                where a.ss_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'pwp'
                from regop_pers_acc a
                where a.pwp_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'af'
                from regop_pers_acc a
                where a.af_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_ls = true,
                w_type = 'raa'
                from regop_pers_acc a
                where a.raa_wallet_id = w.wallet_id;

                -- Дома
                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'bt'
                from regop_ro_payment_account a
                where a.bt_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'dt'
                from regop_ro_payment_account a
                where a.dt_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'p'
                from regop_ro_payment_account a
                where a.p_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'fsu'
                from regop_ro_payment_account a
                where a.fsu_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'rsu'
                from regop_ro_payment_account a
                where a.rsu_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'ssu'
                from regop_ro_payment_account a
                where a.ssu_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'tsu'
                from regop_ro_payment_account a
                where a.tsu_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'os'
                from regop_ro_payment_account a
                where a.os_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'bp'
                from regop_ro_payment_account a
                where a.bp_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'af'
                from regop_ro_payment_account a
                where a.af_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'pwp'
                from regop_ro_payment_account a
                where a.pwp_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'r'
                from regop_ro_payment_account a
                where a.r_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'ss'
                from regop_ro_payment_account a
                where a.ss_wallet_id = w.wallet_id;

                update service.all_wallet_for_test w set
                w_house = true,
                w_type = 'raa'
                from regop_ro_payment_account a
                where a.raa_wallet_id = w.wallet_id;

                -- Заполним трансферы, для быстроты сделаем цикл по каждому периоду

                -- Периоды которые нужно проверить
                drop table if exists _reg_period;
                select *
                    into temp _reg_period
                    from regop_period
                    --where id in (2836)       -- Если нужно по определенному - указываем и откоментариваем, если нужо по всем, то коментим
                    ;

                -- Трансферы
                do
                $$
                declare ""__sql"" varchar;
	                ""__r"" record;

                begin
	                for ""__r"" in select * from _reg_period
	                loop

                        -- Таблица в которой будут находится все трансферы
                        ""__sql"" = 'drop table if exists service.all_transfer_for_test_period_' || ""__r"".id || ';';
                        execute format(""__sql"");

                        ""__sql"" = 'create table service.all_transfer_for_test_period_' || ""__r"".id || ' (transfer_id int, originator_id int, period_id int, reason varchar(1000), source_guid varchar(40),
                                                                                                        target_guid varchar(40), tr_ls boolean, tr_house boolean, is_copy_for_source boolean, join_target boolean,
                                                                                                        join_source boolean, is_affect boolean, wallet_id int, amount numeric(16,5));';
                        execute format(""__sql"");

		                ""__sql"" = 'insert into service.all_transfer_for_test_period_' || ""__r"".id || '(transfer_id, originator_id, period_id, reason, source_guid, target_guid, is_copy_for_source, is_affect, amount)
			                select tr.id, tr.originator_id, tr.period_id, tr.reason, tr.source_guid, tr.target_guid, tr.is_copy_for_source, tr.is_affect, tr.amount
                                from regop_transfer_period_' || ""__r"".id ||' tr';
		                execute format(""__sql"");

                        -- Проставим признаки в трансферы
                        ""__sql"" = '
                        update service.all_transfer_for_test_period_' || ""__r"".id || ' t set
                        tr_ls = coalesce(t.tr_ls, w.w_ls),
                        tr_house = coalesce(t.tr_house, w.w_house),
                        join_target = true,
                        wallet_id = w.wallet_id
                        from service.all_wallet_for_test w
                        where w.wallet_guid = t.target_guid;';
                        execute format(""__sql"");

                        ""__sql"" = '
                        update service.all_transfer_for_test_period_' || ""__r"".id || ' t set
                        tr_ls = coalesce(t.tr_ls, w.w_ls),
                        tr_house = coalesce(t.tr_house, w.w_house),
                        join_source = true,
                        wallet_id = w.wallet_id
                        from service.all_wallet_for_test w
                        where w.wallet_guid = t.source_guid;';
                        execute format(""__sql"");

                        -- Лишние трансферы
                        -- В отдельную таблицу
                        ""__sql"" = 'drop table if exists service.tr_for_del_period_' || ""__r"".id || ';';
                        execute format(""__sql"");

                        ""__sql"" = 'create table service.tr_for_del_period_' || ""__r"".id || ' (transfer_id int, originator_id int, period_id int, reason varchar(1000), source_guid varchar(40),
                                                                                            target_guid varchar(40), tr_ls boolean, tr_house boolean, is_copy_for_source boolean, join_target boolean,
                                                                                            join_source boolean, is_affect boolean, wallet_id int, amount numeric(16,5));';
                        execute format(""__sql"");

                        ""__sql"" = '
                        insert into service.tr_for_del_period_' || ""__r"".id || '
                        select *
                            from service.all_transfer_for_test_period_' || ""__r"".id || '
                            where tr_ls is null
                                and tr_house is null;';
                        execute format(""__sql"");

                        -- удалим из общей
                        ""__sql"" = '
                        delete
                            from service.all_transfer_for_test_period_' || ""__r"".id || '
                            where tr_ls is null
                                and tr_house is null;';
                        execute format(""__sql"");

                        -- Удаляяем Копии зависимых трансферов
                        ""__sql"" = '
                        delete
                            from regop_transfer_period_' || ""__r"".id || ' trc
                            using service.tr_for_del_period_' || ""__r"".id || ' td, regop_transfer_period_' || ""__r"".id || ' tr
                            where tr.originator_id = td.transfer_id
                                and trc.originator_id = tr.id;';
                        execute format(""__sql"");

                        -- Удаляем Зависимые трансферы
                        ""__sql"" = '
                        delete
                            from regop_transfer_period_' || ""__r"".id || ' tr
                            using service.tr_for_del_period_' || ""__r"".id || ' td
                            where tr.originator_id = td.transfer_id;';
                        execute format(""__sql"");

                        -- Удаляем сами трансферы которые не прикреплены ни к дому ни к ЛС
                        ""__sql"" = '
                        delete
                            from regop_transfer_period_' || ""__r"".id || ' tr
                            using service.tr_for_del_period_' || ""__r"".id || ' td
                            where tr.id = td.transfer_id;';
                        execute format(""__sql"");

                        -- Левые копии, вроде как копия, но без признака is_copy_for_source
                        -- Удаляем такие
                        ""__sql"" = 'drop table if exists service.invalid_double_copy_tr_for_del_period_' || ""__r"".id || ';';
                        execute format(""__sql"");

                        ""__sql"" = 'create table service.invalid_double_copy_tr_for_del_period_' || ""__r"".id || ' (transfer_id int);';
                        execute format(""__sql"");

                        -- Сохраняем
                        ""__sql"" = '
                        insert into service.invalid_double_copy_tr_for_del_period_' || ""__r"".id || ' (transfer_id)
                        select transfer_id
                            from service.all_transfer_for_test_period_' || ""__r"".id || '
                            where tr_ls = true
                                and tr_house = true
                                and is_copy_for_source = false
                                and originator_id is not null;';
                        execute format(""__sql"");

                        -- Удаляем из дальнейшего анализа
                        ""__sql"" = '
                        delete
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' tr
                            using service.invalid_double_copy_tr_for_del_period_' || ""__r"".id || ' td
                            where td.transfer_id = tr.transfer_id;';
                        execute format(""__sql"");

                        -- Удаляем
                        ""__sql"" = '
                        delete
                            from regop_transfer_period_' || ""__r"".id || ' tr
                            using service.invalid_double_copy_tr_for_del_period_' || ""__r"".id || ' td
                            where td.transfer_id = tr.id;';
                        execute format(""__sql"");

                        -- Дубли копий на дом
                        ""__sql"" = 'drop table if exists service.double_copy_tr_for_del_period_' || ""__r"".id || ';';
                        execute format(""__sql"");

                        ""__sql"" = 'create table service.double_copy_tr_for_del_period_' || ""__r"".id || ' (originator_id int, count int, transfer_id int);';
                        execute format(""__sql"");

                        ""__sql"" = '
                        insert into service.double_copy_tr_for_del_period_' || ""__r"".id || ' (originator_id, count, transfer_id)
                        select originator_id, count(*) count, min(transfer_id) transfer_id
                            from service.all_transfer_for_test_period_' || ""__r"".id || '
                            where coalesce(is_copy_for_source, false) = true
                            group by originator_id
                            having count(*) > 1;';
                        execute format(""__sql"");

                        -- Удаляем дубли если есть
                        ""__sql"" = '
                        delete
                            from regop_transfer_period_' || ""__r"".id || ' tr
                            using service.double_copy_tr_for_del_period_' || ""__r"".id || ' c, service.all_transfer_for_test_period_' || ""__r"".id || ' t
                            where t.originator_id = c.originator_id
                                and coalesce(t.is_copy_for_source, false) = true
                                and t.transfer_id != c.transfer_id
                                and tr.id = t.transfer_id;';
                        execute format(""__sql"");

                        -- Удаляем из дальнейшего анализа
                        ""__sql"" = '
                        delete
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' t
                            using service.double_copy_tr_for_del_period_' || ""__r"".id || ' c
                            where t.originator_id = c.originator_id
                                and coalesce(t.is_copy_for_source, false) = true
                                and t.transfer_id != c.transfer_id;';
                        execute format(""__sql"");

                        -- Проверка и удаление копий трансферов начислений на дом
                        ""__sql"" = 'drop table if exists service.transfer_for_del_copy_charge_' || ""__r"".id || ';';
                        execute format(""__sql"");

                        ""__sql"" = 'create table service.transfer_for_del_copy_charge_' || ""__r"".id || ' (transfer_id int);';
                        execute format(""__sql"");

                        -- Если есть копии (is_copy_for_source = true)
                        ""__sql"" = '
                        insert into service.transfer_for_del_copy_charge_' || ""__r"".id || ' (transfer_id)
                        select td.transfer_id
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' td
                                inner join service.all_transfer_for_test_period_' || ""__r"".id || ' t
                                    on t.transfer_id = td.originator_id
                                inner join service.reasons_for_fix r
                                    on coalesce(r.reason, '''') = coalesce(t.reason, '''')
                            where coalesce(td.tr_house, false) = true
                                and td.is_copy_for_source = true
                                and r.reason_group not in (''tariff_payment'', ''penalty_payment'', ''tariff_desicion_payment'')
                            group by td.transfer_id;
                                ';
                        execute format(""__sql"");

                        -- Начисление на дом
                        ""__sql"" = '
                        insert into service.transfer_for_del_copy_charge_' || ""__r"".id || ' (transfer_id)
                        select t.transfer_id
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' t
                                inner join service.reasons_for_fix r
                                    on coalesce(r.reason, '''') = coalesce(t.reason, '''')
                            where coalesce(t.tr_house, false) = true
                                and r.reason_group not in (''tariff_payment'', ''penalty_payment'', ''tariff_desicion_payment'')
                            group by t.transfer_id;';
                        execute format(""__sql"");

                        -- Удаляем из дальнейшего анализа
                        ""__sql"" = '
                        delete
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' tr
                            using service.transfer_for_del_copy_charge_' || ""__r"".id || ' td
                            where td.transfer_id = tr.transfer_id;';
                        execute format(""__sql"");

                        -- Удаляем из базы
                        ""__sql"" = '
                        delete
                            from regop_transfer_period_' || ""__r"".id || ' tr
                            using service.transfer_for_del_copy_charge_' || ""__r"".id || ' td
                            where td.transfer_id = tr.id;';
                        execute format(""__sql"");

                        -- Исправляем is_affect
                        ""__sql"" = 'drop table if exists service.transfer_for_update_affect_period_' || ""__r"".id || ';';
                        execute format(""__sql"");

                        ""__sql"" = 'create table service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id int, new_affect boolean);';
                        execute format(""__sql"");

                        -- Чистые трансферы домов
                        ""__sql"" = '
                        insert into service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id, new_affect)
                        select transfer_id, true
                            from service.all_transfer_for_test_period_' || ""__r"".id || '
                            where coalesce(tr_house, false) = true
                                and is_copy_for_source = false
                                and is_affect = false;';
                        execute format(""__sql"");

                        -- Остальные трансферы домов
                        -- Копии на дом
                        ""__sql"" = '
                        insert into service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id, new_affect)
                        select transfer_id, true
                            from service.all_transfer_for_test_period_' || ""__r"".id || '
                            where coalesce(tr_house, false) = true
                                and is_copy_for_source = true
                                and is_affect = false;';
                        execute format(""__sql"");

                        -- Трансферы ЛС
                        -- Оплаты
                        ""__sql"" = '
                        insert into service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id, new_affect)
                        select transfer_id, true
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' t
                                inner join service.reasons_for_fix r
                                    on coalesce(r.reason, '''') = coalesce(t.reason, '''')
                            where coalesce(t.tr_house, false) = false
                                and r.id not in (23, 24)
                                and r.reason_group in (''tariff_payment'', ''penalty_payment'', ''tariff_desicion_payment'')
                                and t.is_affect = false
                            group by transfer_id;';
                        execute format(""__sql"");

                        -- Остальные
                        ""__sql"" = '
                        insert into service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id, new_affect)
                        select transfer_id, false
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' t
                                inner join service.reasons_for_fix r
                                    on coalesce(r.reason, '''') = coalesce(t.reason, '''')
                            where coalesce(t.tr_house, false) = false
                                and r.id not in (23, 24)
                                and r.reason_group not in (''tariff_payment'', ''penalty_payment'', ''tariff_desicion_payment'')
                                and t.is_affect = true
                            group by transfer_id;';
                        execute format(""__sql"");

                        -- Пренос долга при слиянии
                        ""__sql"" = '
                        insert into service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id, new_affect)
                        select transfer_id, false
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' t
                                inner join service.reasons_for_fix r
                                    on coalesce(r.reason, '''') = coalesce(t.reason, '''')
                            where coalesce(t.tr_house, false) = false
                                and r.id in (23, 24)
                                and t.is_affect = true
                                and t.amount > 0
                            group by transfer_Id;';
                        execute format(""__sql"");

                        ""__sql"" = '
                        insert into service.transfer_for_update_affect_period_' || ""__r"".id || ' (transfer_id, new_affect)
                        select transfer_id, true
                            from service.all_transfer_for_test_period_' || ""__r"".id || ' t
                                inner join service.reasons_for_fix r
                                    on coalesce(r.reason, '''') = coalesce(t.reason, '''')
                            where coalesce(t.tr_house, false) = false
                                and r.id in (23, 24)
                                and t.is_affect = false
                                and t.amount < 0
                            group by transfer_id;';
                        execute format(""__sql"");

                        -- обновляем
                        ""__sql"" = '
                        update regop_transfer_period_' || ""__r"".id || ' tr set
                        is_affect = u.new_affect
                        from service.transfer_for_update_affect_period_' || ""__r"".id || ' u
                        where u.transfer_id = tr.id;';
                        execute format(""__sql"");

	                end loop;

                end;
                $$ language plpgsql;

                -- Лишние кошельки
                -- вынесем в отдельную таблицу
                drop table if exists service.w_for_del;
                select a.*
                    into service.w_for_del
                    from regop_wallet w
                        inner join service.all_wallet_for_test a
                            on a.wallet_id = w.id
                    where a.w_house is null
                        and a.w_ls is null;

                -- удаляем из общей таблицы
                delete
                    from service.all_wallet_for_test
                    where w_house is null
                        and w_ls is null;

                -- удаляем из базы
                delete
                    from regop_wallet w
                    using service.w_for_del wd
                    where wd.wallet_id = w.id;
                ";

        /// <inheritdoc />
        public override string Code => nameof(ValidateTransfersBeforeSplittingAction);

        /// <inheritdoc />
        public override string Name => "Рефакторинг - Подготовка данных перед разделением";

        /// <inheritdoc />
        public override string Description => "Действие выполняет подготовку данных перед разделением";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;
       

        private BaseDataResult Execute()
        {
            this.Container.Resolve<ISessionProvider>()
                .InStatelessConnectionTransaction(
                    (cn, tr) => { cn.Execute(ValidateTransfersBeforeSplittingAction.sql, transaction: tr, commandTimeout: 60 * 60 * 100); });

            return new BaseDataResult();
        }
    }
}