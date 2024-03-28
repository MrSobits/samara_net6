namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;
    using Bars.Gkh.Utils;

    using Dapper;

    /// <summary>
    /// Импорт начислений
    /// </summary>
    public class CalcImporter : BaseChesImporter<CalcFileInfo>
    {
        /// <summary>
        /// Таймаут на команду
        /// </summary>
        private int CommandTimeOut = 3600 * 3;

        private const int PartSize = 50000;

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private List<PersonalAccountPeriodSummaryDto> summariesForUpdate;
        private List<PersonalAccountChargeDto> chargesForSave;
        private List<Transfer> transfersForSave;
        private List<MoneyOperation> operationForSave;
        private List<long> calcDebtIdsForUpdate;

        /// <summary>
        /// Репозиторий для <see cref="PersonalAccountPeriodSummary" />
        /// </summary>
        public IRepository<PersonalAccountPeriodSummary> PeriodSummaryRepo { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="PersonalAccountCharge" />
        /// </summary>
        public IRepository<PersonalAccountCharge> AccountChargeRepo { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="UnacceptedChargePacket" />
        /// </summary>
        public IDomainService<UnacceptedChargePacket> UnacceptedChargePacketDomain { get; set; }

        /// <summary>
        /// Обработать данные импорта
        /// </summary>
        /// <param name="calcFileInfo">Файл импорта</param>
        public override void ProcessData(CalcFileInfo calcFileInfo)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var progress = new ProgressSender(calcFileInfo.Rows.Count / CalcImporter.PartSize, this.Indicate, 80);

            UnacceptedChargePacket chargePacket = null;

            if (calcFileInfo.Rows.Count > 0)
            {
                chargePacket = new UnacceptedChargePacket
                {
                    CreateDate = DateTime.Now,
                    Description = "Импорт начислений от ЧЭС",
                    PacketState = PaymentOrChargePacketState.Accepted,
                    UserName = this.Login
                };
                this.UnacceptedChargePacketDomain.Save(chargePacket);
            }

            progress.ForceSend(0, "Импорт начислений: инициализация справочников");

            using (this.Container.Using(sessionProvider))
            {
                foreach (var section in calcFileInfo.Rows.Section(CalcImporter.PartSize))
                {
                    var stateless = sessionProvider.OpenStatelessSession();
                    var conn = stateless.Connection;
                    var sql = string.Empty;
                    var importedRows = 0;
                    var data = section.ToArray();
                    var accountNums = string.Join(",", data.Select(x => $"'{x.LsNum}'"));

                    sql =
                        $@"SELECT 
                                p.acc_num as AccNum, 
                                p.id as AccountId,
                                w1.wallet_guid as BaseTariffWalletGuid,  
                                w2.wallet_guid as DecisionTariffWalletGuid,  
                                w3.wallet_guid as PenaltyWalletGuid
                           FROM public.regop_pers_acc p
                           JOIN regop_wallet w1 on w1.id = p.bt_wallet_id
                           JOIN regop_wallet w2 on w2.id = p.dt_wallet_id
                           JOIN regop_wallet w3 on w3.id = p.p_wallet_id
                           WHERE p.acc_num IN ({accountNums});";

                    var accountsDict = conn.Query<PersonalAccountDto>(sql, commandTimeout: this.CommandTimeOut)
                        .ToDictionary(x => x.AccNum);

                    sql =
                        $@"SELECT distinct rm.ro_id
                           FROM public.regop_pers_acc p
                           join gkh_room rm on p.room_id=rm.id
                           WHERE p.acc_num IN ({accountNums});";

                    var roIds = conn.Query<long>(sql, commandTimeout: this.CommandTimeOut).ToList();

                    sql =
                        $@" SELECT s.Id as Id, s.ACCOUNT_ID as PersonalAccountId, s.PERIOD_ID as PeriodId, s.CHARGE_TARIFF as ChargeTariff,
                                 s.CHARGE_BASE_TARIFF as ChargedByBaseTariff, s.RECALC as RecalcByBaseTariff, s.RECALC_DECISION as RecalcByDecisionTariff,
                                 s.RECALC_PENALTY as RecalcByPenalty,
                                 s.PENALTY as Penalty, s.PENALTY_PAYMENT as PenaltyPayment, s.TARIFF_PAYMENT as TariffPayment,
                                 s.SALDO_IN as SaldoIn, s.SALDO_OUT as SaldoOut, s.TARIFF_DESICION_PAYMENT as TariffDecisionPayment, 
                                 s.OVERHAUL_PAYMENT as OverhaulPayment, s.RECRUITMENT_PAYMENT as RecruitmentPayment, 
                                 s.SALDO_IN_SERV as SaldoInFromServ, s.SALDO_OUT_SERV as SaldoOutFromServ, s.SALDO_CHANGE_SERV as SaldoChangeFromServ, 
                                 s.BALANCE_CHANGE as BaseTariffChange, s.DEC_BALANCE_CHANGE as DecisionTariffChange, s.PENALTY_BALANCE_CHANGE as PenaltyChange,
                                 s.BASE_TARIFF_DEBT as BaseTariffDebt, s.DEC_TARIFF_DEBT as DecisionTariffDebt, s.PENALTY_DEBT as PenaltyDebt, 
                                 s.PERF_WORK_CHARGE as PerformedWorkCharged
                                FROM public.REGOP_PERS_ACC_PERIOD_SUMM_PERIOD_{calcFileInfo.Period.Id} s 
                                WHERE s.ACCOUNT_ID IN ({string.Join(",", accountsDict.Values.Select(x => x.AccountId))})";

                    var accountSummaries = conn.Query<PersonalAccountPeriodSummaryDto>(sql, commandTimeout: this.CommandTimeOut)
                        .ToDictionary(x => x.PersonalAccountId);

                    sql = $@"select cd.ID as ID, ac.ID as AccountId, ac.ACC_NUM as AccountNum, cd.SALDO_OUT_BASE_TARIFF as SaldoOutBaseTariff,
                             cd.SALDO_OUT_DEC_TARIFF as SaldoOutDecisionTariff, cd.SALDO_OUT_PENALTY as SaldoOutPenalty
                             from REGOP_CALC_DEBT_DETAIL cd
                             join REGOP_PERS_ACC ac on cd.ACCOUNT_ID=ac.ID
                             where not cd.IS_IMPORTED
                             and ac.ID IN ({string.Join(",", accountsDict.Values.Select(x => x.AccountId))})";

                    var calcDebtDetails = conn.Query<CalcDebtDetailDto>(sql, commandTimeout: this.CommandTimeOut)
                        .ToDictionary(x => x.AccountId);

                    this.summariesForUpdate = new List<PersonalAccountPeriodSummaryDto>();
                    this.chargesForSave = new List<PersonalAccountChargeDto>();
                    this.transfersForSave = new List<Transfer>();
                    this.operationForSave = new List<MoneyOperation>();
                    this.calcDebtIdsForUpdate = new List<long>();

                    data.ForEach(
                        row =>
                        {
                            if (!accountsDict.ContainsKey(row.LsNum))
                            {
                                this.LogImport.Error("Ошибка", $"Лицевой счет {row.LsNum} не найден. Строка {row.RowNumber}");
                                return;
                            }

                            var accountDto = accountsDict[row.LsNum];

                            if (!accountSummaries.ContainsKey(accountDto.AccountId))
                            {
                                this.LogImport.Error("Ошибка", $"Лицевой счет {row.LsNum}. Отсутствует детализация за период. Строка {row.RowNumber}");
                                return;
                            }

                            if (calcDebtDetails.ContainsKey(accountDto.AccountId))
                            {
                                var calcDebtSucces = true;
                                var calcDebt = calcDebtDetails.Get(accountDto.AccountId);

                                if (calcDebt.SaldoOutBaseTariff != row.SaldoOuthBase)
                                {
                                    this.LogImport.Error("Ошибка", $"Расхождение в суммах исх. сальдо по базовому тарифу: БАРС: {calcDebt.SaldoOutBaseTariff.RegopRoundDecimal(2)}, ЧЭС: {row.SaldoOuthBase.RegopRoundDecimal(2)}. Строка {row.RowNumber}");
                                    calcDebtSucces = false;
                                }

                                if (calcDebt.SaldoOutDecisionTariff != row.SaldoOuthTr)
                                {
                                    this.LogImport.Error("Ошибка", $"Расхождение в суммах исх. сальдо по тарифу решения: БАРС: {calcDebt.SaldoOutDecisionTariff.RegopRoundDecimal(2)}, ЧЭС: {row.SaldoOuthTr.RegopRoundDecimal(2)}. Строка {row.RowNumber}");
                                    calcDebtSucces = false;
                                }

                                if (calcDebt.SaldoOutPenalty != row.SaldoOuthPeni)
                                {
                                    this.LogImport.Error("Ошибка", $"Расхождение в суммах исх. сальдо по пени: БАРС: {calcDebt.SaldoOutPenalty.RegopRoundDecimal(2)}, ЧЭС: {row.SaldoOuthPeni.RegopRoundDecimal(2)}. Строка {row.RowNumber}");
                                    calcDebtSucces = false;
                                }

                                if (!calcDebtSucces)
                                {
                                    return;
                                }

                                this.calcDebtIdsForUpdate.Add(calcDebt.Id);
                            }

                            var summary = accountSummaries[accountDto.AccountId];

                            if (this.NeedUpdateSummary(summary, row))
                            {
                                summary.ChargedByBaseTariff = row.ChargeBase;
                                summary.ChargeTariff = row.ChargeTr + row.ChargeBase;
                                summary.Penalty = row.ChargePeni;

                                summary.RecalcByBaseTariff = row.RecalcBase;
                                summary.RecalcByDecisionTariff = row.RecalcTr;
                                summary.RecalcByPenalty = row.RecalcPeni;

                                summary.BaseTariffChange = row.ChangeBase;
                                summary.DecisionTariffChange = row.ChangeTr;
                                summary.PenaltyChange = row.ChangePeni;

                                summary.RecalcSaldoOut();
                                this.summariesForUpdate.Add(summary);
                            }

                            var charge = new PersonalAccountChargeDto(summary.PersonalAccountId)
                            {
                                PacketId = chargePacket.Id,
                                ChargePeriodId = calcFileInfo.Period.Id,
                                ChargeDate = calcFileInfo.Period.StartDate.AddDays(1),
                                Guid = Guid.NewGuid().ToString(),
                                ChargeTariff = row.ChargeTr + row.ChargeBase,
                                OverPlus = row.ChargeTr,
                                Penalty = row.ChargePeni,
                                RecalcByBaseTariff = row.RecalcBase,
                                RecalcByDecisionTariff = row.RecalcTr,
                                RecalcPenalty = row.RecalcPeni,
                                Charge = row.ChargeTr + row.ChargeBase + row.ChargePeni + row.RecalcBase + row.RecalcTr + row.RecalcPeni,
                                IsActive = true,
                                IsFixed = calcFileInfo.Period.IsClosed
                            };

                            if (charge.IsFixed)
                            {
                                var operation = charge.CreateOperation(calcFileInfo.Period);
                                this.operationForSave.Add(operation);
                                this.transfersForSave.AddRange(charge.CreateChargeTransfers(operation, accountDto.GetWallets()));
                            }

                            this.chargesForSave.Add(charge);

                            Interlocked.Increment(ref importedRows);

                            row.IsImported = true;

                            this.LogImport.Info("Импорт начислений", $"На лс {row.LsNum} добавлены начисления");
                        });

                    progress.TrySend("Импорт начислений: Сохранение");

                    var accountsToUpdate = this.chargesForSave.Select(x => x.BasePersonalAccountId).ToHashSet();

                    TransactionHelper.InsertInManyTransactions(this.Container, this.operationForSave, CalcImporter.PartSize, true, true);

                    using (var trans = conn.BeginTransaction())
                    {
                        if (accountsToUpdate.IsNotEmpty())
                        {
                            // если грузим в закрытый период, то нужно удалять начисления с трансферами
                            if (calcFileInfo.Period.IsClosed)
                            {
                                sql = $@"
                                    DELETE FROM regop_charge_transfer_period_{calcFileInfo.Period.Id} tr
                                    WHERE tr.owner_id in ({string.Join(",", accountsToUpdate)})
                                            and exists (SELECT null FROM public.regop_pers_acc_charge_period_{calcFileInfo.Period.Id} 
                                                WHERE pers_acc_id IN ({string.Join(",", accountsToUpdate)}) and (is_active or is_fixed) and tr.target_guid = guid)";

                                conn.Execute(sql, transaction: trans);

                                sql = $@"
                                  DELETE FROM public.regop_pers_acc_charge_period_{calcFileInfo.Period.Id} 
                                  WHERE pers_acc_id IN ({string.Join(",", accountsToUpdate)}) and (is_active or is_fixed)";
                            }
                            else
                            {
                                sql = $@" 
                                UPDATE public.regop_pers_acc_charge_period_{calcFileInfo.Period.Id} 
                                SET is_active = false, is_fixed = false, object_edit_date = current_timestamp
                                WHERE pers_acc_id IN ({string.Join(",", accountsToUpdate)}) and is_active";
                            }
                       
                            conn.Execute(sql, transaction: trans);
                        }

                        sql =
                            $@"INSERT INTO public.regop_pers_acc_charge_period_{calcFileInfo.Period.Id} (object_version, object_create_date, object_edit_date,
                                           charge_date, guid, charge, charge_tariff, penalty, recalc, pers_acc_id,
                                           overplus, is_fixed, is_active, recalc_decision, recalc_penalty, period_id, packet_id) 
                                   VALUES (@ObjectVersion, @ObjectCreateDate, @ObjectEditDate,@ChargeDate,@Guid,@Charge,@ChargeTariff,
                                           @Penalty,@RecalcByBaseTariff,@BasePersonalAccountId,@OverPlus,@IsFixed, @IsActive, @RecalcByDecisionTariff,
                                           @RecalcPenalty,@ChargePeriodId, @PacketId)";
                        var personalAccountCharges =
                            this.chargesForSave.Select(x => new PersonalAccountChargeDto(x, calcFileInfo.Period, false)).ToList();
                        conn.Execute(sql, personalAccountCharges, trans);

                        sql = $@"UPDATE public.regop_pers_acc_period_summ_period_{calcFileInfo.Period.Id} 
                                     SET 
                                        CHARGE_TARIFF=@ChargeTariff, 
                                        CHARGE_BASE_TARIFF = @ChargedByBaseTariff,
                                        RECALC=@RecalcByBaseTariff, 
                                        RECALC_DECISION =@RecalcByDecisionTariff,
                                        RECALC_PENALTY=@RecalcByPenalty, 
                                        PENALTY = @Penalty, 
                                        SALDO_OUT = @SaldoOut, 
                                        SALDO_IN = @SaldoIn, 
                                        PERF_WORK_CHARGE = @PerformedWorkCharged,
                                        BALANCE_CHANGE = @BaseTariffChange, 
                                        DEC_BALANCE_CHANGE = @DecisionTariffChange, 
                                        TARIFF_PAYMENT = @TariffPayment,
                                        TARIFF_DESICION_PAYMENT = @TariffDecisionPayment,
                                        PENALTY_PAYMENT = @PenaltyPayment,
                                        PENALTY_BALANCE_CHANGE = @PenaltyChange,
                                        object_edit_date = current_timestamp
                                        WHERE ACCOUNT_ID=@PersonalAccountId;";

                        //обновляем текущие сальдовые строки в period_summary
                        conn.Execute(sql, this.summariesForUpdate, trans);

                        sql = $@"UPDATE regop_ro_charge_acc_charge ch
                                 SET
                                 ccharged = q.chargeTariff,
                                 cpaid = q.paidTariff,
                                 ccharged_penalty = q.chargePenalty,
                                 cpaid_penalty = q.paidPenalty,
                                 csaldo_in = q.saldoIn,
                                 csaldo_out = q.saldoOut
                                 FROM
                                 (SELECT 
                                 ch_acc.id acc_id,
                                 ps.period_id,
                                 SUM(ps.charge_tariff + ps.recalc + ps.recalc_decision + ps.balance_change + ps.dec_balance_change + ps.penalty_balance_change 
                                     + ps.penalty + ps.recalc_penalty - ps.perf_work_charge) AS chargeTariff,
                                 SUM(ps.penalty + ps.recalc_penalty + ps.penalty_balance_change) AS chargePenalty,
                                 SUM(ps.tariff_payment + ps.tariff_desicion_payment) AS paidTariff,
                                 SUM(ps.penalty_payment) AS paidPenalty,
                                 SUM(ps.saldo_in) AS saldoIn,
                                 SUM(ps.saldo_out) AS saldoOut
                                 FROM regop_pers_acc_period_summ ps
                                 JOIN regop_pers_acc ac ON ac.id = ps.account_id
                                 JOIN gkh_room r ON r.id = ac.room_id
                                 JOIN regop_ro_charge_account ch_acc ON ch_acc.ro_id = r.ro_id
                                 WHERE ps.period_id = {calcFileInfo.Period.Id} AND r.ro_id IN ({string.Join(",", roIds)})
                                 GROUP BY ch_acc.id, ps.period_id) q
                                 WHERE q.acc_id = ch.acc_id AND ch.period_id = q.period_id";
                        conn.Execute(sql, transaction: trans);

                        if (this.calcDebtIdsForUpdate.Count > 0)
                        {
                            sql = $@"update REGOP_CALC_DEBT_DETAIL
                                 set IS_IMPORTED = true
                                 where id in ({ string.Join(",", this.calcDebtIdsForUpdate)})";
                            conn.Execute(sql, transaction: trans);
                        }

                        sql = $@" INSERT INTO public.regop_charge_transfer_period_{calcFileInfo.Period.Id}
                                  (SOURCE_GUID,TARGET_GUID,TARGET_COEF,OP_ID,AMOUNT,REASON,
                                   ORIGINATOR_NAME,PAYMENT_DATE,OPERATION_DATE,IS_INDIRECT,
                                   ORIGINATOR_ID,IS_AFFECT,IS_LOAN,
                                   IS_RETURN_LOAN,PERIOD_ID, OWNER_ID,
                                   OBJECT_CREATE_DATE,OBJECT_EDIT_DATE,OBJECT_VERSION) VALUES 
                                  (@SourceGuid,@TargetGuid,@TargetCoef,@OperationId,@Amount,
                                   @Reason,@OriginatorName,@PaymentDate,@OperationDate,@IsInDirect,
                                   @OriginatorId,@IsAffect,@IsLoan,@IsReturnLoan,
                                   {calcFileInfo.Period.Id},@OwnerId,@ObjectCreateDate,@ObjectEditDate,@ObjectVersion)";

                        // если в процессе создались трансферы, то сохраняем их
                        foreach (var transfer in this.transfersForSave)
                        {
                            conn.Execute(
                                sql,
                                new
                                {
                                    transfer.SourceGuid,
                                    transfer.TargetGuid,
                                    transfer.TargetCoef,
                                    OperationId = transfer.Operation.Id,
                                    transfer.Amount,
                                    transfer.Reason,
                                    transfer.OriginatorName,
                                    transfer.PaymentDate,
                                    transfer.OperationDate,
                                    transfer.IsInDirect,
                                    OriginatorId = transfer.Originator?.Id,
                                    transfer.IsAffect,
                                    transfer.IsLoan,
                                    transfer.IsReturnLoan,
                                    OwnerId = transfer.Owner.Id,
                                    transfer.ObjectCreateDate,
                                    transfer.ObjectEditDate,
                                    transfer.ObjectVersion
                                },
                                trans);
                        }

                        trans.Commit();

                        this.LogImport.CountAddedRows += importedRows;
                    }
                }

                this.ChesTempDataProvider.UpdateCheckView();

                progress.ForceSend(100, "Импорт начислений: Завершен");
            }
        }

        private bool NeedUpdateSummary(PersonalAccountPeriodSummaryDto summary, CalcFileInfo.Row row)
        {
            return
                summary.SaldoIn != row.SaldoIn
                || summary.BaseTariffDebt != row.SaldoInBase
                || summary.DecisionTariffDebt != row.SaldoInTr
                || summary.PenaltyDebt != row.SaldoInPeni
                || summary.ChargedByBaseTariff != row.ChargeBase
                || summary.ChargeTariff != row.ChargeTr + row.ChargeBase
                || summary.Penalty != row.ChargePeni
                || summary.RecalcByBaseTariff != row.RecalcBase
                || summary.RecalcByDecisionTariff != row.RecalcTr
                || summary.RecalcByPenalty != row.RecalcPeni
                || summary.BaseTariffChange != row.ChangeBase
                || summary.DecisionTariffChange != row.ChangeTr
                || summary.PenaltyChange != row.ChangePeni
                || summary.TariffPayment != row.PaymentBase
                || summary.TariffDecisionPayment != row.PaymentTr
                || summary.PenaltyPayment != row.PaymentPeni;
        }

        private class PersonalAccountDto
        {
            public string AccNum { get; set; }

            public long AccountId { get; set; }

            public string BaseTariffWalletGuid { get; set; }

            public string DecisionTariffWalletGuid { get; set; }

            public string PenaltyWalletGuid { get; set; }

            public IDictionary<WalletType, string> GetWallets()
            {
                return new Dictionary<WalletType, string>
                {
                    { WalletType.BaseTariffWallet, this.BaseTariffWalletGuid },
                    { WalletType.DecisionTariffWallet, this.DecisionTariffWalletGuid },
                    { WalletType.PenaltyWallet, this.PenaltyWalletGuid }
                };
            }
        }

        private class CalcDebtDetailDto
        {
            public long Id { get; set; }
            public long AccountId { get; set; }
            public decimal SaldoOutBaseTariff { get; set; }
            public decimal SaldoOutDecisionTariff { get; set; }
            public decimal SaldoOutPenalty { get; set; }
        }

        private void IsImported(CalcFileInfo calcFileInfo)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            using (this.Container.Using(sessionProvider))
            {
                var stateless = sessionProvider.OpenStatelessSession();
                var connection = stateless.Connection;

                using (var trans = connection.BeginTransaction())
                {
                    var importedRowIds = calcFileInfo.Rows.Where(x => x.IsImported).Select(x => x.Id).ToList();
                    var sql = $@"UPDATE IMPORT.CHES_CALC_{calcFileInfo.Period.Id}
                                 SET isimported=true
                                 WHERE id in ({string.Join(",", importedRowIds)});";
                    connection.Execute(sql, transaction: trans);
                    trans.Commit();
                }
            }
        }
    }
}