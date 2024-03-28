namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface IOperatorService
    {
        IDataResult GetActiveOperatorId();

        IDataResult GetProfile();

        IDataResult GetActiveOperator();

        IDataResult ChangeProfile(BaseParams baseParams);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult AddInspectors(BaseParams baseParams);

        IDataResult AddMunicipalities(BaseParams baseParams);

        IDataResult AddContragents(BaseParams baseParams);

        IDataResult ListContragent(BaseParams baseParams);

        IDataResult ListMunicipality(BaseParams baseParams);

        IDataResult ListInspector(BaseParams baseParams);
                
        IDataResult GenerateNewPassword(BaseParams baseParams);
    }
}