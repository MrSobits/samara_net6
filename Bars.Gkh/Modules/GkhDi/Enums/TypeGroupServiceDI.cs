namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип группы услуги
    /// </summary>
    public enum TypeGroupServiceDi
    {
        [Display("Коммунальная")]
        Communal = 10,

        [Display("Жилищная")]
        Housing = 20,

        [Display("Прочая")]
        Other = 30
    }
}
