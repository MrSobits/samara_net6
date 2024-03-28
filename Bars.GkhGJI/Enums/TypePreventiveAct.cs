namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип акта проф визита ГЖИ
    /// </summary>
    public enum TypePreventiveAct
    {
        /// <summary>
        /// По обращению
        /// </summary>
        [Display("По обращению")]
        Appeal = 10,

        /// <summary>
        /// По поручению руководства
        /// </summary>
        [Display("По поручению руководства")]
        Head = 20,

        /// <summary>
        /// По поручению руководства
        /// </summary>
        [Display("По плану мероприятий")]
        Plan = 30,

        /// <summary>
        /// По поручению руководства
        /// </summary>
        [Display("По решению")]
        Decision = 40,
    }
}