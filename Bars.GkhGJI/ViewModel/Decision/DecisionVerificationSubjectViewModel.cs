﻿namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DecisionVerificationSubjectViewModel : BaseViewModel<DecisionVerificationSubject>
    {
        public override IDataResult List(IDomainService<DecisionVerificationSubject> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

	        var data = domain.GetAll()
		        .Where(x => x.Decision.Id == documentId)
		        .Select(x => new
		        {
			        x.Id,
			        x.SurveySubject.Code,
			        x.SurveySubject.Name
				})
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
