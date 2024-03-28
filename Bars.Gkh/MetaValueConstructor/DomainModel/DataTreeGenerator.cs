namespace Bars.Gkh.MetaValueConstructor.DomainModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataModels;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;

    using Newtonsoft.Json;

    /// <summary>
    /// Генератора дерева
    /// </summary>
    public class DataGenerator<TElement> where TElement : class, IHasParent<TElement>, IHasNameCode, IHasId
    {
        /// <summary>
        /// Сгенерировать дерево мета-информации
        /// </summary>
        /// <param name="values"> Список мета-информации </param>
        /// <param name="rootElement">Родительский элемент</param>
        /// <returns>
        /// </returns>
        public DataTree<TElement> GetTree(IEnumerable<TElement> values, TElement rootElement = null)
        {
            var root = DataTree<TElement>.CreateRoot();
            this.AddNode(root, values);
            return root;
        }

        /// <summary>
        /// Добавить потомка к дереву
        /// </summary>
        /// <param name="root">Родитель</param>
        /// <param name="children">Потомки</param>
        protected virtual void AddNode(DataTree<TElement> root, IEnumerable<TElement> children)
        {
            // рекурсивно добавляем элементы
            foreach (var source in children.Where(x => object.Equals(x.Parent?.Id, root.Id)).OrderBy(x => x.Id))
            {
                this.AddNode(root.AddChildren(source), children);
            }
        }
    }

    /// <summary>
    /// The data tree.
    /// </summary>
    /// <typeparam name="TElement">
    /// </typeparam>
    public class DataTree<TElement> : IHasNameCode where TElement : IHasNameCode, IHasId
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
        public DataTree<TElement> Parent { get; set; }

        /// <summary>
        /// Узлы-потомки
        /// </summary>
        public List<DataTree<TElement>> Children { get; }

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

        private DataTree()
        {
            this.Children = new List<DataTree<TElement>>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        private DataTree(TElement element, DataTree<TElement> parent) : this()
        {
            this.Id = element.Id;
            this.Name = element.Name;
            this.Code = element.Code;
            this.Parent = parent;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public object Id { get; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Добавить элемент-потомок
        /// </summary>
        /// <param name="value">Значение</param>
        /// <returns>Экземпляр узла дерева</returns>
        public virtual DataTree<TElement> AddChildren(TElement value)
        {
            ArgumentChecker.NotNull(value, nameof(value));

            var result = new DataTree<TElement>(value, this);

            this.Children.Add(result);

            return result;
        }

        /// <summary>
        /// Создать элемент корня дерева
        /// </summary>
        /// <returns></returns>
        public static DataTree<TElement> CreateRoot()
        {
            return new DataTree<TElement>();
        }
    }
}