namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface IPropertyOwnerDecisionWorkService
    {
        IDataResult AddWorks(BaseParams baseParams);

        IDataResult PropertyOwnerDecisionTypeList(BaseParams baseParams);

        IDataResult MethodFormFundCrTypeList(BaseParams baseParams);
    }
}