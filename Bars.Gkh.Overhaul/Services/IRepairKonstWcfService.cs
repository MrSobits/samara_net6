namespace Bars.Gkh.Overhaul.DomainService
{
    using Bars.Gkh.Overhaul.Services.DataContracts;

    public interface IRepairKonstWcfService
    {
        RepairKonstProxy[] GetRepairKonstWcfService(long roId);
    }
}