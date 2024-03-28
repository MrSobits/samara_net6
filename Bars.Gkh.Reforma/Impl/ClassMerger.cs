namespace Bars.Gkh.Reforma.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.Attributes;
    using Bars.Gkh.Reforma.Interface;

    /// <summary>
    ///     Дурацкий склеиватель инстансов
    /// </summary>
    public class ClassMerger : IClassMerger
    {
        private bool _aggressiveMode;

        /// <summary>
        ///     Конструктор
        /// </summary>
        public ClassMerger()
        {
            Maps = new Dictionary<Type, IPropertyMerger[]>();
        }

        private Dictionary<Type, IPropertyMerger[]> Maps { get; set; }

        /// <summary>
        ///     Агрессивный режим. В этом режиме "нежная" замена включается только для полей, помеченных
        ///     атрибутом <see cref="MergeableAttribute"/>. Для остальных же полей производится безусловная замена значений.
        /// </summary>
        public bool AggressiveMode
        {
            get
            {
                return this._aggressiveMode;
            }
            set
            {
                if (this._aggressiveMode != value)
                {
                    this.Maps.Clear();
                }

                this._aggressiveMode = value;
            }
        }

        /// <summary>
        ///     Заполнить значения в dst непустыми значениями из src
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dst"></param>
        /// <param name="src"></param>
        public void Apply<T>(T dst, T src)
        {
            var type = dst.GetType();
            if (!type.IsInstanceOfType(src))
            {
                throw new ArgumentException(string.Format("dst {0}, src {1}", type.Name, src.GetType().Name), "src");
            }

            if (!IsMapped(type))
            {
                Map(type);
            }

            var mergers = Maps[type];
            foreach (var merger in mergers)
            {
                merger.Apply(dst, src);
            }
        }

        private bool IsMapped(Type type)
        {
            return Maps.ContainsKey(type);
        }

        private void Map(Type type)
        {
            var properties = type.GetProperties();
            var mergers = new List<IPropertyMerger>(properties.Length);
            foreach (var property in properties)
            {
                if (this.AggressiveMode && !property.HasAttribute<MergeableAttribute>(true))
                {
                    mergers.Add(new NonMergingMerger(property));
                    continue;
                }

                var propType = property.PropertyType;
                if (propType == typeof(string))
                {
                    mergers.Add(new StringMerger(property));
                }
                else if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    mergers.Add(new NullableMerger(property));
                }
                else if (propType.IsValueType)
                {
                    mergers.Add(new StructMerger(property));
                }
                else if (propType.IsArray
                         && propType.GetElementType()
                                    .GetMember(
                                        "id",
                                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)
                                    .Length > 0)
                {
                    mergers.Add(new EntityArrayMerger(this, property));
                }
                else if (propType.FullName.StartsWith("System.") || propType.IsArray)
                {
                    mergers.Add(new NullableMerger(property));
                }
                else
                {
                    mergers.Add(new ObjectMerger(this, property));
                }
            }

            Maps[type] = mergers.ToArray();
        }

        private interface IPropertyMerger
        {
            void Apply(object dst, object src);
        }

        private abstract class PropertyMerger : IPropertyMerger
        {
            protected readonly PropertyInfo Property;

            protected PropertyMerger(PropertyInfo property)
            {
                Property = property;
            }

            public abstract void Apply(object dst, object src);

            protected object GetValue(object o)
            {
                return Property.GetValue(o, null);
            }

            protected void SetValue(object o, object v)
            {
                Property.SetValue(o, v, null);
            }
        }

        private class NonMergingMerger : PropertyMerger
        {
            public NonMergingMerger(PropertyInfo property)
                : base(property)
            {
            }

            public override void Apply(object dst, object src)
            {
                SetValue(dst, GetValue(src));
            }
        }

        private class StringMerger : PropertyMerger
        {
            public StringMerger(PropertyInfo property)
                : base(property)
            {
            }

            public override void Apply(object dst, object src)
            {
                var srcValue = GetValue(src) as string;
                if (!string.IsNullOrEmpty(srcValue))
                {
                    SetValue(dst, srcValue);
                }
            }
        }

        private class NullableMerger : PropertyMerger
        {
            public NullableMerger(PropertyInfo property)
                : base(property)
            {
            }

            public override void Apply(object dst, object src)
            {
                var srcValue = GetValue(src);
                if (srcValue != null)
                {
                    SetValue(dst, srcValue);
                }
            }
        }

        private class ObjectMerger : PropertyMerger
        {
            private readonly ClassMerger _merger;

            public ObjectMerger(ClassMerger merger, PropertyInfo property)
                : base(property)
            {
                _merger = merger;
            }

            public override void Apply(object dst, object src)
            {
                var srcValue = GetValue(src);
                if (srcValue != null)
                {
                    var destValue = GetValue(dst) ?? Activator.CreateInstance(Property.PropertyType);
                    _merger.Apply(destValue, srcValue);
                    SetValue(dst, destValue);
                }
            }
        }

        private class StructMerger : PropertyMerger
        {
            private readonly ValueType _defaultValue;

            public StructMerger(PropertyInfo property)
                : base(property)
            {
                _defaultValue = (ValueType)Activator.CreateInstance(property.PropertyType);
            }

            public override void Apply(object dst, object src)
            {
                var srcValue = GetValue(src);
                if (!_defaultValue.Equals(srcValue))
                {
                    SetValue(dst, srcValue);
                }
            }
        }

        private class EntityArrayMerger : PropertyMerger
        {
            private readonly object _idDefaultValue;

            private readonly MemberInfo _idMember;

            private readonly ClassMerger _merger;

            public EntityArrayMerger(ClassMerger merger, PropertyInfo property)
                : base(property)
            {
                this._merger = merger;
                this._idMember =
                    property.PropertyType.GetElementType()
                            .GetMember("id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase)[0];
                this._idDefaultValue =
                    Activator.CreateInstance(
                        this._idMember.MemberType == MemberTypes.Property
                            ? ((PropertyInfo)this._idMember).PropertyType
                            : ((FieldInfo)this._idMember).FieldType);
            }

            public override void Apply(object dst, object src)
            {
                var srcValue = GetValue(src) as Array;
                var srcItems = srcValue != null ? GetItems(srcValue, true) : new KeyValuePair<string, object>[0];
                var dstValue = GetValue(dst) as Array;
                var dstItems = dstValue != null ? GetItems(dstValue, false) : new KeyValuePair<string, object>[0];

                var result = new ArrayList();

                var commonItems =
                    dstItems.Select(
                        x =>
                        new { dst = x.Value, src = srcItems.FirstOrDefault(y => y.Key == x.Key).Return(y => y.Value) })
                            .Where(x => x.src != null);

                foreach (var commonItem in commonItems)
                {
                    this._merger.Apply(commonItem.dst, commonItem.src);
                    result.Add(commonItem.dst);
                }

                var newItems =
                    srcItems.Where(x => string.IsNullOrEmpty(x.Key) || !dstItems.Any(y => y.Key == x.Key))
                            .Select(x => x.Value);

                foreach (var newItem in newItems)
                {
                    result.Add(newItem);
                }

                SetValue(dst, result.ToArray(this.Property.PropertyType.GetElementType()));
            }

            private KeyValuePair<string, object>[] GetItems(Array array, bool allowEmptyKeys)
            {
                return
                    array.Cast<object>()
                         .Select(
                             x =>
                             new
                                 {
                                     value = x,
                                     id =
                                 this._idMember.MemberType == MemberTypes.Property
                                     ? ((PropertyInfo)this._idMember).GetValue(x, null)
                                     : ((FieldInfo)this._idMember).GetValue(x)
                                 })
                         .Where(t => allowEmptyKeys || t.id != this._idDefaultValue)
                         .Select(x => new KeyValuePair<string, object>(x.id.ToStr(), x.value))
                         .ToArray();
            }
        }
    }
}