namespace Bars.Gkh.Overhaul.Tat
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("accountoperation", "B4.controller.dict.AccountOperation", requiredPermission: "Ovrhl.Dictionaries.AccountOperation.View"));

            map.AddRoute(new ClientRoute("programfirststage", "B4.controller.program.FirstStage", requiredPermission: "Ovrhl.Program1Stage.View"));

            map.AddRoute(new ClientRoute("correctionresult", "B4.controller.program.CorrectionResult", requiredPermission: "Ovrhl.ProgramCorrection.View"));
            map.AddRoute(new ClientRoute("correctionresult/{id}", "B4.controller.program.CorrectionResult", "index", requiredPermission: "Ovrhl.ProgramCorrection.View"));

            map.AddRoute(new ClientRoute("dpkr", "B4.controller.program.ThirdStage", requiredPermission: "Ovrhl.LongTermProgram.View"));
            map.AddRoute(new ClientRoute("dpkr/{id}", "B4.controller.program.ThirdStage", "index", requiredPermission: "Ovrhl.LongTermProgram.View"));
            map.AddRoute(new ClientRoute("subsidy", "B4.controller.Subsidy", requiredPermission: "Ovrhl.Subcidy.View"));

            map.AddRoute(new ClientRoute("creditorg", "B4.controller.CreditOrg", requiredPermission: "Ovrhl.CreditOrg.View"));

            map.AddRoute(new ClientRoute("viewdetails/{id}", "B4.controller.program.ThirdStage", "viewdetails", requiredPermission: "Ovrhl.LongProgram.View"));
            map.AddRoute(new ClientRoute("longtermprobject", "B4.controller.LongTermPrObject", requiredPermission: "Ovrhl.LongProgram.View"));
            map.AddRoute(new ClientRoute("regoperator", "B4.controller.RegOperator", requiredPermission: "Ovrhl.RegOperator"));

            map.AddRoute(new ClientRoute("dpkr_versions", "B4.controller.version.ProgramVersion", requiredPermission: "Ovrhl.LongProgram.ProgramVersion.View"));
            map.AddRoute(new ClientRoute("show_version/{id}", "B4.controller.version.ProgramVersion", "version", requiredPermission: "Ovrhl.LongProgram.ProgramVersion.View"));

            map.AddRoute(new ClientRoute("nsorealityobjimport", "B4.controller.import.RealityObjectImport", requiredPermission: "Import.TatRealtyObjectImport.View"));

            map.AddRoute(new ClientRoute("publicationprogs", "B4.controller.program.Publication", requiredPermission: "Ovrhl.PublicationProgs.View"));

            map.AddRoute(new ClientRoute("shortprogram", "B4.controller.ShortProgram", requiredPermission: "Ovrhl.ShortProgram.View"));
            map.AddRoute(new ClientRoute("shortprogram/{id}", "B4.controller.ShortProgram", "index", requiredPermission: "Ovrhl.ShortProgram.View"));

            map.AddRoute(new ClientRoute("priorityparam", "B4.controller.PriorityParam", requiredPermission: "Ovrhl.PriorityParam.View"));

            map.AddRoute(new ClientRoute("decisiomnoticeregister", "B4.controller.DecisionNoticeRegister", requiredPermission: "Ovrhl.DecisionNoticeRegister.View"));

            map.AddRoute(new ClientRoute("fundformationcontract", "B4.controller.FundFormationContract", requiredPermission: "Ovrhl.FundFormationContract.View"));

            map.AddRoute(new ClientRoute("massdeleterose", "B4.controller.administration.MassDeleteRoSe", requiredPermission: "Administration.MassDeleteRoSe.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/ownerprotocol", "B4.controller.realityobj.OwnerProtocol", requiredPermission: "Gkh.RealityObject.Register.OwnerProtocol.View"));

            map.AddRoute(new ClientRoute("dpkrdocument", "B4.controller.dpkrdocument.DpkrDocument", requiredPermission: "Ovrhl.DpkrDocument.View"));
        }
    }
}