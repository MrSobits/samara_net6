namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class PeriodPaymentDocumentsViewModel : BaseViewModel<PeriodPaymentDocuments>
    {
        public override IDataResult List(IDomainService<PeriodPaymentDocuments> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var periodId = baseParams.Params.GetAsId("periodid");

            var data = domainService.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Select(x => new
                {
                    x.DocumentCode,
                    PeriodName = x.Period.Name,
                    Link = "FileUpload/Download?Id=" + x.File.Id.ToString()
                }).Filter(loadParams, Container).Order(loadParams);

            return new ListDataResult(data.Paging(loadParams), data.Count());
        }
    }
}