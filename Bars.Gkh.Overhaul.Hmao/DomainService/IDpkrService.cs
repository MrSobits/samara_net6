namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Impl;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public interface IDpkrService
    {
        IDataResult GetYears(BaseParams baseParams);

        IDataResult GetMunicipality(BaseParams baseParams);

        IDataResult GetRealityObjects(BaseParams baseParams);

        ListDataResult GetRecords(BaseParams baseParams);

        /// <summary>
        /// Список МО
        /// </summary>
        IDataResult MunicipalityListWithoutPaging(BaseParams baseParams);
        
        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома не попавшие в версии ДПКР
        /// </summary>
        IDataResult GetNotIncludedInCrHousesCount(BaseParams baseParams);

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Работы из основной версии ДПКР,
        /// не попавшие в опубликованную программу
        /// </summary>
        IDataResult GetWorksNotIncludedPublishProgramCount(BaseParams baseParams);

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома, у которых в Реестре жилых домов не заполнен код ФИАС
        /// </summary>
        IDataResult GetHousesWithNotFilledFias(BaseParams baseParams);

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома, включенные в ДПКР в разрезе годов
        /// </summary>
        IDataResult GetIncludedInCrHousesByYearsCount(BaseParams baseParams);

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Количество работ ДПКР в разрезе ООИ
        /// </summary>
        IDataResult GetCrCeoWorkCounts(BaseParams baseParams);

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома, с отсутствующими параметрами для расчета ДПКР
        /// </summary>
        IDataResult GetHousesWithMissingParamsCount(BaseParams baseParams);

        /// <summary>
        /// Получить количественныеданные для виджета Статистические данные КР. Данные по стоимости работ в разрезе КЭ
        /// </summary>
        IDataResult GetCostOfWorksInStructuralElementContext(BaseParams baseParams);

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Бюджетирование
        /// </summary>
        IDataResult GetCrBudgetingCount(BaseParams baseParams);

        /// <summary>
        /// Получить выгрузку Excel для виджетов
        /// </summary>
        ReportStreamResult GetExcelFileExport(BaseParams baseParams);
        
        /// <summary>
        /// Получить базовый запрос с общими условиями для объектов
        /// </summary>
        /// <param name="municipalityId">Идентификатор МО</param>
        /// <returns name="baseQuery"></returns>
        IQueryable<RealityObject> GetRealityObjectBaseQuery(long municipalityId);
        
        /// <summary>
        /// Получить базовый запрос с общими условиями для документов ДПКР, привязанных к объектам
        /// </summary>
        /// <param name="municipalityId">Идентификатор МО</param>
        /// <returns name="baseQuery"></returns>
        IQueryable<DpkrDocumentRealityObject> GetDpkrDocumentRealityObjectBaseQuery(long municipalityId);

        /// <summary>
        /// Получить запрос для выгрузки домов, не включенных в ДПКР
        /// </summary>
        DpkrService.CountQueryDto GetNotIncludedInCrHousesQuery(bool onlyCount, long municipalityId);

        /// <summary>
        /// Получить МО с учетом городского округа
        /// </summary>
        List<long> GetMunicipalityIds();
    }
}