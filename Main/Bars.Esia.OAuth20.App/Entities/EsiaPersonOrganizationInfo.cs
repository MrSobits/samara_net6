namespace Bars.Esia.OAuth20.App.Entities
{
    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Extensions;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Информация об организации пользователя в ЕСИА
    /// </summary>
    public class EsiaPersonOrganizationInfo : BaseEsiaOrganizationInfo
    {
        /// <summary>
        /// Является ли шефом
        /// </summary>
        public bool IsChief { get; set; }

        /// <summary>
        /// Является ли администратором
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Является ли активным (признак НЕ блокировки)
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Обладает правом замены (?)
        /// </summary>
        public bool HasRightOfSubstitution { get; set; }

        /// <summary>
        /// Обладает доступом к вкладке подтверждения (?)
        /// </summary>
        public bool HasApprovalTabAccess { get; set; }

        /// <summary>
        /// Является ли ликвидированным
        /// </summary>
        public bool IsLiquidated { get; set; }

        public EsiaPersonOrganizationInfo()
        {
        }

        public EsiaPersonOrganizationInfo(JObject organizationInfo) : base(organizationInfo)
        {
            if (organizationInfo == null)
                return;

            this.IsChief = organizationInfo.GetPropertyValue("chief").ToBool();
            this.IsAdmin = organizationInfo.GetPropertyValue("admin").ToBool();
            this.IsActive = organizationInfo.GetPropertyValue("active").ToBool();
            this.HasRightOfSubstitution = organizationInfo.GetPropertyValue("hasRightOfSubstitution").ToBool();
            this.HasApprovalTabAccess = organizationInfo.GetPropertyValue("hasApprovalTabAccess").ToBool();
            this.IsLiquidated = organizationInfo.GetPropertyValue("isLiquidated").ToBool();
        }
    }
}