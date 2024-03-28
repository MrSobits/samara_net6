namespace Bars.Gkh.Gis.Utils
{
    using Bars.B4;

    public static class MenuExtensions
    {
        public static MenuItem AddRequiredPermissionNullable(this MenuItem mi, string permission)
        {
            return mi != null ? mi.AddRequiredPermission(permission) : null;            
        }

        public static MenuItem GetNullable(this MenuItem mi, string selector)
        {
            return mi != null ? mi.Get(selector) : null;
        }

        public static MenuItem AddNullable(this MenuItem mi, string caption, string @ref)
        {
            return mi != null ? mi.Add(caption, @ref) : null;
        }
    }
}
