namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Группировка по организационно-правовой форме
    /// </summary>
    public enum OrgFormGroup
    {
        /// <summary>
        /// Не используется
        /// </summary>
        [Display("Не используется")]
        NoGroup = 0,

        /// <summary>
        /// Группировка выбранных форм в отдельной папке
        /// </summary>
        [Display("Группировка выбранных форм в отдельной папке")]
        FolderGroup = 1,

        /// <summary>
        /// Группировка выбранных форм в отдельной папке и разделение по формам
        /// </summary>
        [Display("Группировка выбранных форм в отдельной папке и разделение по формам")]
        FolderAndFormsGroup = 2
    }
}