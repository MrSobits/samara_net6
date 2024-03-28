namespace Bars.Gkh.Ris.Extractors.HouseManagement.AccountData
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
    /// Экстрактор данных по ЛС
    /// </summary>
    public class AccountDataExtractor : BaseSlimDataExtractor<RisAccount>
    {
        /// <summary>
        /// Получить внутренние сущности системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Внутренние cущности системы</returns>
        public override List<RisAccount> Extract(DynamicDictionary parameters)
        {
            var houseList = parameters.GetAs("houseList", string.Empty);

            long[] selectedHouses = { };
            if (houseList.ToUpper() == "ALL")
            {
                selectedHouses = new long[0]; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedHouses = houseList.ToLongArray();
            }

            //Выполнение хранимых процедур
            if (!this.ExecuteProcedures(selectedHouses))
            {
                return null;
            }

            //Получение данных
            return this.GetData(selectedHouses);
        }

        /// <summary>
        /// Вызовы хранимых процедур для заполнения таблиц ris_*
        /// </summary>
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
                    "ris_transfer_livingroom",
                    "ris_transfer_account"
                };

                foreach (var procedure in procedures)
                {
                    if (selectedHouses.Any())
                    {
                        //Если передан список домов, то вызываем процедурки для каждого из них
                        foreach (var houseId in selectedHouses)
                        {
                            if (!this.CallProcedure(session, procedure, this.Contragent.Id, houseId))
                            {
                                return false;
                            }

                        }
                    }
                    else
                    {
                        //Иначе - вызываем их без указания дома
                        if (!this.CallProcedure(session, procedure, this.Contragent.Id))
                        {
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

        private bool CallProcedure(ISession session, string procedureName, params long[] parameters)
        {
            var sql = $@"
                select error_code ""ErrorCode"", error_text ""ErrorText"" 
                from master.{procedureName}({string.Join(", ", parameters)})
            ";
            var procedureResult = session
                .CreateSQLQuery(sql)
                .SetTimeout(900000)
                .SetResultTransformer(new AliasToBeanResultTransformer(typeof(ProcedureResultDto)))
                .UniqueResult<ProcedureResultDto>();
            if (procedureResult.ErrorCode == -1)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Error, $"Ошибка выполнения процедуры {procedureName}: {procedureResult.ErrorText}"));
                return false;
            }

            return true;
        }

        /// <summary>
        /// Получение данных
        /// </summary>
        private List<RisAccount> GetData(long[] selectedHouses)
        {
            var risAccountRelationsRepository = this.Container.ResolveDomain<RisAccountRelations>();

            try
            {
                var res = risAccountRelationsRepository
                    .GetAll()
                    .Where(x => x.Account != null)
                    .Where(x => x.Account.Contragent.Id == this.Contragent.Id)
                    .WhereIf(selectedHouses.Any(), x => x.House != null && selectedHouses.Contains(x.House.ExternalSystemEntityId))
                    .Select(
                        x => new RisAccount
                        {
                            Id = x.Account.Id,
                            Contragent = x.Account.Contragent,
                            OwnerInd = x.Account.OwnerInd,
                            AccountNumber = x.Account.AccountNumber,
                            RisAccountType = x.Account.RisAccountType,
                            IsRenter = x.Account.IsRenter,
                            BeginDate = x.Account.BeginDate,
                            CloseDate = x.Account.CloseDate,
                            CloseReasonCode = x.Account.CloseReasonCode,
                            CloseReasonGuid = x.Account.CloseReasonGuid,
                            Closed = x.Account.Closed,
                            ExternalSystemEntityId = x.Account.ExternalSystemEntityId,
                            ExternalSystemName = x.Account.ExternalSystemName,
                            Guid = x.Account.Guid,
                            HeatedArea = x.Account.HeatedArea,
                            LivingPersonsNumber = x.Account.LivingPersonsNumber,
                            OwnerOrg = x.Account.OwnerOrg,
                            ResidentialSquare = x.Account.ResidentialSquare,
                            TotalSquare = x.Account.TotalSquare,
                            LivingRoomGuid = x.LivingRoom.Guid,
                            HouseFiasGuid = x.House.FiasHouseGuid,
                            LivingPremiseGuid = x.ResidentialPremise.Guid,
                            NonLivingPremiseGuid = x.NonResidentialPremises.Guid
                        }).ToList();

                return res;
            }
            finally
            {
                this.Container.Release(risAccountRelationsRepository);
            }
        }

        private class ProcedureResultDto
        {
            public int ErrorCode { get; set; }
            public string ErrorText { get; set; }
        }
    }
}