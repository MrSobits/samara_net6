using Bars.GkhCr.Modules.ClaimWork.Entities;

namespace Bars.GkhCr
{
    using B4.Modules.States;
    using System.Collections.Generic;
    using Entities;

    public class StatefulEntityManifest : IStatefulEntitiesManifest
    {
        public IEnumerable<StatefulEntityInfo> GetAllInfo()
        {
            return new[]
            {
                new StatefulEntityInfo("cr_object", "Объект КР", typeof (ObjectCr)),
                new StatefulEntityInfo("cr_obj_performed_work_act", "Акт выполненных работ", typeof (PerformedWorkAct)),
                new StatefulEntityInfo("cr_obj_defect_list", "Дефектная ведомость", typeof (DefectList)),
                new StatefulEntityInfo("cr_obj_build_contract", "Договор подряда", typeof (BuildContract)),
                new StatefulEntityInfo("cr_mass_build_contract", "Договор подряда по нескольким объектам", typeof (MassBuildContract)),
                new StatefulEntityInfo("cr_obj_estimate_calc", "Смета КР", typeof (EstimateCalculation)),
                new StatefulEntityInfo("cr_obj_contract", "Договор на услуги объекта КР", typeof (ContractCr)),
                new StatefulEntityInfo("housekeeper_report", "Отчет старшего по дому", typeof (HousekeeperReport)),
                new StatefulEntityInfo("cr_obj_monitoring_cmp", "Мониторинг СМР", typeof (MonitoringSmr)),
                new StatefulEntityInfo("cr_competition", "Конкурс", typeof (Competition)),
                new StatefulEntityInfo("ovrhl_proposal", "Предложение КР", typeof (OverhaulProposal)),
                new StatefulEntityInfo("cr_type_work", "Объект КР (работы)", typeof (TypeWorkCr)),
                new StatefulEntityInfo("build_control_type_work_smr", "Ход выполнения работ - Стройконтроль", typeof (BuildControlTypeWorkSmr)),
                new StatefulEntityInfo("clw_buildctr_claim_work", "Претензионная работа по подрядчикам", typeof(BuildContractClaimWork)),
                new StatefulEntityInfo("cr_obj_design_assignment", "Задание на проектирование", typeof (DesignAssignment)),

                new StatefulEntityInfo("special_cr_object", "Объект КР для владельцев специальных счетов", typeof (SpecialObjectCr)),
                new StatefulEntityInfo("special_cr_obj_performed_work_act", "Объекты КР для владельцев специальных счетов: Акт выполненных работ", typeof (SpecialPerformedWorkAct)),
                new StatefulEntityInfo("special_cr_obj_defect_list", "Объекты КР для владельцев специальных счетов: Дефектная ведомость", typeof (SpecialDefectList)),
                new StatefulEntityInfo("special_cr_obj_build_contract", "Объекты КР для владельцев специальных счетов: Договор подряда", typeof (SpecialBuildContract)),
                new StatefulEntityInfo("special_cr_obj_estimate_calc", "Объекты КР для владельцев специальных счетов: Смета КР", typeof (SpecialEstimateCalculation)),
                new StatefulEntityInfo("special_cr_obj_contract", "Объекты КР для владельцев специальных счетов: Договор на услуги объекта КР", typeof (SpecialContractCr)),
                new StatefulEntityInfo("special_cr_obj_monitoring_cmp", "Объекты КР для владельцев специальных счетов: Мониторинг СМР", typeof (SpecialMonitoringSmr)),
            };
        }
    }
}