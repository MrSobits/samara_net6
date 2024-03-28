namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    //подменяем сервис
	public class ChelyabinskDisposalProvidedDocService : IDisposalProvidedDocService
    {
		public IDomainService<ChelyabinskDisposalProvidedDoc> ChelyabinskDisposalProvidedDocDomain { get; set; }
        public IDomainService<ProvidedDocGji> ProvidedDocGjiDomain { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
			var documentId = baseParams.Params.GetAs<long>("documentId");
			var ids = baseParams.Params.GetAs<string>("providedDocIds").ToLongArray();
	        var result = this.AddProvidedDocs(documentId, ids);

	        return result;
        }

		public IDataResult AddProvidedDocs(long documentId, long[] ids)
		{
			try
			{
				var existingIds = new List<long>();

				var dictProvDocs = this.ProvidedDocGjiDomain.GetAll()
					.Where(x => ids.Contains(x.Id))
					.Select(x => new { x.Id, x.Name })
					.AsEnumerable()
					.GroupBy(x => x.Id)
					.ToDictionary(x => x.Key, y => y.Select(z => z.Name).First());

				existingIds.AddRange(this.ChelyabinskDisposalProvidedDocDomain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Select(x => x.ProvidedDoc.Id)
					.Distinct()
					.ToList());

				var max = this.ChelyabinskDisposalProvidedDocDomain.GetAll()
					.Where(x => x.Disposal.Id == documentId)
					.Max(x => (int?)x.Order) ?? -1;

				var newOrder = max + 1;

				foreach (var newId in ids.Distinct())
				{
					if (existingIds.Contains(newId))
						continue;

					var newObj = new ChelyabinskDisposalProvidedDoc
					{
						Disposal = new Disposal { Id = documentId },
						Description = (dictProvDocs.ContainsKey(newId) ? dictProvDocs[newId] : string.Empty),
						ProvidedDoc = this.ProvidedDocGjiDomain.Load(newId),
						Order = newOrder
					};

					this.ChelyabinskDisposalProvidedDocDomain.Save(newObj);
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