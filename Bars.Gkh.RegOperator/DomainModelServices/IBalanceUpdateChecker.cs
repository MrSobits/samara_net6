namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using B4.Modules.Quartz;

    public interface IBalanceUpdateChecker : ITask
    {
        void PerformUpdate();
    }
}