namespace Bars.GkhGji.Regions.Khakasia
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("legalreason", "B4.controller.dict.LegalReason", requiredPermission: "GkhGji.Dict.LegalReason.View"));
            map.AddRoute(new ClientRoute("surveysubject", "B4.controller.dict.SurveySubject", requiredPermission: "GkhGji.Dict.SurveySubject.View"));
        }
    }
}