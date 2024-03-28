namespace Bars.GkhDi.Services.Impl
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Enums;
    using Gkh.Services.DataContracts;

    public partial class Service
    {
        public GetReductionPaymentInfoResponse GetReductionPaymentInfo(string houseId, string periodId)
        {
            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }

            var idHouse = houseId.ToLong();
            var idPeriod = periodId.ToLong();

            if (idHouse != 0 && idPeriod != 0)
            {
                var diRealObj = Container.Resolve<IDomainService<DisclosureInfoRealityObj>>()
                    .GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == idHouse && x.PeriodDi.Id == idPeriod);

                if (diRealObj == null)
                {
                    return new GetReductionPaymentInfoResponse { Result = Result.DataNotFound };
                }

                var hasReductionPayment = diRealObj.ReductionPayment.GetEnumMeta().Display;

                var reductionPayments = new List<ReductionPayment>();

                if (diRealObj.ReductionPayment == YesNoNotSet.Yes)
                {
                    reductionPayments = Container.Resolve<IDomainService<InfoAboutReductionPayment>>()
                             .GetAll()
                             .Where(x => x.DisclosureInfoRealityObj.Id == diRealObj.Id)
                             .Select(x => new ReductionPayment
                                     {
                                         Id = x.Id,
                                         Name = x.BaseService.TemplateService.Name,
                                         Reason = x.ReasonReduction,
                                         Date = x.OrderDate.HasValue ? x.OrderDate.Value.ToShortDateString() : null,
                                         Sum = x.RecalculationSum.HasValue ? x.RecalculationSum.Value.RoundDecimal(2).ToString(numberformat) : string.Empty
                                     })
                             .ToList();
                }

                return new GetReductionPaymentInfoResponse { ReductionPayments = reductionPayments.ToArray(), HasReductionPayment = hasReductionPayment, Result = Result.NoErrors };
            }

            return new GetReductionPaymentInfoResponse { Result = Result.DataNotFound };
        }
    }
}
