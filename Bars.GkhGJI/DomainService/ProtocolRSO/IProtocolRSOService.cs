namespace Bars.GkhGji.DomainService
{
    using System.Collections;

    using Bars.B4;

    public interface IProtocolRSOService
    {
        IList GetViewModelList(BaseParams baseParams, bool paging, ref int totalCount);
    }
}