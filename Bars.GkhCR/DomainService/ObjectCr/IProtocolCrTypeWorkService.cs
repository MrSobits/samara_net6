namespace Bars.GkhCr.DomainService
{
    using B4;

    public interface IProtocolCrTypeWorkService
    {
        IDataResult ListProtocolCrTypeWork(BaseParams baseParams);

        IDataResult AddTypeWorks(BaseParams baseParams);
    }
}