namespace Bars.GkhDiCr.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
 

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;
    using Bars.GkhDi.Entities;

    using Microsoft.AspNetCore.Mvc;

#warning Убрать из контроллера логику в сервисы и вью-модели
    public class RealityObjCopyCrController : DataController<ManOrgContractRealityObject>
    {
        public override ActionResult List(StoreLoadParams storeParams)
        {
            var disclosureInfoId = Request.Headers["disclosureInfoId"].ToLong();
            var programCrId = Request.Headers["programCrId"].ToLong();

            var serviceDisInfo = this.Resolve<IDomainService<DisclosureInfo>>();
            var disInfoObj = serviceDisInfo.Get(disclosureInfoId);

            // Получаем дома в данном раскрытие у которых есть услуга с типом КР
            var realityObjIdsWithCapRepairServList = this.Resolve<IDomainService<DisclosureInfoRelation>>()
                .GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                .Select(x => x.DisclosureInfoRealityObj.RealityObject.Id)
                .Distinct()
                .ToList();

            // Получаем дома из объектов КР по данной программе
            var objectCrRealObjIdList = this.Resolve<IDomainService<ObjectCr>>()
                .GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Select(x => x.RealityObject.Id)
                .Distinct()
                .ToList();

            var realityObjectIds = new List<long>();
            if (realityObjIdsWithCapRepairServList.Count != 0 && objectCrRealObjIdList.Count != 0)
            foreach (var objectCrRealObjId in objectCrRealObjIdList)
            {
                if (realityObjIdsWithCapRepairServList.Contains(objectCrRealObjId))
                {
                    realityObjectIds.Add(objectCrRealObjId);
                }
            }

            var service = this.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var data = service.GetAll()
            .Where(x => x.ManOrgContract.ManagingOrganization.Id == disInfoObj.ManagingOrganization.Id)

                // нач2>=нач1 и кон1>=нач2
                // или
                // нач1>=нач2 и кон2>=нач1
                // кон1 или кон2 == null считаем что null соответ + бесконечность
            .Where(
                x =>
                ((x.ManOrgContract.StartDate.HasValue && disInfoObj.PeriodDi.DateStart.HasValue
                  && (x.ManOrgContract.StartDate.Value >= disInfoObj.PeriodDi.DateStart.Value)
                  || !disInfoObj.PeriodDi.DateStart.HasValue)
                 && (x.ManOrgContract.StartDate.HasValue && disInfoObj.PeriodDi.DateEnd.HasValue
                     && (disInfoObj.PeriodDi.DateEnd.Value >= x.ManOrgContract.StartDate.Value)
                     || !disInfoObj.PeriodDi.DateEnd.HasValue))
                || ((x.ManOrgContract.StartDate.HasValue && disInfoObj.PeriodDi.DateStart.HasValue && (disInfoObj.PeriodDi.DateStart.Value >= x.ManOrgContract.StartDate.Value) || !x.ManOrgContract.StartDate.HasValue)
                    && (x.ManOrgContract.StartDate.HasValue && disInfoObj.PeriodDi.DateEnd.HasValue
                        && (x.ManOrgContract.EndDate.Value >= disInfoObj.PeriodDi.DateStart.Value)
                        || !x.ManOrgContract.EndDate.HasValue)))
            .Select(x => new
            {
                RealityObjectId = x.RealityObject.Id,
                x.RealityObject.FiasAddress.AddressName,
                x.RealityObject.AreaLiving,
                x.RealityObject.DateLastOverhaul
            })
            .ToList()
            .Where(x => objectCrRealObjIdList.Contains(x.RealityObjectId))
            .Distinct().AsQueryable()
            .Filter(storeParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(storeParams).Paging(storeParams);

            return new JsonListResult(data.ToList(), totalCount);
        }

        public ActionResult LoadWorkCr()
        {
            try
            {
                var realityObjIds = !string.IsNullOrEmpty(Request.Headers["realityObjIds"].ToStr()) ? Request.Headers["realityObjIds"].ToStr().Split(',').Select(x => x.ToLong()).ToArray() : new long[0];

                var workCapRepairService = this.Container.Resolve<IDomainService<WorkCapRepair>>();

                // Получаем предварительные списки видов работ капремонта, работ капремонта услуги капремонт, услуг с типом капремонт
                var typeWorkCrFullList = this.Container.Resolve<IDomainService<TypeWorkCr>>().GetAll().Where(x => realityObjIds.Contains(x.ObjectCr.RealityObject.Id) && x.YearRepair <= x.ObjectCr.ProgramCr.Period.DateEnd.Value.Year && x.YearRepair >= x.ObjectCr.ProgramCr.Period.DateStart.Year).ToList();
                var capRepairServiceFullList = this.Container.Resolve<IDomainService<CapRepairService>>().GetAll().Where(x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id)).ToList();
                var workCapRepairServiceFullList = workCapRepairService.GetAll().Where(x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id)).ToList();

                var workCapRepairServiceForSave = new List<WorkCapRepair>();
                var workCapRepairServiceForDelete = new List<long>();

                // Бежим по домам которые пришли с клиента
                foreach (var realityObjId in realityObjIds)
                {
                    // Получаем виды работ объекта КР по данному дому
                    var typeWorkCrList = typeWorkCrFullList.Where(x => x.ObjectCr.RealityObject.Id == realityObjId).ToList();

                    // Схлопываем виды работ с одинаковыми работами
                    var typeWorkCrListProxy = this.MergeTypeWorkCr(typeWorkCrList);

                    // Получаем услуги с типом капремонт по данному дому
                    var capRepairServiceList = capRepairServiceFullList.Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == realityObjId).ToList();

                    // Для каждого вида работ из объекта КР анализируем работы по полученным услугам капремонт
                    foreach (var typeWorkCr in typeWorkCrListProxy)
                    {
                        foreach (var capRepairService in capRepairServiceList)
                        {
                            // Получаем и удаляем существующие работы
                            var existingWorks = workCapRepairServiceFullList.Where(x => x.BaseService.Id == capRepairService.Id && x.Work.Id == typeWorkCr.WorkId);
                            workCapRepairServiceForDelete.AddRange(existingWorks.Select(x => x.Id).ToList());

                            // Добавляем в список работы для сохранения 
                            workCapRepairServiceForSave.Add(new WorkCapRepair
                            {
                                Id = 0,
                                BaseService = new BaseService { Id = capRepairService.Id },
                                Work = new Work { Id = typeWorkCr.WorkId },
                                PlannedVolume = typeWorkCr.Volume,
                                PlannedCost = typeWorkCr.Sum,
                                FactedVolume = typeWorkCr.VolumeOfCompletion,
                                FactedCost = typeWorkCr.CostSum,
                            });
                        }
                    }
                }

                this.InTransactionDelete(workCapRepairServiceForDelete.Distinct().Select(x => (object)x), workCapRepairService);
                this.InTransactionSave(workCapRepairServiceForSave, workCapRepairService);

                return new JsonNetResult(new { success = true });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        // Процедура склейки (однотипных по Work) работ
        private List<TypeWorkCrProxy> MergeTypeWorkCr(List<TypeWorkCr> list)
        {
            var resultList = new List<TypeWorkCrProxy>();

            foreach (var item in list)
            {
                var existingItem = resultList.FirstOrDefault(x => x.WorkId == item.Work.Id);
                if (existingItem != null)
                {
                    existingItem.Volume += item.Volume.HasValue ? item.Volume.Value : 0;
                    existingItem.Sum += item.Sum.HasValue ? item.Sum.Value : 0;
                    existingItem.VolumeOfCompletion += item.VolumeOfCompletion.HasValue ? item.VolumeOfCompletion.Value : 0;
                    existingItem.CostSum += item.CostSum.HasValue ? item.CostSum.Value : 0;
                }
                else
                {
                    resultList.Add(new TypeWorkCrProxy
                    {
                        WorkId = item.Work.Id,
                        Volume = item.Volume.HasValue ? item.Volume.Value : 0,
                        Sum = item.Sum.HasValue ? item.Sum.Value : 0,
                        VolumeOfCompletion = item.VolumeOfCompletion.HasValue ? item.VolumeOfCompletion.Value : 0,
                        CostSum = item.CostSum.HasValue ? item.CostSum.Value : 0
                    });
                }
            }

            return resultList;
        }

        public ActionResult GetPeriodsDi()
        {
            try
            {
                var disclosureInfoId = Request.Headers["disclosureInfoId"].ToLong();

                var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Get(disclosureInfoId);


                return new JsonNetResult(new { dateStartPeriodDi = disclosureInfo.PeriodDi.DateStart, dateEndPeriodDi = disclosureInfo.PeriodDi.DateEnd });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        private class TypeWorkCrProxy
        {
            public long WorkId { get; set; }

            public decimal Volume { get; set; }

            public decimal Sum { get; set; }

            public decimal VolumeOfCompletion { get; set; }

            public decimal CostSum { get; set; }
        }

        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        private void InTransactionSave(IEnumerable<IEntity> list, IDomainService repos)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                            repos.Save(entity);
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        private void InTransactionDelete(IEnumerable<object> list, IDomainService repos)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var id in list)
                    {
                        repos.Delete(id);
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}
