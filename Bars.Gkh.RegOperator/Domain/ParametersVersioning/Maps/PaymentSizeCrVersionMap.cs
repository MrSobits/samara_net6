namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning.Maps
{
    using Gkh.Domain.ParameterVersioning;
    using Overhaul.Entities;

    public class PaymentSizeCrVersionMap : VersionedEntity<PaymentSizeCr>
    {
        public PaymentSizeCrVersionMap() : base(VersionedParameters.BaseTariff)
        {
            Map(x => x.PaymentSize, null, "Размер взноса на КР");
        }
    }
}