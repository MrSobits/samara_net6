namespace Bars.Gkh.AlphaBI
{
    using B4;

    public class AlphaPermissionMap : PermissionMap
    {
        public AlphaPermissionMap()
        {
            Namespace("OLAP", "Модуль OLAP аналитики");
            Permission("OLAP.Alpha", "OLAP аналитика");
        }
    }
}