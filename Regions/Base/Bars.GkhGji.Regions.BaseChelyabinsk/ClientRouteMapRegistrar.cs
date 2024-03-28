namespace Bars.GkhGji.Regions.BaseChelyabinsk
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("reminderappealcits", "B4.controller.ReminderAppealCits", requiredPermission: "GkhGji.ManagementTask.ReminderAppealCits.View"));
            map.AddRoute(new ClientRoute("mkdchangenotification", "B4.controller.MkdChangeNotification", requiredPermission: "GkhGji.MkdChangeNotification.View"));
            map.AddRoute(new ClientRoute("protocol197", "B4.controller.protocol197.Protocol197", requiredPermission: "GkhGji.DocumentsGji.Protocol197.View"));
            //СЭД
            map.AddRoute(new ClientRoute("eds", "B4.controller.EDSInspection", requiredPermission: "GkhGji.EDS.EDSRegistry.View"));
            map.AddRoute(new ClientRoute("requestregistry", "B4.controller.RequestRegistry", requiredPermission: "GkhGji.EDS.EDSRegistry.View"));
            map.AddRoute(new ClientRoute("edssign", "B4.controller.eds.EDSDocumentRegistry", requiredPermission: "GkhGji.EDS.EDSRegistrySign.View"));

            map.AddRoute(new ClientRoute("licenseaction", "B4.controller.LicenseAction", requiredPermission: "GkhGji.License.LicenseAction.View"));
            map.AddRoute(new ClientRoute("mvdpassport", "B4.controller.MVDPassport", requiredPermission: "GkhGji.SMEV.MVDPassport.View"));
            map.AddRoute(new ClientRoute("mvdlivingplacereg", "B4.controller.MVDLivingPlaceRegistration", requiredPermission: "GkhGji.SMEV.MVDPassport.View"));
            map.AddRoute(new ClientRoute("mvdstayingplacereg", "B4.controller.MVDStayingPlaceRegistration", requiredPermission: "GkhGji.SMEV.MVDPassport.View"));
            map.AddRoute(new ClientRoute("taskcalendar", "B4.controller.TaskCalendar", requiredPermission: "GkhGji.TaskCalendar.TaskCalendarPanel.View"));
            map.AddRoute(new ClientRoute("complaintsrequest", "B4.controller.SMEVComplaintsRequest", requiredPermission: "GkhGji.SMEV.SMEVComplaints.View"));
            map.AddRoute(new ClientRoute("complaints", "B4.controller.SMEVComplaints", requiredPermission: "GkhGji.SMEV.SMEVComplaints.View"));
            map.AddRoute(new ClientRoute("complaintsdecdict", "B4.controller.dict.SMEVComplaintsDecision", requiredPermission: "GkhGji.SMEV.SMEVComplaints.View"));
            map.AddRoute(new ClientRoute("smeverul", "B4.controller.SMEVERULReqNumber", requiredPermission: "GkhGji.License.LicenseAction.View"));
            map.AddRoute(new ClientRoute("certinfo", "B4.controller.SMEVCertInfo", requiredPermission: "GkhGji.SMEV.CertInfo.View"));

            map.AddRoute(new ClientRoute("answerregistration", "B4.controller.AppealCitsAnswerRegistration", requiredPermission: "GkhGji.AppealCitizens.AppealCitsAnswerRegistration.View"));
        }
    }
}