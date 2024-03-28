namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class ManOrgRequestRPGUViewModel : BaseViewModel<ManOrgRequestRPGU>
    {

        public override IDataResult List(IDomainService<ManOrgRequestRPGU> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = loadParams.Filter.GetAs("requestId", 0L);

           var data = domain.GetAll()
               .Where(x => x.LicRequest.Id == requestId)
               .Select(x => new
               {
                   x.Id,
                   x.AnswerFile,
                   x.AnswerText,
                   x.LicRequest,
                   Date = x.ObjectCreateDate,
                   x.RequestRPGUState,
                   x.RequestRPGUType,
                   x.Text,
                   x.File
               })
               .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}