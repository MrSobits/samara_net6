
namespace Bars.Gkh.Modules.ClaimWork.DomainService
{
    using Bars.B4;

    public interface IClaimWorkPermission
    {
        /// <summary>
        /// Возращает настройки ограничений 
        /// </summary>
        PermissionMap GetPermissionMap();
    }
}