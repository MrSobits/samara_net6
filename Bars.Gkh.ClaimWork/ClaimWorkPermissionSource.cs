namespace Bars.Gkh.ClaimWork
{
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Castle.Core.Internal;

    public class ClaimWorkPermissionSource : IPermissionSource
    {
        public IEnumerable<PermissionInfo> GetPermissions()
        {
            var container = ApplicationContext.Current.Container;

            var result = new List<PermissionInfo>();
            container.ResolveAll<IClaimWorkPermission>().ForEach(x => result.AddRange(x.GetPermissionMap().GetPermissions()));

            return result;
        }
    }
}
