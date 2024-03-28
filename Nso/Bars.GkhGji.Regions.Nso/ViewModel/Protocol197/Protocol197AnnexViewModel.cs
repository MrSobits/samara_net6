namespace Bars.GkhGji.Regions.Nso.ViewModel
{
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Regions.Nso.Entities;
	using System.Linq;

	public class Protocol197AnnexViewModel : BaseViewModel<Protocol197Annex>
    {
        public override IDataResult List(IDomainService<Protocol197Annex> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
				.Where(x => x.Protocol197.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.File
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}