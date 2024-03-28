namespace Bars.GkhGji.Regions.Smolensk.ViewModel.Protocol
{
    using System.Linq;
    using B4;

    using Bars.GkhGji.ViewModel;

    using Entities.Protocol;
    using Gkh.Domain;

    public class ProtocolDefinitionSmolViewModel : ProtocolDefinitionViewModel<ProtocolDefinitionSmol>
    {
        public override IDataResult List(IDomainService<ProtocolDefinitionSmol> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAsId("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.ExecutionDate,
                    IssuedDefinition = x.IssuedDefinition.Fio,
                    x.Description,
                    x.TypeDefinition,
                    TimeDefinition = x.TimeDefinition.HasValue ? x.TimeDefinition.Value.ToShortTimeString() : "",
                    x.DateOfProceedings
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }

        public override IDataResult Get(IDomainService<ProtocolDefinitionSmol> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId("id");

            return new BaseDataResult(
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.Id,
                        Protocol = x.Protocol.Id,
                        x.DocumentDate,
                        x.DocumentNum,
                        x.ExecutionDate,
                        IssuedDefinition = x.IssuedDefinition.Fio,
                        x.Description,
                        x.TypeDefinition,
                        TimeDefinition = x.TimeDefinition.HasValue ? x.TimeDefinition.Value.ToShortTimeString() : "",
                        x.DateOfProceedings,
                        x.DefinitionResult,
                        x.DescriptionSet,
                        x.ExtendUntil
                    })
                    .FirstOrDefault());
        }
    }
}
