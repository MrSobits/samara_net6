namespace Bars.Gkh.Overhaul.DomainService
{
    using System.Collections;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Entities;

    public interface IWorkPriceService<T>
        where T : WorkPrice
    {
        /// <summary>
        /// Получить список уникальных годов по муниципальным образованиям
        /// </summary>
        IDataResult YearList(BaseParams baseParams);

        /// <summary>
        /// Возвращает уникальный список муниципалов
        /// </summary>
        IDataResult MunicipalityList(BaseParams baseParams);

        IDataResult DoMassAddition(BaseParams baseParams);

        /// <summary>
        /// Возвращает список расценок заданного МО
        /// </summary>
        IDataResult ListByFromMunicipality(BaseParams baseParams);

        /// <summary>
        /// Возвращает список расценок заданного МО
        /// </summary>
        IDataResult ListByToMunicipality(BaseParams baseParams);

        IDataResult AddWorkPricesByMunicipality(BaseParams baseParams);

        IList GetListView(BaseParams baseParams, bool paging, ref int totalCount);
    }
}