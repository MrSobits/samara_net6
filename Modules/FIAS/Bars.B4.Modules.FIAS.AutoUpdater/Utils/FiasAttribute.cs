namespace Bars.B4.Modules.FIAS.AutoUpdater.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Utils;

    using NHibernate.Loader.Custom;

    public static class FiasAttribute
    {
        public static Type GetTypesWithFiasEntityNameAttribute(Assembly assembly, string value)
        {
            foreach(Type type in assembly.GetTypes())
            {
                var attribute = type.GetCustomAttributes(typeof(FiasEntityNameAttribute), true).FirstOrDefault() as FiasEntityNameAttribute;
                if (attribute!= null && attribute.EntityName == value) 
                {
                    return type;
                }
            }

            throw new Exception($"Ошибка обновления словаря ФИАС: тип с со значением аттрибута FiasEntityName = {value} не найден");
        }
    }
}