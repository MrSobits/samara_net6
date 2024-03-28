namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Использование износа по основным КЭ - используется в настроках параметров ДПКР
    /// </summary>
    public enum TypeUseWearMainCeo
    {
        [Display("Не используется")]
        NotUsed = 0,

        [Display("Все ООИ")]
        AllCeo = 10,

        [Display("1 и более")]
        OnlyOne = 20
    }
}