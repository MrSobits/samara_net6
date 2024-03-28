namespace Bars.GkhGji.Regions.Tomsk
{
    using Bars.B4;

    class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("frameverificationGrid", "B4.controller.dict.FrameVerification", requiredPermission: "GkhGji.Dict.FrameVerification.View"));
            map.AddRoute(new ClientRoute("admincase", "B4.controller.AdminCase", requiredPermission: "GkhGji.DocumentsGji.AdminCase.View"));
            map.AddRoute(new ClientRoute("socialstatus", "B4.controller.dict.SocialStatus", requiredPermission: "GkhGji.Dict.SocialStatus.View"));
            map.AddRoute(new ClientRoute("reminderappealcits", "B4.controller.ReminderAppealCits", requiredPermission: "GkhGji.ManagementTask.ReminderAppealCits.View"));
			map.AddRoute(new ClientRoute("surveysubject", "B4.controller.dict.SurveySubject", requiredPermission: "GkhGji.Dict.SurveySubject.View"));
			map.AddRoute(new ClientRoute("surveysubjectlicensing", "B4.controller.dict.SurveySubjectLicensing", requiredPermission: "GkhGji.Dict.SurveySubject.View"));

        }
    }
}
