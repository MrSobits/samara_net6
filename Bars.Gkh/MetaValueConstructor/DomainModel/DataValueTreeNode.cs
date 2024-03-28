namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Узел дерева
    /// </summary>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public class DataValueTreeNode<TValue> where TValue : class, IDataValue, IHasParent<TValue>, IHasNameCode, IHasId
    {
        private bool? isLeaf;

        /// <summary>
        /// Идентификатор
        /// </summary>
        public object Id => this.Current?.Id;

        /// <summary>
        /// Расширенный элемент дерева
        /// </summary>
        [JsonProperty("expanded")]
        public virtual bool Expanded => true;

        /// <summary>
        /// Родитель
        /// </summary>
        [JsonIgnore]
        public DataValueTreeNode<TValue> Parent { get; set; }

        /// <summary>
        /// Текущий элемент
        /// </summary>
        [JsonIgnore]
        public TValue Current { get; set; }

        /// <summary>
        /// Узлы-потомки
        /// </summary>
        public virtual List<DataValueTreeNode<TValue>> Children { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name => this.Current?.Name;

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
        public DataValueTreeNode(TValue value) : this()
        {
            this.Current = value;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public DataValueTreeNode()
        {
            this.Children = new List<DataValueTreeNode<TValue>>();
        }

        /// <summary>
        /// Добавить элемент-потомок
        /// </summary>
        /// <param name="dataValue">Значение</param>
        /// <param name="iconCls">Класс иконки</param>
        /// <returns>Экземпляр узла дерева</returns>
        public virtual DataValueTreeNode<TValue> AddChildren(TValue dataValue)
        {
            ArgumentChecker.NotNull(dataValue, nameof(dataValue));

            var result = this.CreateNodeInternal(dataValue);
           this.Children.Add(result);

            return result;
        }

        /// <summary>
        /// Создать дочерний объект
        /// </summary>
        /// <param name="dataValue">Объект-значение</param>
        /// <returns>Лист дерева</returns>
        protected virtual DataValueTreeNode<TValue> CreateNodeInternal(TValue dataValue)
        {
            return new DataValueTreeNode<TValue>(dataValue)
            {
                Parent = this
            };
        }

        /// <summary>
        /// Значение элемента дерева
        /// </summary>
        public object Value => this.Current?.Value;
    }
}