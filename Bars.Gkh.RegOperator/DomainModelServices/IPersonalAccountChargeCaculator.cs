namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using Bars.Gkh.Entities;

    using Entities;
    using Impl.ChargeCalculators;

    public interface IPersonalAccountChargeCaculator
    {
        ChargeResult Calculate(IPeriod period, BasePersonalAccount account, UnacceptedCharge unaccepted);
    }
}