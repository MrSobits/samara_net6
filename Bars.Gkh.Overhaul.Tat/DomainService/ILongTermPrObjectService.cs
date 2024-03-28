namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface ILongTermPrObjectService
    {
        
        IDataResult GetOrgForm(BaseParams baseParams);

        IDataResult GetManagingOrganization(BaseParams baseParams);

        IDataResult SaveDecision(BaseParams baseParams);
    }
}