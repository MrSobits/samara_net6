namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;

    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts.Reminder;

    using Entities;
    using Gkh.Entities;

    using Castle.Windsor;

    public class InspectionGjiInspectorService : IInspectionGjiInspectorService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddInspectors(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceInspector = Container.Resolve<IDomainService<Inspector>>();
            var servReminderRule = Container.ResolveAll<IReminderRule>();

            try
            {
                var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
                var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
                var listInspectorsToSave = new List<InspectionGjiInspector>();

                // В этом словаре будет существующие инспектора
                // key - идентификатор Инспектора
                // value - объект Инспектора в проверке
                var dictInspectors =
                    service.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => new { x.Id, InspectorId = x.Inspector.Id })
                        .AsEnumerable()
                        .GroupBy(x => x.InspectorId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                var inspection = serviceInspection.Load(inspectionId);

                // По переданным id инспекторов если их нет в списке существующих, то добавляем
                foreach (var id in objectIds)
                {
                    if (dictInspectors.ContainsKey(id))
                    {
                        // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                        // без добавления в БД
                        dictInspectors.Remove(id);
                        continue;
                    }

                    if (id > 0)
                    {
                        var newObj = new InspectionGjiInspector
                        {
                            Inspection = inspection,
                            Inspector = new Inspector { Id = id }
                        };

                        listInspectorsToSave.Add(newObj);
                    }
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Сохраняем список инспекторов
                        foreach (var item in listInspectorsToSave)
                            service.Save(item);

                        // Если какието инспектора остались в dictInspectors то их удаляем
                        // поскольку среди переданных inspectorIds их небыло, но в БД они остались
                        foreach (var keyValue in dictInspectors)
                            service.Delete(keyValue.Value);

                        //Поскольку создание Инспектором могло повлиять на Напоминания проверки то запускаем метод создания напоминаний
                        var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");
                        if (rule != null)
                        {
                            rule.Create(inspection);
                        }

                        transaction.Commit();
                        return new BaseDataResult();
                    }
                    catch (ValidationException e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult {Success = false, Message = e.Message};
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch(Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(service);
                Container.Release(serviceInspection);
                Container.Release(serviceInspector);
                Container.Release(servReminderRule);
            }
        }
    }
}