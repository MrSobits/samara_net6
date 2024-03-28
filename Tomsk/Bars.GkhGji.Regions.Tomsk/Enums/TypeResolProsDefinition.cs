namespace Bars.GkhGji.Regions.Tomsk.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип определения
    /// </summary>
    public enum TypeResolProsDefinition
    {
        [Display("Определение о возврате в прокуратуру")]
        ProsecutorReturn = 10,

        [Display("Определение о назначении даты и времени")]
        AppointmentDateTime = 20,

        [Display("Определение об отложении")]
        Deposition = 30
    }
}