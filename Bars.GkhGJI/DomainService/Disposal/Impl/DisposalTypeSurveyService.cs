namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4.Modules.Analytics.Utils;
    using Bars.B4;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class DisposalTypeSurveyService : IDisposalTypeSurveyService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<DisposalTypeSurvey> TypeSurveyDomain { get; set; }

	    public IDataResult AddTypeSurveys(BaseParams baseParams)
	    {
		    var documentId = baseParams.Params.GetAs<long>("documentId");
		    var typeIds = baseParams.Params.GetAs<string>("typeIds").ToLongArray();
		    var result = AddTypeSurveys(documentId, typeIds);

		    return result;
	    }

	    public IDataResult AddTypeSurveys(long documentId, long[] ids)
	    {
		    try
		    {
			    var existingIds = TypeSurveyDomain.GetAll()
				    .Where(x => x.Disposal.Id == documentId)
				    .Select(x => x.TypeSurvey.Id)
				    .ToArray();

			    foreach (var id in ids.Distinct())
			    {
				    if (!existingIds.Contains(id))
				    {
					    var newObj = new DisposalTypeSurvey
					    {
						    Disposal = new Disposal {Id = documentId},
						    TypeSurvey = new TypeSurveyGji {Id = id}
					    };

					    TypeSurveyDomain.Save(newObj);
				    }
			    }
			    return new BaseDataResult();
		    }
		    catch (ValidationException e)
		    {
			    return new BaseDataResult {Success = false, Message = e.Message};
		    }
	    }
    }
}