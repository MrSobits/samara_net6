namespace Bars.GkhGji.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalSurveyObjectiveService : IDisposalSurveyObjectiveService
	{
		public IDomainService<DisposalSurveyObjective> Domain { get; set; }

		public IDomainService<SurveyObjective> SurveyObjectiveDomain { get; set; }

		public IDataResult AddSurveyObjectives(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
			var result = this.AddSurveyObjectives(documentId, ids);

			return result;
		}

		public IDataResult AddSurveyObjectives(long documentId, long[] ids)
		{
			try
			{
				var extingIds = this.Domain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Select(x => x.SurveyObjective.Id)
					.ToArray();

				foreach (var id in ids.Distinct())
				{
					if (!extingIds.Contains(id))
					{
						var objName = SurveyObjectiveDomain.Get(id).Name;

						var newObj = new DisposalSurveyObjective
						{
							Disposal = new Disposal { Id = documentId },
							SurveyObjective = new SurveyObjective { Id = id },
							Description = objName
						};

						this.Domain.Save(newObj);
					}
				}
				return new BaseDataResult();
			}
			catch (ValidationException e)
			{
				return new BaseDataResult { Success = false, Message = e.Message };
			}
		}
	}
}