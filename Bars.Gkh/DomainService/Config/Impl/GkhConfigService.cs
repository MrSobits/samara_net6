namespace Bars.Gkh.DomainService.Config.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.DataAnnotations;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Config.Impl.Internal;
    using Bars.Gkh.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    ///     Сервис конфигурации приложения
    /// </summary>
    public class GkhConfigService : IGkhConfigService
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="configProvider">Провайдер конфигурации</param>
        /// <param name="authorizationService">Сервис авторизации</param>
        /// <param name="userIdentity">Идентификатор пользователя</param>
        public GkhConfigService(
            IGkhConfigProvider configProvider,
            IAuthorizationService authorizationService,
            IUserIdentity userIdentity)
        {
            this.ConfigProvider = configProvider;
            this.AuthorizationService = authorizationService;
            this.UserIdentity = userIdentity;
        }

        private IAuthorizationService AuthorizationService { get; set; }

        private IGkhConfigProvider ConfigProvider { get; set; }

        private IUserIdentity UserIdentity { get; set; }

        /// <summary>
        ///     Получение всех параметров конфигурации
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, object> GetAllConfigs()
        {
            return this.ConfigProvider.Map.Where(x => !typeof(IGkhConfigSection).IsAssignableFrom(x.Value.Type))
                .Where(x => !x.Value.UIHidden)
                .OrderBy(x => x.Value.Order)
                .ToDictionary(x => x.Key, x => this.ConfigProvider.ValueHolders[x.Key].Value);
        }

        /// <inheritdoc />
        public string GetSerializedConfig()
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ConfigJsonConverter());
            var configParams = this.ConfigProvider.Map.Where(x => !typeof(IGkhConfigSection).IsAssignableFrom(x.Value.Type))
                .Where(x => !x.Value.UIHidden)
                .OrderBy(x => x.Value.Order)
                .ToDictionary(x => x.Key, x => this.ConfigProvider.ValueHolders[x.Key].Value);

            return JsonConvert.SerializeObject(configParams, settings);
        }

        /// <inheritdoc />
        public string GetSerializedConfig(IDictionary<string, ValueHolder> configParams)
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new ConfigJsonConverter());
            return JsonConvert.SerializeObject(configParams.ToDictionary(x => x.Key, x => x.Value.Value), settings);
        }

        /// <summary>
        ///     Получение списка дочерних элементов
        /// </summary>
        /// <param name="parent">Идентификатор родительского элемента</param>
        /// <returns></returns>
        public MetaItem[] GetItems(string parent)
        {
            return this.GetItems(parent, false);
        }

        /// <summary>
        ///     Получение списка корневых элементов
        /// </summary>
        /// <returns></returns>
        public MetaItem[] GetRoots()
        {
            return this.GetItems(null, true);
        }

        /// <summary>
        ///     Обновление конфигурации
        /// </summary>
        /// <param name="config">Сериализованный словарь параметров</param>
        /// <param name="errors">Список ошибок</param>
        /// <returns>Признак успешности</returns>
        public bool UpdateConfigs(string config, out IDictionary<string, string> errors)
        {
            var records = JsonConvert.DeserializeObject<Dictionary<string, string>>(config);

            errors = new Dictionary<string, string>();
            var holders = this.ConfigProvider.ValueHolders;
            foreach (var record in records)
            {
                ValueHolder holder;
                if (!holders.TryGetValue(record.Key, out holder))
                {
                    continue;
                }

                try
                {
                    var v = JsonConvert.DeserializeObject(record.Value, holder.Type);
                    string[] validationErrors;
                    if (holder.ValidateValue(v, out validationErrors))
                    {
                        holder.SetValue(v, true);
                    }
                    else
                    {
                        errors.Add(record.Key, string.Join(", ", validationErrors));
                    }
                }
                catch (Exception e)
                {
                    errors.Add(record.Key, e.Message);
                }
            }

            var exception = this.ConfigProvider.SaveChanges();
            if (exception != null)
            {
                throw exception;
            }

            return errors.Count == 0;
        }

        private bool CheckNavigation(PropertyMetadata meta, bool navigation)
        {
            var navigatable = meta.AttributeProvider.HasAttribute<NavigationAttribute>(true)
                || meta.AttributeProvider.GetAttribute<GkhConfigSectionAttribute>(true)
                    .Return(x => x.UIParent == null);

            return navigation ? navigatable : !navigatable;
        }

        private bool CheckPermission(KeyValuePair<string, PropertyMetadata> item)
        {
            var attr = item.Value.AttributeProvider.GetAttribute<PermissionableAttribute>(true);
            if (attr == null)
            {
                return true;
            }
            var isSection = typeof(IGkhConfigSection).IsAssignableFrom(item.Value.Type);
            return this.AuthorizationService.Grant(
                this.UserIdentity,
                $"Gkh.Config.{item.Key}{(isSection ? '.' : '_')}View");
        }

        private MetaItem[] GetItems(string parent, bool navigation)
        {
            return
                this.ConfigProvider.Map.Where(x => x.Value.Parent == parent && !x.Value.Hidden)
                    .Where(x => this.CheckNavigation(x.Value, navigation))
                    .OrderBy(x => x.Value.Order)
                    .Where(this.CheckPermission)
                    .Select(x => new MetaItem
                    {
                        Id = x.Key,
                        Value = this.ConfigProvider.ValueHolders[x.Key],
                        DefaultValue = x.Value.DefaultValue ?? x.Value.Type.Return(y => y.GetDefaultValue()),
                        DisplayName = x.Value.DisplayName,
                        Type = x.Value.Type.Return(y => MetaHelper.DescribeType(y, x.Value.AttributeProvider)),
                        Children = this.GetItems(x.Key, navigation),
                        GroupName = x.Value.AttributeProvider.GetAttribute<GroupAttribute>(true).Return(y => y.Value),
                        ExtraParams = this.GetExtraParams(x.Value)
                    })
                    .ToArray();
        }

        private IDictionary<string, object> GetExtraParams(PropertyMetadata meta)
        {
            var extraParams = new Dictionary<string, object>
            {
                ["readOnly"] = meta.AttributeProvider?.GetAttribute<ReadOnlyAttribute>(true)?.IsReadOnly
            };

            if (meta.PropertyInfo != null)
            {
                extraParams["disabled"] = meta.PropertyInfo.SetMethod == null || !(bool)meta.PropertyInfo.SetMethod?.IsPublic || !meta.PropertyInfo.CanWrite;
            }

            foreach (var attribute in meta.AttributeProvider.GetAttributes<UIExtraParamAttribute>(true))
            {
                extraParams[attribute.Name] = attribute.Value;
            }

            return extraParams;
        }

        private static class MetaHelper
        {
            public static ITypeDescription DescribeType(
                Type type,
                ICustomAttributeProvider customAttributes,
                bool skipSection = true)
            {
                type = Nullable.GetUnderlyingType(type) ?? type;
                ITypeDescription res;
                if (skipSection && typeof(IGkhConfigSection).IsAssignableFrom(type))
                {
                    res = new TypeDescription { Editor = "section" };
                }
                else if (type.IsEnum)
                {
                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
                    var instance = Activator.CreateInstance(type);
                    res = new EnumDescription
                    {
                        Values = fields
                            .Where(x => !x.GetAttribute<GkhConfigPropertyAttribute>(true).Return(a => a.Hidden))
                            .Select(
                                x => new EnumValue
                                {
                                    Value = x.GetValue(instance),
                                    DisplayName = x.GetDisplayName().Or(x.Name)
                                })
                            .ToArray(),
                        DisplayName = type.GetDisplayName()
                    };
                }
                else if (type == typeof(string))
                {
                    res = new PrimitiveTypeDescription(type, customAttributes) { Editor = "text" };
                }
                else if (type == typeof(bool))
                {
                    res = new TypeDescription { Editor = "bool" };
                }
                else if (MetaHelper.IsNumericType(type))
                {
                    res = new NumberDescription(type, customAttributes);
                }
                else if (type == typeof(DateTime))
                {
                    res = new TypeDescription { Editor = "date" };
                }
                else if (type.Is<IDictionary>())
                {
                    var generics = type.GetInterfaces()
                        .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                        .Return(x => x.GetGenericArguments());

                    res = new DictionaryDescription
                    {
                        KeyType = MetaHelper.DescribeType(
                            generics.Return(x => x[0]) ?? typeof(object),
                            null,
                            false),
                        ValueType = MetaHelper.DescribeType(
                            generics.Return(x => x[1]) ?? typeof(object),
                            null,
                            false)
                    };
                }
                else if (type.IsArray)
                {
                    res = new CollectionDescription
                    {
                        ElementType = MetaHelper.DescribeType(type.GetElementType(), null, false),
                        HideToolbar = customAttributes.GetAttribute<GkhConfigPropertyAttribute>(false).Return(x => x.HideToolbar)
                    };
                }
                else if (type.Is<IList>())
                {
                    res = new CollectionDescription
                    {
                        ElementType = MetaHelper.DescribeType(
                            type.GetMethod("Add")
                                .GetParameters()
                                .First()
                                .ParameterType,
                            null,
                            false),
                        HideToolbar = customAttributes.GetAttribute<GkhConfigPropertyAttribute>(false).Return(x => x.HideToolbar)
                    };
                }
                else if (type.Assembly.FullName.Contains("mscorlib"))
                {
                    object sample = null;
                    try
                    {
                        var constructor = type.GetConstructors().FirstOrDefault();
                        if (constructor != null)
                        {
                            var args = constructor.GetParameters()
                                .Select(x => x.ParameterType)
                                .Select(x => x.GetDefaultValue())
                                .ToArray();
                            sample = Activator.CreateInstance(type, args);
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    res = new RawDescription { TypeName = MetaHelper.GetNicerTypeName(type), Sample = sample };
                }
                else
                {
                    res = new ObjectDescription
                    {
                        Editor = "object",
                        Properties = type.GetProperties()
                            .Where(x => !x.HasAttribute<JsonIgnoreAttribute>(true))
                            .Where(x => !x.HasAttribute<IgnoreAttribute>(true))
                            .Select(
                                x => new PropertyDescription
                                {
                                    DisplayName = x.GetDisplayName().Or(x.Name),
                                    Name = x.Name,
                                    Type = MetaHelper.DescribeType(x.PropertyType, x, false),
                                    ReadOnly = x.GetAttribute<ReadOnlyAttribute>(false).Return(y => y.IsReadOnly),
                                    DefaultValue = x.GetAttribute<DefaultValueAttribute>(false).Return(y => y.Value) ?? x.PropertyType.GetDefaultValue()
                                })
                            .ToArray(),
                        TypeName = MetaHelper.GetNicerTypeName(type),
                        DisplayName = type.GetDisplayName()
                    };
                }

                if (type.HasAttribute<GkhConfigPropertyEditorAttribute>(true))
                {
                    var editor = type.GetAttribute<GkhConfigPropertyEditorAttribute>(true);
                    return new CustomEditorDescription
                    {
                        Path = editor.Path,
                        XType = editor.Xtype,
                        Meta = res
                    };
                }

                if (customAttributes != null && customAttributes.HasAttribute<GkhConfigPropertyEditorAttribute>(true))
                {
                    var editor = customAttributes.GetAttribute<GkhConfigPropertyEditorAttribute>(true);
                    return new CustomEditorDescription
                    {
                        Path = editor.Path,
                        XType = editor.Xtype,
                        Meta = res
                    };
                }

                return res;
            }

            private static string GetNicerTypeName(Type t)
            {
                if (!t.IsGenericType)
                {
                    return t.Name;
                }
                var sb = new StringBuilder();

                sb.Append(t.Name.Substring(0, t.Name.LastIndexOf("`", StringComparison.Ordinal)));
                sb.Append(t.GetGenericArguments().Aggregate(
                    "<",
                    (aggregate, type) => aggregate + (aggregate == "<" ? string.Empty : ",") + MetaHelper.GetNicerTypeName(type)));
                sb.Append(">");

                return sb.ToString();
            }

            private static bool IsIntegralType(TypeCode typeCode)
            {
                switch (typeCode)
                {
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        return true;
                    default:
                        return false;
                }
            }

            private static bool IsNumericType(TypeCode typeCode)
            {
                switch (typeCode)
                {
                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        return true;
                    default:
                        return false;
                }
            }

            private static bool IsNumericType(Type type)
            {
                return MetaHelper.IsNumericType(Type.GetTypeCode(type));
            }

            private class PropertyDescription
            {
                [JsonProperty("readOnly")]
                public bool ReadOnly { get; set; }

                [JsonProperty("defaultValue")]
                public object DefaultValue { get; set; }

                [JsonProperty("displayName")]
                public string DisplayName { get; set; }

                [JsonProperty("name")]
                public string Name { get; set; }

                [JsonProperty("type")]
                public ITypeDescription Type { get; set; }
            }

            private class TypeDescription : ITypeDescription
            {
                [JsonProperty("editor")]
                public virtual string Editor { get; internal set; }
            }

            private class PrimitiveTypeDescription : TypeDescription
            {
                public PrimitiveTypeDescription(Type type, ICustomAttributeProvider provider)
                {
                    this.PossibleValues = type.GetAttribute<PossibleValuesAttribute>(true).Return(a => a.Values);

                    if (this.PossibleValues.IsEmpty() && provider != null)
                    {
                        this.PossibleValues = provider.GetAttribute<PossibleValuesAttribute>(true).Return(a => a.Values);
                    }
                }

                [JsonProperty("possibleValues")]
                private object[] PossibleValues { get; set; }
            }

            private class ObjectDescription : ITypeDescription
            {
                [JsonProperty("displayName")]
                public string DisplayName { get; set; }

                [JsonProperty("properties")]
                public PropertyDescription[] Properties { get; set; }

                [JsonProperty("typeName")]
                public string TypeName { get; set; }

                [JsonProperty("editor")]
                public string Editor { get; internal set; }
            }

            private class CollectionDescription : ITypeDescription
            {
                [JsonProperty("elementType")]
                public ITypeDescription ElementType { get; set; }

                [JsonProperty("hideToolbar")]
                public bool HideToolbar { get; set; }

                [JsonProperty("editor")]
                public string Editor => "collection";
            }

            private class DictionaryDescription : ITypeDescription
            {
                [JsonProperty("keyType")]
                public ITypeDescription KeyType { get; set; }

                [JsonProperty("valueType")]
                public ITypeDescription ValueType { get; set; }

                [JsonProperty("editor")]
                public string Editor => "dictionary";
            }

            private class NumberDescription : PrimitiveTypeDescription
            {
                public NumberDescription(Type type, ICustomAttributeProvider provider)
                    : base(type, provider)
                {
                    var rangeAttribute = type.GetAttribute<RangeAttribute>(true) ?? (provider != null ? provider.GetAttribute<RangeAttribute>(true) : null);
                    var defaultInstance = rangeAttribute == null ? type.GetDefaultValue() : null;
                    this.Decimals = !MetaHelper.IsIntegralType(Type.GetTypeCode(type));
                    this.MinValue = rangeAttribute == null
                        ? type.GetField("MinValue").GetValue(defaultInstance)
                        : rangeAttribute.Minimum;
                    this.MaxValue = rangeAttribute == null
                        ? type.GetField("MaxValue").GetValue(defaultInstance)
                        : rangeAttribute.Maximum;
                }

                [JsonProperty("decimals")]
                public bool Decimals { get; set; }

                [JsonProperty("editor")]
                public override string Editor => "number";

                [JsonProperty("maxValue")]
                public object MaxValue { get; set; }

                [JsonProperty("minValue")]
                public object MinValue { get; set; }
            }

            private class EnumDescription : ITypeDescription
            {
                [JsonProperty("displayName")]
                public string DisplayName { get; set; }

                [JsonProperty("values")]
                public EnumValue[] Values { get; set; }

                [JsonProperty("editor")]
                public string Editor => "enum";
            }

            private class EnumValue
            {
                [JsonProperty("displayName")]
                public string DisplayName { get; set; }

                [JsonProperty("value")]
                public object Value { get; set; }
            }

            private class RawDescription : ITypeDescription
            {
                [JsonProperty("sample")]
                public object Sample { get; set; }

                [JsonProperty("typeName")]
                public string TypeName { get; set; }

                [JsonProperty("editor")]
                public string Editor => "raw";
            }

            private class CustomEditorDescription : ITypeDescription
            {
                [JsonProperty("meta")]
                public ITypeDescription Meta { get; set; }

                [JsonProperty("path")]
                public string Path { get; set; }

                [JsonProperty("xtype")]
                public string XType { get; set; }

                [JsonProperty("editor")]
                public string Editor => "custom";
            }
        }

        /// <summary>
        /// Сериализатор параметров конфигурации
        /// </summary>
        public class ConfigJsonConverter : JsonConverter
        {
            /// <inheritdoc />
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            /// <inheritdoc />
            public override bool CanConvert(Type objectType)
            {
                var result = typeof(Dictionary<string, object>).IsAssignableFrom(objectType);
                return result;
            }

            /// <inheritdoc />
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var result = new JObject();
                var dict = value as Dictionary<string, object> ?? new Dictionary<string, object>();

                foreach (var item in dict.OrderBy(x => x.Key))
                {
                    var parts = item.Key.Split('.');
                    JToken obj = result;
                    for (int i = 0; i < parts.Length - 1; i++)
                    {
                        if (obj[parts[i]] == null)
                        {
                            obj[parts[i]] = new JObject();
                        }

                        obj = obj[parts[i]];
                    }
                    var valieKey = parts[parts.Length - 1];

                    if (item.Value != null)
                    {
                        obj[valieKey] = JToken.FromObject(item.Value);
                    }
                    else
                    {
                        obj[valieKey] = null;
                    }
                }

                result.WriteTo(writer);
            }
        }
    }
}