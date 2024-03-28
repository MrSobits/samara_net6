namespace Bars.Gkh.Gis.DomainService.ImportData
{
    using B4;

    /// <summary>
    /// Загрузка данных из БД биллинга
    /// </summary>
    public interface ILoadFromBillingBasesService
    {
        /// <summary>
        /// Заполнение справочника тарифов из нижних банков биллинга
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        IDataResult FillTarifsDictionaryFromBillingBases(int year);

        /// <summary>
        /// Заполнение справочника нормативов из нижних банков биллинга
        /// </summary>
        /// <returns></returns>
        IDataResult FillNormativeStorageFromBillingBases();

        /// <summary>
        /// Заполнение справочника bil_dict_service услугами из нижних банков биллинга 
        /// </summary>
        /// <returns></returns>
        IDataResult FillServicesDictionaryFromBillingBases();


        /// <summary>
        /// Новый метод переноса данных для аналитики из нижних банков 
        /// в разрезе одного месяца
        /// </summary>
        /// <param name="yy">Расчетный год</param>
        /// <param name="mm">Расчетный месяц</param>
        /// <returns></returns>
        IDataResult GetAnalyzeDataFromBillingBases(int yy, int mm);

        /// <summary>
        /// Заполнение справочника bil_manorg_storage УО из нижних банков биллинга
        /// </summary>
        /// <returns></returns>
        IDataResult FillManOrgStorageFromBillingBases();
    }
}