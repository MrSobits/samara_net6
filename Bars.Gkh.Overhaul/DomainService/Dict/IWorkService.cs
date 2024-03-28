namespace Bars.Gkh.Overhaul.DomainService
{
    using B4;
    using Gkh.Entities.Dicts;

    public interface IWorkService
    {
        IDataResult SaveWithFinanceType(BaseParams baseParams, IDomainService<Work> service);
        IDataResult UpdateWithFinanceType(BaseParams baseParams, IDomainService<Work> service);
        IDataResult DeleteWithFinanceType(BaseParams baseParams, IDomainService<Work> service);
    }
}
