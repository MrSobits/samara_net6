namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using Entities;

    public class AppealCitsAnswerViewModel : BaseViewModel<AppealCitsAnswer>
    {
        public override IDataResult List(IDomainService<AppealCitsAnswer> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var data = domainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    DocumentDate = x.DocumentDate != DateTime.MinValue ? x.DocumentDate : null,
                    x.DocumentNumber,
                    Addressee = x.Addressee.Name,
                    x.File,
                    x.IsUploaded,
                    x.AdditionalInfo,
                    x.IsMoved,
                    x.State,
                    x.IsSent
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}