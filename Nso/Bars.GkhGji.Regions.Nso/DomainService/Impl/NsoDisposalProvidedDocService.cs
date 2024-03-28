namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
	using Bars.B4;
	using Bars.Gkh.Domain;
	using Bars.GkhGji.DomainService;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Regions.Nso.Entities.Disposal;
	using System.Collections.Generic;
	using System.Linq;

	//подменяем сервис
	public class NsoDisposalProvidedDocService : IDisposalProvidedDocService
    {
		public IDomainService<NsoDisposalProvidedDoc> NsoDisposalProvidedDocDomain { get; set; }
        public IDomainService<ProvidedDocGji> ProvidedDocGjiDomain { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<string>("providedDocIds").ToLongArray();
	        var result = AddProvidedDocs(documentId, ids);

	        return result;
        }

		public IDataResult AddProvidedDocs(long documentId, long[] ids)
		{
			try
			{
				var existingIds = new List<long>();

				var dictProvDocs = ProvidedDocGjiDomain.GetAll()
					.Where(x => ids.Contains(x.Id))
					.Select(x => new { x.Id, x.Name })
					.AsEnumerable()
					.GroupBy(x => x.Id)
					.ToDictionary(x => x.Key, y => y.Select(z => z.Name).First());

				existingIds.AddRange(NsoDisposalProvidedDocDomain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Select(x => x.ProvidedDoc.Id)
					.Distinct()
					.ToList());

				var max = NsoDisposalProvidedDocDomain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Max(x => (int?)x.Order) ?? -1;

				var newOrder = max + 1;

				foreach (var newId in ids.Distinct())
				{
					if (existingIds.Contains(newId))
						continue;

					var newObj = new NsoDisposalProvidedDoc
					{
						Disposal = new Disposal { Id = documentId },
						Description = (dictProvDocs.ContainsKey(newId) ? dictProvDocs[newId] : string.Empty),
						ProvidedDoc = ProvidedDocGjiDomain.Load(newId),
						Order = newOrder
					};

					NsoDisposalProvidedDocDomain.Save(newObj);
					newOrder++;
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