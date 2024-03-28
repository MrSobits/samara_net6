namespace Bars.GkhRf.DomainService
{
    using Bars.B4;

    public interface ITransferFundsRfService
    {
        IDataResult AddTransferFundsObjects(BaseParams baseParams);

        IDataResult ListPersonalAccount(BaseParams baseParams);
    }
}