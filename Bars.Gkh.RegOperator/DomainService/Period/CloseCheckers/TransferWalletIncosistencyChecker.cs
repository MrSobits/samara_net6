namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Entities.Refactor.TransferOwner;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Extenstions;

    using Castle.Windsor;

    using Dapper;

    /// <summary>
    /// Проверка - Несоответствие трансферов кошелькам
    /// </summary>
    public partial class TransferWalletIncosistencyChecker : IPeriodCloseChecker
    {
        public IWindsorContainer Container { get; set; }
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Код проверки
        /// </summary>
        public static readonly string Id = typeof(TransferWalletIncosistencyChecker).FullName;

        /// <inheritdoc />
        public string Impl => TransferWalletIncosistencyChecker.Id;

        /// <inheritdoc />
        public string Code => "9";

        /// <inheritdoc />
        public string Name => "П- Несоответствие трансферов кошелькам";

        /// <inheritdoc />
        public PeriodCloseCheckerResult Check(long periodId)
        {
            var session = this.SessionProvider.GetCurrentSession();
            var period = session.Get<ChargePeriod>(periodId);
            if (period == null)
            {
                throw new ValidationException("Не найден период начислений");
            }

            var result = new PeriodCloseCheckerResult { Success = true };
            var errorTransfers = this.GetErrorData(periodId);

            IList<QueryDto> errorTransfersAfterCorrection = null;

            if (errorTransfers.Any())
            {
                this.CorrectTransfers(errorTransfers, periodId, result.FullLog);
                errorTransfersAfterCorrection = this.GetErrorData(periodId);
                result.Success = errorTransfersAfterCorrection.Count == 0;

                result.Note = $" Был запущен режим \"Перенос трансферов на кошелек\". Ошибок до: {errorTransfers.Count}, после: {errorTransfersAfterCorrection.Count}";
            }
            
            if (!result.Success)
            {
                // к сожалению идентфикаторы домов нет смысла проставлять
                result.InvalidAccountIds = errorTransfersAfterCorrection
                    .Where(x => x.OwnerType == WalletOwnerType.BasePersonalAccount)
                    .Select(x => x.ObjectId)
                    .Distinct()
                    .ToList();

                result.Log.AppendLine("Объект;Дата операции;Сумма трансфера;Тип трансфера;Кошелек, куда посажен трансфер;Входящий");
                foreach (var tr in errorTransfersAfterCorrection)
                {
                    result.Log.AppendLine($"\"{tr.ObjectInfo}\";{tr.PaymentDate};{tr.TransferSum};{tr.TransferReason};{tr.WalletType.GetDisplayName()};{(tr.Incoming ? "Да" : "Нет")}");
                }
            }

            return result;
        }

        private List<QueryDto> GetErrorData(long periodId)
        {
            var errorTransfers = new List<QueryDto>();

            this.SessionProvider.InStatelessConnectionTransaction((connection, transaction) =>
            {
                foreach (var transferReasonPacket in this.transferInfo)
                {
                    // оплаты и возвраты ЛС
                    var tableName = $"regop_transfer_period_{periodId}";

                    // 1. Трансферы оплат на ЛС
                    var sql = $@"SELECT
                                tr.id as Id,
                                ac.id as ObjectId,
                                ac.acc_num as ObjectInfo,
                                tr.amount as TransferSum,
                                tr.reason as TransferReason,
                                tr.payment_date as PaymentDate,
                                w.wallet_type as WalletType,
                                'regop_transfer' as CurrentTable,
                                w.owner_type as OwnerType,
                                true as Incoming
                                from {tableName} tr
                                join regop_pers_acc ac on ac.id = tr.owner_id
                                join regop_wallet w 
                                    on w.wallet_guid = tr.target_guid 
                                    and w.owner_type = {(int)WalletOwnerType.BasePersonalAccount}
                                    and w.wallet_type = {(int)transferReasonPacket.Key}
                                join regop_money_operation op on op.id = tr.op_id
                                where (not tr.is_indirect) 
                                    and (coalesce(tr.reason, op.reason) not in ({transferReasonPacket.Value.IncomingTransfersString}) or not tr.is_affect)";

                    errorTransfers.AddRange(connection.Query<QueryDto>(sql));

                    // 2. Трансферы возвратов на ЛС
                    sql = $@"SELECT
                                tr.id as Id,
                                ac.id as ObjectId,
                                ac.acc_num as ObjectInfo,
                                tr.amount as TransferSum,
                                tr.reason as TransferReason,
                                tr.payment_date as PaymentDate,
                                w.wallet_type as WalletType,
                                'regop_transfer' as CurrentTable,
                                w.owner_type as OwnerType,
                                false as Incoming
                                from {tableName} tr
                                join regop_pers_acc ac on ac.id = tr.owner_id
                                join regop_wallet w 
                                    on w.wallet_guid = tr.source_guid 
                                    and w.owner_type = {(int)WalletOwnerType.BasePersonalAccount}
                                    and w.wallet_type = {(int)transferReasonPacket.Key}
                                join regop_money_operation op on op.id = tr.op_id
                                where (not tr.is_indirect) 
                                    and (coalesce(tr.reason, op.reason) not in ({transferReasonPacket.Value.OutcomingTransfersString}) or not tr.is_affect)";

                    errorTransfers.AddRange(connection.Query<QueryDto>(sql));

                    if (transferReasonPacket.Value.ChargeTransfers.IsNotEmpty()
                        && transferReasonPacket.Key.In(WalletType.BaseTariffWallet, WalletType.DecisionTariffWallet, WalletType.PenaltyWallet))
                    {
                        // начисления
                        tableName = $"regop_charge_transfer_period_{periodId}";

                        // 3. Трансферы начислений на ЛС (входящие - только слияние)
                        sql = $@"SELECT
                                tr.id as Id,
                                ac.id as ObjectId,
                                ac.acc_num as ObjectInfo,
                                tr.amount as TransferSum,
                                tr.reason as TransferReason,
                                tr.payment_date as PaymentDate,
                                w.wallet_type as WalletType,
                                'regop_charge_transfer' as CurrentTable,
                                w.owner_type as OwnerType,
                                true as Incoming
                                from {tableName} tr
                                join regop_pers_acc ac on ac.id = tr.owner_id
                                join regop_wallet w 
                                    on w.wallet_guid = tr.target_guid 
                                    and w.owner_type = {(int)WalletOwnerType.BasePersonalAccount}
                                    and w.wallet_type = {(int)transferReasonPacket.Key}
                                join regop_money_operation op on op.id = tr.op_id
                                where not (tr.reason is null and not tr.is_affect)";

                        errorTransfers.AddRange(connection.Query<QueryDto>(sql));

                        // 4. Трансферы начислений на ЛС (исходящие)
                        sql = $@"SELECT
                                tr.id as Id,
                                ac.id as ObjectId,
                                ac.acc_num as ObjectInfo,
                                tr.amount as TransferSum,
                                tr.reason as TransferReason,
                                tr.payment_date as PaymentDate,
                                w.wallet_type as WalletType,
                                'regop_charge_transfer' as CurrentTable,
                                w.owner_type as OwnerType,
                                false as Incoming
                                from {tableName} tr
                                join regop_pers_acc ac on ac.id = tr.owner_id
                                join regop_wallet w 
                                    on w.wallet_guid = tr.source_guid 
                                    and w.owner_type = {(int)WalletOwnerType.BasePersonalAccount}
                                    and w.wallet_type = {(int)transferReasonPacket.Key}
                                join regop_money_operation op on op.id = tr.op_id
                                where coalesce(tr.reason, op.reason) not in ({transferReasonPacket.Value.ChargeTransfersString}) or tr.is_affect";

                        errorTransfers.AddRange(connection.Query<QueryDto>(sql));
                    }

                    // счет оплат дома
                    tableName = $"regop_reality_transfer_period_{periodId}";

                    // 5. Копии оплат на дом
                    sql = $@"SELECT
                                tr.id as Id,
                                ac.id as ObjectId,
                                ro.address as ObjectInfo,
                                tr.amount as TransferSum,
                                tr.reason as TransferReason,
                                tr.payment_date as PaymentDate,
                                w.wallet_type as WalletType,
                                'regop_reality_transfer' as CurrentTable,
                                w.owner_type as OwnerType,
                                true as Incoming
                                from {tableName} tr
                                join regop_ro_payment_account ac on ac.id = tr.owner_id
                                join gkh_reality_object ro on ro.id = ac.ro_id
                                join regop_wallet w 
                                    on w.wallet_guid = tr.target_guid 
                                    and w.owner_type = {(int)WalletOwnerType.RealityObjectPaymentAccount}
                                    and w.wallet_type = {(int)transferReasonPacket.Key}
                                join regop_money_operation op on op.id = tr.op_id
                                where tr.copy_transfer_id is not null
                                    and (tr.reason not in ({transferReasonPacket.Value.IncomingTransfersString}) or not tr.is_affect)";

                    errorTransfers.AddRange(connection.Query<QueryDto>(sql));

                    // 6. Копии возвратов на дом
                    sql = $@"SELECT
                                tr.id as Id,
                                ac.id as ObjectId,
                                ro.address as ObjectInfo,
                                tr.amount as TransferSum,
                                tr.reason as TransferReason,
                                tr.payment_date as PaymentDate,
                                w.wallet_type as WalletType,
                                'regop_reality_transfer' as CurrentTable,
                                w.owner_type as OwnerType,
                                false as Incoming
                                from {tableName} tr
                                join regop_ro_payment_account ac on ac.id = tr.owner_id
                                join gkh_reality_object ro on ro.id = ac.ro_id
                                join regop_wallet w 
                                    on w.wallet_guid = tr.source_guid 
                                    and w.owner_type = {(int)WalletOwnerType.RealityObjectPaymentAccount}
                                    and w.wallet_type = {(int)transferReasonPacket.Key}
                                join regop_money_operation op on op.id = tr.op_id
                                where tr.copy_transfer_id is not null
                                    and (tr.reason not in ({transferReasonPacket.Value.OutcomingTransfersString}) or not tr.is_affect)";

                    errorTransfers.AddRange(connection.Query<QueryDto>(sql));
                }
            });

            return errorTransfers;
        }

        private void CorrectTransfers(List<QueryDto> errorTransfers, long periodId, StringBuilder resultFullLog)
        {
            var listCommand = new List<CommandInfo>();
            resultFullLog.AppendLine("Объект;Дата операции;Сумма трансфера;Тип трансфера;Кошелек до проверки;Соответствующий кошелек;Входящий;Выполненная корректировка");

            foreach (var errorTransfer in errorTransfers)
            {
                CommandInfo command;

                if (this.TryGetCorrectCommand(errorTransfer, out command))
                {
                    resultFullLog.AppendLine(
                        $"\"{errorTransfer.ObjectInfo}\";{errorTransfer.PaymentDate};{errorTransfer.TransferSum};"
                        + $"{errorTransfer.TransferReason};{errorTransfer.WalletType.GetDisplayName()};{command.WalletType.GetDisplayName()};"
                        + $"{(errorTransfer.Incoming ? "Да" : "Нет")};{command.Operation}");

                    listCommand.Add(command);
                }
            }

            this.SessionProvider.InStatelessConnectionTransaction((connection, transaction) =>
            {
                foreach (var command in listCommand)
                {
                    string sqlcommand = BuildCommand(command, periodId);
                  connection.Execute(sqlcommand, transaction: transaction);
                }
            });
        }

        private bool TryGetCorrectCommand(QueryDto transfer, out CommandInfo command)
        {
            var isIncomingInvalidChargeTransfer = transfer.CurrentTable == "regop_charge_transfer" 
                && transfer.TransferReason.IsNotEmpty() 
                && transfer.Incoming;

            var isReasonIncomingButTransferOutcoming = transfer.TransferReason.IsNotEmpty() 
                && transfer.CurrentTable != "regop_charge_transfer"
                && this.transferInfo[transfer.WalletType].IncomingTransfers.Contains(transfer.TransferReason)
                && !transfer.Incoming;

            var isReasonOutcomingButTransferIncoming = transfer.TransferReason.IsNotEmpty()
                && transfer.CurrentTable != "regop_charge_transfer"
                && this.transferInfo[transfer.WalletType].OutcomingTransfers.Contains(transfer.TransferReason)
                && transfer.Incoming;

            if (isIncomingInvalidChargeTransfer || isReasonIncomingButTransferOutcoming || isReasonOutcomingButTransferIncoming)
            {
                command = null;
                return false;
            }

            // 1. Трансфер не в том кошельке, но в верной таблице
            var kvp = this.transferInfo
                .FirstOrDefault(x => x.Value?
                    .GetReasonByTableType(transfer.CurrentTable)? 
                    .Contains(transfer.TransferReason) ?? false);

            if (kvp.IsNotDefault())
            {
                command = new CommandInfo
                {
                    TransferId = transfer.Id,
                    OwnerType = transfer.OwnerType,
                    OwnerId = transfer.ObjectId,
                    AtributeName = transfer.Incoming ? "target_guid" : "source_guid",
                    WalletType = kvp.Key,
                    TableName = transfer.CurrentTable,
                    Operation = "Перенос на другой кошелек"
                };

                return true;
            }

            // 2. Трансфер принадлежит другой таблице (оплата <-> начисление), значит переносим трансферов
            if (transfer.CurrentTable == "regop_charge_transfer")
            {
                kvp = this.transferInfo
                    .FirstOrDefault(x => x.Value.GetReasonByTableType("regop_transfer").Contains(transfer.TransferReason));
                if (kvp.IsNotDefault())
                {
                    command = new CommandInfo
                    {
                        TransferId = transfer.Id,
                        OwnerType = transfer.OwnerType,
                        OwnerId = transfer.ObjectId,
                        AtributeName = transfer.Incoming ? "target_guid" : "source_guid",
                        WalletType = kvp.Key,
                        TableName = "regop_transfer",
                        MoveFrom = transfer.CurrentTable,
                        Operation = "Перенос в таблицу оплат ЛС"
                    };

                    return true;
                }
            }

            if (transfer.CurrentTable == "regop_transfer")
            {
                kvp = this.transferInfo
                    .FirstOrDefault(x => x.Value.GetReasonByTableType("regop_charge_transfer").Contains(transfer.TransferReason));
                if (kvp.IsNotDefault())
                {
                    command = new CommandInfo
                    {
                        TransferId = transfer.Id,
                        OwnerType = transfer.OwnerType,
                        OwnerId = transfer.ObjectId,
                        AtributeName = transfer.Incoming ? "target_guid" : "source_guid",
                        WalletType = kvp.Key,
                        TableName = "regop_charge_transfer",
                        MoveFrom = transfer.CurrentTable,
                        Operation = "Перенос в таблицу начислений ЛС"
                    };

                    return true;
                }
            }

            command = null;
            return false;
        }

        private string BuildCommand(CommandInfo info, long periodId)
        {
            if (info.MoveFrom.IsNotNull())
            {
                // для входящих и исходящих разные атрибуты подменяются
                var guidSelectors = info.AtributeName == "target_guid" ? "tr.source_guid, w.wallet_guid" : "w.wallet_guid, tr.target_guid";

                return $@"INSERT INTO {info.TableName}_period_{periodId} 
                        (object_version, object_create_date, object_edit_date,
                            source_guid, target_guid, target_coef,
                            op_id, amount, reason,
                            originator_name, operation_date, is_indirect, is_affect,
                            is_loan, is_return_loan, payment_date, period_id, owner_id)
                        select tr.object_version, tr.object_create_date, tr.object_edit_date,
                            {guidSelectors}, tr.target_coef,
                            tr.op_id, tr.amount, tr.reason,
                            tr.originator_name, tr.operation_date, tr.is_indirect, tr.is_affect,
                            tr.is_loan, tr.is_return_loan, tr.payment_date, tr.period_id, tr.owner_id
                        from {info.MoveFrom}_period_{periodId} tr
                            join regop_wallet w on w.id = (select {info.WalletType.GetWalletColumnName()} from {info.OwnerType.GetOwnerTableName()} where id = {info.OwnerId})
                        where tr.id = {info.TransferId};
                        delete from {info.MoveFrom}_period_{periodId} where id = {info.TransferId};";
            }
            else
            {
                return $@"UPDATE {info.TableName}_period_{periodId} tr set {info.AtributeName} = w.wallet_guid
                    from regop_wallet w
                    where w.id = (select {info.WalletType.GetWalletColumnName()} from {info.OwnerType.GetOwnerTableName()} where id = {info.OwnerId}) and tr.id = {info.TransferId}";
            }
        }

        private class CommandInfo
        {
            public long TransferId { get; set; }
            public WalletOwnerType OwnerType { get; set; }
            public long OwnerId { get; set; }
            public WalletType WalletType { get; set; }
            public string AtributeName { get; set; }
            public string TableName { get; set; }
            /// <summary>
            /// Если указано, то переносим трансфер через таблицу
            /// </summary>
            public string MoveFrom { get; set; }
            public string Operation { get; set; }
        }

        private class QueryDto
        {
            public long Id { get; set; }
            public long ObjectId { get; set; }
            public string ObjectInfo { get; set; }
            public decimal TransferSum { get; set; }
            public string TransferReason { get; set; }
            public DateTime PaymentDate { get; set; }
            public WalletOwnerType OwnerType { get; set; }
            public WalletType WalletType { get; set; }
            public string CurrentTable { get; set; }
            public bool Incoming { get; set; }
        }
    }
}