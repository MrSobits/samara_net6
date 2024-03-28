namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания проверки по требованию прокуратуры
    /// </summary>
    public enum TypeBaseProsClaim
    {
        [Display("Предоставление специалиста")]
        ProvidingSpecialist = 10,

        [Display("Проведение проверки")]
        ExecutionInspection = 20
    }
}