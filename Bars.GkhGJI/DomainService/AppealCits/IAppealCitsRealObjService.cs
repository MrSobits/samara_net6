namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IAppealCitsRealObjService
    {
        IDataResult AddRealityObjects(BaseParams baseParams);

        IDataResult AddStatementRealityObjects(BaseParams baseParams);

        IDataResult GetRealityObjects(BaseParams baseParams);

        IDataResult GetJurOrgs(BaseParams baseParams);
    }
}
