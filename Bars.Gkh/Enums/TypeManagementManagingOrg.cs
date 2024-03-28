namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип управления управляющей организацией 
    /// </summary>
    public enum TypeManagementManOrg
    {
        [Display("УК")]
        UK = 10,

        [Display("ТСЖ")]
        TSJ = 20,

        [Display("ЖСК")]
        JSK = 40,

        [Display("Прочее")]
        Other = 100
    }
}
