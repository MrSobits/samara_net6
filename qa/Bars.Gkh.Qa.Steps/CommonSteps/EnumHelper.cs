namespace Bars.Gkh.Qa.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Utils;

    using TechTalk.SpecFlow;

    public static class EnumHelper
    {
        public static IList<T> GetValues<T>(Enum value) where T : struct, IConvertible
        {
            ValidateIsEnum(typeof(T));

            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => (T)Enum.Parse(value.GetType(), fi.Name, false)).ToList();
        }

        public static T Parse<T>(string value) where T : struct, IConvertible
        {
            ValidateIsEnum(typeof(T));

            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        public static IList<string> GetDisplayValues<T>(Enum value) where T : struct, IConvertible
        {
            ValidateIsEnum(typeof(T));

            return GetNames(value).Select(obj => GetDisplayValue(Parse<T>(obj))).ToList();
        }

        public static string GetDisplayValue<T>(T value) where T : struct, IConvertible
        {
            ValidateIsEnum(typeof(T));

            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null)
            {
                return string.Empty;
            }

            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Value : value.ToString();
        }

        public static T GetFromDisplayValue<T>(string value) where T : struct, IConvertible
        {
            return (T)(object)GetFromDisplayValue(typeof(T), value);

            //ValidateIsEnum(typeof(T));

            //var fieldInfos = typeof(T).GetFields();

            //foreach (var fieldInfo in fieldInfos)
            //{
            //    var descriptionAttributes = fieldInfo.GetCustomAttributes(
            //    typeof(DisplayAttribute), false) as DisplayAttribute[];

            //    if (descriptionAttributes.Any(x => x.Value == value))
            //    {
            //        return (T)fieldInfo.GetValue(null);
            //    }
            //}

            //throw new SpecFlowException(string.Format("отсутствует значение {0} типа {1}", value, typeof(T)));
        }

        public static dynamic GetFromDisplayValue(Type enumType, string value)
        {
            ValidateIsEnum(enumType);

            var fieldInfos = enumType.GetFields();

            foreach (var fieldInfo in fieldInfos)
            {
                var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

                if (descriptionAttributes.Any(x => x.Value == value))
                {
                    return fieldInfo.GetValue(null);
                }
            }

            throw new SpecFlowException(string.Format("отсутствует значение {0} типа {1}", value, enumType));
        }

        public static void ValidateIsEnum(Type type)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumHelper может принимать T только Enum");
            }
        }
    }
}
