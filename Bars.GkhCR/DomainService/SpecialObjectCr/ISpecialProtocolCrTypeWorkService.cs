namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface ISpecialProtocolCrTypeWorkService
    {
        IDataResult ListProtocolCrTypeWork(BaseParams baseParams);

        IDataResult AddTypeWorks(BaseParams baseParams);
    }
}