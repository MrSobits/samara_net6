namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

    using Castle.Windsor;

	public class ActRemovalProvidedDocService : IActRemovalProvidedDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var serviceDocs = Container.Resolve<IDomainService<ActRemovalProvidedDoc>>();

            try
            {
                var documentId = baseParams.Params.ContainsKey("documentId")
                                     ? baseParams.Params["documentId"].ToLong()
                                     : 0;
                var providedDocIds = baseParams.Params.ContainsKey("providedDocIds")
                                         ? baseParams.Params["providedDocIds"].ToString()
                                         : "";

                // в этом списке будут id gпредоставляемых документов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых документов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var provIds = providedDocIds.Split(',').Select(id => id.ToLong()).ToList();

                listIds.AddRange(
                    serviceDocs.GetAll()
                               .Where(x => x.ActRemoval.Id == documentId)
                               .Select(x => x.ProvidedDoc.Id)
                               .Distinct()
                               .ToList());

                var listToSave = new List<ActRemovalProvidedDoc>();
                
                foreach (var newId in provIds)
                {

                    // Если среди существующих документов уже есть такой документ то пролетаем мимо
                    if (listIds.Contains(newId))
                    {
                        continue;
                    }

                    // Если такого эксперта еще нет то добалвяем
                    listToSave.Add(new ActRemovalProvidedDoc
                        {
                            ActRemoval = new ActRemoval { Id = documentId },
                            ProvidedDoc = new ProvidedDocGji { Id = newId }
                        });
                }

                if (listToSave.Count > 0)
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            listToSave.ForEach(serviceDocs.Save);

                            tr.Commit();
                        }
                        catch (Exception exc)
                        {
                            tr.Rollback();
                            throw exc;
                        }
                    }
                }
                
                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                Container.Release(serviceDocs);
            }
        }
    }
}