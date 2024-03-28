namespace Bars.Gkh.Overhaul.Nso
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("accountoperation", "B4.controller.dict.AccountOperation", requiredPermission: "Ovrhl.Dictionaries.AccountOperation.View"));

            map.AddRoute(new ClientRoute("programfirststage", "B4.controller.program.FirstStage", requiredPermission: "Ovrhl.Program1Stage.View"));

            map.AddRoute(new ClientRoute("dpkr", "B4.controller.program.ThirdStage", requiredPermission: "Ovrhl.LongTermProgram.View"));
            map.AddRoute(new ClientRoute("subsidy", "B4.controller.Subsidy", requiredPermission: "Ovrhl.Subcidy.View"));

            map.AddRoute(new ClientRoute("creditorg", "B4.controller.CreditOrg", requiredPermission: "Ovrhl.CreditOrg.View"));

            map.AddRoute(new ClientRoute("viewdetails/{id}", "B4.controller.program.ThirdStage", "viewdetails", requiredPermission: "Ovrhl.LongTermProgram.View"));
            map.AddRoute(new ClientRoute("longtermprobject", "B4.controller.LongTermPrObject", requiredPermission: "Ovrhl.LongTermProgramObject.View"));
            
            map.AddRoute(new ClientRoute("dpkr_versions", "B4.controller.version.ProgramVersion", requiredPermission: "Ovrhl.ProgramVersions.View"));
            map.AddRoute(new ClientRoute("show_version/{id}", "B4.controller.version.ProgramVersion", "version", requiredPermission: "Ovrhl.ProgramVersions.View"));

            map.AddRoute(new ClientRoute("nsorealityobjimport", "B4.controller.import.RealityObjectImport", requiredPermission: "Import.NsoRealtyObjectImport.View"));
            map.AddRoute(new ClientRoute("nsokpkrimport", "B4.controller.import.KpkrImport", requiredPermission: "Import.KpkrForNsk.View"));

            map.AddRoute(new ClientRoute("publicationprogs", "B4.controller.program.Publication", requiredPermission: "Ovrhl.PublicationProgs.View"));

            map.AddRoute(new ClientRoute("shortprogram", "B4.controller.ShortProgram", requiredPermission: "Ovrhl.ShortProgram.View"));
            map.AddRoute(new ClientRoute("shortprogramdef", "B4.controller.ShortProgramDeficit", requiredPermission: "Ovrhl.ShortProgramDeficit.View"));

            map.AddRoute(new ClientRoute("loadprogram", "B4.controller.program.LoadProgram", requiredPermission: "Ovrhl.LoadProgram.View"));
            map.AddRoute(new ClientRoute("billing", "B4.controller.Billing"));

            map.AddRoute(new ClientRoute("roimportfromfundpart3", "B4.controller.import.RoImportFromFundPart3", requiredPermission: "Import.RoImportFromFundPart3.View"));
            map.AddRoute(new ClientRoute("roimportfromfundpart5", "B4.controller.import.RoImportFromFundPart5", requiredPermission: "Import.RoImportFromFundPart5.View"));
            map.AddRoute(new ClientRoute("worksimportbystructelements", "B4.controller.import.WorksImportByStructElements", requiredPermission: "Import.WorksImportByStructElements.View"));

            map.AddRoute(new ClientRoute("priorityparam", "B4.controller.PriorityParam", requiredPermission: "Ovrhl.PriorityParam.View"));

            map.AddRoute(new ClientRoute("paymentsizecr", "B4.controller.dict.PaymentSizeCr", requiredPermission: "Ovrhl.Dictionaries.PaymentSizeCr.View"));

            map.AddRoute(new ClientRoute("suspenseaccount", "B4.controller.SuspenseAccount", requiredPermission: "Ovrhl.SuspenseAccount.View"));

            map.AddRoute(new ClientRoute("realityobjectedit/{id}/ownerprotocol", "B4.controller.realityobj.OwnerProtocol", requiredPermission: "Gkh.RealityObject.Register.OwnerProtocol.View"));
        }
    }
}