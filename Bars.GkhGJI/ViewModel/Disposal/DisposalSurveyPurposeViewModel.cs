namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalSurveyPurposeViewModel : BaseViewModel<DisposalSurveyPurpose>
    {
        public override IDataResult List(IDomainService<DisposalSurveyPurpose> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

	        var data = domain.GetAll()
		        .Where(x => x.Disposal.Id == documentId)
		        .Select(x => new
		        {
			        x.Id,
			        x.SurveyPurpose.Code,
			        x.SurveyPurpose.Name,
					x.Description
		        })
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
