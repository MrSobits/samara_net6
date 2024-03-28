namespace Bars.GkhGji.Regions.Stavropol
{
    public class PermissionMap : B4.PermissionMap
    {
        public PermissionMap()
        {
            Namespace("GkhGji.DocumentsGji.ActCheck.Register.Definition.Fields", "Поля");
            Permission("GkhGji.DocumentsGji.ActCheck.Register.Definition.Fields.DocumentNum_View", "Номер определения");
            Permission("GkhGji.DocumentsGji.ActCheck.Register.Definition.Fields.DocumentNumber_View", "Номер определения (автом.нумерация)");

            Namespace("GkhGji.DocumentsGji.Protocol.Register.Definition.Fields", "Поля");
            Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.Fields.DocumentNum_View", "Номер определения");
            Permission("GkhGji.DocumentsGji.Protocol.Register.Definition.Fields.DocumentNumber_View", "Номер определения (автом.нумерация)");

            Namespace("GkhGji.DocumentsGji.Resolution.Register.Definition.Fields", "Поля");
            Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.Fields.DocumentNum_View", "Номер определения");
            Permission("GkhGji.DocumentsGji.Resolution.Register.Definition.Fields.DocumentNumber_View", "Номер определения (автом.нумерация)");
        }
    }
}