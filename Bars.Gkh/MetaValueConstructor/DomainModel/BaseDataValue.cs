namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.MetaValueConstructor.Enums;

    /// <summary>
    /// Объект-значение
    /// </summary>
    public class BaseDataValue : BaseEntity, IDataValue, IHasParent<BaseDataValue>
    {
        /// <summary>
        /// Тип мета-информации для конструктора
        /// </summary>
        public virtual DataMetaObjectType ObjectType { get; }

        /// <summary>
        /// Родитель
        /// </summary>
        public virtual BaseDataValue Parent { get; set; }

        /// <summary>
        /// Описатель объекта
        /// </summary>
        public virtual DataMetaInfo MetaInfo { get; set; }

        /// <summary>
        /// Значение объекта
        /// </summary>
        public virtual object Value { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name => this.MetaInfo.Name;

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code => this.MetaInfo.Code;

        /// <summary>
        /// Идентификатор
        /// </summary>
        object IHasId.Id => this.Id;
    }
}
