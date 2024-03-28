namespace Bars.Gkh.RegOperator.Controllers.PersonalAccount
{
    using B4;
    using B4.DomainService.BaseParams;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Castle.Windsor;
    using System.Linq;
    using EnumerableExtension = Bars.B4.Utils.EnumerableExtension;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер прогесса документов на оплату
    /// </summary>
    public class PaymentDocumentLogController : BaseController
    {
        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<PaymentDocumentLog> PaymentDocumentLogDomainService { get; set; }

        /// <summary>
        /// Список информации о прогрессе доккументов на оплату
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список информации о прогрессе доккументов на оплату</returns>
        public ActionResult List(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var paymentDocumentLogQuery = this.PaymentDocumentLogDomainService.GetAll()
                .Where(x => x.Parent == null)
                .OrderByDescending(x => x.StartTime)
                .Filter(loadParams, this.Container);
            var count = paymentDocumentLogQuery.Count();
            var data = paymentDocumentLogQuery
                .Paging(loadParams)
                .ToList();

            var ids = data.Select(x => x.Id).ToList();
            var parentToCountMap = this.PaymentDocumentLogDomainService.GetAll()
                .Where(x => ids.Contains(x.Parent.Id))
                .GroupBy(x => x.Parent.Id, x => x)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Count));

            data.ForEach(x => x.Count = EnumerableExtension.Get(parentToCountMap, x.Id));

            return new JsonListResult(data, count);
        }
        
        protected LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }
    }
}
