namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Overhaul.Nso.Enum;

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