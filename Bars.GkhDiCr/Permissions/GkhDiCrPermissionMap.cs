namespace Bars.GkhDiCr.Permissions
{
    using Bars.B4;

    public class GkhDiCrPermissionMap : PermissionMap
    {
        public GkhDiCrPermissionMap()
        {
            this.Permission("GkhDi.DisinfoRealObj.Actions.CopyCrServiceGroupAction", "Загрузка сведений по капитальному ремонту из модуля КР");
        }
    }
}