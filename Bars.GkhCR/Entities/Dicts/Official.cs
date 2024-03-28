namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;

    /// <summary>
    /// Должностное лицо
    /// </summary>
    public class Official : BaseImportableEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Фио
        /// </summary>
        public virtual string Fio { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}