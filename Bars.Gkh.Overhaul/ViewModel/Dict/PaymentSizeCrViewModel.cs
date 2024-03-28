namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Gkh.Utils;

    public class PaymentSizeCrViewModel : BaseViewModel<PaymentSizeCr>
    {
        #region Dependency injection members

        public IDomainService<PaymentSizeMuRecord> PaymentSizeMuRecordDomain { get; set; }

        #endregion

        public override IDataResult List(IDomainService<PaymentSizeCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var dictMunicipalityCount =
                PaymentSizeMuRecordDomain.GetAll()
                    .Select(x => new
                    {
                        PaySizeId = x.PaymentSizeCr.Id,
                        MuId = x.Municipality.Id,
                        MunicipalityName = x.Municipality.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.PaySizeId)
                    .ToDictionary(x => x.Key, y => y.Select(x => new {x.MuId, x.MunicipalityName}));

            // Сначала получаем временные данные, так как получить сразу MunicipalityCount почему-то у меня так и не вышло, даже если после
            //  GetAll() сделать ToList()
            var data =
                domainService.GetAll()
                    .Select(x => new
                    {
                        x.Id, 
                        x.PaymentSize, 
                        x.DateStartPeriod, 
                        x.DateEndPeriod,
                        x.TypeIndicator,
                        MunicipalityCount = 0,
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.TypeIndicator,
                        x.PaymentSize,
                        x.DateStartPeriod,
                        x.DateEndPeriod,
                        MunicipalityCount = dictMunicipalityCount.ContainsKey(x.Id) ? dictMunicipalityCount[x.Id].Count() : 0,
                        MunicipalityNames = dictMunicipalityCount.ContainsKey(x.Id) ? dictMunicipalityCount[x.Id].Select(z => z.MunicipalityName).AggregateWithSeparator(", "): ""
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
