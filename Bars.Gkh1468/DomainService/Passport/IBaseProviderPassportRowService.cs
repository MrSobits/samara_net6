namespace Bars.Gkh1468.DomainService
{
    using B4;

    public interface IBaseProviderPassportRowService<T> where T : BaseProviderPassportRow
    {
        /// <summary>
        ///     Возвращает значения группового множественного аттруибута.
        ///     Вида данные по метааттрибутам, кол-во блоков указзаного метааттрибута.
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetMultyMetaValues(BaseParams baseParams);

        IDataResult GetMetaValues(BaseParams baseParams);

        IDataResult GetMultyMetaValue(BaseParams baseParams);

        IDataResult SaveRecord(BaseParams baseParams);

        IDataResult DeleteMultyMetaValues(BaseParams baseParams);

        void UpdateFillPercent(long providerPassportId);
    }
}