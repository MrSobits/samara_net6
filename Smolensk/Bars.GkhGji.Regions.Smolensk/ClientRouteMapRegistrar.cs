namespace Bars.GkhGji.Regions.Smolensk
{
    using Bars.B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("surveysubject", "B4.controller.dict.SurveySubject", requiredPermission: "GkhGji.Dict.SurveySubject.View"));
        }
    }
}