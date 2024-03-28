namespace Bars.Gkh.DomainService
{
    using System.Collections;

    using Bars.B4;

    public interface ILocalGovernmentService
    {
        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);

        IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging);
    }
}