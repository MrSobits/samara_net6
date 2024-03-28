namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;
    
    public class DocumentGjiInspectorService : IDocumentGjiInspectorService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddInspectors(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var serviceDocument = this.Container.Resolve<IDomainService<DocumentGji>>();
            var servReminderRule = Container.ResolveAll<IReminderRule>();
            var listInspectorsToSave = new List<DocumentGjiInspector>();

            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");
                var inspectorIds = baseParams.Params.GetAs<string>("inspectorIds").ToLongArray();

                // Признак очистки - если true, то пустой inspectorIds будет удалять всех инспекторов
                var clearingEnabled = baseParams.Params.GetAs("clearingEnabled", false);

                // В этом словаре будут существующие инспектора
                // key - идентификатор Инспектора
                // value - объект Инспектора в акте проверки
                var dictInspectors = GetInspectorsByDocumentId(documentId)
                        .GroupBy(x => x.Inspector.Id)
                        .AsEnumerable()
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                if (documentId > 0 && (clearingEnabled || inspectorIds.Any()))
                {
                    var document = serviceDocument.GetAll().FirstOrDefault(x => x.Id == documentId);

                    var order = 0;
                    // По переданным id инспекторов если их нет в списке существующих, то добавляем
                    foreach (var newId in inspectorIds)
                    {
                        // Если с таким id и order уже есть в списке то удалем его из списка и просто пролетаем дальше
                        // без добавления в БД
                        if (dictInspectors.ContainsKey(newId))
                        {
                            //Если инспектор есть, но у него другой порядковый номер - то обновляем номер
                            if (dictInspectors[newId].Order != order)
                            {
                                var inspectorToUpdate = dictInspectors[newId];
                                inspectorToUpdate.Order = order;
                                listInspectorsToSave.Add(inspectorToUpdate);
                            }
                            dictInspectors.Remove(newId);
                            order++;
                            continue;
                        }

                        if (newId > 0)
                        {
                            var newObj = new DocumentGjiInspector
                            {
                                DocumentGji = document,
                                Inspector = new Inspector { Id = newId },
                                Order = order
                            };

                            listInspectorsToSave.Add(newObj);    
                        }
                        order++;

                    }

                    using (var transaction = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var item in listInspectorsToSave)
                            {
                                service.Save(item);
                            }

                            // Если какието инспектора остались в dictInspectors то их удаляем
                            // поскольку среди переданных inspectorIds их небыло, но в БД они остались
                            foreach (var keyValue in dictInspectors)
                            {
                                service.Delete(keyValue.Value.Id);
                            }

                            // Поскольку создание Инспектором могло повлиять на Напоминания по Распоряжениям или предписаниям,
                            // то запускаем метод создания напоминаний
                            if (document != null && (document.TypeDocumentGji == TypeDocumentGji.Disposal
                                || document.TypeDocumentGji == TypeDocumentGji.Prescription))
                            {
                                var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");
                                if (rule != null)
                                {
                                    rule.Create(document);
                                }
                            }

                            transaction.Commit();
                            return new BaseDataResult();
                        }
                        catch (ValidationException e)
                        {
                            transaction.Rollback();
                            return new BaseDataResult { Success = false, Message = e.Message };
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                
                return new BaseDataResult();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(service);
                Container.Release(serviceDocument);
                Container.Release(servReminderRule);
            }
        }

        public IQueryable<DocumentGjiInspector> GetInspectorsByDocumentId(long? documentId)
        {
            var service = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            try
            {
                return service.GetAll().Where(x => x.DocumentGji.Id == documentId).OrderBy(x => x.Order);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}