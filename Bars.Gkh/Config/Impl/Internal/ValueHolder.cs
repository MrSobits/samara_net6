// ReSharper disable InconsistentNaming

namespace Bars.Gkh.Config.Impl.Internal
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config.Impl.Internal.Serialization;
    using Bars.Gkh.Utils;

    using Newtonsoft.Json;

    /// <summary>
    ///     Контейнер значения
    /// </summary>
    [JsonConverter(typeof(Converter))]
    public class ValueHolder
    {
        private object _previousValue;

        private Type _type;

        private ValidationContext _validationContext;

        private object _value;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="metadata">Метаописание родительского свойства</param>
        public ValueHolder(object value, PropertyMetadata metadata)
            : this(value)
        {
            ArgumentChecker.NotNull(metadata, "metadata");
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Attach(metadata);
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="value">Значение</param>
        public ValueHolder(object value)
        {
            _value = value;
        }

        /// <summary>
        ///     Признак типизированного значения (связанного с конкретным типом)
        /// </summary>
        public virtual bool IsAttached { get; private set; }

        /// <summary>
        ///     Признак изменения значения
        /// </summary>
        public virtual bool IsModified { get; set; }

        /// <summary>
        ///     Метаописание родительского свойства
        /// </summary>
        public virtual PropertyMetadata Metadata { get; private set; }

        /// <summary>
        ///     Тип значения.
        ///     До связывания (<see cref="Attach" />) равен null
        /// </summary>
        public virtual Type Type
        {
            get
            {
                return this._type;
            }
        }

        /// <summary>
        ///     Значение свойства.
        ///     До связывания с конкретным полем (<see cref="Attach" />) может возвращать че попало
        /// </summary>
        public virtual object Value
        {
            get
            {
                if (_type != null && typeof(IGkhConfigSection).IsAssignableFrom(_type))
                {
                    return null;
                }

                return _value ?? (_type != null ? _type.GetDefaultValue() : null);
            }
        }

        /// <summary>
        ///     Присоединение значения к типизированному метаописанию поля
        /// </summary>
        /// <param name="metadata"></param>
        public virtual void Attach(PropertyMetadata metadata)
        {
            Metadata = metadata;
            SetType(metadata.Type);
        }

        /// <summary>
        ///     Откатывает значение к последнему сохраненному состоянию
        /// </summary>
        /// <returns></returns>
        public virtual ValueHolder Revert()
        {
            if (IsModified)
            {
                _value = _previousValue;
                IsModified = false;
            }

            return this;
        }

        /// <summary>
        ///     Задание хранимого значения.
        ///     До связывания с конкретным полем (<see cref="Attach" />) в качестве аргумента
        ///     принимает любые значения. После - только приводимые к хранимому типу.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="skipValidation"></param>
        public virtual ValueHolder SetValue(object value, bool skipValidation = false)
        {
            if (_type != null)
            {
                object bindResult;
                if (!TryConvert(value, _type, out bindResult))
                {
                    ExceptionHelper.Throw<ArgumentException>(
                        "[{0}] Значение типа {1} не приводимо к {2}",
                        Metadata.DisplayName,
                        value.GetType().Name,
                        _type.Name);
                }

                value = bindResult;

                if (!skipValidation)
                {
                    string[] validationErrors;
                    if (!this.ValidateValue(value, out validationErrors))
                    {
                        ExceptionHelper.Throw<ValidationException>(
                            "[{0}] Значение не удовлетворяет наложенным ограничениям:\r\n{1}",
                            Metadata.DisplayName,
                            string.Join("\r\n", validationErrors));
                    }
                }
            }

            IsModified = IsAttached && !typeof(IGkhConfigSection).IsAssignableFrom(_type) && !Equals(value, _value);

            _previousValue = _value;
            _value = value;

            return this;
        }

        /// <summary>
        ///     Попытается привести значение к требуемому типу
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryConvert(object value, Type type, out object result)
        {
            result = null;
            if (value != null)
            {
                if (value is string)
                {
                    try
                    {
                        result = ConfigSerializer.Deserialize((string)value, type);
                        return true;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (type.IsInstanceOfType(value))
                {
                    result = value;
                    return true;
                }

                try
                {
                    result = ConvertHelper.ConvertTo(value, type);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            if (type.IsValueType)
            {
                result = Activator.CreateInstance(type);
            }

            return true;
        }

        /// <summary>
        ///     Проверяет значение на допустимость.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public bool ValidateValue(object value, out string[] errors)
        {
            errors = new string[0];
            if (!IsAttached)
            {
                return true;
            }

            if (value != null && !_type.IsInstanceOfType(value))
            {
                errors = new[]
                             {
                                 string.Format(
                                     "Переданное значение {0} не приводимо к типу свойства {1}",
                                     value,
                                     _type.Name)
                             };
                return false;
            }

            var attrs = Metadata.AttributeProvider.GetAttributes<ValidationAttribute>(true);
            if (attrs.Length == 0)
            {
                return true;
            }

            var messages = new List<ValidationResult>();
            var vc = _validationContext
                     ?? (_validationContext =
                         new ValidationContext(this, null, null) { DisplayName = Metadata.DisplayName });
            if (Validator.TryValidateValue(value, vc, messages, attrs))
            {
                return true;
            }

            errors = messages.Select(x => x.ErrorMessage).ToArray();
            return false;
        }

        /// <summary>
        ///     Связывает значение с конкретным типом.
        ///     Если на момент вызова метода контейнер содержит уже какое-то
        ///     значение, в процессе связывания будет сделана попытка привести его
        ///     к требуемому типу. Она может закончиться исключением.
        ///     Более того, попытка сменить уже связанный тип тоже приведет к исключению.
        ///     Поэтому в любой непонятной ситуации лучше перепроверить значение поля <see cref="IsAttached" />
        /// </summary>
        /// <param name="type"></param>
        protected virtual ValueHolder SetType(Type type)
        {
            ArgumentChecker.NotNull(type, "type");

            if (_type != null && _type != type)
            {
                ExceptionHelper.Throw<InvalidOperationException>(
                    "[{0}] Попытка сменить тип уже типизированного поля: {1} -> {2}",
                    Metadata.DisplayName,
                    _type.Name,
                    type.Name);
            }

            _type = type;

            object value;
            if (!TryConvert(_value, _type, out value) && _type.IsValueType)
            {
                value = Activator.CreateInstance(_type);
            }

            SetValue(value);

            IsAttached = true;

            return this;
        }

        #region JsonConverter

        private class Converter : JsonConverter
        {
            /// <summary>
            ///     Gets a value indicating whether this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON.
            /// </summary>
            /// <value>
            ///     <c>true</c> if this <see cref="T:Newtonsoft.Json.JsonConverter" /> can read JSON; otherwise, <c>false</c>.
            /// </value>
            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            ///     Determines whether this instance can convert the specified object type.
            /// </summary>
            /// <param name="objectType">Type of the object.</param>
            /// <returns>
            ///     <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
            /// </returns>
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(ValueHolder);
            }

            /// <summary>
            ///     Reads the JSON representation of the object.
            /// </summary>
            /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
            /// <param name="objectType">Type of the object.</param>
            /// <param name="existingValue">The existing value of object being read.</param>
            /// <param name="serializer">The calling serializer.</param>
            /// <returns>
            ///     The object value.
            /// </returns>
            public override object ReadJson(
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var holder = value as ValueHolder;
                if (holder == null)
                {
                    writer.WriteNull();
                    return;
                }

                if (holder.Type != null)
                {
                    serializer.Serialize(writer, holder.Value, holder.Type);
                }
                else
                {
                    serializer.Serialize(writer, holder.Value);
                }
            }
        }

        #endregion
    }
}