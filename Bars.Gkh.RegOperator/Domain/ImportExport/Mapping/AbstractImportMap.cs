namespace Bars.Gkh.RegOperator.Domain.ImportExport.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using B4.Utils;
    using Castle.Windsor;
    using Enums;

	/// <summary>
	/// Базовый класс импорт-провайдера
	/// </summary>
	/// <typeparam name="T">Обрабатываемая сущность</typeparam>
    public abstract class AbstractImportMap<T> : IImportMap
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

		/// <summary>
		/// Код провайдера
		/// </summary>
        public abstract string ProviderCode { get; }

		/// <summary>
		/// Наименование провайдера
		/// </summary>
        public abstract string ProviderName { get; }

		/// <summary>
		/// Формат
		/// </summary>
        public abstract string Format { get; }

		/// <summary>
		/// Получить наименование провайдера
		/// </summary>
		/// <returns></returns>
        public virtual string GetName()
        {
            return ProviderName;
        }

		/// <summary>
		/// Ограничение
		/// </summary>
        public virtual string PermissionKey { get { return string.Empty; } }

		/// <summary>
		/// Направление импорта
		/// </summary>
        public abstract ImportExportType Direction { get; }

		/// <summary>
		/// Конструктор
		/// </summary>
        protected AbstractImportMap()
        {
            _compiledMap = new Dictionary<string, ProviderMapper>();
        }

		/// <summary>
		/// Задать соответствие свойств
		/// </summary>
		/// <param name="member">Выражение члена соответствия</param>
		/// <param name="mapper">Маппер</param>
		/// <param name="parser">Парсер</param>
		protected void Map(Expression<Func<T, object>> member, Action<ProviderMapper> mapper, Func<object, object> parser)
        {
            var provMap = new ProviderMapper();

            if (mapper.IsNotNull())
            {
                mapper(provMap);
            }

            if (parser.IsNotNull())
            {
                provMap.SetValueParser(parser);
            }

            if (member.NodeType == ExpressionType.MemberAccess || member.NodeType == ExpressionType.Lambda)
            {
                var mInfo = member.MemberInfo();

                _compiledMap[mInfo.Name] = provMap;
            }
            else if (member.NodeType == ExpressionType.Constant && member.ReturnType == typeof(string))
            {
                var value = member.Compile()(Activator.CreateInstance<T>()).CastAs<string>();
                if (value.IsNotEmpty())
                {
                    _compiledMap[value] = provMap;
                }
            }
        }

		/// <summary>
		/// Задать соответствие свойств
		/// </summary>
		/// <param name="member">Выражение члена соответствия</param>
		/// <param name="mapper">Маппер</param>
		protected void Map(Expression<Func<T, object>> member, Action<ProviderMapper> mapper)
        {
            Map(member, mapper, 0);
        }

		/// <summary>
		/// Задать соответствие свойств
		/// </summary>
		/// <param name="member">Выражение члена соответствия</param>
		/// <param name="mapper">Маппер</param>
		/// <param name="nLength">Длина</param>
		protected void Map(Expression<Func<T, object>> member, Action<ProviderMapper> mapper, int nLength)
        {
            Map(member, mapper, nLength, 0);
        }

		/// <summary>
		/// Задать соответствие свойств
		/// </summary>
		/// <param name="member">Выражение члена соответствия</param>
		/// <param name="mapper">Маппер</param>
		/// <param name="nLength">Длина N</param>
		/// <param name="dLength">Длина D</param>
		protected void Map(Expression<Func<T, object>> member, Action<ProviderMapper> mapper, int nLength, int dLength)
        {
            var provMap = new ProviderMapper
            {
                DLength = dLength,
                NLength = nLength
            };

            if (mapper.IsNotNull())
            {
                mapper(provMap);
            }

            if (member.NodeType == ExpressionType.MemberAccess || member.NodeType == ExpressionType.Lambda)
            {
                var mInfo = member.MemberInfo();

                _compiledMap[mInfo.Name] = provMap;
            }
            else if (member.NodeType == ExpressionType.Constant && member.ReturnType == typeof(string))
            {
                var value = member.Compile()(Activator.CreateInstance<T>()).CastAs<string>();
                if (value.IsNotEmpty())
                {
                    _compiledMap[value] = provMap;
                }
            }
        }

		/// <summary>
		/// Замапить свойства автоматически
		/// </summary>
        protected void AutoMapProperties()
        {
            var pe = Expression.Parameter(typeof (T));

            var props = typeof (T).GetProperties();

            foreach (var propertyInfo in props)
            {
                var param = Expression.Convert(Expression.Property(pe, propertyInfo), typeof(object));
                var lambda = Expression.Lambda<Func<T, object>>(param, pe);

                var name = propertyInfo.Name;

                Map(lambda, m => m.SetLookuper(new Lookuper(name)));
            }
        }

		/// <summary>
		/// Тип объекта
		/// </summary>
        public Type ObjectType { get { return typeof (T); }}

		/// <summary>
		/// Получить соответствия
		/// </summary>
		/// <returns></returns>
        public Dictionary<string, ProviderMapper> GetMap()
        {
            return _compiledMap;
        }

        private readonly Dictionary<string, ProviderMapper> _compiledMap;
    }
}