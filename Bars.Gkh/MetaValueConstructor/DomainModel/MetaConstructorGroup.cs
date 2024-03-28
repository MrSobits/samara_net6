namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Группа конструктора
    /// </summary>
    public class MetaConstructorGroup : BaseEntity
    {
        /// <summary>
        /// Тип конструктора
        /// </summary>
        public virtual DataMetaObjectType ConstructorType { get; set; }
    }
}
