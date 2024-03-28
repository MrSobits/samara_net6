namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.B4;

    public interface IRegionSpecificService
    {
        IDataResult GetAppealCitizenResponder(BaseParams baseParams);

        IDataResult CreateNewBaseStatment(BaseParams baseParams);

        IDataResult CreateAnswerAddressee(BaseParams baseParams);
    }
}