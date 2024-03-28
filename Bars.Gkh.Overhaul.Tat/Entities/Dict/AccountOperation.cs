namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Операция по счету
    /// </summary>
    public class AccountOperation : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual int Code { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        public virtual AccountOperationType Type { get; set; }
    }
}