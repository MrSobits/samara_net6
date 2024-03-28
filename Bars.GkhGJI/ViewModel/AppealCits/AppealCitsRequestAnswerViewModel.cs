namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;

    public class AppealCitsRequestAnswerViewModel : BaseViewModel<AppealCitsRequestAnswer>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        public override IDataResult List(IDomainService<AppealCitsRequestAnswer> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appcitRequestId = baseParams.Params.GetAs<long>("appcitRequestId");
          

                var data = domainService.GetAll()
                    .Where(x => x.AppealCitsRequest.Id == appcitRequestId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentName,
                        x.DocumentNumber,
                        x.Description,
                        x.DocumentDate,
                        x.File,
                        x.SignedFile
                    })
                    .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);   
           
        }
    }
}