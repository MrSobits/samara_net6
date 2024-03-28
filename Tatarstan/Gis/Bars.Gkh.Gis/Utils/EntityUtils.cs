namespace Bars.Gkh.Gis.Utils
{
    using System;

    /// <summary>
    /// Класс утилит для любой сущности
    /// </summary>
    public static class EntityUtils
    {
        /// <summary>
        /// Копировать Свойства верхнего класса
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="sourceEntity">Объект источник</param>
        /// <param name="retEntity">Результирующий объект</param>
        public static void CopyEntityProperties<T>(T sourceEntity, out T retEntity)
        {
            retEntity = Activator.CreateInstance<T>();

            var propertiesList =
                typeof(T).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance |
                                         System.Reflection.BindingFlags.DeclaredOnly);

            //получить свойства верхнего класса
            foreach (var propInfo in propertiesList)
            {
                typeof(T).GetProperty(propInfo.Name).SetValue(retEntity, propInfo.GetValue(sourceEntity, null), null);
            }
        }
    }
}
