namespace Sobits.GisGkh
{

    public class PermissionMap : Bars.B4.PermissionMap
    {
        public PermissionMap()
        {
            this.Namespace("Administration.OutsideSystemIntegrations", "Интеграция с внешними системами");

            this.Namespace("Administration.OutsideSystemIntegrations.GisGkh", "Интеграция с ГИС ЖКХ");
            this.Permission("Administration.OutsideSystemIntegrations.GisGkh.View", "Просмотр");

            this.Namespace("GkhGji.GisGkh", "Интеграция с ГИС ЖКХ");
            this.Permission("GkhGji.GisGkh.View", "Просмотр");
        }
    }
}