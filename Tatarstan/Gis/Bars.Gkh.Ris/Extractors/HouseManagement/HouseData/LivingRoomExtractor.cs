namespace Bars.Gkh.Ris.Extractors.HouseManagement.HouseData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using NHibernate;
    using NHibernate.Transform;

    /// <summary>
    /// Экстрактор данных по комнатам
    /// </summary>
    public class LivingRoomExtractor : BaseSlimDataExtractor<LivingRoom>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<LivingRoom> Extract(DynamicDictionary parameters)
        {
            var houses = parameters.GetAs<List<RisHouse>>("houses");
            var houseIds = houses?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[0];

            if (!this.ExecuteProcedure(houseIds))
            {
                return null;
            }

            return this.GetData(houseIds);
        }

        /// <summary>
        /// Выполнение хранимой процедуры
        /// </summary>
        /// <param name="selectedHouseIds">Идентификаторы домов</param>
        /// <returns></returns>
        private bool ExecuteProcedure(long[] selectedHouseIds)
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            try
            {
                var session = sessionProvider.GetCurrentSession();
                var procedure = "ris_transfer_livingroom";

                if (selectedHouseIds == null)
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
                    foreach (var houseId in selectedHouseIds)
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
            string sqlParams = "(" + this.Contragent.Id;

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
        /// <param name="selectedHouses">Идентификаторы домов</param>
        /// <returns>Список комнат</returns>
        private List<LivingRoom> GetData(long[] selectedHouses)
        {
            var livingRoomDomain = this.Container.ResolveDomain<LivingRoom>();
            var realityObjectDomain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                var houseIds = realityObjectDomain.GetAll()
                    .WhereIf(selectedHouses != null, x => selectedHouses.Contains(x.Id))
                    .Select(x => x.Id);

                return livingRoomDomain.GetAll()
                    .Where(x => x.Contragent == this.Contragent)
                    .WhereIf(selectedHouses != null, 
                        x => (x.House != null && houseIds.Contains(x.House.ExternalSystemEntityId)) ||
                            (x.ResidentialPremises != null && x.ResidentialPremises.ApartmentHouse != null && houseIds.Contains(x.ResidentialPremises.ApartmentHouse.ExternalSystemEntityId)))
                    .ToList();
            }
            finally
            {
                this.Container.Release(livingRoomDomain);
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