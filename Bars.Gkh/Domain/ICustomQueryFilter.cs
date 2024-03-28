namespace Bars.Gkh.Domain
{
    using System.Linq;
    using B4;

    /// <summary>
    /// Интерфейс фильтрации коллекции сущностей 
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    public interface ICustomQueryFilter<T>
    {
        IQueryable<T> Filter(IQueryable<T> source, BaseParams baseParams);
    }
}