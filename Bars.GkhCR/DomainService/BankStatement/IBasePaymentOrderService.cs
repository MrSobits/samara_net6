namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using Entities;

    public interface IBasePaymentOrderService
    {
        IQueryable<BasePaymentOrder> GetFilteredByOperator();
    }
}