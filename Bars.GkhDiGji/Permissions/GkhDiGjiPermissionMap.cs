namespace Bars.GkhDiGji.Permissions
{
    using B4;

    public class GkhDiGjiPermissionMap : PermissionMap
    {
        public GkhDiGjiPermissionMap()
        {
            Permission("GkhDi.Disinfo.AdminResp.AddFromGji", "Загрузить данные из блока ГЖИ");
        }
    }
}