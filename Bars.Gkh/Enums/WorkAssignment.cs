namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Назначение работ
    /// </summary>
    public enum WorkAssignment
    {
        /// <summary>
        /// Обслуживание
        /// </summary>
        [Display("Обслуживание")]
        Service = 10,

        /// <summary>
        /// Текущий ремонт
        /// </summary>
        [Display("Текущий ремонт")]
        CurrentRepair = 20,

        /// <summary>
        /// Аварийные работы
        /// </summary>
        [Display("Аварийные работы")]
        AbnormalOperations = 30,

        /// <summary>
        /// По обращениям граждан
        /// </summary>
        [Display("По обращениям граждан")]
        CitizensAddressesOperations = 40,

        /// <summary>
        /// По ограничениям поставки
        /// </summary>
        [Display("По ограничениям поставки")]
        DeliveryRestrictionsOperations = 50,

        /// <summary>
        /// Подготовка ОКИ к отопительному сезону
        /// </summary>
        [Display("Подготовка ОКИ к отопительному сезону")]
        OkiPreparationHeatingSeason = 60,

        /// <summary>
        /// Иной вид работы
        /// </summary>
        [Display("Иной вид работы")]
        OtherOperationType = 70,

        /// <summary>
        /// Все
        /// </summary>
        [Display("Все")]
        All = 80
    }
}
