namespace Bars.Gkh.DomainService.MetaValueConstructor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.Gkh.Config.Impl.Internal;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;

    using Newtonsoft.Json;

    public partial class AbstractDataValueService
    {
        /// <summary>
        /// Класс помощник
        /// </summary>
        private static class ConstructorMetaHelper
        {
            /// <summary>
            /// Метод возвращает информацию о редакторе
            /// </summary>
            /// <param name="metaInfo">Мета-описание</param>
            /// <returns>База для мета-описания типа параметра конфигурации</returns>
            public static ITypeDescription DescribeType(DataMetaInfo metaInfo)
            {
                var type = metaInfo.DataValueType;

                switch (type)
                {
                    case DataValueType.String:
                        return new StringTypeDescription(metaInfo);

                    case DataValueType.Number:
                        return new NumberDescription(metaInfo);

                    case DataValueType.Boolean:
                        return new BooleanTypeDescriptor(metaInfo);

                    case DataValueType.Dictionary:
                        return new NumberDescription(metaInfo);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private class TypeDescriptor : ITypeDescription
            {
                public TypeDescriptor(DataMetaInfo metaInfo, string editor)
                {
                    this.Editor = editor;
                }

                /// <inheritdoc />
                [JsonProperty("editor")]
                public string Editor { get; }
            }

            private class BooleanTypeDescriptor : TypeDescriptor
            {
                public BooleanTypeDescriptor(DataMetaInfo metaInfo) : base(metaInfo, "bool")
                {
                }
            }

            private class StringTypeDescription : TypeDescriptor
            {
                public StringTypeDescription(DataMetaInfo metaInfo) : base(metaInfo, "text")
                {
                    this.MinLength = metaInfo.MinLength;
                    this.MaxLength = metaInfo.MaxLength;
                }

                [JsonProperty("minLength")]
                public int? MinLength { get; set; }

                [JsonProperty("maxLength")]
                public int? MaxLength { get; set; }
            }

            private class NumberDescription : TypeDescriptor
            {
                public NumberDescription(DataMetaInfo metaInfo) : base(metaInfo, "number")
                {
                    this.Decimals = !metaInfo.Decimals.HasValue || metaInfo.Decimals > 0;
                    this.DecimalsCount = metaInfo.Decimals;
                }

                [JsonProperty("decimals")]
                public bool Decimals { get; set; }

                [JsonProperty("decimalsCount")]
                public int? DecimalsCount { get; set; }
            }
        }

        /// <summary>
        /// Элемент дерева для элементов-значений конструктора
        /// </summary>
        /// <typeparam name="TElement">Тип элемента</typeparam>
        protected class DataValueTree<TElement> : DataValueTreeNode<TElement>
            where TElement : class, IDataValue, IHasParent<TElement>
        {
            /// <summary>
            /// Собрать полное дерево (для UI собирается неполное дерево)
            /// </summary>
            [JsonIgnore]
            public bool CreateFullTree { get; set; }

            /// <summary>
            /// Объекты нижнего уровня для генерации формы
            /// </summary>
            public IList<DataValueMetaItem> AttributeObjects { get; set; }

            /// <inheritdoc />
            public DataValueTree(TElement value)
                : base(value)
            {
                this.AttributeObjects = new List<DataValueMetaItem>();
                this.ObjectType = value?.MetaInfo.Group.ConstructorType ?? 0;
            }

            /// <inheritdoc />
            public override DataValueTreeNode<TElement> AddChildren(TElement dataValue)
            {
                if (dataValue.MetaInfo.Level < 2 || this.CreateFullTree)
                {
                    return base.AddChildren(dataValue);
                }

                this.AttributeObjects.Add(new DataValueMetaItem
                {
                    Id = dataValue.Id.ToString(),
                    Value = new ValueHolder(dataValue.Value),
                    Children = new MetaItem[0],
                    DisplayName = dataValue.MetaInfo.Name,
                    Type = ConstructorMetaHelper.DescribeType(dataValue.MetaInfo),
                    ExtraParams = new Dictionary<string, object>
                    {
                        { "readOnly", dataValue.MetaInfo.DataValueType == DataValueType.Dictionary },
                        { "hideTrigger", true },
                        { "allowBlank", !dataValue.MetaInfo.Required }
                    }
                });

                return null;
            }

            /// <inheritdoc />
            protected override DataValueTreeNode<TElement> CreateNodeInternal(TElement dataValue)
            {
                return new DataValueTree<TElement>(dataValue)
                {
                    CreateFullTree = this.CreateFullTree,
                    Parent = this
                };
            }

            /// <summary>
            /// Тип объекта
            /// </summary>
            public DataMetaObjectType ObjectType { get; set; }
        }

        /// <summary>
        /// Мета-описание параметра конфигурации
        /// </summary>
        public class DataValueMetaItem : MetaItem
        {
        }

        /// <summary>
        /// Генератора дерева
        /// </summary>
        protected class DataValueTreeGenerator<TElement, TTree>
            where TElement : class, IDataValue, IHasParent<TElement>
            where TTree : DataValueTree<TElement>, new()
        {
            /// <summary>
            /// Сгенерировать дерево мета-информации
            /// </summary>
            /// <param name="values"> Список мета-информации </param>
            /// <param name="rootElement">Родительский элемент</param>
            /// <returns>
            /// </returns>
            public TTree GetTree(IEnumerable<TElement> values, TElement rootElement = null, Func<TElement, object> orderSelector = null, bool forUI = true)
            {
                var root = Activator.CreateInstance<TTree>();
                root.Current = rootElement;
                root.CreateFullTree = !forUI;

                this.AddNode(root, values, orderSelector);
                return root;
            }

            /// <summary>
            /// Добавить потомка к дереву
            /// </summary>
            /// <param name="root">Родитель</param>
            /// <param name="children">Потомки</param>
            protected virtual void AddNode(DataValueTreeNode<TElement> root, IEnumerable<TElement> children, Func<TElement, object> orderSelector = null)
            {
                if (root.IsNull())
                {
                    return;
                }

                // рекурсивно добавляем элементы
                foreach (var source in children.Where(x => object.Equals(x.Parent?.Id, root.Id)).OrderBy(orderSelector ?? (x => x.Id)))
                {
                    this.AddNode(root.AddChildren(source), children, orderSelector);
                }
            }
        }

        /// <summary>
        /// Параметры сохранения
        /// </summary>
        protected class SaveParams
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Значение
            /// </summary>
            public object Value { get; set; }
        }
    }
}