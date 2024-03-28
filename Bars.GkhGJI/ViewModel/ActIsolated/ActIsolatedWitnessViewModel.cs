namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActIsolatedWitnessViewModel : BaseViewModel<ActIsolatedWitness>
    {
        public override IDataResult List(IDomainService<ActIsolatedWitness> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.ActIsolated.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Position,
                    x.IsFamiliar,
                    x.Fio
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}