namespace Bars.GkhDi.DomainService
{
    using B4;

    public interface IGroupDiService
    {
        IDataResult GetGroupActions(BaseParams baseParams);
    }
}