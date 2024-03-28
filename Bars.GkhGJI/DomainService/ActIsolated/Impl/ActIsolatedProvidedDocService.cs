namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Utils;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для "Предоставленные документы акта без взаимодействия"
    /// </summary>
    public class ActIsolatedProvidedDocService : IActIsolatedProvidedDocService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить Предоставленные документы акта без взаимодействия
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var serviceDocs = this.Container.Resolve<IDomainService<ActIsolatedProvidedDoc>>();
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var providedDocIds = baseParams.Params.GetAs<string>("providedDocIds", string.Empty);

                // в этом списке будут id gпредоставляемых документов, которые уже связаны с этим распоряжением
                // (чтобы недобавлять несколько одинаковых документов в одно и тоже распоряжение)
                var listIds = new List<long>();

                var provIds = providedDocIds.ToLongArray();

                listIds.AddRange(
                    serviceDocs.GetAll()
                               .Where(x => x.ActIsolated.Id == documentId)
                               .Select(x => x.ProvidedDoc.Id)
                               .Distinct()
                               .ToList());

                var listToSave = new List<ActIsolatedProvidedDoc>();
                
                foreach (var newId in provIds)
                {
                    // Если среди существующих документов уже есть такой документ то пролетаем мимо
                    if (listIds.Contains(newId))
                    {
                        continue;
                    }
                    
                    listToSave.Add(new ActIsolatedProvidedDoc
                    {
                        ActIsolated = new ActIsolated { Id = documentId },
                        ProvidedDoc = new ProvidedDocGji { Id = newId }
                    });
                }

                if (listToSave.Count > 0)
                {
                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            listToSave.ForEach(serviceDocs.Save);
                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
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
                this.Container.Release(serviceDocs);
            }
        }
    }
}