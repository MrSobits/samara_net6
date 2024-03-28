namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Entities;

    public interface ITakeLoanService
    {
        void TakeLoan(RealityObjectLoan loan);
    }
}