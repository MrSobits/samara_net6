namespace Bars.Gkh.RegOperator.Imports.Ches
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Tasks.TaskHelpers;

    /// <summary>
    /// Импортер для протоколов начислений
    /// </summary>
    public class CalcProtImporter : BaseChesImporter<CalcProtFileInfo>
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// Репозиторий для <see cref="PersonalAccountCharge"/>
        /// </summary>
        public IRepository<PersonalAccountCharge> AccountChargeRepo { get; set; }

        private List<CalculationParameterTrace> tracesForSave; 

        public override void ProcessData(CalcProtFileInfo calcProtFileInfo)
        {
            var progress = new ProgressSender(calcProtFileInfo.Rows.Count, this.Indicate, 80);

            progress.ForceSend(0, "Импорт протоколов расчета: инициализация справочников");

            var charges = this.AccountChargeRepo.GetAll()
                .Where(x => x.ChargePeriod.Id == calcProtFileInfo.Period.Id)
                .Where(x => x.IsActive)
                .Select(
                    x => new
                    {
                        x.BasePersonalAccount.PersonalAccountNum,
                        x.BasePersonalAccount.AreaShare,
                        x.Guid
                    })
                .AsEnumerable()
                .ToDictionary(x => x.PersonalAccountNum);

            this.tracesForSave = new List<CalculationParameterTrace>(calcProtFileInfo.Rows.Count);

            foreach (var row in calcProtFileInfo.Rows)
            {
                if (!charges.ContainsKey(row.LsNum))
                {
                    this.LogImport.Error("Ошибка", $"Лицевой счет {row.LsNum} не найден. Строка {row.RowNumber}");
                    continue;
                }

                var charge = charges[row.LsNum];
                var trace = new CalculationParameterTrace
                {
                    DateStart = row.PeriodStart,
                    DateEnd = row.PeriodEnd,
                    CalculationType = CalculationTraceType.Charge,
                    ChargePeriod = calcProtFileInfo.Period,
                    CalculationGuid = charge.Guid,
                    ParameterValues = new Dictionary<string, object>
                    {
                        {VersionedParameters.BaseTariff, row.Tariff.ToString(CultureInfo.InvariantCulture)},
                        {VersionedParameters.AreaShare, charge.AreaShare.ToString(CultureInfo.InvariantCulture)},
                        {VersionedParameters.RoomArea, row.CalcArea.ToString(CultureInfo.InvariantCulture)},
                        {"charge", row.Charge.ToString(CultureInfo.InvariantCulture)}
                    }
                };

                this.tracesForSave.Add(trace);
                this.LogImport.CountAddedRows++;
                this.LogImport.Info("Импорт протокола расчета", $"На лс {row.LsNum} добавлен протокол расчета");

                progress.TrySend("Импорт протоколов расчета");
            }

            progress.ForceSend(80, "Импорт протоколов расчета: сохранение");

            TransactionHelper.InsertInManyTransactions(this.Container, this.tracesForSave, 50000, true, true);

            progress.ForceSend(100, "Импорт протоколов расчета: завершен");
        }
    }
}