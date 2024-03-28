namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class ManOrgRequestProvDocViewModel : BaseViewModel<ManOrgRequestProvDoc>
    {

        public override IDataResult List(IDomainService<ManOrgRequestProvDoc> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = loadParams.Filter.GetAs("requestId", 0L);

           var data = domain.GetAll()
               .Where(x => x.LicRequest.Id == requestId)
               .Select(x => new
               {
                   x.Id,
                   LicProvidedDoc = x.LicProvidedDoc.Name,
                   x.Number,
                   x.SignedInfo,
                   x.Date,
                   x.File
               })
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}