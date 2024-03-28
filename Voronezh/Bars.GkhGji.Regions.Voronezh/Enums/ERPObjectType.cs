namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Простой тип - коды типов объектов проведения проверки
    /// </summary>
    public enum ERPObjectType
    {

        /// <summary>
        /// Филиал
        /// </summary>
        [Display("Филиал")]
        TYPE_I = 10,

        /// <summary>
        /// Представительство
        /// </summary>
        [Display("Представительство")]
        TYPE_II = 20,

        /// <summary>
        /// Обособленное структурное подразделение
        /// </summary>
        [Display("Обособленное структурное подразделение")]
        TYPE_III = 30,

        /// <summary>
        ///Иное
        /// </summary>
        [Display("Иное")]
        TYPE_OTHER = 40

    }
}