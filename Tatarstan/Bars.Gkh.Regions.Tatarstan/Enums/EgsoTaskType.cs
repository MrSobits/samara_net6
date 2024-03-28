namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип задачи по интеграции с ЕГСО ОВ
    /// </summary>
    public enum EgsoTaskType
    {
        /// <summary>
        /// Отправка сведений о количествах МКД
        /// </summary>
        [Display("Отправка сведений о количествах МКД")]
        ManyApartmentsCount = 10,
        
        /// <summary>
        /// Отправка сведений о количествах МКД, прошедших капитальный ремонт
        /// </summary>
        [Display("Отправка сведений о количествах МКД, прошедших капитальный ремонт")]
        OverhauledManyApartmentsCount = 20
    }
}