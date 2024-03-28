namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalAdminRegulationService
	{
        IDataResult AddAdminRegulations(BaseParams baseParams);
        IDataResult AddAdminRegulations(long documentId, long[] ids);
    }
}