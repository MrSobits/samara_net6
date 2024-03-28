namespace Bars.GkhGji.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalSurveyPurposeService : IDisposalSurveyPurposeService
	{
		public IDomainService<DisposalSurveyPurpose> Domain { get; set; }

		public IDomainService<SurveyPurpose> SurveyPurposeDomain { get; set; }

		public IDataResult AddSurveyPurposes(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
			var result = this.AddSurveyPurposes(documentId, ids);

			return result;
		}

		public IDataResult AddSurveyPurposes(long documentId, long[] ids)
		{
			try
			{
				var extingIds = Enumerable.ToArray<long>(this.Domain.GetAll()
					    .Where(x => x.Disposal.Id == documentId)
					    .Select(x => x.SurveyPurpose.Id));

				foreach (var id in ids.Distinct())
				{
					if (!extingIds.Contains(id))
					{
						var objName = SurveyPurposeDomain.Get(id).Name;

						var newObj = new DisposalSurveyPurpose
						{
							Disposal = new Disposal {Id = documentId},
							SurveyPurpose = new SurveyPurpose {Id = id},
							Description = objName
						};

						this.Domain.Save(newObj);
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