namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Bars.B4.Utils;
    using Entities;

    public class ResolutionDecisionViewModel : BaseViewModel<ResolutionDecision>
    {
        public override IDataResult List(IDomainService<ResolutionDecision> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;


                var data = domainService.GetAll()
                     .Where(x => x.Resolution.Id == documentId)
                    .Select(x => new
                    {
                        x.Id,
                        x.DocumentName,
                        x.Apellant,
                        x.AppealNumber,
                        x.AppealDate,
                        x.TypeDecisionAnswer
                    })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
           
     
        }
    }
}