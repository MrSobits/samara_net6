namespace Bars.GkhGji
{
    using Bars.B4.Application;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Permissions;

    /// <summary>
    /// Класс GjiPermissionSource
    /// </summary>
    public class GjiPermissionSource : IPermissionSource
    {   
        /// <summary>
        /// Метод получения пермишна по приоритету
        /// </summary>
        public IEnumerable<PermissionInfo> GetPermissions()
        {
            var container = ApplicationContext.Current.Container;

            var result = new List<PermissionInfo>();
            container.ResolveAll<IGjiPermission>()
                .OrderByDescending(x => x.Priority)
                .FirstOrDefault()?
                .GetPermissionMap()
                .GetPermissions()
                .AddTo(result);

            return result;
        }
    }
}
