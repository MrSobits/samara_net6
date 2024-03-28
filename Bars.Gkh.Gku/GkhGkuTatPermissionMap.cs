namespace Bars.Gkh.Gku
{
    using Bars.B4;

    public class GkhGkuTatPermissionMap : PermissionMap
    {
        public GkhGkuTatPermissionMap()
        {
            Namespace("Gkh.GkuInfo", "Сведения о ЖКУ");
            Permission("Gkh.GkuInfo.View", "Просмотр");

            Namespace("Gkh.GkuInfo.MeteringDeviceValue", "Показания общедомовых приборов учета");
            Permission("Gkh.GkuInfo.MeteringDeviceValue.View", "Просмотр");

            Namespace("Gkh.GkuInfo.CitizenClaim", "Претензии граждан");
            Permission("Gkh.GkuInfo.CitizenClaim.View", "Просмотр");

            #region Отчеты

            Permission("Reports.GKH.OperationalDataOfPayments", "Отчет по сведениям о ЖКУ");

            #endregion
        }
    }
}