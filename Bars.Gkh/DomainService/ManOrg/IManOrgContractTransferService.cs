namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Service для договора УК с ЖСК/ТСЖ (Управление домами)
    /// </summary>
    public interface IManOrgContractTransferService
    {
        IDataResult GetInfo(BaseParams baseParams);
    }
}