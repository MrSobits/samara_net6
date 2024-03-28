namespace Bars.GkhRf.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип программы заявки перечисления рег.фонда
    /// </summary>
    public enum TypeProgramRequest
    {
        /// <summary>
        /// Основная
        /// </summary>
        [Display("Основная")]
        Primary = 10,

        /// <summary>
        /// Дополнительная
        /// </summary>
        [Display("Дополнительная")]
        Additional = 20,

        /// <summary>
        /// Другая
        /// </summary>
        [Display("Другая")]
        Another = 30,

        /// <summary>
        /// ЛП прямые договора 2010
        /// </summary>
        [Display("ЛП прямые договора 2010")]
        LpDirectContracts2010 = 40,

        /// <summary>
        /// ЛП по схеме ТБН 2010
        /// </summary>
        [Display("ЛП по схеме ТБН 2010")]
        LpSchemeTbn2010 = 50,

        /// <summary>
        /// ЛП прочие
        /// </summary>
        [Display("ЛП прочие")]
        LpOther = 60,

        /// <summary>
        /// ЛП прямые договора 2011
        /// </summary>
        [Display("ЛП прямые договора 2011")]
        LpDirectContracts2011 = 70,

        /// <summary>
        /// ЛП по схеме ТБН 2011
        /// </summary>
        [Display("ЛП по схеме ТБН 2011")]
        LpSchemeTbn2011 = 80
    }
}