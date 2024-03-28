namespace Bars.GkhGji.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с Предоставляемыми документами
    /// </summary>
    public class DisposalProvidedDocService : IDisposalProvidedDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProvidedDocGji> ProvidedDocGjiDomain { get; set; }

        /// <inheritdoc />
        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
            var providedDocIds = baseParams.Params.ContainsKey("providedDocIds") ? baseParams.Params["providedDocIds"].ToString() : "";

            var provIds = providedDocIds.Split(',').Select(id => id.ToLong()).ToArray();

            return this.AddProvidedDocs(documentId, provIds);
        }

        /// <inheritdoc />
        public IDataResult AddProvidedDocs(long documentId, long[] ids)
        {
            try
            {
                // в этом списке будут id gпредоставляемых документов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых документов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var serviceDocs = this.Container.Resolve<IDomainService<DisposalProvidedDoc>>();
                var serviceProvDocs = this.Container.Resolve<IDomainService<ProvidedDocGji>>();

                var dictProvDocs =
                    serviceProvDocs.GetAll()
                        .Where(x => ids.Contains(x.Id))
                        .Select(x => new {x.Id, x.Name})
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.Name).First());

                listIds.AddRange(
                    serviceDocs.GetAll()
                        .Where(x => x.Disposal.Id == documentId)
                        .Select(x => x.ProvidedDoc.Id)
                        .Distinct()
                        .ToList());

                foreach (var newId in ids)
                {
                    // Если среди существующих документов уже есть такой документ то пролетаем мимо
                    if (listIds.Contains(newId))
                    {
                        continue;
                    }
                   // var objName = ProvidedDocGjiDomain.Get(newId).Name;
                    // Если такого эксперта еще нет то добалвяем
                    var newObj = new DisposalProvidedDoc
                    {
                        Disposal = new Disposal {Id = documentId},
                        Description = (dictProvDocs.ContainsKey(newId) ? dictProvDocs[newId] : string.Empty),
                        ProvidedDoc = new ProvidedDocGji {Id = newId}
                    };

                    serviceDocs.Save(newObj);
                }

                this.Container.Release(serviceProvDocs);
                this.Container.Release(serviceDocs);
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Success = false, Message = e.Message};
            }
        }
    }
}