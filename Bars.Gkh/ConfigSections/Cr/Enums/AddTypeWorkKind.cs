namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Добавление работ из ДПКР
    /// </summary>
    public enum AddTypeWorkKind
    {
        [Display("Не указывать основание")]
        NotSpecifyBase = 0,

        [Display("Указывать основание")]
        SpecifyBase = 10
    }
}