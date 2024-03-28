namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;
    using Bars.Gkh.Repositories.ChargePeriod;

    /// <summary>
    /// Имопрт перерасчетов
    /// </summary>
    public class RecalcImporter : BaseChesImporter<RecalcFileInfo>
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly List<RecalcHistory> recalcHistoryForSave = new List<RecalcHistory>();

        /// <summary>
        /// Домен-сервис Лицевой счет
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomian { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Репозиторий для <see cref="PersonalAccountCharge"/>
        /// </summary>
        public IRepository<PersonalAccountCharge> AccountChargeRepo { get; set; }

        /// <inheritdoc />
        public override void ProcessData(RecalcFileInfo recalcFileInfo)
        {
            var progress = new ProgressSender(recalcFileInfo.Rows.Count, this.Indicate, 80);

            progress.ForceSend(0, "Импорт перерасчетов: инициализация справочников");

            var accounts = this.PersonalAccountDomian.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.PersonalAccountNum
                    })
                .AsEnumerable()
                .ToDictionary(x => x.PersonalAccountNum, y => y.Id);

            var closePeriods = this.ChargePeriodRepository.GetAllClosedPeriods()
                .AsEnumerable()
                .GroupBy(x => x.StartDate)
                .ToDictionary(x => x.Key, x => x.First());

            var charges = this.AccountChargeRepo.GetAll()
                .Where(x => x.ChargePeriod.Id == recalcFileInfo.Period.Id)
                .Where(x => x.IsActive)
                .AsEnumerable()
                .GroupBy(x => x.BasePersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.First());

            foreach (var row in recalcFileInfo.Rows)
            {
                var period = closePeriods.Get(new DateTime(row.RecalcMonth.Year, row.RecalcMonth.Month, 1));

                if (period.IsNull())
                {
                    this.LogImport.Error("Ошибка", $"Не найдет период перерасчета. Строка {row.RowNumber}");
                    continue;
                }

                if (!accounts.ContainsKey(row.LsNum))
                {
                    this.LogImport.Error("Ошибка", $"Лицевой счет {row.LsNum} не найден. Строка {row.RowNumber}");
                    continue;
                }

                var accountId = accounts.Get(row.LsNum);

                var charge = charges.Get(accountId);

                if (charge == null)
                {
                    this.LogImport.Error("Ошибка", $"Не созданы начисления для ЛС {row.LsNum}. Строка {row.RowNumber}");
                    continue;
                }

                if (row.BaseRecalc != 0)
                {
                    var recalcHistory = new RecalcHistory
                    {
                        PersonalAccount = new BasePersonalAccount {Id = accountId},
                        CalcPeriod = recalcFileInfo.Period,
                        RecalcPeriod = period,
                        RecalcSum = row.BaseRecalc,
                        RecalcType = RecalcType.BaseTariffCharge,
                        UnacceptedChargeGuid = charge.Guid
                    };
                    this.recalcHistoryForSave.Add(recalcHistory);
                    this.LogImport.CountAddedRows++;
                }

                if (row.TariffDecisionRecalc != 0)
                {
                    var recalcHistory = new RecalcHistory
                    {
                        PersonalAccount = new BasePersonalAccount { Id = accountId },
                        CalcPeriod = recalcFileInfo.Period,
                        RecalcPeriod = period,
                        RecalcSum = row.TariffDecisionRecalc,
                        RecalcType = RecalcType.DecisionTariffCharge,
                        UnacceptedChargeGuid = charge.Guid
                    };
                    this.recalcHistoryForSave.Add(recalcHistory);
                    this.LogImport.CountAddedRows++;
                }

                if (row.PenaltyRecalcRecalc != 0)
                {
                    var recalcHistory = new RecalcHistory
                    {
                        PersonalAccount = new BasePersonalAccount { Id = accountId },
                        CalcPeriod = recalcFileInfo.Period,
                        RecalcPeriod = period,
                        RecalcSum = row.PenaltyRecalcRecalc,
                        RecalcType = RecalcType.Penalty,
                        UnacceptedChargeGuid = charge.Guid
                    };
                    this.recalcHistoryForSave.Add(recalcHistory);
                    this.LogImport.CountAddedRows++;
                }

                this.LogImport.Info("Импорт перерасчетов", string.Format("На лс {0} добавлен перерасчет по базовому:{1}; по тарифу:{2}; по пени:{3}",
                    row.LsNum, row.BaseRecalc, row.TariffDecisionRecalc, row.PenaltyRecalcRecalc));

                progress.TrySend("Импорт перерасчетов");
            }

            progress.ForceSend(80, "Импорт перерасчетов: сохранение");

            TransactionHelper.InsertInManyTransactions(this.Container, this.recalcHistoryForSave, 10000);

            progress.ForceSend(100, "Импорт перерасчетов: завершен");
        }
    }
}