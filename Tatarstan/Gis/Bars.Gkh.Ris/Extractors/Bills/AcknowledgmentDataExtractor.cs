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
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор "Сведения о квитировании"
    /// </summary>
    public class AcknowledgmentDataExtractor : BaseSlimDataExtractor<RisAcknowledgment>
    {
        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public override List<RisAcknowledgment> Extract(DynamicDictionary parameters)
        {
            var selectedList = parameters.GetAs("selectedList", string.Empty);
            var selectedIds = selectedList.ToUpper() == "ALL" ? null : selectedList.ToLongArray();

            var acknowledgmentDomain = this.Container.ResolveDomain<RisAcknowledgment>();
            var accountRelationsDomain = this.Container.ResolveDomain<RisAccountRelations>();
            var notificationOfOrderExecutionDomain = this.Container.ResolveDomain<NotificationOfOrderExecution>();

            using (this.Container.Using(acknowledgmentDomain, accountRelationsDomain, notificationOfOrderExecutionDomain))
            {
                var notifications = notificationOfOrderExecutionDomain.GetAll()
                    .Where(x => x.Guid != null && x.OrderDate != null)
                    .Where(x => x.Contragent.Id == this.Contragent.Id);

                // через уведомления достаём дома по периодам
                var dictByPeriodAndHouse = accountRelationsDomain.GetAll()
                    .Where(x => notifications.Any(y => y.RisPaymentDocument.Account.Id == x.Account.Id))
                    .ToDictionary(x => x.Account.Id, x => x.House.Id);

                var accDict = notifications
                    .AsEnumerable()
                    .GroupBy(x => x.OrderDate)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.RisPaymentDocument.Account.Id).Distinct());

                var dictHousesByPeriod = accDict
                    .Select(
                        dateAccs => new KeyValuePair<DateTime, long[]>(
                            dateAccs.Key.Value,
                            dictByPeriodAndHouse.Where(accHouse => dateAccs.Value.Any(acc => acc == accHouse.Key)).Select(y => y.Value).ToArray()));

                if (dictHousesByPeriod.Any(kvp => !this.ExecuteProcedures(kvp.Value, kvp.Key)))
                {
                    return null;
                }

                return acknowledgmentDomain.GetAll()
                    .Where(x => x.Contragent.Id == this.Contragent.Id)
                    .Where(x => notifications.Any(y => y.Id == x.Notification.Id))
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
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
                    "ris_transfer_acknowledgment"
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

        private ProcedureResult ExecuteProcedure(ISession session, string procedureName, DateTime reportingPeriod, long? houseId = null)
        {
            var sql = $@"select error_code ""ErrorCode"", error_text ""ErrorText"" 
        from master.{procedureName}({this.Contragent.Id}, '{reportingPeriod.ToString("yyyy-MM-dd")}'," + (houseId == null ? ")" : $", {houseId})");

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