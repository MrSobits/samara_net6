namespace Bars.Gkh.Ris.Extractors.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.Gkh.Repair.Entities;

    /// <summary>
    /// Экстрактор данных "План работ по перечню работ/услуг"
    /// </summary>
    public class WorkPlanItemExtractor : BaseDataExtractor<WorkPlanItem, RepairWork>
    {
        private Dictionary<long, WorkingPlan> workingPlanByRepairObjectId;
        private Dictionary<long, WorkListItem> workListItemByRepairObjectId;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<RepairWork> GetExternalEntities(DynamicDictionary parameters)
        {
            return this.GetWorkQuery(parameters).ToList();
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="repairWork">Сущность внешней системы</param>
        /// <param name="workPlanItem">Ris сущность</param>
        protected override void UpdateRisEntity(RepairWork repairWork, WorkPlanItem workPlanItem)
        {
            workPlanItem.ExternalSystemEntityId = repairWork.Id;
            workPlanItem.ExternalSystemName = "gkh";
            workPlanItem.WorkingPlan = this.workingPlanByRepairObjectId.Get(repairWork.RepairObject.Id);
            workPlanItem.WorkListItem = this.workListItemByRepairObjectId.Get(repairWork.Id);

            workPlanItem.Year = (short)(repairWork.DateStart?.Year ?? 0);
            workPlanItem.Month = repairWork.DateStart?.Month ?? 0;
            workPlanItem.WorkDate = repairWork.DateStart ?? default(DateTime);
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Например, подготовить словари с данными
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var workingPlans = parameters.GetAs<List<WorkingPlan>>("workingPlan");
            this.workingPlanByRepairObjectId = workingPlans.ToDictionary(x => x.ExternalSystemEntityId);

            var workListIds = workingPlans.Select(x => x.WorkList.Id).ToArray();

            this.Container.UsingForResolved<IDomainService<WorkListItem>>(
                (c, domain) =>
                {
                    this.workListItemByRepairObjectId = domain.GetAll()
                        .Where(x => workListIds.Contains(x.WorkList.Id))
                        .ToDictionary(x => x.ExternalSystemEntityId);
                });
        }

        private IQueryable<RepairWork> GetWorkQuery(DynamicDictionary parameters)
        {
            var workingPlan = this.workingPlanByRepairObjectId.Select(x => x.Key).ToArray();

            var repairWorkDomain = this.Container.ResolveDomain<RepairWork>();
            var workListItemDomain = this.Container.ResolveDomain<WorkListItem>();

            try
            {
                var workListItemQuery = workListItemDomain.GetAll()
                    //TODO: при переходе в боевые условия убрать комментирование условия
                    //.Where(x => x.Guid != null)
                    .Where(x => x.Contragent.Id == this.Contragent.Id);

                return repairWorkDomain.GetAll()
                    .Where(x => workListItemQuery.Any(y => y.ExternalSystemEntityId == x.Id))
                    .Where(x => workingPlan.Contains(x.RepairObject.Id));
            }
            finally
            {
                this.Container.Release(repairWorkDomain);
                this.Container.Release(workListItemDomain);
            }

            
        }
    }
}
