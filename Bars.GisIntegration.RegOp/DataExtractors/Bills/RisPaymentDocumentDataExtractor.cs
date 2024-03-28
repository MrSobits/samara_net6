namespace Bars.GisIntegration.RegOp.DataExtractors.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор платёжных документов
    /// </summary>
    public class RisPaymentDocumentDataExtractor : BaseSlimDataExtractor<RisPaymentDocument>
    {
        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public override List<RisPaymentDocument> Extract(DynamicDictionary parameters)
        {
            var selectedHouses = parameters.GetAs("selectedList", string.Empty);
            var reportingPeriod = parameters.GetAs("reportingPeriod", default(DateTime), true);
            var selectedIds = selectedHouses.ToUpper() == "ALL" ? null : selectedHouses.ToLongArray();

            reportingPeriod = new DateTime(reportingPeriod.Year, reportingPeriod.Month, 1);

            if (!this.ExecuteProcedures(selectedIds, reportingPeriod))
            {
                return null;
            }

            var paymentDocumentDomain = this.Container.ResolveDomain<RisPaymentDocument>();
            var accountRelationsDomain = this.Container.ResolveDomain<RisAccountRelations>();
            var houseDomain = this.Container.ResolveDomain<RisHouse>();

            using (this.Container.Using(paymentDocumentDomain, accountRelationsDomain, houseDomain))
            {
                var houseIds = houseDomain.GetAll().Where(x => selectedIds.Contains(x.ExternalSystemEntityId)).Select(x => x.Id).ToArray();

                var accQuery = accountRelationsDomain.GetAll()
                    .Where(x => x.Contragent.Id == this.Contragent.Id)
                    .WhereIf(houseIds.Any(), x => houseIds.Contains(x.House.Id));

                return paymentDocumentDomain.GetAll()
                    .Where(x => x.Contragent.Id == this.Contragent.Id)
                    .Where(x => accQuery.Any(y => y.Account.Id == x.Account.Id))
                    .WhereIf(reportingPeriod > DateTime.MinValue, x => x.PeriodYear == reportingPeriod.Year && x.PeriodMonth == reportingPeriod.Month)
                    .ToList();
            }
        }

        private bool ExecuteProcedures(long[] selectedHouseIds, DateTime reportingPeriod)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var session = sessionProvider.GetCurrentSession();
                var procedures = new[]
                {
                     "ris_transfer_address_info",
                     "ris_transfer_payment_info",
                     "ris_transfer_payment_doc"
                };

                foreach (var procedure in procedures)
                {
                    if (selectedHouseIds == null)
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
                        foreach (var houseId in selectedHouseIds)
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
        /// <param name="reportingPeriod">Отчетный период</param>
        /// <param name="houseId">Идентификатор дома</param>
        /// <returns></returns>
        private ProcedureResult ExecuteProcedure(ISession session, string procedureName, DateTime reportingPeriod, long? houseId = null)
        {
            var sql = $@"select error_code ""ErrorCode"", error_text ""ErrorText"" 
        from ris.{procedureName}({this.Contragent.Id}, '{reportingPeriod.ToString("yyyy-MM-dd")}'" + (houseId == null ? ")" : $", {houseId})");

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