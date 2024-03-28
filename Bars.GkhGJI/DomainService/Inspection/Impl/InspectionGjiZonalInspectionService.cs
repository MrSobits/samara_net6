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

    /// <summary>
    /// Сервис для работы с полем "Отделы" из формы "Жилищная инспекция \ Основания проверок \ Плановые проверки юридических лиц"
    /// </summary>
    public class InspectionGjiZonalInspectionService : IInspectionGjiZonalInspectionService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавление отделов к плановой проверке юридических лиц
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult AddZonalInspections(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<InspectionGjiZonalInspection>>();
            var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceZonalInspection = Container.Resolve<IDomainService<ZonalInspection>>();
            var servReminderRule = Container.ResolveAll<IReminderRule>();

            try
            {
                var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
                var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
                var listZonalInspectionsToSave = new List<InspectionGjiZonalInspection>();

                // В этом словаре будет существующие отделы
                // key - идентификатор отдела
                // value - объект отдела в проверке
                var dictZonalInspections =
                    service.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => new { x.Id, ZonalInspectionId = x.ZonalInspection.Id })
                        .AsEnumerable()
                        .GroupBy(x => x.ZonalInspectionId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Id).FirstOrDefault());

                var inspection = serviceInspection.Load(inspectionId);

                // По переданным id отделов если их нет в списке существующих, то добавляем
                foreach (var id in objectIds)
                {
                    if (dictZonalInspections.ContainsKey(id))
                    {
                        // Если с таким id уже есть в списке то удалем его из списка и просто пролетаем дальше
                        // без добавления в БД
                        dictZonalInspections.Remove(id);
                        continue;
                    }

                    if (id > 0)
                    {
                        var newObj = new InspectionGjiZonalInspection()
                        {
                            Inspection = inspection,
                            ZonalInspection = new ZonalInspection { Id = id }
                        };

                        listZonalInspectionsToSave.Add(newObj);
                    }
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Сохраняем список отделов
                        foreach (var item in listZonalInspectionsToSave)
                            service.Save(item);

                        // Если какието отделы остались в dictZonalInspections то их удаляем
                        // поскольку среди переданных zonalInspectionIds их небыло, но в БД они остались
                        foreach (var keyValue in dictZonalInspections)
                            service.Delete(keyValue.Value);

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
            finally
            {
                Container.Release(service);
                Container.Release(serviceInspection);
                Container.Release(serviceZonalInspection);
                Container.Release(servReminderRule);
            }
        }
    }
}