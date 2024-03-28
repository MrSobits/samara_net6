namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalSurveyObjectiveViewModel : BaseViewModel<DisposalSurveyObjective>
    {
        public override IDataResult List(IDomainService<DisposalSurveyObjective> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

	        var data = domain.GetAll()
		        .Where(x => x.Disposal.Id == documentId)
		        .Select(x => new
		        {
			        x.Id,
			        x.SurveyObjective.Code,
			        x.SurveyObjective.Name,
					x.Description
				})
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
