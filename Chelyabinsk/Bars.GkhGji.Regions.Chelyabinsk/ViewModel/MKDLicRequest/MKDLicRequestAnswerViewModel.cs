namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System;
    using System.Linq;

    public class MKDLicRequestAnswerViewModel : BaseViewModel<MKDLicRequestAnswer>
    {
        public override IDataResult List(IDomainService<MKDLicRequestAnswer> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var requestId = baseParams.Params.GetAs<long>("mkdlicrequestId");

            var data = domainService.GetAll()
                .Where(x => x.MKDLicRequest.Id == requestId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentName,
                    DocumentDate = x.DocumentDate != DateTime.MinValue ? x.DocumentDate : null,
                    x.DocumentNumber,
                    Addressee = x.Addressee.Name,
                    x.File,
                    x.TypeAppealAnswer,
                    x.IsUploaded,
                    x.SerialNumber,
                    x.AdditionalInfo,
                    x.FileDoc,
                    x.IsMoved,
                    x.Description,
                    x.State,
                    x.TypeAppealFinalAnswer,
                    Executor = x.Executor.Fio,
                    x.Address
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}