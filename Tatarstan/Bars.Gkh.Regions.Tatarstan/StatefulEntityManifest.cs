namespace Bars.Gkh.Regions.Tatarstan
{ 
    using System.Collections.Generic;

    using B4.Modules.States;

    using Bars.Gkh.Regions.Tatarstan.Entities.UtilityDebtor;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    using Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("gkh_construct_obj", "Объект строительства", typeof(ConstructionObject)),
                new StatefulEntityInfo("gkh_construct_obj_contract", "Договор объекта строительства", typeof(ConstructionObjectContract)),
                new StatefulEntityInfo("gkh_construct_obj_smr", "Мониторинг СМР для объекта строительства", typeof(ConstructObjMonitoringSmr)),
                new StatefulEntityInfo("clw_utility_debtor_claim_work", "Претензионная работа по неплательщикам ЖКУ", typeof(UtilityDebtorClaimWork)),
                new StatefulEntityInfo("clw_document_exec_process", "Документ ПИР - Исполнительное производство", typeof(ExecutoryProcess)),
                new StatefulEntityInfo("gkh_norm_consumption", "Нормативы потребления", typeof(NormConsumption)),
                new StatefulEntityInfo("gkh_contr_period_summ_rso", "Расщепление платежей РСО", typeof(ContractPeriodSummRso)),
                new StatefulEntityInfo("gkh_contr_period_summ_uo", "Расщепление платежей УО", typeof(ContractPeriodSummUo))
            };
        }
    }
}