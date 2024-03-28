namespace Bars.Gkh.DomainService
{
    using System.Collections;

    using Bars.B4;

    public interface IPaymentAgentService
    {
        IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging);

        IDataResult ListForExport(BaseParams baseParams);
    }
}