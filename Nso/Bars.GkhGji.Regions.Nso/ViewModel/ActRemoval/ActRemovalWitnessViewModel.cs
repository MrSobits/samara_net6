namespace Bars.GkhGji.Regions.Nso.ViewModel
{
	using System.Linq;
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Regions.Nso.Entities;

	public class ActRemovalWitnessViewModel : BaseViewModel<ActRemovalWitness>
    {
        public override IDataResult List(IDomainService<ActRemovalWitness> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActRemoval.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Position,
                    x.IsFamiliar,
                    x.Fio
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}