namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип собственника
    /// </summary>
    public enum OwnerType
    {
        [Display("Юридическое лицо/Индивидуальный предприниматель")]
        JurPerson = 10,

        [Display("Физическое лицо")]
        Individual = 20
    }
}
