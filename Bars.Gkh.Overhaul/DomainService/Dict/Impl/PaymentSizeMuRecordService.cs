using System.Linq;

namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System;
    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;

    using Gkh.Entities;
    using Entities;

    using Castle.Windsor;

    public class PaymentSizeMuRecordService : IPaymentSizeMuRecordService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddMuRecords(BaseParams baseParams)
        {
            var paymentSizeCrId = baseParams.Params.GetAs<long>("paymentSizeCrId");
            var municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");

            if (paymentSizeCrId == 0)
            {
                return new BaseDataResult { Success = false, Message = "Некорретный параметр" };
            }

            var domainService = Container.Resolve<IDomainService<PaymentSizeMuRecord>>();
            var municipalityDomainService = Container.Resolve<IDomainService<Municipality>>();
            var paysizeService = Container.Resolve<IDomainService<PaymentSizeCr>>();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var paymentSizeCr = paysizeService.Get(paymentSizeCrId);

                    if (paymentSizeCr == null)
                    {
                        return new BaseDataResult
                        {
                            Success = false,
                            Message = "Некорректная ссылка на родительский элемент"
                        };
                    }

                    //проверки на повторяющиеся МО
                    var records = domainService.GetAll()
                        .Where(x => x.PaymentSizeCr.Id != paymentSizeCr.Id
                                    && municipalityIds.Contains(x.Municipality.Id))
                        .ToList();

                    if (records.IsNotEmpty())
                    {
                        var newEnd = paymentSizeCr.DateEndPeriod.GetValueOrDefault(DateTime.MaxValue);
                        var newBegin = paymentSizeCr.DateStartPeriod.GetValueOrDefault();

                        foreach (var record in records)
                        {
                            var oldStart = record.PaymentSizeCr.DateStartPeriod;
                            var oldEnd = record.PaymentSizeCr.DateEndPeriod.GetValueOrDefault(DateTime.MaxValue);

                            if (newBegin >= oldStart && newBegin <= oldEnd
                                || newEnd >= oldStart && newEnd <= oldEnd)
                            {
                                return new BaseDataResult
                                {
                                    Success = false,
                                    Message =
                                        string.Format(
                                            "Муниципальное образование \"{0}\" уже существует в открытом периоде от {1}. Сначала закройте этот период!",
                                            record.Municipality.Name,
                                            record.PaymentSizeCr.DateStartPeriod.HasValue
                                                ? record.PaymentSizeCr.DateStartPeriod.Value.ToShortDateString()
                                                : "")
                                };
                            }
                        }
                    }

                    foreach (var id in municipalityIds)
                    {
                        var municipality = municipalityDomainService.Load(id);

                        var paymentSizeMuRecord = new PaymentSizeMuRecord
                        {
                            PaymentSizeCr = paymentSizeCr,
                            Municipality = municipality
                        };

                        domainService.Save(paymentSizeMuRecord);
                    }

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(paysizeService);
                    Container.Release(municipalityDomainService);
                    Container.Release(domainService);
                }
            }

            return new BaseDataResult();
        }
    }
}
