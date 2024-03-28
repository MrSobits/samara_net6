namespace Bars.Gkh.Entities.RealityObj
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Таблица-сопоставлятор для значений техпаспорта
    /// </summary>
    public class TechnicalPassportCompareCode : BaseEntity
    {
        /// <summary>
        /// Форма
        /// </summary>
        public virtual string FormCode { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string CellCode { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Код в МЖФ
        /// </summary>
        public virtual string CodeMjf { get; set; }

        /// <summary>
        /// Код в РИС ЖКХ
        /// </summary>
        public virtual string CodeRis{ get; set; }

        /// <summary>
        /// Код в Реформе
        /// </summary>
        public virtual string CodeReforma { get; set; }
    }
}