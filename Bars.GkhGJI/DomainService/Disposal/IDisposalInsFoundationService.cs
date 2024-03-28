namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalInsFoundationService
	{
        IDataResult AddInspFoundations(BaseParams baseParams);
        IDataResult AddInspFoundations(long documentId, long[] ids);
    }
}