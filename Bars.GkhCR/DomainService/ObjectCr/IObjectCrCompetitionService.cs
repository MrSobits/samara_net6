namespace Bars.GkhCr.DomainService
{
    using Bars.B4;

    public interface IObjectCrCompetitionService
    {
        IDataResult ListCompetitions(BaseParams baseParams);
    }
}
