namespace Bars.GkhRf.Services.Impl
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Services.DataContracts;
    using Bars.GkhRf.Services.DataContracts.GetRegOpPayment;

    using Payment = Bars.GkhRf.Services.DataContracts.GetRegOpPayment.Payment;

    public partial class Service
    {
        public GetRegOpPaymentResponse GetRegOpPayment(string houseId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            long id;
            var payments = new Payment[] { };
            if (long.TryParse(houseId, out id))
            {
                payments =
                    this.Container.Resolve<IDomainService<PaymentItem>>()
                        .GetAll()
                        .Where(x => x.Payment.RealityObject.Id == id)
                        .Where(x => x.TypePayment == TypePayment.Cr)
                        .Select(
                            x => new Payment
                                     {
                                         Id = x.Id,
                                         Code = "", // TODO: Код услуги
                                         Credit = x.ChargePopulation.HasValue ? x.ChargePopulation.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         Date =
                                             x.ChargeDate.HasValue ? x.ChargeDate.Value.ToShortDateString() : null,
                                         Paid = x.PaidPopulation.HasValue ? x.PaidPopulation.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         ImpBalance = x.IncomeBalance.HasValue ? x.IncomeBalance.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         ExpBalance = x.OutgoingBalance.HasValue ? x.OutgoingBalance.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         Recalculation = x.Recalculation.HasValue ? x.Recalculation.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         TotalArea = x.TotalArea.HasValue ? x.TotalArea.Value.RoundDecimal(2).ToString(numberformat) : string.Empty,
                                         ManOrg = x.ManagingOrganization.Contragent.Name
                                     }).ToArray();
            }

            var result = payments.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new GetRegOpPaymentResponse { Payments = payments, Result = result };
        }
    }
}