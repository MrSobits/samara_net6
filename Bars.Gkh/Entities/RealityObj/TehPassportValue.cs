namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Значение ТехПаспорта
    /// </summary>
    public class TehPassportValue : BaseGkhEntity
    {
        /// <summary>
        /// ТехПаспорт
        /// </summary>
        public virtual TehPassport TehPassport { get; set; }

        /// <summary>
        /// Код Формы
        /// </summary>
        public virtual string FormCode { get; set; }

        /// <summary>
        /// Код Ячейки
        /// </summary>
        public virtual string CellCode { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}
