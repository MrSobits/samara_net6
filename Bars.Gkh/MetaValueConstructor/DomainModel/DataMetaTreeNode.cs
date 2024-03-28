namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.MetaValueConstructor.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Узел дерева
    /// </summary>
    public class DataTreeNode : IDataMetaInfo, IHasParent<DataTreeNode>
    {
        private bool? isLeaf;

        /// <summary>
        /// Расширенный элемент дерева
        /// </summary>
        [JsonProperty("expanded")]
        public virtual bool Expanded => true;

        /// <summary>
        /// Родитель
        /// </summary>
        [JsonIgnore]
        public DataTreeNode Parent { get; set; }

        /// <summary>
        /// Узлы-потомки
        /// </summary>
        public List<DataTreeNode> Children { get; }

        /// <summary>
        /// Индикатор конечного узла
        /// </summary>
        [JsonProperty("leaf")]
        public virtual bool IsLeaf
        {
            get
            {
                return this.isLeaf ?? this.Children.IsEmpty();
            }
            set
            {
                this.isLeaf = value;
            }
        } 

        /// <summary>
        /// Индикатор коренного узла
        /// </summary>
        [JsonIgnore]
        public virtual bool IsRoot => this.Parent.IsNull();

        /// <summary>
        /// Класс иконки
        /// </summary>
        [JsonProperty("iconCls")]
        public virtual string IconCls { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="value">Значение узла дерева</param>
        public DataTreeNode(IDataMetaInfo value)
        {
            if (value != null)
            {
                this.Id = value.Id;
                this.Code = value.Code;
                this.Name = value.Name;

                var props = typeof(DataTreeNode).GetProperties().ToDictionary(x => x.Name);
                typeof(IDataMetaInfo).GetProperties().ForEach(x => props.Get(x.Name).SetValue(this, x.GetValue(value)));
            }

            this.Children = new List<DataTreeNode>();
        }

        /// <summary>
        /// Добавить элемент-потомок
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Экземпляр узла дерева</returns>
        public virtual DataTreeNode AddChildren(IDataMetaInfo value)
        {
            ArgumentChecker.NotNull(value, nameof(value));

            var result = new DataTreeNode(value)
            {
                Parent = this
            };

            this.Children.Add(result);

            return result;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public object Id { get; set; } = 0;

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Вес объекта
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Формула расчета
        /// </summary>
        public string Formula { get; set; }

        /// <summary>
        /// Уровень объекта
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Тип значения
        /// </summary>
        public DataValueType DataValueType { get; set; }

        /// <summary>
        /// Минимальная длина (для строки)
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// Максимальная длина (для строки)
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// Знаков после запятой
        /// </summary>
        public int? Decimals { get; set; }

        /// <summary>
        /// Обязательный
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Источник данных
        /// <para>Если указан, значит данные будут не заполняться, а тянуться из системы</para>
        /// </summary>
        public string DataFillerName { get; set; }
    }
}