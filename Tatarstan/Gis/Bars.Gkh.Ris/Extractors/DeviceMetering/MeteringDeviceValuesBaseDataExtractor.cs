namespace Bars.Gkh.Ris.Extractors.DeviceMetering
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.DeviceMetering;
    using Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор показаний ПУ
    /// </summary>
    /// <typeparam name="TRisEntity">Тип показаний</typeparam>
    public class MeteringDeviceValuesBaseDataExtractor<TRisEntity> : BaseSlimDataExtractor<TRisEntity> where TRisEntity : BaseRisEntity
    {
        private readonly IDictionary<Type, string> proceduresName = new Dictionary<Type, string>
        {
            {typeof(RisMeteringDeviceControlValue),  "ris_transfer_metering_device_control_value"},
            {typeof(RisMeteringDeviceCurrentValue),  "ris_transfer_metering_device_current_value"},
            {typeof(RisMeteringDeviceVerificationValue),  "ris_transfer_metering_device_verification_value"},
        }; 

        /// <summary>
        /// Получить сущности внутренней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности внутренней системы</returns>
        public override List<TRisEntity> Extract(DynamicDictionary parameters)
        {
            var meteringDeviceBasePrepareAssistant = (IMeteringDeviceBasePrepareAssistant<TRisEntity>)parameters.GetValue("meteringDeviceBasePrepareAssistant");
            var selectedHouses = parameters.GetAs("selectedList", string.Empty);

            var selectedIds = selectedHouses.ToUpper() == "ALL" ? null : selectedHouses.ToLongArray();

            if (!this.ExecuteProcedures(selectedIds))
            {
                return null;
            }
            
            return meteringDeviceBasePrepareAssistant.GetData(parameters);
        }

        private bool ExecuteProcedures(long[] selectedHouses)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var session = sessionProvider.GetCurrentSession();
                var procedures = new[]
                {
                    "ris_compare_metering_device_data",
                    this.proceduresName.Get(typeof(TRisEntity))
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
                (houseId == null ? ")" :$", {houseId})");

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