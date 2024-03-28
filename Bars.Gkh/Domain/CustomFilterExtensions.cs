namespace Bars.Gkh.Domain
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;

    public static class CustomFilterExtensions
    {
        /// <summary>
        /// Метод кастомной фильтрации коллекции
        /// </summary>
        /// <typeparam name="T">Тип сущности фильтрации</typeparam>
        /// <param name="source">Источник данных</param>
        /// <param name="container"><see cref="IWindsorContainer">Контейнер</see></param>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Отфильтрованная коллекция</returns>
        public static IQueryable<T> CustomFilter<T>(this IQueryable<T> source, IWindsorContainer container, BaseParams baseParams)
        {
            var filters = container.ResolveAll<ICustomQueryFilter<T>>();

            filters.ForEach(x => source = x.Filter(source, baseParams));

            return source;
        } 
    }
}