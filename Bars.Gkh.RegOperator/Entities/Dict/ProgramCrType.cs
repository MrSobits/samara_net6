namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Типы программы КР
    /// </summary>
    public class ProgramCrType : BaseImportableEntity
    {
        /// <summary>
        /// Наименование 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}
