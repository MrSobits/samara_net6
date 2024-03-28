namespace Bars.Gkh.ConfigSections.RegOperator.Administration
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Раздел Региональный фонд - Настройки сложности пароля
    /// </summary>
    [Permissionable]
    public class UserPasswordConfig : IGkhConfigSection
    {
        /// <summary>
        /// Отправлять на электронную почту
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить проверку сложности пароля")]
        [DefaultValue(YesNo.Yes)]
        public virtual YesNo PasswordDifficultySwitcher { get; set; }

        /// <summary>
        /// Минимальная длинна
        /// </summary>
        [GkhConfigProperty(DisplayName = "Минимальная длинна")]
        [DefaultValue(4)]
        public virtual int MinimalLength { get; set; }

        /// <summary>
        ///     Рассмотрение дел в суде
        /// </summary>
        [GkhConfigProperty(DisplayName = "Допустимые символы")]
        // [InlineGridEditor]
        public virtual List<PasswordMasksConfig> PasswordMasks { get; set; }
    }
}
