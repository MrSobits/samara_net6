namespace Bars.B4.Modules.Analytics.Extensions
{
    using System;
    using System.Collections;

    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type type)
        {
            return type.IsPrimitive || type.IsAssignableFrom(typeof(decimal)) || type.IsValueType ||
                   type.IsAssignableFrom(typeof(string));
        }

        public static bool IsCollectionType(this Type type)
        {
            return type.IsArray || (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type));
        }

        public static Type GetItemType(this Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return collectionType.GetElementType();
            }

            if (typeof(IEnumerable).IsAssignableFrom(collectionType))
            {
                var genArgs = collectionType.GetGenericArguments();
                return genArgs.Length > 0 ? genArgs[0] : typeof(object);
            }

            return collectionType;
        }

    }
}
