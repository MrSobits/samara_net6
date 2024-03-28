namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.Gkh.MetaValueConstructor.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Описание объекта дерева
    /// </summary>
    public class DataMetaInfo : BaseEntity, IDataMetaInfo, IHasParent<DataMetaInfo>
    {
        private readonly IList<DataMetaInfo> children;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="type">Тип описателя</param>
        public DataMetaInfo(MetaConstructorGroup group) : this()
        {
            this.Group = group;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public DataMetaInfo()
        {
            this.children = new List<DataMetaInfo>();
        }

        /// <summary>
        /// Объект родитель
        /// </summary>
        public virtual DataMetaInfo Parent { get; set; }

        /// <summary>
        /// Группа конструктора
        /// </summary>
        public virtual MetaConstructorGroup Group { get; set; }

        /// <summary>
        /// Объекты потомки
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<DataMetaInfo> Children => this.children;

        /// <summary>
        /// Наименование объекта
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код объекта
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Вес объекта
        /// </summary>
        public virtual decimal? Weight { get; set; }

        /// <summary>
        /// Формула расчета
        /// </summary>
        public virtual string Formula { get; set; }

        /// <summary>
        /// Уровень объекта
        /// </summary>
        public virtual int Level { get; set; }

        /// <summary>
        /// Тип значения
        /// </summary>
        public virtual DataValueType DataValueType { get; set; }

        /// <summary>
        /// Минимальная длина (для строки)
        /// </summary>
        public virtual int? MinLength { get; set; }

        /// <summary>
        /// Максимальная длина (для строки)
        /// </summary>
        public virtual int? MaxLength { get; set; }

        /// <summary>
        /// Знаков после запятой
        /// </summary>
        public virtual int? Decimals { get; set; }

        /// <summary>
        /// Обязательный, если да, то учитывается в расчёте
        /// </summary>
        public virtual bool Required { get; set; }

        /// <summary>
        /// Источник данных
        /// <para>Если указан, значит данные будут не заполняться, а тянуться из системы</para>
        /// </summary>
        public virtual string DataFillerName { get; set; }

        /// <summary>
        /// Добавить объект потомок
        /// </summary>
        /// <param name="value">Значение</param>
        public virtual void AddChildren(DataMetaInfo value)
        {
            value.Parent = this;
            value.Level = this.Level + 1;
            value.Group = this.Group;

            this.children.Add(value);
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        object IHasId.Id => this.Id;
    }
}