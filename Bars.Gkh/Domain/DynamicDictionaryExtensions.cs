namespace Bars.Gkh.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Utils;

    public static class DynamicDictionaryExtensions
    {
        /// <summary>
        /// Получить идентификатор
        /// </summary>
        public static long GetAsId(this DynamicDictionary dictionary, string key = "id", bool ignorecase = true)
        {
            return key.IsEmpty() ? 0 : dictionary.GetAs<long>(key, ignoreCase: ignorecase);
        }
        
        /// <summary>
        /// Получить набор идентификаторов из параметров запроса
        /// </summary>
        /// <param name="dictionary">Словарь</param>
        /// <param name="paramName">Наименование параметра</param>
        /// <param name="ignoreCase">Игнорировать регистр символов</param>
        /// <returns>Набор уникальных значений, либо пустой массив</returns>
        public static HashSet<long> GetIdSet(this DynamicDictionary dictionary, string paramName, bool ignoreCase = true)
        {
            var obj = dictionary.GetValue(paramName, ignoreCase);
            var stringValue = obj?.ToString();

            if (string.IsNullOrEmpty(stringValue))
            {
                return new HashSet<long>();
            }

            var arrayValue = ConvertHelper.ConvertTo<long[]>(obj);
            if (arrayValue != null)
            {
                return arrayValue.ToHashSet();
            }

            return stringValue
                .Split(StringSplitOptions.RemoveEmptyEntries, ",")
                .Select(x => long.TryParse(x, out var val) ? val : (long?) null)
                .Where(x => x.HasValue)
                .Select(x => x.Value)
                .ToHashSet();
        }

        /// <summary>
        /// Получить параметры фильтрации
        /// </summary>
        public static LoadParam GetLoadParam(this DynamicDictionary dictionary)
        {
            return dictionary.Read<LoadParam>().Execute(B4.DomainService.BaseParams.Converter.ToLoadParam);
        }

        /// <summary>
        /// Получить параметры фильтрации
        /// </summary>
        public static LoadParam GetLoadParam(this DynamicDictionary dictionary, string loadParamName)
        {
            return dictionary.GetAs<DynamicDictionary>(loadParamName).GetLoadParam();
        }
    }
}