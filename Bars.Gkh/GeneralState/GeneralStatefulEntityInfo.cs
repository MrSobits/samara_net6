namespace Bars.Gkh.GeneralState
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    
    using Bars.B4.DataModels;

    using NHibernate.Util;

    /// <summary>
    /// Описатель обобщенного состояния
    /// </summary>
    public class GeneralStatefulEntityInfo
    {
        /// <summary>
        /// Код
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// Полное имя типа
        /// </summary>
        public string EntityType { get; private set; }

        /// <summary>
        /// Свойство, которое логируется
        /// </summary>
        public PropertyInfo PropertyInfo { get; private set; }

        /// <summary>
        /// Функция форматирования значения свойства. Принимает на вход значение свойства объекта
        /// и возвращает строковое преставление журналируемого свойства
        /// </summary>
        public Func<object, string> ValueFormatter { get; private set; }

        /// <summary>
        /// Метод для создания описателя
        /// </summary>
        /// <typeparam name="TEntity">Тип логируемой сущности</typeparam>
        /// <typeparam name="TProp">Тип логируемого свойста</typeparam>
        /// <param name="propExpr">Выражение для получения свойста</param>
        /// <param name="code">Уникальный код</param>
        /// <param name="valueFormatterFunc">Функция форматирования</param>
        /// <param name="nullValueString">Значенеие, подставляемое, если свойство не заполнено</param>
        /// <returns></returns>
        public static GeneralStatefulEntityInfo Register<TEntity, TProp>(
            Expression<Func<TEntity, TProp>> propExpr,
            string code,
            Func<TProp, string> valueFormatterFunc,
            string nullValueString = "") 
            where TEntity : IHaveId
            where TProp : struct
        {
            Func<object, string> valueFormatter;        
           
            if ((typeof(TProp)).IsNullableOrReference())
            {
                valueFormatter = x => valueFormatterFunc((TProp)x);
            }
            else
            {
                var type = typeof(TProp);
                if (type.IsEnum)
                {
                    TProp value;
                    valueFormatter = x => Enum.TryParse(x.ToString(), out value)
                        ? valueFormatterFunc(value)
                        : nullValueString;
                }
                else
                {
                    valueFormatter = x => x == null ? nullValueString : valueFormatterFunc((TProp) x);
                }
            }
           
            var entity = new GeneralStatefulEntityInfo {
                ValueFormatter = valueFormatter,
                EntityType = typeof(TEntity).FullName,
                PropertyInfo = (PropertyInfo)((MemberExpression)propExpr.Body).Member,
                Code = code
            };

            return entity;
        } 
    }
}