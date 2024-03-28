namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    public interface IConfirmContributionService
    {
        IDataResult ManagOrgsList(BaseParams baseParams);

        IDataResult RealObjList(BaseParams baseParams);
    }
}