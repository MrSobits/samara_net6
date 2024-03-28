namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class BuilderProductionBaseViewModel : BaseViewModel<BuilderProductionBase>
    {
        public override IDataResult List(IDomainService<BuilderProductionBase> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var builderId = baseParams.Params.GetAs<long>("builderId");

            var data = domain.GetAll()
                .Where(x => x.Builder.Id == builderId)
                .Select(x => new
                {
                    x.Id,
                    KindEquipment = x.KindEquipment.Name,
                    x.Notation,
                    x.Volume,
                    x.DocumentRight
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}