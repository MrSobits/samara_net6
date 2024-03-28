namespace Bars.GkhCr.DomainService
{
    using B4;

    /// <summary>
    /// Сервис договоров на услуги
    /// </summary>
    public interface IContractCrService
    {
        /// <summary>
        /// Добавить виды работ
        /// </summary>
        IDataResult AddTypeWorks(BaseParams baseParams);
    }
}
