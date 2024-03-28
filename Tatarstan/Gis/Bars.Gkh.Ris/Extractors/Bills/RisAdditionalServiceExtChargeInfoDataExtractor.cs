namespace Bars.Gkh.Ris.Extractors.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор "Начисление по услуге - Объемы потребления по дополнительным услугам"
    /// </summary>
    public class RisAdditionalServiceExtChargeInfoDataExtractor : BaseSlimDataExtractor<RisAdditionalServiceExtChargeInfo>
    {
        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public override List<RisAdditionalServiceExtChargeInfo> Extract(DynamicDictionary parameters)
        {
            var extractedPayDocs = parameters.GetAs<List<RisPaymentDocument>>("extractedPayDocs");
            var extractedPayDocsIds = extractedPayDocs.Select(x => x.Id).ToArray();
            var selectedHouses = parameters.GetAs("selectedList", string.Empty);
            var reportingPeriod = parameters.GetAs("reportingPeriod", default(DateTime), true);
            var selectedIds = selectedHouses.ToUpper() == "ALL" ? null : selectedHouses.ToLongArray();
            reportingPeriod = new DateTime(reportingPeriod.Year, reportingPeriod.Month, 1);

            if (!this.ExecuteProcedures(selectedIds, reportingPeriod))
            {
                return null;
            }

            var risAdditionalServiceExtChargeInfoDomain = this.Container.ResolveDomain<RisAdditionalServiceExtChargeInfo>();

            using (this.Container.Using(risAdditionalServiceExtChargeInfoDomain))
            {
                return risAdditionalServiceExtChargeInfoDomain.GetAll()
                     .Where(x => extractedPayDocsIds.Contains(x.PaymentDocument.Id))
                    .ToList();
            }
        }


        private bool ExecuteProcedures(long[] extractedPayDocs, DateTime reportingPeriod)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var session = sessionProvider.GetCurrentSession();
                var procedures = new[] { "ris_transfer_additional_service_charge_info" };
                foreach (var procedure in procedures)
                {
                    if (extractedPayDocs == null)
                    {
                        var procedureResult = this.ExecuteProcedure(session, procedure, reportingPeriod);
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
                        foreach (var houseId in extractedPayDocs)
                        {
                            var procedureResult = this.ExecuteProcedure(session, procedure, reportingPeriod, houseId);
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
        /// <param name="houseId">Идентификатор дома</param>
        /// <returns></returns>
        private ProcedureResult ExecuteProcedure(ISession session, string procedureName, DateTime reportingPeriod, long? houseId = null)
        {
            var sql = $@"select error_code ""ErrorCode"", error_text ""ErrorText"" 
        from master.{procedureName}({this.Contragent.Id}, '{reportingPeriod.ToString("yyyy-MM-dd")}'" + (houseId == null ? ")" : $", {houseId})");

            return session
                .CreateSQLQuery(sql)
                .SetTimeout(900000)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(ProcedureResult)))
                .UniqueResult<ProcedureResult>();
        }

        private class ProcedureResult
        {
            public int ErrorCode { get; set; }
            public string ErrorText { get; set; }
        }
    }
}