namespace Bars.Gkh.Overhaul.Hmao
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("sharefinancingceo", "B4.controller.dict.ShareFinancingCeo", requiredPermission: "Ovrhl.Dictionaries.ShareFinancingCeo.View"));
            map.AddRoute(new ClientRoute("accountoperation", "B4.controller.dict.AccountOperation", requiredPermission: "Ovrhl.Dictionaries.AccountOperation.View"));
            map.AddRoute(new ClientRoute("crperiod", "B4.controller.dict.CrPeriod", requiredPermission: "Ovrhl.Dictionaries.CrPeriod.View"));

            map.AddRoute(new ClientRoute("programfirststage", "B4.controller.program.FirstStage", requiredPermission: "Ovrhl.Program1Stage.View"));

            //map.AddRoute(new ClientRoute("programsecondstage", "B4.controller.program.SecondStage"));
            //map.AddRoute(new ClientRoute("longprogram", "B4.controller.program.ThirdStage"));

            map.AddRoute(new ClientRoute("dpkr", "B4.controller.program.ThirdStage", requiredPermission: "Ovrhl.LongTermProgram.View"));
            map.AddRoute(new ClientRoute("subdpkr", "B4.controller.program.SubStage", requiredPermission: "Ovrhl.LongTermSubProgram.View"));

            map.AddRoute(new ClientRoute("subsidy", "B4.controller.Subsidy", requiredPermission: "Ovrhl.Subcidy.View"));

            map.AddRoute(new ClientRoute("creditorg", "B4.controller.CreditOrg", requiredPermission: "Ovrhl.CreditOrg.View"));

            map.AddRoute(new ClientRoute("viewdetails/{id}", "B4.controller.program.ThirdStage", "viewdetails", requiredPermission: "Ovrhl.LongTermProgramObject.View"));
            map.AddRoute(new ClientRoute("longtermprobject", "B4.controller.LongTermPrObject", requiredPermission: "Ovrhl.LongTermProgramObject.View"));

            map.AddRoute(new ClientRoute("dpkr_versions", "B4.controller.version.ProgramVersion", requiredPermission: "Ovrhl.ProgramVersions.View"));
            map.AddRoute(new ClientRoute("show_version/{id}", "B4.controller.version.ProgramVersion", "version", requiredPermission: "Ovrhl.ProgramVersions.View"));

            map.AddRoute(new ClientRoute("nsorealityobjimport", "B4.controller.import.RealityObjectImport", requiredPermission: "Import.HmaoRealtyObjectImport.View"));

            map.AddRoute(new ClientRoute("publicationprogs", "B4.controller.program.Publication", requiredPermission: "Ovrhl.PublicationProgs.View"));
            map.AddRoute(new ClientRoute("publicationprogs/{muId}", "B4.controller.program.Publication", "index", requiredPermission: "Ovrhl.PublicationProgs.View"));

            map.AddRoute(new ClientRoute("shortprogram", "B4.controller.ShortProgram", requiredPermission: "Ovrhl.ShortProgram.View"));
            map.AddRoute(new ClientRoute("shortprogramdef", "B4.controller.ShortProgramDeficit", requiredPermission: "Ovrhl.ShortProgramDeficit.View"));

            map.AddRoute(new ClientRoute("loadprogram", "B4.controller.program.LoadProgram", requiredPermission: "Ovrhl.LoadProgram.View"));
            map.AddRoute(new ClientRoute("billing", "B4.controller.Billing", requiredPermission: "Ovrhl.Billing"));

            map.AddRoute(new ClientRoute("roimportfromfundpart3", "B4.controller.import.RoImportFromFundPart3", requiredPermission: "Import.RoImportFromFundPart3.View"));
            map.AddRoute(new ClientRoute("roimportfromfundpart5", "B4.controller.import.RoImportFromFundPart5", requiredPermission: "Import.RoImportFromFundPart5.View"));
            map.AddRoute(new ClientRoute("worksimportbystructelements", "B4.controller.import.WorksImportByStructElements", requiredPermission: "Import.WorksImportByStructElements.View"));
            map.AddRoute(new ClientRoute("dpkr1cdataimport", "B4.controller.import.Dpkr1CImport"));

            map.AddRoute(new ClientRoute("priorityparam", "B4.controller.PriorityParam", requiredPermission: "Ovrhl.PriorityParam.View"));
            map.AddRoute(new ClientRoute("actualisedpkr", "B4.controller.ActualiseDPKR", requiredPermission: "Ovrhl.ActualiseDPKR.View"));
            map.AddRoute(new ClientRoute("actualisesubprogram", "B4.controller.ActualiseSubProgram", requiredPermission: "Ovrhl.ActualiseSubProgram.View"));
            map.AddRoute(new ClientRoute("maxsumbyyear", "B4.controller.MaxSumByYear", requiredPermission: "Ovrhl.MaxSumByYear.View"));

            map.AddRoute(new ClientRoute("loanregister", "B4.controller.LoanRegister", requiredPermission: "Ovrhl.LoanRegister.View"));
            map.AddRoute(new ClientRoute("decisiomnoticeregister", "B4.controller.DecisionNoticeRegister", requiredPermission: "Ovrhl.DecisionNoticeRegister.View"));
            map.AddRoute(new ClientRoute("masscalclongprogram", "B4.controller.MassCalcLongProgram", requiredPermission: "Ovrhl.MassCalcLongProgram.View"));
            map.AddRoute(new ClientRoute("dpkrdocument", "B4.controller.DpkrDocument", requiredPermission: "Ovrhl.DpkrDocument.View"));
            map.AddRoute(new ClientRoute("realityobjectedit/{id}/ownerprotocol", "B4.controller.realityobj.OwnerProtocol", requiredPermission: "Gkh.RealityObject.Register.OwnerProtocol.View"));

            map.AddRoute(new ClientRoute("protocolmkd", "B4.controller.PropertyOwnerProtocols", requiredPermission: "Gkh.RealityObject.Register.OwnerProtocol.View"));

            map.AddRoute(new ClientRoute("costlimit", "B4.controller.CostLimit", requiredPermission: "Ovrhl.Dictionaries.CostLimit.View"));
            map.AddRoute(new ClientRoute("costlimitooi", "B4.controller.CostLimitOOI", requiredPermission: "Ovrhl.Dictionaries.CostLimitOOI.View"));

            map.AddRoute(new ClientRoute("ownerprottype", "B4.controller.dict.OwnerProtocolType", requiredPermission: "Ovrhl.Dictionaries.OwnerProtocolType.View"));
           
            map.AddRoute(new ClientRoute("econfeasibilitycalc", "B4.controller.EconFeasibilityCalc", requiredPermission: "Ovrhl.EconFeasibilityCalc.View"));

            map.AddRoute(new ClientRoute("criterias", "B4.controller.dict.CriteriaForActualizeVersion", requiredPermission: "Ovrhl.Dictionaries.CriteriaForActualizeVersion.View"));
        }
    }
}