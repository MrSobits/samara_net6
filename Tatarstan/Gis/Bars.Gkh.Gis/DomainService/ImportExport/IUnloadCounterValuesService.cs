namespace Bars.Gkh.Gis.DomainService.ImportExport
{
    using B4;

    public interface IUnloadCounterValuesService
    {
        /// <summary>
        /// Выгрузить показания ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult Unload(BaseParams baseParams);

        /// <summary>
        /// Получить список выгрузок показаний ПУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetList(BaseParams baseParams);
    }
}