using Bars.B4.Modules.Security;
using Bars.Gkh.Entities;

namespace Bars.Gkh.InspectorMobile.Entities
{
    /// <summary>
    /// Роль мобильного приложения
    /// </summary>
    public class MpRole : BaseGkhEntity
    {
        /// <summary>
        /// Роль ЖКХ
        /// </summary>
        public virtual Role Role { get; set; }
    }
}