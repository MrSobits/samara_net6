namespace Bars.GkhGji.ViewModel.Dict
{
	using System.Linq;

	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Entities.Dict;
	using Bars.GkhGji.Enums;

	public class SurveySubjectViewModel : BaseViewModel<SurveySubject>
    {
		public IUserIdentity UserIdentity { get; set; }
		public IAuthorizationService AuthorizationService { get; set; }

        public override IDataResult List(IDomainService<SurveySubject> domainService, BaseParams baseParams)
        {
			var loadParams = baseParams.GetLoadParam();
            var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
            var forSelect = baseParams.Params.GetAs<bool>("forSelect");
	        var showActual = false;
	        var showNotActual = false;
	        
			if (forSelect)
	        {
				showActual = this.AuthorizationService.Grant(this.UserIdentity, "GkhGji.DocumentsGji.Disposal.Register.SurveySubject.ShowActual");
				showNotActual = this.AuthorizationService.Grant(this.UserIdentity, "GkhGji.DocumentsGji.Disposal.Register.SurveySubject.ShowNotActual");
	        }

	        var data = domainService.GetAll()
		        .Where(x => !ids.Contains(x.Id))
		        .WhereIf(forSelect, x => (showActual && x.Relevance == SurveySubjectRelevance.Actual) ||
		                                 (showNotActual && x.Relevance == SurveySubjectRelevance.NotActual))
		        .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}