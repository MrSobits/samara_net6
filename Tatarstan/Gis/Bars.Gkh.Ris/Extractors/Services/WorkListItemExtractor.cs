namespace Bars.Gkh.Ris.Extractors.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.Gkh.Repair.Entities;

    /// <summary>
    /// Экстрактор данных работ и услуг перечня
    /// </summary>
    public class WorkListItemExtractor : BaseDataExtractor<WorkListItem, RepairWork>
    {
        private Dictionary<long, WorkList> workListByRepairObjectId = null;
        private Dictionary<RepairWork, int> repairWorkIndices = null;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<RepairWork> GetExternalEntities(DynamicDictionary parameters)
        {
            var repairWorkDomain = this.Container.ResolveDomain<RepairWork>();

            try
            {
                var repairObjectIds = this.workListByRepairObjectId
                    .Select(x => x.Key)
                    .ToArray();

                var repairWorks = repairWorkDomain.GetAll()
                    .Where(x => x.RepairObject != null)
                    .AsEnumerable()
                    .Where(x => repairObjectIds.Contains(x.RepairObject.Id))
                    .ToList();

                return repairWorks;
            }
            finally
            {
                this.Container.Release(repairWorkDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="repairWork">Сущность внешней системы</param>
        /// <param name="workListItem">Ris сущность</param>
        protected override void UpdateRisEntity(RepairWork repairWork, WorkListItem workListItem)
        {
            workListItem.ExternalSystemEntityId = repairWork.Id;

            workListItem.ExternalSystemName = "gkh";

            workListItem.WorkList = this.workListByRepairObjectId.Get(repairWork.RepairObject.Id);

            workListItem.TotalCost = repairWork.CostSum ?? 0m;

            // TODO: Использовать WorkItemCode и WorkItemGuid
            // workListItem.WorkItemNsi = this.serviceTypeByGkhId.Get(repairWork.Work.Id);

            workListItem.Index = this.repairWorkIndices.Get(repairWork);
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var workLists = parameters.GetAs<List<WorkList>>("workLists");

            this.workListByRepairObjectId = workLists.ToDictionary(x => x.ExternalSystemEntityId);
        }

        /// <summary>
        /// Выполнить обработку перед подготовкой Ris сущностей
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        /// <param name="externalEntities">Выбранные сущности внешней системы</param>
        protected override void BeforePrepareRisEntitiesHandle(DynamicDictionary parameters, List<RepairWork> externalEntities)
        {
            this.repairWorkIndices = externalEntities
                .GroupBy(x => x.RepairObject.Id)
                .SelectMany(x => x.Select((y, index) => new
                {
                    RepairWork = y,
                    Index = index + 1
                }))
                .ToDictionary(x => x.RepairWork, y => y.Index);
        }
    }
}
