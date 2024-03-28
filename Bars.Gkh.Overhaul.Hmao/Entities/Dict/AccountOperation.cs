namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    using Enum;

    /// <summary>
    /// Операция по счету
    /// </summary>
    public class AccountOperation : BaseImportableEntity
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