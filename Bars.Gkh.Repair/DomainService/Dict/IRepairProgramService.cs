namespace Bars.Gkh.Repair.DomainService 
{
    using B4;

    public interface IRepairProgramService
    {
        IDataResult ListWithoutPaging(BaseParams baseParams);

        IDataResult ListAvailableRealtyObjects(BaseParams baseParams);
    }
}
