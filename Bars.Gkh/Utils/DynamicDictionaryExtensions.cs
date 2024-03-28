using Bars.B4.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Utils
{
    public static class DynamicDictionaryExtensions
    {
        /// <summary>
        /// Получение значения, и попытка привести к указанному типу, включая коллекции объектов Enum
        /// Если значения получить не удалось, возвращается объект по умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key">ключ в словаре</param>
        /// <param name="defValue">Объект по умолчанию</param>
        /// <param name="separator">разделитель используется, если нужно преобразовать строку в массив</param>
        /// <returns></returns>
        public static T Get<T>(this DynamicDictionary dictionary, string key, T defValue = default, string separator = ",")
        {
            try
            {
                var outType = typeof(T);
                
                if (dictionary.TryGetValue(key, out object value))
                {
                    // если нужно получить enum по значению
                    if (outType.IsEnum)
                    {
                        var item = value.GetType().Is<string>()
                            ? Enum.Parse(outType, (string)value)
                            : Enum.ToObject(outType, value);

                        if (Enum.IsDefined(outType, item))
                            return (T)item;
                    }

                    // если нужно получить коллекцию
                    if (outType.GetInterface("IEnumerable") != null)
                    {
                        var elementType = outType.GetElementType();

                        if (elementType == null)
                            elementType = outType.GetGenericArguments().FirstOrDefault();

                        if (elementType != null)
                        {
                            if (value.GetType().Is<string>())
                            {
                                var str = (string)value;
                                value = str.Split(separator);
                            }

                            if (value.GetType().GetInterface("IEnumerable") != null)
                            {
                                var newList = new List<object>();
                                var enumMethod = value.GetType().GetMethod("GetEnumerator");
                                var ie = (IEnumerator)enumMethod.Invoke(value, null);

                                while (ie.MoveNext())
                                {
                                    // здесь парсим входную коллекцию
                                    if (elementType.IsEnum)
                                    {
                                        var item = ie.Current.GetType().Is<string>()
                                                ? Enum.Parse(elementType, (string)ie.Current)
                                                : Enum.ToObject(elementType, ie.Current);

                                        if (Enum.IsDefined(elementType, item))
                                            newList.Add(item);
                                    }
                                }

                                if (newList.Any())
                                {
                                    // инициируем итоговый объект
                                    T resultObj = (T)outType.GetConstructor(new[] { typeof(int) }).Invoke(new object[] { newList.Count() });

                                    // для добавления элементов используем или метод SetValue, или Add, смотря что есть
                                    var addMethod = outType.GetMethods()
                                       .Where(x => x.Name == "SetValue")
                                       .Where(x => x?.GetParameters().Count() == 2 && x?.GetParameters().Last().ParameterType == typeof(int))?
                                       .FirstOrDefault();

                                    var isSetValueMethod = addMethod != null;

                                    if (addMethod == null)
                                        addMethod = outType.GetMethods()
                                             .Where(x => x.Name == "Add")
                                             .FirstOrDefault();

                                    if (addMethod != null)
                                    {
                                        var i = 0;
                                        foreach (var item in newList)
                                        {
                                            addMethod.Invoke(resultObj, isSetValueMethod ? new[] { item, i } : new[] { item });
                                            i++;
                                        }
                                    }

                                    return resultObj;
                                }
                            }
                        }

                        return defValue;
                    }

                     return EnumerableExtension.Get(dictionary,key, defValue);
                }
            }
            catch
            {
                // ignored
            }

            return defValue;
        }
    }
}
