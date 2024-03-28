namespace Bars.Gkh.ClaimWork
{
    using Bars.B4;

    public class ClientRouteMapRegistrar: IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("violclaimwork", "B4.controller.ViolClaimWork", requiredPermission: "Clw.Dictionaries.ViolClaimWork.View"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/deletedocument", "B4.controller.claimwork.Navi", "deletedocument"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{nextroute}/updatenavimenu", "B4.controller.claimwork.Navi", "updateNaviAspectMenu"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/notification", "B4.controller.claimwork.Notification"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/pretension", "B4.controller.claimwork.Pretension"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/lawsuit", "B4.controller.claimwork.Lawsuit"));
            map.AddRoute(new ClientRoute("claimwork/{type}/{id}/{docId}/{docType}", "B4.controller.claimwork.RestructDebt"));

            map.AddRoute(new ClientRoute("jurinstitution", "B4.controller.JurInstitution", requiredPermission: "Clw.Dictionaries.JurInstitution.View"));
            map.AddRoute(new ClientRoute("petitiontocourt", "B4.controller.dict.PetitionToCourt", requiredPermission: "Clw.Dictionaries.PetitionToCourt.View"));
            map.AddRoute(new ClientRoute("jurjournal", "B4.controller.JurJournal", requiredPermission: "Clw.JurJournal.View"));
            map.AddRoute(new ClientRoute("documentregister", "B4.controller.DocumentRegister", requiredPermission: "Clw.DocumentRegister.View"));
            map.AddRoute(new ClientRoute("documentregister/actviolidentification", "B4.controller.documentregister.ActViolIdentification", requiredPermission: "Clw.DocumentRegister.View"));
            map.AddRoute(new ClientRoute("documentregister/notification", "B4.controller.documentregister.Notification", requiredPermission: "Clw.DocumentRegister.View"));
            map.AddRoute(new ClientRoute("documentregister/pretension", "B4.controller.documentregister.Pretension", requiredPermission: "Clw.DocumentRegister.View"));
            map.AddRoute(new ClientRoute("documentregister/lawsuit", "B4.controller.documentregister.Lawsuit", requiredPermission: "Clw.DocumentRegister.View"));
            map.AddRoute(new ClientRoute("stateduty", "B4.controller.dict.StateDuty", requiredPermission: "Clw.Dictionaries.StateDuty.View"));
            map.AddRoute(new ClientRoute("stateduty_edit/{id}", "B4.controller.dict.stateduty.Edit", requiredPermission: "Clw.Dictionaries.StateDuty.View"));
            map.AddRoute(new ClientRoute("flattenedclaimwork", "B4.controller.claimwork.FlattenedClaimWork", requiredPermission: "Clw.FlattenedClaimWork.View"));
            map.AddRoute(new ClientRoute("partialclaimwork", "B4.controller.claimwork.PartialClaimWork"));
        }
    }
}