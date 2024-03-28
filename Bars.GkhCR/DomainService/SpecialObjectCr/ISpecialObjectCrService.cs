namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using Bars.B4;

    using Entities;

    /// <summary>
    /// Сервис для работы с <see cref="SpecialObjectCr"/>
    /// </summary>
    public interface ISpecialObjectCrService
    {
        /// <summary>
        /// Получить отфильтрованный по Оператору список
        /// </summary>
        /// <returns></returns>
        IQueryable<ViewSpecialObjectCr> GetFilteredByOperator();

        /// <summary>
        /// Получить подрядчиков
        /// </summary>
        IDataResult GetBuilders(BaseParams baseParams);

        /// <summary>
        /// Восстановить
        /// </summary>
        IDataResult Recover(BaseParams baseParams);

        /// <summary>
        /// Получить дополнительные параметры
        /// </summary>
        IDataResult GetAdditionalParams(BaseParams baseParams);
    }
}