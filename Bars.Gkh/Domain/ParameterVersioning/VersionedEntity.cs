namespace Bars.Gkh.Domain.ParameterVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    /// <summary>
    /// Класс для регистрации маппинга сущностей на параметр.
    /// Например параметр base_tariff в разное время может зависеть от разных сущностей
    /// </summary>
    public abstract class VersionedEntity<T> : IVersionedEntity
        where T : IEntity
    {
        private readonly string className;

        private readonly HashSet<ParameterEntity> map;

        private readonly string parameterName;

        private readonly Dictionary<string, Func<T, bool>> skipper;

        private readonly Type type;

        protected VersionedEntity()
            : this(null)
        {
        }

        protected VersionedEntity(string parameterName)
        {
            this.parameterName = parameterName;
            this.map = new HashSet<ParameterEntity>();
            this.type = typeof(T);
            this.className = this.type.Name;
            this.skipper = new Dictionary<string, Func<T, bool>>();
        }

        /// <inheritdoc />
        public ReadOnlyCollection<ParameterEntity> GetParameterMap()
        {
            return this.map.ToList().AsReadOnly();
        }

        /// <inheritdoc />
        public bool SkipProperty(object entity, string parameterName)
        {
            if (this.skipper.ContainsKey(parameterName))
            {
                return this.skipper[parameterName](entity.To<T>());
            }

            return false;
        }

        /// <inheritdoc />
        public IDataResult Validate(object entity, object value, DateTime factDate, string parameterName)
        {
            if (this.CanValidate(parameterName, entity))
            {
                return this.ValidateInternal(entity, value, factDate, parameterName);
            }

            return new BaseDataResult(true);
        }

        /// <summary>
        /// Маппинг сущности на параметр
        /// </summary>
        /// <typeparam name="TProp">Поле</typeparam>
        /// <param name="memberExpr">Выражение для получения поля</param>
        /// <param name="parameterName">Наименование параметра</param>
        /// <param name="friendlyName">Отображаемое имя</param>
        /// <param name="shouldSkip">Условие, по которому параметр должен версионироваться</param>
        /// <param name="renderer">Рендерер</param>
        public void Map<TProp>(
            Expression<Func<T, TProp>> memberExpr,
            string parameterName,
            string friendlyName,
            Func<T, bool> shouldSkip = null,
            Func<string, string> renderer = null)
        {
            var memberName = memberExpr.MemberName();
            var paramName = string.IsNullOrWhiteSpace(parameterName) ? this.parameterName : parameterName;
            this.map.Add(
                new ParameterEntity
                    {
                        Type = this.type,
                        ClassName = this.className,
                        ParameterName = paramName,
                        PropertyName = memberName,
                        FriendlyName = friendlyName,
                        Renderer = renderer ?? this.CreateDefaultRenderer<TProp>()
                    });

            if (shouldSkip != null)
            {
                this.skipper.Add(paramName, shouldSkip);
            }
        }

        protected virtual Func<string, string> CreateDefaultRenderer<TProp>()
        {
            var type = typeof(TProp);
            type = Nullable.GetUnderlyingType(type) ?? type;

            if (type.IsEnum)
            {
                return v =>
                    {
                        try
                        {
                            var parsed = (TProp)Enum.Parse(type, v, true);
                            var name = Enum.GetName(type, parsed);
                            return type.GetField(name).GetDisplayName(name);
                        }
                        catch
                        {
                            return v;
                        }
                    };
            }

            return v => v;
        }

        protected virtual bool CanValidate(string parameterName, object entity)
        {
            return false;
        }

        protected virtual IDataResult ValidateInternal(
            object entity, 
            object value, 
            DateTime factDate, 
            string parameterName)
        {
            return new BaseDataResult(true);
        }
    }

    public class ParameterEntity
    {
        public string ClassName { get; set; }

        public string FriendlyName { get; set; }

        public string ParameterName { get; set; }

        public string PropertyName { get; set; }

        public string PropertyValue { get; set; }

        public Type Type { get; set; }

        public Func<string, string> Renderer { get; set; }
    }
}