namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Castle.Windsor;
    using Config;
    using Enums;
    using B4.Utils;

    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.FormatDataExport.Enums;

    /// <summary>
    /// Методы-расширения для Enum-ов
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Получить из enum атрибут <see cref="CustomValueAttribute"/> по ключу
        /// </summary>
        /// <param name="value">Enum с указанным атрибутом</param>
        /// <param name="key">Ключ для атрибута</param>
        /// <typeparam name="T">Тип результирующего атрибута</typeparam>
        public static T GetCustomValueAttribute<T>(this Enum value, string key)
            where T : CustomValueAttribute =>
            (T) value.GetType()
                .GetMember(value.ToString())[0]
                .GetCustomAttributes(typeof (T), false)
                .FirstOrDefault(x => ((T)x).Key == key);

        /// <summary>
        /// Метод возвращает уровень МО или МР для ДПКР в зависимости от единых настроек
        /// </summary>
        /// <param name="type">Тип муниципалитета</param>
        /// <param name="container">Контейнер</param>
        /// <returns>Уровень муниципалитета</returns>
        public static MoLevel ToMoLevel(this TypeMunicipality type, IWindsorContainer container)
        {
            return type.ToMoLevel(container.Resolve<IGkhParams>().GetParams());
        }

        /// <summary>
        /// Метод возвращает уровень МО или МР для ДПКР в зависимости от единых настроек
        /// </summary>
        /// <param name="type">Тип муниципалитета</param>
        /// <param name="appParams">Параметры</param>
        /// <returns>Уровень муниципалитета</returns>
        public static MoLevel ToMoLevel(this TypeMunicipality type, DynamicDictionary appParams)
        {
            var showUrbanAreaHigh = appParams.ContainsKey("ShowUrbanAreaHigh") && appParams["ShowUrbanAreaHigh"].ToBool();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            return type == TypeMunicipality.MunicipalArea || (type == TypeMunicipality.UrbanArea && showUrbanAreaHigh && moLevel == MoLevel.MunicipalUnion)
                ? MoLevel.MunicipalUnion
                : MoLevel.Settlement;
        }

        /// <summary>
        /// Метод возвращает уровень МО или МР для типов домов в зависимости от единых настроек
        /// </summary>
        /// <param name="type">Тип муниципалитета</param>
        /// <param name="container">Контейнер</param>
        /// <returns>Уровень муниципалитета</returns>
        public static MoLevel ToRealEstTypeMoLevel(this TypeMunicipality type, IWindsorContainer container)
        {
            return type.ToRealEstTypeMoLevel(container.Resolve<IGkhParams>().GetParams());
        }

        /// <summary>
        /// Метод возвращает уровень МО или МР для типов домов в зависимости от единых настроек
        /// </summary>
        /// <param name="type">Тип муниципалитета</param>
        /// <param name="appParams">Параметры</param>
        /// <returns>Уровень муниципалитета</returns>
        public static MoLevel ToRealEstTypeMoLevel(this TypeMunicipality type, DynamicDictionary appParams)
        {
            var showUrbanAreaHigh = appParams.ContainsKey("ShowUrbanAreaHigh") && appParams["ShowUrbanAreaHigh"].ToBool();

            var moLevel = appParams.ContainsKey("RealEstTypeMoLevel") && !string.IsNullOrEmpty(appParams["RealEstTypeMoLevel"].To<string>())
                ? appParams["RealEstTypeMoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            return moLevel == MoLevel.MunicipalUnion
                ? (type == TypeMunicipality.MunicipalArea || (type == TypeMunicipality.UrbanArea && showUrbanAreaHigh))
                    ? MoLevel.MunicipalUnion
                    : MoLevel.Settlement
                : type != TypeMunicipality.MunicipalArea
                    ? MoLevel.Settlement
                    : MoLevel.MunicipalUnion;
        }

        /// <summary>
        /// Метод возвращает уровень МО или МР видов работ в зависимости от единых настроек
        /// </summary>
        /// <param name="type">Тип муниципалитета</param>
        /// <param name="container">Контейнер</param>
        /// <returns>Уровень муниципалитета</returns>
        public static WorkpriceMoLevel ToWorkpriceMoLevel(this TypeMunicipality type, IWindsorContainer container)
        {
            return type.ToWorkpriceMoLevel(container.Resolve<IGkhParams>().GetParams());
        }

        /// <summary>
        /// Метод возвращает уровень МО или МР видов работ в зависимости от единых настроек
        /// </summary>
        /// <param name="type">Тип муниципалитета</param>
        /// <param name="appParams">Параметры</param>
        /// <returns>Уровень муниципалитета</returns>
        public static WorkpriceMoLevel ToWorkpriceMoLevel(this TypeMunicipality type, DynamicDictionary appParams)
        {
            var showUrbanAreaHigh = appParams.ContainsKey("ShowUrbanAreaHigh") && appParams["ShowUrbanAreaHigh"].ToBool();

            return type == TypeMunicipality.MunicipalArea || (type == TypeMunicipality.UrbanArea && showUrbanAreaHigh)
                ? WorkpriceMoLevel.MunicipalUnion
                : WorkpriceMoLevel.Settlement;
        }

        /// <summary>
        /// Побитовая проверка флагов
        /// </summary>
        /// <returns>Истина если единичные биты совпадают</returns>
        public static bool CheckFlags(this FormatDataExportProviderFlags leftFlags, FormatDataExportProviderFlags rightFlags)
        {
            return (leftFlags & rightFlags) != 0;
        }

        /// <summary>
        /// Метод приводит строку в Enum
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="obj">Объект</param>
        /// <param name="defaultValue">Значение по умолчанию</param>
        /// <returns>Перечисление</returns>
        public static T ToEnum<T>(this string obj, T defaultValue = default (T)) where T : struct, IConvertible
        {
            if (!typeof (T).IsEnum)
            {
                throw new ArgumentException("T must be Enum");
            }

            if (string.IsNullOrEmpty(obj))
            {
                return defaultValue;
            }

            T result;

            if (Enum.TryParse(obj, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// Вернуть следующее значение Enum-а
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="src">Текущее значение</param>
        /// <returns>Следующее значение</returns>
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");
            }

            var enumValues = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf<T>(enumValues, src) + 1;
            return (enumValues.Length == j) ? enumValues[0] : enumValues[j];
        }

        /// <summary>
        /// Вернуть предыдущее значение Enum-а
        /// </summary>
        /// <typeparam name="T">Тип перечисления</typeparam>
        /// <param name="src">Текущее значение</param>
        /// <returns>Предыдущее значение</returns>
        public static T Prev<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException($"Argumnent {typeof(T).FullName} is not an Enum");
            }

            var enumValues = (T[])Enum.GetValues(src.GetType());
            var j = Array.IndexOf<T>(enumValues, src) - 1;
            return (j < 0) ? enumValues[enumValues.Length - 1] : enumValues[j];
        }


        /// <summary>
        /// Зарегистрировать Enum
        /// </summary>
        /// <param name="container">Контейнер ресурсов</param>
        /// <param name="hiddenMembers">Скрываемые поля</param>
        /// <typeparam name="TEnum">Тип перечисления</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static void RegisterGkhEnum<TEnum>(this IResourceManifestContainer container, params TEnum [] hiddenMembers )
            where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Переданный тип должен быть перечислением");
            }

            var hiddenMemberList = hiddenMembers.ToList();

            var names = Enum.GetNames(enumType);
            var valueType = Enum.GetUnderlyingType(enumType);
            var values = Enum
                .GetValues(enumType)
                .Cast<object>()
                .Select(x => ConvertHelper.ConvertTo(x, valueType))
                .ToArray();

            var enumItems = new List<EnumMemberView>(names.Length);

            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var value = values[i];

                var member = enumType.GetMember(name).Single();
                var attributes = member.GetCustomAttributes(true);

                var isHidden = hiddenMemberList.Contains((TEnum) value);

                if (isHidden)
                {
                    continue;
                }

                var display = attributes
                    .OfType<CustomValueAttribute>()
                    .FirstOrDefault(x => x.Key == "Display")
                    .Return(x => x.Value)
                    .As<string>()
                    .Or(enumType.Name);

                var description = attributes
                    .OfType<DescriptionAttribute>()
                    .FirstOrDefault()
                    .Return(x => x.Value, string.Empty)
                    .As<string>();

                if (description.IsEmpty())
                {
                    description = attributes
                        .OfType<CustomValueAttribute>()
                        .FirstOrDefault(x => x.Key == "Description")
                        .Return(x => x.Value)
                        .As<string>()
                        .Or(enumType.Name);
                }

                enumItems.Add(new EnumMemberView
                {
                    Name = name,
                    Description = description,
                    Display = display,
                    Value = value
                });
            }
            container.RegisterVirtualExtJsEnum(enumType.Name, enumItems);
        }

        /// <summary>
        /// Зарегистрировать Enum c указанием имени
        /// </summary>
        /// <param name="container">Контейнер ресурсов</param>
        /// <param name="enumName">Наименование Enum B4.enums.{0}</param>
        /// <param name="hiddenMembers">Скрываемые поля</param>
        /// <typeparam name="TEnum">Тип перечисления</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public static void RegisterGkhEnum<TEnum>(this IResourceManifestContainer container, string enumName, params TEnum[] hiddenMembers)
            where TEnum : struct
        {
            var enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Переданный тип должен быть перечислением");
            }

            var hiddenMemberList = hiddenMembers.ToList();

            var names = Enum.GetNames(enumType);
            var valueType = Enum.GetUnderlyingType(enumType);
            var values = Enum
                .GetValues(enumType)
                .Cast<object>()
                .Select(x => ConvertHelper.ConvertTo(x, valueType))
                .ToArray();

            var enumItems = new List<EnumMemberView>(names.Length);

            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var value = values[i];

                var member = enumType.GetMember(name).Single();
                var attributes = member.GetCustomAttributes(true);

                var isHidden = hiddenMemberList.Contains((TEnum)value);

                if (isHidden)
                {
                    continue;
                }

                var display = attributes
                    .OfType<CustomValueAttribute>()
                    .FirstOrDefault(x => x.Key == "Display")
                    .Return(x => x.Value)
                    .As<string>()
                    .Or(enumName);

                var description = attributes
                    .OfType<DescriptionAttribute>()
                    .FirstOrDefault()
                    .Return(x => x.Value, string.Empty)
                    .As<string>();

                if (description.IsEmpty())
                {
                    description = attributes
                        .OfType<CustomValueAttribute>()
                        .FirstOrDefault(x => x.Key == "Description")
                        .Return(x => x.Value)
                        .As<string>()
                        .Or(enumName);
                }

                enumItems.Add(new EnumMemberView
                {
                    Name = name,
                    Description = description,
                    Display = display,
                    Value = value

                });
            }
            var path = $"libs/B4/enums/{enumName.Replace(".", "/")}.js";
            var virtualEnumResource = new ExtJsVirtualEnumResource($"B4.enums.{enumName}", enumItems);
            container.Add(path, virtualEnumResource);
            container.RegisterVirtualExtJsEnum(enumName, enumItems);
        }

        /// <summary>
        /// Приводит enum к int
        /// </summary>
        public static int ToInt(this Enum enumValue, int defaultValue = default(int))
        {
            try
            {
                return enumValue == null ? defaultValue : Convert.ToInt32(enumValue);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Приводит enum к int?
        /// </summary>
        public static int? ToIntNullable(this Enum enumValue, int? defaultValue = default(int?))
        {
            try
            {
                return enumValue == null ? defaultValue : Convert.ToInt32(enumValue);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}