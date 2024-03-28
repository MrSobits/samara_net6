namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    public interface ITaskCalendarService
    {
        IDataResult GetDays(BaseParams baseParams);
        IDataResult GetListDisposal(BaseParams baseParams);
        IDataResult GetListProtocolsGji(BaseParams baseParams);
        IDataResult GetListAppeals(BaseParams baseParams);
        IDataResult GetListSOPR(BaseParams baseParams);
        IDataResult GetListAdmonitions(BaseParams baseParams);
        IDataResult GetListPrescriptionsGji(BaseParams baseParams);
        IDataResult GetListCourtPractice(BaseParams baseParams);

        IDataResult GetListResolPros(BaseParams baseParams);
    }
}