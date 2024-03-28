namespace Bars.Gkh.RegOperator.DomainService
{
    using Bars.B4;

    public interface IPaymentPenaltiesService
    {
        IDataResult AddExcludePersAccs(BaseParams baseParams);
    }
}