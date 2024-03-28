namespace Bars.GkhCr
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("bankstatement", "B4.controller.BankStatement", requiredPermission: "GkhCr.BankStatement.View"));
            map.AddRoute(new ClientRoute("controldate", "B4.controller.ControlDate", requiredPermission: "GkhCr.ControlDate.View"));
            map.AddRoute(new ClientRoute("estimateregister", "B4.controller.EstimateRegister", requiredPermission: "GkhCr.Estimate.View"));
            map.AddRoute(new ClientRoute("objectcr", "B4.controller.ObjectCr", requiredPermission: "GkhCr.ObjectCrViewCreate.View"));
            map.AddRoute(new ClientRoute("objcrmasschangestate", "B4.controller.ObjectCrMassChangeState", requiredPermission: "GkhCr.ObjectCrMassStateChange.View"));
            map.AddRoute(new ClientRoute("qualificationregister", "B4.controller.QualificationRegister", requiredPermission: "GkhCr.QualificationMember.View"));
            map.AddRoute(new ClientRoute("wrkactregister", "B4.controller.WorkActRegister", requiredPermission: "GkhCr.WorkAct.View"));
            map.AddRoute(new ClientRoute("financesource", "B4.controller.dict.FinanceSource", requiredPermission: "GkhCr.Dict.FinanceSource.View"));
            map.AddRoute(new ClientRoute("stageworkcr", "B4.controller.dict.StageWorkCr", requiredPermission: "GkhCr.Dict.StageWorkCr.View"));
            map.AddRoute(new ClientRoute("qualificationmember", "B4.controller.dict.QualificationMember", requiredPermission: "GkhCr.Dict.QualMember.View"));
            map.AddRoute(new ClientRoute("programcr", "B4.controller.dict.ProgramCr", requiredPermission: "GkhCr.ProgramCr.View"));
            map.AddRoute(new ClientRoute("paymentorder", "B4.controller.bankstatement.PaymentOrder", requiredPermission: "GkhCr.PaymentOrder.View"));
            map.AddRoute(new ClientRoute("official", "B4.controller.dict.Official", requiredPermission: "GkhCr.Dict.Official.View"));
            map.AddRoute(new ClientRoute("terminationreason", "B4.controller.dict.TerminationReason", requiredPermission: "GkhCr.Dict.TerminationReason.View"));
            map.AddRoute(new ClientRoute("performedworkimport", "B4.controller.import.PerformedWork", requiredPermission: "Import.PerformedWork.View"));
            map.AddRoute(new ClientRoute("crfileregister", "B4.controller.CrFileRegister", requiredPermission: "GkhCr.CrFileRegister.View"));

            map.AddRoute(new ClientRoute("dotmap/{id}", "B4.controller.objectcr.YandexBuildContractMap"));

            map.AddRoute(new ClientRoute("overhaulpropose", "B4.controller.OverhaulProposal", requiredPermission: "GkhCr.OverhaulProposal.View"));
            map.AddRoute(new ClientRoute("massbuildcontract", "B4.controller.MassBuildContract", requiredPermission: "GkhCr.OverhaulProposal.View"));

            map.AddRoute(new ClientRoute("objectcredit/{id}", "B4.controller.objectcr.Navi", requiredPermission: "GkhCr.ObjectCrViewCreate.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/edit", "B4.controller.objectcr.Edit", requiredPermission: "GkhCr.ObjectCrViewCreate.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/contractcr", "B4.controller.objectcr.ContractCr", requiredPermission: "GkhCr.ObjectCr.Register.ContractCrViewCreate.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/protocol", "B4.controller.objectcr.Protocol", requiredPermission: "GkhCr.ObjectCr.Register.Protocol.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/defectlist", "B4.controller.objectcr.DefectList", requiredPermission: "GkhCr.ObjectCr.Register.DefectListViewCreate.Create"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/housekeeper", "B4.controller.objectcr.HousekeeperReport", requiredPermission: "GkhCr.ObjectCr.Register.HousekeeperReport.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/financesourceres", "B4.controller.objectcr.FinanceSourceRes", requiredPermission: "GkhCr.ObjectCr.Register.FinanceSourceRes.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/typeworkcr", "B4.controller.objectcr.TypeWorkCr", requiredPermission: "GkhCr.ObjectCr.Register.TypeWork.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/personalaccount", "B4.controller.objectcr.PersonalAccount", requiredPermission: "GkhCr.ObjectCr.Register.PersonalAccount.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/designassignment", "B4.controller.objectcr.DesignAssignment", requiredPermission: "GkhCr.ObjectCr.Register.DesignAssignment.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/competition", "B4.controller.objectcr.Competition", requiredPermission: "GkhCr.ObjectCr.Register.Competition.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/estimatecalculation", "B4.controller.objectcr.EstimateCalculation", requiredPermission: "GkhCr.ObjectCr.Register.EstimateCalculationViewCreate.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/qualification", "B4.controller.objectcr.Qualification", requiredPermission: "GkhCr.ObjectCr.Register.Qualification.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/monitoringsmr", "B4.controller.objectcr.MonitoringSmr", requiredPermission: "GkhCr.ObjectCr.Register.MonitoringSmr.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/scheduleexecutionwork", "B4.controller.objectcr.ScheduleExecutionWork", requiredPermission: "GkhCr.ObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/progressexecutionwork", "B4.controller.objectcr.ProgressExecutionWork", requiredPermission: "GkhCr.ObjectCr.Register.MonitoringSmr.ProgressExecutionWork.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/workerscountwork", "B4.controller.objectcr.WorkersCountWork", requiredPermission: "GkhCr.ObjectCr.Register.MonitoringSmr.WorkersCount.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/documentworkcr", "B4.controller.objectcr.DocumentWorkCr", requiredPermission: "GkhCr.ObjectCr.Register.MonitoringSmr.Document.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/performedworkact", "B4.controller.objectcr.PerformedWorkAct", requiredPermission: "GkhCr.ObjectCr.Register.PerformedWorkActViewCreate.View"));
            map.AddRoute(new ClientRoute("objectcredit/{id}/additionalparams", "B4.controller.objectcr.AdditionalParameters", requiredPermission: "GkhCr.ObjectCr.AdditionalParametersViewCreate.View"));

            map.AddRoute(new ClientRoute("specialobjectcr", "B4.controller.SpecialObjectCr", requiredPermission: "GkhCr.SpecialObjectCrViewCreate.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}", "B4.controller.specialobjectcr.Navi", requiredPermission: "GkhCr.SpecialObjectCrViewCreate.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/edit", "B4.controller.specialobjectcr.Edit", requiredPermission: "GkhCr.SpecialObjectCrViewCreate.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/contractcr", "B4.controller.specialobjectcr.ContractCr", requiredPermission: "GkhCr.SpecialObjectCr.Register.ContractCrViewCreate.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/protocol", "B4.controller.specialobjectcr.Protocol", requiredPermission: "GkhCr.SpecialObjectCr.Register.Protocol.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/defectlist", "B4.controller.specialobjectcr.DefectList", requiredPermission: "GkhCr.SpecialObjectCr.Register.DefectListViewCreate.Create"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/financesourceres", "B4.controller.specialobjectcr.FinanceSourceRes", requiredPermission: "GkhCr.SpecialObjectCr.Register.FinanceSourceRes.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/typeworkcr", "B4.controller.specialobjectcr.TypeWorkCr", requiredPermission: "GkhCr.SpecialObjectCr.Register.TypeWork.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/personalaccount", "B4.controller.specialobjectcr.PersonalAccount", requiredPermission: "GkhCr.SpecialObjectCr.Register.PersonalAccount.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/designassignment", "B4.controller.specialobjectcr.DesignAssignment", requiredPermission: "GkhCr.SpecialObjectCr.Register.DesignAssignment.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/competition", "B4.controller.specialobjectcr.Competition", requiredPermission: "GkhCr.SpecialObjectCr.Register.Competition.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/estimatecalculation", "B4.controller.specialobjectcr.EstimateCalculation", requiredPermission: "GkhCr.SpecialObjectCr.Register.EstimateCalculationViewCreate.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/qualification", "B4.controller.specialobjectcr.Qualification", requiredPermission: "GkhCr.SpecialObjectCr.Register.Qualification.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/monitoringsmr", "B4.controller.specialobjectcr.MonitoringSmr", requiredPermission: "GkhCr.SpecialObjectCr.Register.MonitoringSmr.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/scheduleexecutionwork", "B4.controller.specialobjectcr.ScheduleExecutionWork", requiredPermission: "GkhCr.SpecialObjectCr.Register.MonitoringSmr.ScheduleExecutionWork.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/progressexecutionwork", "B4.controller.specialobjectcr.ProgressExecutionWork", requiredPermission: "GkhCr.SpecialObjectCr.Register.MonitoringSmr.ProgressExecutionWork.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/workerscountwork", "B4.controller.specialobjectcr.WorkersCountWork", requiredPermission: "GkhCr.SpecialObjectCr.Register.MonitoringSmr.WorkersCount.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/documentworkcr", "B4.controller.specialobjectcr.DocumentWorkCr", requiredPermission: "GkhCr.SpecialObjectCr.Register.MonitoringSmr.Document.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/performedworkact", "B4.controller.specialobjectcr.PerformedWorkAct", requiredPermission: "GkhCr.SpecialObjectCr.Register.PerformedWorkActViewCreate.View"));
            map.AddRoute(new ClientRoute("specialobjectcredit/{id}/additionalparams", "B4.controller.specialobjectcr.AdditionalParameters", requiredPermission: "GkhCr.SpecialObjectCr.AdditionalParametersViewCreate.View"));

            map.AddRoute(new ClientRoute("competition", "B4.controller.Competition", requiredPermission: "GkhCr.Competition.View"));
            map.AddRoute(new ClientRoute("competitionedit/{id}", "B4.controller.competition.Navi", requiredPermission: "GkhCr.Competition.View"));
            map.AddRoute(new ClientRoute("competitionedit/{id}/edit", "B4.controller.competition.Edit", requiredPermission: "GkhCr.Competition.Edit"));
            map.AddRoute(new ClientRoute("competitionedit/{id}/lot", "B4.controller.competition.Lot", requiredPermission: "GkhCr.Competition.Lot.View"));
            map.AddRoute(new ClientRoute("competitionedit/{id}/lot/{lotId}", "B4.controller.competition.Lot", "GkhCr.Competition.Lot.View"));
            map.AddRoute(new ClientRoute("competitionedit/{id}/doc", "B4.controller.competition.Document", requiredPermission: "GkhCr.Competition.Document.View"));
            map.AddRoute(new ClientRoute("competitionedit/{id}/protocol", "B4.controller.competition.Protocol", requiredPermission: "GkhCr.Competition.Protocol.View"));

            map.AddRoute(new ClientRoute("buildcontractclaimworksettings", "B4.controller.claimwork.BuildContractSettings"));
            map.AddRoute(new ClientRoute("builderviolator", "B4.controller.claimwork.BuilderViolator", requiredPermission: "GkhCr.BuilderViolator.View"));
            map.AddRoute(new ClientRoute("buildcontractclaimwork", "B4.controller.claimwork.BuildContract"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/buildctredit", "B4.controller.claimwork.EditBuildContract"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/{docId}/lawsuit", "B4.controller.claimwork.LawsuitBuildContract"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/{docId}/pretension", "B4.controller.claimwork.PretensionBuildContract"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/{docId}/notification", "B4.controller.claimwork.NotificationBuildContract"));
            map.AddRoute(new ClientRoute("claimworkbc/BuildContractClaimWork/{id}/{docId}/actviolidentification", "B4.controller.claimwork.ActViolIdentification"));

            map.AddRoute(new ClientRoute("workscr", "B4.controller.WorksCr", requiredPermission: "GkhCr.TypeWorkCr.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}", "B4.controller.workscr.Navi", requiredPermission: "GkhCr.TypeWorkCr.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/edit", "B4.controller.workscr.Edit", requiredPermission: "GkhCr.TypeWorkCr.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/inspection", "B4.controller.workscr.Inspection", requiredPermission: "GkhCr.TypeWorkCr.Register.Inspection.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/contract", "B4.controller.workscr.Contract", requiredPermission: "GkhCr.TypeWorkCr.Register.ContractCrViewCreate.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/protocol", "B4.controller.workscr.Protocol", requiredPermission: "GkhCr.TypeWorkCr.Register.Protocol.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/defectlist", "B4.controller.workscr.DefectList", requiredPermission: "GkhCr.TypeWorkCr.Register.DefectListViewCreate.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/finsources", "B4.controller.workscr.FinSources", requiredPermission: "GkhCr.TypeWorkCr.Field.FinanceSource_Edit"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/estimate", "B4.controller.workscr.Estimate", requiredPermission: "GkhCr.TypeWorkCr.Register.EstimateCalculationViewCreate.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/buildcontract", "B4.controller.workscr.BuildContract", requiredPermission: "GkhCr.TypeWorkCr.Register.BuildContractViewCreate.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/smr", "B4.controller.workscr.Smr", requiredPermission: "GkhCr.ObjectCr.Register.MonitoringSmr.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/progressexecution", "B4.controller.workscr.ProgressExecution", requiredPermission: "GkhCr.TypeWorkCr.Register.MonitoringSmr.ProgressExecutionWork.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/scheduleexecution", "B4.controller.workscr.ScheduleExecution", requiredPermission: "GkhCr.TypeWorkCr.Register.MonitoringSmr.ScheduleExecutionWork.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/workerscount", "B4.controller.workscr.WorkersCount", requiredPermission: "GkhCr.TypeWorkCr.Register.MonitoringSmr.WorkersCount.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/document", "B4.controller.workscr.Document", requiredPermission: "GkhCr.TypeWorkCr.Register.MonitoringSmr.Document.View"));
            map.AddRoute(new ClientRoute("workscredit/{id}/{objectId}/perfact", "B4.controller.workscr.PerfAct", requiredPermission: "GkhCr.TypeWorkCr.Register.PerformedWorkActViewCreate.View"));

            map.AddRoute(new ClientRoute("jurjournal/buildcontract", "B4.controller.claimwork.JurJournalBuildContract", requiredPermission: "GkhCr.ObjectCr.Register.BuildContractViewCreate"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/programcr",
                "B4.controller.realityobj.ProgramCr",
                requiredPermission: "Gkh.RealityObject.Register.ProgramCr.View"));
        }
    }
}