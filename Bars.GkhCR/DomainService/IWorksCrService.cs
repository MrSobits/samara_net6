namespace Bars.GkhCr.DomainService
{
    using B4;

    /// <summary>
    /// 
    /// </summary>
    public interface IWorksCrService
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult List(BaseParams baseParams);
    }
}