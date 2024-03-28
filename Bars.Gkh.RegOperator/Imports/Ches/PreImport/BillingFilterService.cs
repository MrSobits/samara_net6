namespace Bars.Gkh.RegOperator.Imports.Ches.PreImport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.ConfigSections.RegOperator.Administration.BillingInfo;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Dapper;

    /// <summary>
    /// Реализация интерфейса сервиса фильтрации импорта
    /// </summary>
    public class BillingFilterService : IBillingFilterService
    {
        private IDictionary<string, DenyReason> DisallowAccounts { get; set; }
        private IDictionary<long, DenyReason> DisallowRealityObjects { get; set; }
        private IEnumerable<int?> FundTypes { get; set; }
        private IEnumerable<int?> RoStates { get; set; }
        private IEnumerable<int?> RoTypes { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public bool IsNotAllowAllRows { get; protected set; }

        /// <inheritdoc />
        public string ConfigDescription { get; protected set; }

        /// <inheritdoc />
        public bool CheckByAccountNumber(string accountNumber, out string errorMessage)
        {
            if (this.IsNotAllowAllRows)
            {
                errorMessage = "Данная строка не была загружена: ошибка настройки импорта сведений от биллинга";
                return false;
            }

            DenyReason reason;
            if (this.DisallowAccounts.TryGetValue(accountNumber, out reason))
            {
                errorMessage = this.GetErrorMessage(reason);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <inheritdoc />
        public bool CheckByRealityObject(RealityObject realityObject, out string errorMessage)
        {
            if (this.IsNotAllowAllRows)
            {
                errorMessage = "Данная строка не была загружена: ошибка настройки импорта сведений от биллинга";
                return false;
            }

            DenyReason reason;
            if (this.DisallowRealityObjects.TryGetValue(realityObject.Id, out reason))
            {
                errorMessage = this.GetErrorMessage(reason);
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <inheritdoc />
        public void ValidateConfig()
        {
            if (this.IsNotAllowAllRows)
            {
                throw new Exception("Ошибка настройки импорта сведений от биллинга");
            }
        }

        /// <inheritdoc />
        public void Initialize(DateTime date)
        {
            var accountRepository = this.Container.ResolveRepository<BasePersonalAccount>();
            var decisionsService = this.Container.Resolve<IRealityObjectDecisionsService>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var config = this.Container.GetGkhConfig<AdministrationConfig>().Import.BillingInfo;

            this.IsNotAllowAllRows = !this.CheckConfig(config);
            this.ConfigDescription = this.SerializeConfig(config);

            using (this.Container.Using(accountRepository, decisionsService, sessionProvider))
            {
                var stateless = sessionProvider.OpenStatelessSession();
                var conn = stateless.Connection;

                this.FundTypes = new []
                {
                    config.FundFormManagement.IsSpecialAccount ? (int?)2 : null,
                    config.FundFormManagement.IsSpecialRegOpAccount ? (int?)0 : null,
                    config.FundFormManagement.IsRegOpAccount ? (int?)1 : null,
                    config.FundFormManagement.IsNotSet ? (int?)3 : null

                }.Where(x => x != null);

                this.RoStates = new[]
                {
                    config.RealityObjectState.IsNotSet ? (int?)0 : null,
                    config.RealityObjectState.IsEmergency ? (int?)10 : null,
                    config.RealityObjectState.IsDilapidated ? (int?)20 : null,
                    config.RealityObjectState.IsServiceable ? (int?)30 : null,
                    config.RealityObjectState.IsRazed ? (int?)40 : null

                }.Where(x => x != null);

                this.RoTypes = new[]
                {
                    config.RealityObjectType.IsNotSet ? (int?)0 : null,
                    config.RealityObjectType.IsBlockedBuilding ? (int?)10 : null,
                    config.RealityObjectType.IsIndividual ? (int?)20 : null,
                    config.RealityObjectType.IsManyApartments ? (int?)30 : null,
                    config.RealityObjectType.IsSocialBehavior ? (int?)40 : null

                }.Where(x => x != null);

                var sql =
                    $@"drop table if exists protocol;
                       create temp table protocol as 
                                   select id as ro_id, date_start,
                       case when decision_value = 0 and decision_type = 2 then 2
                            when decision_value = 0 and decision_type = 4 then 0
                       		 when decision_value = 0 then 1 else 3 end cr_fund
                       from (
                       select ro.id, coalesce(cf.decision_value, -1) decision_value, aco.decision_type, 
                       coalesce(case op.date_start when '-infinity' then null else op.date_start end,'2014-01-01'::date) date_start,
                       row_number() over(partition BY ro.id ORDER BY op.date_start desc) as num 
                       from gkh_reality_object ro
                       left join (gkh_obj_d_protocol op join b4_state st on st.id=op.state_id and st.final_state) on  op.ro_id=ro.id and op.date_start <= '{date:O}'
                       left join 
                       (dec_ultimate_decision dud join dec_cr_fund cf on cf.id=dud.id)  on dud.protocol_id=op.id
                       left join 
                       (dec_ultimate_decision dud1 join dec_account_owner aco on aco.id=dud1.id) on dud1.protocol_id=op.id 
                       group by ro.id, cf.decision_value, aco.decision_type,op.date_start) a where num=1;
                       
                       drop table if exists gov;
                       create temp table gov as 
											 select * from (
                       select *,row_number() over(partition BY ro_id ORDER BY date_start desc) as num  from (
                       select ro.id as ro_id, coalesce(case gov.date_start when '-infinity' then null else gov.date_start end, coalesce(gov.protocol_date,'001-01-01')) date_start,
                       (case when fund_by_regop then 1 else 3 end) cr_fund
                       from gkh_reality_object ro
                       left join (dec_gov_decision gov join b4_state st on st.id=gov.state_id and st.final_state) on gov.ro_id=ro.id and fund_by_regop and gov.date_start <= '{date:O}') a
                       group by ro_id, cr_fund,date_start)b
											 where num = 1;
                       
                       select * from (
                       select ac.acc_num as AccNum, a.ro_id as RoId, date_start, cr_fund as AccountFormationVariant, 
							  ro.condition_house as ConditionHouse, ro.type_house as TypeHouse,
							  row_number() over(partition BY ac.acc_num ORDER BY date_start desc) as num 
                       from regop_pers_acc ac  
					   join gkh_room rm on rm.id=ac.room_id
					   join gkh_reality_object ro on ro.id=rm.ro_id
                       
					   left join (
                       select ro_id, date_start, cr_fund
                       from protocol 
                       union 
                       select ro_id, date_start, cr_fund
                       from gov
                       ) a on a.ro_id=rm.ro_id)b
					   where num =1 
                       and (AccountFormationVariant not in ({string.Join(",", this.FundTypes)}) or ConditionHouse not in ({string.Join(",", this.RoStates)}) or TypeHouse not in ({string.Join(",", this.RoTypes)}));";

                var accounts = conn.Query<AccountDto>(sql)
                    .ToList();

                this.DisallowAccounts = accounts
                    .ToDictionary(x => x.AccNum,
                        x => new DenyReason
                        {
                            AccountFormationVariant = x.AccountFormationVariant,
                            TypeHouse = x.TypeHouse,
                            ConditionHouse = x.ConditionHouse
                        });
                this.DisallowRealityObjects = accounts
                    .GroupBy(x => x.RoId, x => new DenyReason
                    {
                        AccountFormationVariant = x.AccountFormationVariant,
                        TypeHouse = x.TypeHouse,
                        ConditionHouse = x.ConditionHouse
                    })
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                accounts.Clear();
            }
        }

        
        private bool CheckConfig(BillingInfoConfig config)
        {
            var fundType = config.FundFormManagement;
            var roType = config.RealityObjectType;
            var roState = config.RealityObjectState;

            return (fundType.IsNotSet || fundType.IsSpecialRegOpAccount || fundType.IsRegOpAccount || fundType.IsSpecialAccount) ||
                (roType.IsNotSet || roType.IsManyApartments || roType.IsBlockedBuilding || roType.IsIndividual || roType.IsSocialBehavior) ||
                (roState.IsNotSet || roState.IsServiceable || roState.IsEmergency || roState.IsDilapidated || roState.IsRazed);
        }

        private string GetErrorMessage(DenyReason reason)
        {
            var sb = new StringBuilder("Данная строка не была загружена: ");
            var reasonList = new List<string>(3);

            if (!this.FundTypes.Contains(reason.AccountFormationVariant.ToInt()))
            {
                reasonList.Add($"Способ формирования фонда = {reason.AccountFormationVariant.GetDisplayName()}");
            }
            if (!this.RoTypes.Contains(reason.TypeHouse.ToInt()))
            {
                reasonList.Add($"Тип дома = {reason.TypeHouse.GetDisplayName()}");
            }
            if (!this.RoStates.Contains(reason.ConditionHouse.ToInt()))
            {
                reasonList.Add($"Состояние дома = {reason.ConditionHouse.GetDisplayName()}");
            }

            sb.Append(string.Join(", ", reasonList));

            return sb.ToString();
        }

        private string SerializeConfig(BillingInfoConfig config)
        {
            var sb = new StringBuilder();

            sb.AppendLine(config.GetDisplayName());
            this.SerializeProperty(sb, config.FundFormManagement);
            this.SerializeProperty(sb, config.RealityObjectState);
            this.SerializeProperty(sb, config.RealityObjectType);

            return sb.ToString();
        }

        private void SerializeProperty(StringBuilder sb, IGkhConfigSection config)
        {
            sb.AppendLine($"{config.GetDisplayName()}:");
            var configParams = new List<string>();
            foreach (var property in config.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var name = property.GetCustomAttribute<GkhConfigPropertyAttribute>().DisplayName;
                var value = property.GetValue(config);
                configParams.Add($"{name}: {value}");
            }
            sb.AppendLine(configParams.AggregateWithSeparator(", "));
        }

        private class DenyReason
        {
            public CrFundFormationType? AccountFormationVariant { get; set; }
            public TypeHouse? TypeHouse { get; set; }
            public ConditionHouse? ConditionHouse { get; set; }
        }

        private class AccountDto
        {
            public long RoId { get; set; }
            public string AccNum { get; set; }
            public CrFundFormationType? AccountFormationVariant { get; set; }
            public TypeHouse? TypeHouse { get; set; }
            public ConditionHouse? ConditionHouse { get; set; }
        }
    }
}