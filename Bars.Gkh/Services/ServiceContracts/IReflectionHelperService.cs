namespace Bars.Gkh.Services.ServiceContracts
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Интерфейс сервиса для работы с рефлексией
    /// </summary>
    public interface IReflectionHelperService
    {
        /// <summary>
        /// Получить обобщенный метод
        /// </summary>
        /// <param name="name">Наименование метода</param>
        /// <param name="resultTypes">Результирующие типы</param>
        /// <typeparam name="T">Тип метода</typeparam>
        MethodInfo GetGenericMethod<T>(string name, params Type[] resultTypes);

        /// <summary>
        /// Получить обощенный метод
        /// </summary>
        /// <param name="methodType">Тип метода</param>
        /// <param name="name">Наименование метода</param>
        /// <param name="bindingFlags">Флаги привязки</param>
        /// <param name="resultTypes">Результирующие типы</param>
        /// <returns></returns>
        MethodInfo GetGenericMethod(Type methodType, string name, BindingFlags bindingFlags, params Type[] resultTypes);
    }
}