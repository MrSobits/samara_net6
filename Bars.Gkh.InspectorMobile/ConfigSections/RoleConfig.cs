using Bars.Gkh.Config;
using Bars.Gkh.Config.Attributes;
using Bars.Gkh.InspectorMobile.Entities;
using System.Collections.Generic;

namespace Bars.Gkh.InspectorMobile.ConfigSections
{
    public class RoleConfig : IGkhConfigSection
    {
        /// <summary>
        /// Роль пользователя в ГИС МЖФ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Роль пользователя в ГИС МЖФ")]
        [GkhConfigPropertyEditor("B4.ux.config.MpRole", "mproleeditor")]
        public virtual List<MpRole> Roles { get; set; }
    }
}
