namespace Bars.GkhGji.Regions.Khakasia.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    public class KhakasiaResolutionDefinitionViewModel : Bars.GkhGji.DomainService.ResolutionDefinitionViewModel<KhakasiaResolutionDefinition>
    {
        public override IDataResult List(IDomainService<KhakasiaResolutionDefinition> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition,
                    x.TimeStart,
                    x.TimeEnd,
                    x.FileDescription
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<KhakasiaResolutionDefinition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    Resolution = x.Resolution.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.DocumentNumber,
                    x.ExecutionDate,
                    x.IssuedDefinition,
                    x.Description,
                    x.TypeDefinition,
                    TimeStart = x.TimeStart.HasValue ? x.TimeStart.Value.ToShortTimeString() : "",
                    TimeEnd = x.TimeEnd.HasValue ? x.TimeEnd.Value.ToShortTimeString() : "",
                    x.FileDescription
                })
                .FirstOrDefault());
        }
    }
}