namespace Bars.Gkh.Ris.Extractors.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор данных документов «Извещение о принятии к исполнению распоряжения»
    /// </summary>
    public class NotificationOfOrderExecutionExtractor : BaseSlimDataExtractor<NotificationOfOrderExecution>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<NotificationOfOrderExecution> Extract(DynamicDictionary parameters)
        {
            var selectedHouses = parameters.GetAs("selectedList", string.Empty);
            var reportingPeriod = parameters.GetAs("reportingPeriod", default(DateTime), true);
            var selectedIds = selectedHouses.ToUpper() == "ALL" ? null : selectedHouses.ToLongArray();
            reportingPeriod = new DateTime(reportingPeriod.Year, reportingPeriod.Month, 1);

            if (!this.ExecuteProcedure(selectedIds, reportingPeriod))
            {
                return null;
            }

            return this.GetData(selectedIds, reportingPeriod);

        }

        /// <summary>
        /// Выполнение хранимой процедуры
        /// </summary>
        /// <param name="selectedHouseIds">Идентификаторы домов</param>
        /// <param name="period">Период</param>
        /// <returns></returns>
        private bool ExecuteProcedure(long[] selectedHouseIds, DateTime period)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var session = sessionProvider.GetCurrentSession();
                var procedure = "ris_transfer_notiforderexecut";
                
                if (selectedHouseIds == null)
                {
                    var procedureResult = this.ExecuteProcedure(session, procedure, period);
                    if (procedureResult.ErrorCode == -1)
                    {
                        this.AddLogRecord(
                            new BaseLogRecord(
                                MessageType.Error,
                                $"Ошибка выполнения процедуры {procedure}: {procedureResult.ErrorText}"));

                        return false;
                    }
                }
                else
                {
                    foreach (var houseId in selectedHouseIds)
                    {
                        var procedureResult = this.ExecuteProcedure(session, procedure, period, houseId);
                        if (procedureResult.ErrorCode == -1)
                        {
                            this.AddLogRecord(
                                new BaseLogRecord(
                                    MessageType.Error,
                                    $"Ошибка выполнения процедуры {procedure}: {procedureResult.ErrorText}"));

                            return false;
                        }
                    }
                }

                return true;
            }
            finally
            {
                this.Container.Release(sessionProvider);
            }
        }

        /// <summary>
        /// Выполнить хранимую процедуру
        /// </summary>
        /// <param name="session">Сессия</param>
        /// <param name="procedureName">Наименование процедуры</param>
        /// <param name="period">Период</param>
        /// <param name="houseId">Идентификатор дома</param>
        /// <returns></returns>
        private ProcedureResult ExecuteProcedure(ISession session, string procedureName, DateTime period, long? houseId = null)
        {
            string sqlParams = "(" + this.Contragent.Id.ToString() + ",'" + period.ToString("yyyy-MM-dd") + "'";

            if (houseId == null)
            {
                sqlParams += ")";
            }
            else
            {
                sqlParams += "," + houseId.ToString() + ")";
            }

            string sqlQuery = "select error_code \"ErrorCode\", error_text \"ErrorText\" from master." + procedureName + sqlParams;

            return session
                .CreateSQLQuery(sqlQuery)
                .SetTimeout(900000)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(ProcedureResult)))
                .UniqueResult<ProcedureResult>();
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        private List<NotificationOfOrderExecution> GetData(long[] selectedHouses, DateTime reportingPeriod)
        {
            var notificationOfOrderExecutionDomain = this.Container.ResolveDomain<NotificationOfOrderExecution>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                var fiasHouseGuids = realityObjectDomain.GetAll()
                    .WhereIf(selectedHouses != null, x => selectedHouses.Contains(x.Id))
                    .Select(x => x.HouseGuid);

                return notificationOfOrderExecutionDomain.GetAll()
                    .Where(x => x.Contragent == this.Contragent)
                    .WhereIf(selectedHouses != null, x => fiasHouseGuids.Any(y => y == x.FiasHouseGuid))
                    .WhereIf(reportingPeriod > DateTime.MinValue, x => x.Year == reportingPeriod.Year && x.Month == reportingPeriod.Month)
                    .ToList();
            }
            finally
            {
                this.Container.Release(notificationOfOrderExecutionDomain);
                this.Container.Release(realityObjectDomain);
            }
        }

        private class ProcedureResult
        {
            public int ErrorCode { get; set; }
            public string ErrorText { get; set; }
        }
    }
}
