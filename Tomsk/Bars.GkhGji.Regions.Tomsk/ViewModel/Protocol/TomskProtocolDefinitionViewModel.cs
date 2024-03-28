namespace Bars.GkhGji.Regions.Tomsk.ViewModel.Protocol
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.ViewModel;

    public class TomskProtocolDefinitionViewModel : ProtocolDefinitionViewModel<TomskProtocolDefinition>
    {
        public override IDataResult Get(IDomainService<TomskProtocolDefinition> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            return new BaseDataResult(
                domainService.GetAll()
                .Where(x => x.Id == id)
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
                    x.DateOfProceedings,
                    x.PlaceReview
                })
                .FirstOrDefault());
        }
    }
}
