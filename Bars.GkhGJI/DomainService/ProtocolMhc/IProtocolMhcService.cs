namespace Bars.GkhGji.DomainService
{
    using System.Collections;

    using Bars.B4;

    public interface IProtocolMhcService
    {
        IList GetViewModelList(BaseParams baseParams, bool paging, ref int totalCount);
    }
}