namespace Bars.Gkh.RegOperator.Regions.Tyumen.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид уполномоченного лица
    /// </summary>
    public enum RequestStatePersonEnum
    {
        [Display("Предоставление доступа для редактирования")]
        Edit = 10,

        [Display("Смена роли оператору ЕСИА")]
        Esia = 20
    }
}
