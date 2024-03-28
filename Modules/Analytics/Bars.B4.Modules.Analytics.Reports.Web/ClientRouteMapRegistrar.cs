namespace Bars.B4.Modules.Analytics.Reports.Web
{
    using Bars.B4;

    /// <summary>
    /// 
    /// </summary>
    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("reportcustoms", "B4.controller.al.ReportCustom"));
            map.AddRoute(new ClientRoute("storedreports", "B4.controller.al.StoredReport"));
            map.AddRoute(new ClientRoute("reportpanel", "B4.controller.al.ReportPanel"));
            map.AddRoute(new ClientRoute("kp60reports", "B4.controller.al.Kp60Reports"));
        }
    }
}
