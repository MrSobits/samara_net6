namespace Bars.Gkh.Ris.Extractors.HouseManagement.MeteringDeviceData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор данных по ПУ
    /// </summary>
    public class MeteringDeviceDataExtractor : BaseSlimDataExtractor<RisMeteringDeviceData>
    {
        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public override List<RisMeteringDeviceData> Extract(DynamicDictionary parameters)
        {
            long[] selectedIds = { };

            var selectedHouses = parameters.GetAs("selectedList", string.Empty);
            if (selectedHouses.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedHouses.ToLongArray();
            }

            if (!this.ExecuteProcedures(selectedIds))
            {
                return null;
            }

            return this.GetData(selectedIds);
        }

        /// <summary>
        /// Выполнение хранимых процедур
        /// </summary>
        /// <param name="selectedHouses"></param>
        /// <returns></returns>
        private bool ExecuteProcedures(long[] selectedHouses)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var session = sessionProvider.GetCurrentSession();
                var procedures = new[]
                {
                    "ris_compare_house",
                    "ris_compare_premise",
                    "ris_compare_living_room",
                    "ris_compare_account",
                    "ris_transfer_metering_device_data",
                    "ris_transfer_metering_device_account",
                    "ris_transfer_metering_device_living_room"
                };

                foreach (var procedure in procedures)
                {
                    if (selectedHouses == null)
                    {
                        var procedureResult = this.ExecuteProcedure(session, procedure);
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
                        foreach (var houseId in selectedHouses)
                        {
                            var procedureResult = this.ExecuteProcedure(session, procedure, houseId);
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
        private ProcedureResult ExecuteProcedure(ISession session, string procedureName, long? houseId = null)
        {
            var sql = $@"select error_code ""ErrorCode"", error_text ""ErrorText"" from master.{procedureName}({this.Contragent.Id}" +
                (houseId == null
                    ? ")"
                    : $", {houseId})");

            return session
                .CreateSQLQuery(sql)
                .SetTimeout(900000)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(ProcedureResult)))
                .UniqueResult<ProcedureResult>();
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        private List<RisMeteringDeviceData> GetData(long[] selectedHouses)
        {
            var risMeteringDeviceDataDomain = this.Container.ResolveDomain<RisMeteringDeviceData>();

            try
            {
                return risMeteringDeviceDataDomain.GetAll()
                    .WhereIf(selectedHouses != null, x => x.House != null && selectedHouses.Contains(x.House.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(risMeteringDeviceDataDomain);
            }
        }

        private class ProcedureResult
        {
            public int ErrorCode { get; set; }
            public string ErrorText { get; set; }
        }
    }
}
