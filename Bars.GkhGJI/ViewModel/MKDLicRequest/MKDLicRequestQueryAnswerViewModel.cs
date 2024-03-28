namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;

    public class MKDLicRequestQueryAnswerViewModel : BaseViewModel<MKDLicRequestQueryAnswer>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }

        public override IDataResult List(IDomainService<MKDLicRequestQueryAnswer> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var MkdLicRequest = baseParams.Params.GetAs<long>("MkdLicRequest");
          

                var data = domainService.GetAll()
                    .Where(x => x.MKDLicRequestQuery.Id == MkdLicRequest)
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