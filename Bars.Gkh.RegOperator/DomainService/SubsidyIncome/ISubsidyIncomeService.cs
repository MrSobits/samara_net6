namespace Bars.Gkh.RegOperator.DomainService
{
    using B4;

    public interface ISubsidyIncomeService
    {
        IDataResult Apply(BaseParams baseParams);

        IDataResult Undo(BaseParams baseParams);

        IDataResult CheckDate(BaseParams baseParams);
    }
}