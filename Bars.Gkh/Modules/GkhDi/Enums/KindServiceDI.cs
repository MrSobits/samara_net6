namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Вид услуги
    /// </summary>
    public enum KindServiceDi
    {
        [Display("Коммунальная")]
        Communal = 10,

        [Display("Жилищная")]
        Housing = 20,

        [Display("Ремонт")]
        Repair = 30,

        [Display("Кап. ремонт")]
        CapitalRepair = 40,

        [Display("Управление МКД")]
        Managing = 50,

        [Display("Дополнительная")]
        Additional = 60
    }
}
