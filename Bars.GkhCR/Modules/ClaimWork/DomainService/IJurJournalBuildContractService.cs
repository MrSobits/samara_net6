namespace Bars.GkhCr.Modules.ClaimWork.DomainService
{
    using System.Collections;
    using B4;

    /// <summary>
    /// 
    /// </summary>
    public interface IJurJournalBuildContractService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="usePaging"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList GetList(BaseParams baseParams, bool usePaging, out int totalCount);
    }
}