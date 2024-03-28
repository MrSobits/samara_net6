namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Проверка - Отсутствие копий трансферов в доме по лицевым счетам
    /// </summary>
    public class NotExistingCopiesTransfersChecker : IPeriodCloseChecker
    {
        #region Transfer Reasons
        /// <summary>
        /// Тип кошелька и виды оплат и возвратов/отмен по ним.
        /// <returns>Внимание! в массиве всего 2 элемента, отмена записана в формате, чтобы сразу вставить в sql как in (...)</returns>
        /// </summary>
        private readonly Dictionary<string, Tuple<string[], string[]>> walletNameReasonDict = new Dictionary<string, Tuple<string[], string[]>>
        {
                {
                    "bt_wallet_id",
                    new Tuple<string[], string[]>(
                        new[]
                            {
                                "Оплата по базовому тарифу",
                                "Зачисление по базовому тарифу в счет отмены возврата средств",
                                "Корректировка оплат по базовому тарифу"
                            },
                        new[]
                            {
                                "Отмена оплаты по базовому тарифу",
                                "Возврат оплаты по базовому тарифу",
                                "Возврат взносов на КР",
                                "Возврат взносов на КР по базовому тарифу"
                            })
                },
                {
                    "dt_wallet_id",
                    new Tuple<string[], string[]>(
                        new[]
                            {
                                "Оплата по тарифу решения",
                                "Зачисление по тарифу решения в счет отмены возврата средств",
                                "Корректировка оплат по тарифу решения"
                            },
                        new[]
                            {
                                "Отмена оплаты по тарифу решения",
                                "Возврат оплаты по тарифу решения",
                                "Возврат взносов на КР",
                                "Возврат взносов на КР по тарифу решения"
                            })
                },
                {
                    "p_wallet_id",
                    new Tuple<string[], string[]>(
                        new[]
                            {
                                "Оплата пени",
                                "Зачисление по пени в счет отмены возврата средств",
                                "Зачисление по пеням в счет отмены возврата",
                                "Корректировка оплат по пени"
                            },
                        new[]
                            {
                                "Возврат пени",
                                "Отмена оплаты пени"
                            })
                },
                {
                    "ss_wallet_id",
                    new Tuple<string[], string[]>(
                        new[] { "Поступление денег соц. поддержки" },
                        new[]
                            {
                                "Возврат МСП",
                                "Отмена поступления по соц. поддержке"
                            })
                },
                {
                    "af_wallet_id",
                    new Tuple<string[], string[]>(
                        new[]
                            {
                                "Ранее накопленные средства",
                                "Поступление ранее накопленных средств"
                            },
                        new[] { "Отмены поступления ранее накопленных средств" })
                },
                {
                    "r_wallet_id",
                    new Tuple<string[], string[]>(
                        new[] { "Поступление оплаты аренды" },
                        new[] { "Отмена поступления за аренду" })
                },
                {
                    "pwp_wallet_id",
                    new Tuple<string[], string[]>(
                        new[] { "Поступление за проделанные работы" },
                        new[] { "Отмена поступления средств за ранее выполненные работы" })
                },
                {
                    "raa_wallet_id",
                    new Tuple<string[], string[]>(
                        new[] { "Оплата по мировому соглашению" },
                        new[] { "Отмена оплаты по мировому соглашению" })
                }
        };
        #endregion

        /// <summary>
        /// Код проверки
        /// </summary>
        public static readonly string Id = typeof(NotExistingCopiesTransfersChecker).FullName;

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Репозиторий периодов
        /// </summary>
        public IRepository<ChargePeriod> PeriodRepository { get; set; }

        /// <summary>
        /// Системный код проверки
        /// </summary>
        public string Impl => NotExistingCopiesTransfersChecker.Id;

        /// <summary>
        /// Бессмысленный код проверки, для отображения
        /// </summary>
        public string Code => "7";

        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name => "П - Отсутствие копий трансферов в доме по лицевым счетам";

        /// <summary>
        /// Выполнить проверку
        /// </summary>
        /// <param name="periodId">Идентификатор проверяемого периода</param>
        /// <returns>Результат проверки</returns>
        public PeriodCloseCheckerResult Check(long periodId)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var result = new PeriodCloseCheckerResult();
            try
            {
                var session = sessionProvider.GetCurrentSession();
                result.Success = true;

                var period = this.PeriodRepository.Get(periodId);
                if (period == null)
                {
                    throw new ValidationException("Не найден период начислений");
                }

                var transfersDiff = new List<QueryDto>();
                this.walletNameReasonDict.ForEach(x =>
                {
                    this.GetTransfersDiff(session, period, x, true).AddTo(transfersDiff);
                    this.GetTransfersDiff(session, period, x, false).AddTo(transfersDiff);
                });

                if (transfersDiff.Count > 0)
                {
                    var transferDict = transfersDiff
                        .GroupBy(x => x.WalletKey)
                        .ToDictionary(
                            x => x.Key, 
                            x => x.GroupBy(y => y.IsPayment)
                                  .ToDictionary(y => y.Key, y => y.Select(z => z.Id).ToArray()));

                    transferDict.ForEach(x =>
                    {
                        // true - оплаты, false - возвраты/отмены
                        this.CopyPaymentTransfersToRo(session, x.Key, x.Value.Get(true) ?? new long[0], period);     
                        this.CopyRefundTransfersToRo(session, x.Key, x.Value.Get(false) ?? new long[0], period);     
                    });

                    var transfersDiffAfterCopying = new List<QueryDto>();
                    this.walletNameReasonDict.ForEach(x =>
                    {
                        this.GetTransfersDiff(session, period, x, true).AddTo(transfersDiffAfterCopying);
                        this.GetTransfersDiff(session, period, x, false).AddTo(transfersDiffAfterCopying);
                    });

                    result.Note = $"Была запущена процедура копирования трансферов на дом. Ошибок до: {transfersDiff.Count}, после: {transfersDiffAfterCopying.Count}";

                    result.FullLog.AppendLine("Адрес дома;Лицевой счет;Сумма;Название трансфера\r\n");
                    foreach (var tr in transfersDiff)
                    {
                        result.FullLog.AppendFormat(
                            "Скопирован трансфер на дом \"{0}\";{1};{2};{3}\r\n",
                            tr.Address,
                            tr.AccountNumber,
                            tr.TransferSum,
                            tr.TransferReason);
                    }

                    result.Success = transfersDiffAfterCopying.Count == 0;
                }

                if (!result.Success)
                {
                    result.InvalidAccountIds = transfersDiff.Select(x => x.AccountId).ToList();
                    result.Log.AppendLine("Номер ЛС;Адрес;Дата операции;Тип трансфера;Сумма трансфера");
                    foreach (var tr in transfersDiff)
                    {
                        result.Log.AppendLine($"\"{tr.AccountNumber}\";{tr.Address};{tr.PaymentDate};{tr.TransferReason};{tr.TransferSum}");
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(sessionProvider);
            }
        }

        private void CopyRefundTransfersToRo(ISession session, string walletKey, long[] transferIds, ChargePeriod period)
        {
            if (transferIds.IsEmpty())
            {
                return;
            }

            var query = session.CreateSQLQuery($@"
                insert into regop_reality_transfer_period_{period.Id}
                    (object_version, object_create_date, object_edit_date,
                    source_guid, target_guid, target_coef,
                    op_id, amount, reason,
                    originator_name, operation_date, is_indirect,
                    copy_transfer_id, is_affect,
                    is_loan, is_return_loan, payment_date, period_id, owner_id)
                select
                    0, now(), now(),
                    w2.wallet_guid, t.target_guid, t.target_coef,
                    t.op_id, t.amount, 
                    case when t.reason <> 'Возврат взносов на КР' then t.reason
                        else t.reason || '{this.GetTransferInfo(walletKey)}' end as reason,
                    t.originator_name, t.operation_date, t.is_indirect,
                    t.id, t.is_affect,
                    false, false, t.payment_date,
                    t.period_id,
                    rpa.id
                from regop_transfer_period_{period.Id} t
                join regop_wallet w on w.wallet_guid = t.source_guid
                join regop_pers_acc pa on pa.{walletKey} = w.id
                join gkh_room r on r.id = pa.room_id
                join regop_ro_payment_account rpa on rpa.ro_id = r.ro_id
                join regop_wallet w2 on rpa.{walletKey} = w2.id
                where t.id in (:ids)");

            // таймаут 4 часа
            query.SetTimeout(4 * 60 * 60);

            foreach (var ids in transferIds.Split(1000))
            {
                query.SetParameterList("ids", ids);
                query.ExecuteUpdate();
            }
        }

        private void CopyPaymentTransfersToRo(ISession session, string walletKey, long[] transferIds, ChargePeriod period)
        {
            if (transferIds.IsEmpty())
            {
                return;
            }

            var query = session.CreateSQLQuery($@"
                    insert into regop_reality_transfer_period_{period.Id}
                        (object_version, object_create_date, object_edit_date, 
                        source_guid, target_guid, target_coef, 
                        op_id, amount, reason, 
                        originator_name, operation_date, is_indirect, 
                        copy_transfer_id, is_affect, 
                        is_loan, is_return_loan, payment_date, period_id, owner_id)
                    select 
                        0, now(), now(),
                        t.target_guid, w2.wallet_guid, t.target_coef, 
                        t.op_id, t.amount, t.reason, 
                        t.originator_name, t.operation_date, t.is_indirect, 
                        t.id, t.is_affect, 
                        false, false, t.payment_date,
                        t.period_id,
                        rpa.id
                    from regop_transfer_period_{period.Id} t
                    join regop_wallet w on w.wallet_guid = t.target_guid
                    join regop_pers_acc pa on pa.{walletKey} = w.id 
                    join gkh_room r on r.id = pa.room_id
                    join regop_ro_payment_account rpa on rpa.ro_id = r.ro_id
                    join regop_wallet w2 on rpa.{walletKey} = w2.id
                    where t.id in (:ids)");

            // таймаут 4 часа
            query.SetTimeout(4 * 60 * 60);

            foreach (var ids in transferIds.Split(1000))
            {
                
                query.SetParameterList("ids", ids);
                query.ExecuteUpdate();
            }
        }

        private IList<QueryDto> GetTransfersDiff(
            ISession session, 
            ChargePeriod period, 
            KeyValuePair<string, Tuple<string[], string[]>> walletInfo, 
            bool isPayment)
        {
            ISQLQuery query;

            if (isPayment)
            {
                query = session.CreateSQLQuery($@"
                    select 
                        t.id as Id,
                        t.payment_date as PaymentDate,
                        ro.address as Address,
                        pa.id as AccountId,
                        pa.acc_num as AccountNumber,
                        t.amount as TransferSum,
                        t.reason as TransferReason,
                        true as IsPayment,
                      '{walletInfo.Key}' as WalletKey
                    from regop_transfer_period_{period.Id} t
                    join regop_wallet w on w.wallet_guid = t.target_guid
                    join regop_pers_acc pa on pa.{walletInfo.Key} = w.id
                    join gkh_room r on r.id = pa.room_id
                    join gkh_reality_object ro on ro.id = r.ro_id
                    where not exists (
                        select tt.id
                        from regop_reality_transfer_period_{period.Id} tt
                        where t.target_guid = tt.source_guid
                      and t.op_id = tt.op_id
                        and tt.copy_transfer_id = t.id)
                    and not t.is_indirect and t.is_affect
                    and  t.reason in (:reasons)");

                query.SetParameterList("reasons", walletInfo.Value.Item1);
            }
            else
            {
                query = session.CreateSQLQuery($@"
                    select 
                        t.id as Id,
                        t.payment_date as PaymentDate,
                        ro.address as Address,
                        pa.id as AccountId,
                        pa.acc_num as AccountNumber,
                        t.amount as TransferSum,
                        t.reason as TransferReason,
                        false as IsPayment,
                        '{walletInfo.Key}' as WalletKey
                    from regop_transfer_period_{period.Id} t
                    join regop_wallet w on w.wallet_guid = t.source_guid
                    join regop_pers_acc pa on pa.{walletInfo.Key} = w.id
                    join gkh_room r on r.id = pa.room_id
                    join gkh_reality_object ro on ro.id = r.ro_id
                    where not exists (
                        select tt.id
                        from regop_reality_transfer_period_{period.Id} tt
                        where t.target_guid = tt.target_guid
                        and t.op_id = tt.op_id
                      and tt.copy_transfer_id = t.id)
                    and not t.is_indirect and t.is_affect
                    and t.reason in (:reasons)");

                query.SetParameterList("reasons", walletInfo.Value.Item2);
            }

            query.SetResultTransformer(Transformers.AliasToBean<QueryDto>());

            return query.List<QueryDto>();
        }

        private string GetTransferInfo(string walletKey)
        {
            switch (walletKey)
            {
                case "bt_wallet_id":
                    return " по базовому тарифу";

                case "dt_wallet_id":
                    return " по тарифу решения";

                case "p_wallet_id":
                    return " по пени";
            }

            return string.Empty;
        }

        private struct QueryDto
        {
            public long Id { get; set; }
            public long AccountId { get; set; }
            public string Address { get; set; }
            public string AccountNumber { get; set; }
            public decimal TransferSum { get; set; }
            public string TransferReason { get; set; }
            public DateTime PaymentDate { get; set; }
            public bool IsPayment { get; set; }
            public string WalletKey { get; set; }
        }
    }
}