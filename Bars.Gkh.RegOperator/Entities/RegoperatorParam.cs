namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    public class RegoperatorParam : BaseImportableEntity
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }
    }
}