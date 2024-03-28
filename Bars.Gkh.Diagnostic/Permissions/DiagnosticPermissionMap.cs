namespace Bars.Gkh.Diagnostic.Permissions
{
    using B4;

    public class DiagnosticPermissionMap : PermissionMap
    {
        public DiagnosticPermissionMap()
        {
            Namespace("Administration.Diagnostic", "Диагностика");
            Permission("Administration.Diagnostic.View", "Просмотр");
        }
    }
}
