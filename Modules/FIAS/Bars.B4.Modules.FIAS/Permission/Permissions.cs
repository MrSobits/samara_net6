namespace Bars.B4.Modules.FIAS
{
    public class Permissions : PermissionMap
    {
        public Permissions()
        {
            Namespace("B4.FIAS", "Модуль ФИАС");
            CRUDandViewPermissions("B4.FIAS");
        }
    }
}