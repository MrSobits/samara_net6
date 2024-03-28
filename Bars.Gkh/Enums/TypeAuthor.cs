namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип автора
    /// </summary>
    public enum TypeAuthor
    {
        [Display("Житель")]
        Inhabitant = 10,

        [Display("Заказчик")]
        Customer = 20,

        [Display("Другие")]
        Other = 30
    }
}
