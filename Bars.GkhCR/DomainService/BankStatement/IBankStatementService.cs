namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using Entities;

    public interface IBankStatementService
    {
        IQueryable<BankStatement> GetFilteredByOperator();
    }
}