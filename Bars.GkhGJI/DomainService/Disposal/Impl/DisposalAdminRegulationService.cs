namespace Bars.GkhGji.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;

    public class DisposalAdminRegulationService : IDisposalAdminRegulationService
	{
		public IDomainService<DisposalAdminRegulation> Domain { get; set; }

		public IDataResult AddAdminRegulations(BaseParams baseParams)
		{
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];
			var result = this.AddAdminRegulations(documentId, ids);

			return result;
		}

		public IDataResult AddAdminRegulations(long documentId, long[] ids)
		{
			try
			{
				var extingIds = this.Domain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Select(x => x.AdminRegulation.Id)
					.ToArray();

				foreach (var id in ids.Distinct())
				{
					if (!extingIds.Contains(id))
					{
						var newObj = new DisposalAdminRegulation()
						{
							Disposal = new Disposal { Id = documentId },
							AdminRegulation = new NormativeDoc { Id = id }
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