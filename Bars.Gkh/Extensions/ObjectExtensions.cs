namespace Bars.Gkh.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    using AutoMapper;

    using Fasterflect;

    using FastMember;
    using Newtonsoft.Json;

    /// <summary>
    /// Утилитные методы для работы с любым объектом
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="source">Объект</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <returns>Значение свойства. Если объект == null, то null</returns>
        public static object GetValue(this object source, string propertyName)
        {
            if (source == null) return null;

            var accessor = TypeAccessor.Create(source.GetType());
            return accessor[source, propertyName];
        }

        /// <summary>
        /// Получить json представление объекта
        /// </summary>
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Метод проверяет, является ли объект <paramref name="value"/> равным значению по умолчанию для заданного типа
        /// </summary>
        public static bool IsDefault<T>(this T value)
        {
            return value.Equals(default(T));
        }

        /// <summary>
        /// Метод проверяет, является ли объект <paramref name="value"/> не равным значению по умолчанию для заданного типа
        /// </summary>
        public static bool IsNotDefault<T>(this T value)
        {
            return !value.IsDefault();
        }
        
        /// <summary>
        /// Скопировать идентичные свойства
        /// </summary>
        public static TResult DeepCopy<TResult>(this TResult value) where TResult : new()
        {
            var res = new TResult();

            foreach (var propertyInfo in typeof(TResult).GetProperties())
            {
                propertyInfo.Set(res, propertyInfo.Get(value));
            }

            return res;
        }

        /// <summary>
        /// Скопировать идентичные свойства
        /// </summary>
        public static TResult DeepCopy<TSource, TResult>(this TSource value)
            where TSource : TResult
            where TResult : new()
        {
            var res = new TResult();

            foreach (var propertyInfo in typeof(TResult).GetProperties())
            {
                propertyInfo.Set(res, propertyInfo.Get(value));
            }

            return res;
        }

        /// <summary>
        /// Преобразовать объект в ExpandoObject
        /// </summary>
        public static ExpandoObject ToExpandoObject(this object value)
        {
            var res = new ExpandoObject() as IDictionary<string, object>;

            foreach (var property in value.GetType().GetProperties())
            {
                res[property.Name] = property.GetValue(value);
            }

            return (ExpandoObject)res;
        }

        /// <summary>
        /// Скопировать идентичные свойства
        /// </summary>
        public static TResult CopyIdenticalProperties<TResult>(this object value)
        {
            var currentValueType = value.GetType();
            var mapper = new Mapper(new MapperConfiguration(x => x.CreateMap(currentValueType, typeof(TResult))));
            return (TResult)mapper.Map(value, currentValueType, typeof(TResult));
        }

        /// <summary>
        /// Скопировать идентичные свойства
        /// </summary>
        public static TResult CopyIdenticalProperties<TEntity, TResult>(this object value)
        {
            var mapper = new Mapper(new MapperConfiguration(x => x.CreateMap(typeof(TEntity), typeof(TResult))));
            return (TResult)mapper.Map(value, typeof(TEntity), typeof(TResult));
        }
    }
}