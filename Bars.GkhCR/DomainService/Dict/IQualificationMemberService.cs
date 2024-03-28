namespace Bars.GkhCr.DomainService 
{
    using B4;

    public interface IQualificationMemberService
    {
        IDataResult GetInfo(BaseParams baseParams);

        IDataResult AddRoles(BaseParams baseParams);

        IDataResult ListRoles(BaseParams baseParams);
    }
}
