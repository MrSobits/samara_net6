namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using Bars.B4;

    public interface ILongTermPrObjectService
    {
        IDataResult ListHasDecisionRegopAccount(BaseParams baseParams);

        IDataResult GetOrgForm(BaseParams baseParams);

        IDataResult SaveDecision(BaseParams baseParams);

        IDataResult ListAccounts(BaseParams baseParams);
    }
}