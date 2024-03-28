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
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Enums;

    /// <summary>
    /// Экстрактор "План по перечню работ/услуг"
    /// </summary>
    public class WorkingPlanExtractor : BaseDataExtractor<WorkingPlan, RepairObject>
    {
        private Dictionary<long, WorkList> workListById;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<RepairObject> GetExternalEntities(DynamicDictionary parameters)
        {
            return this.GetRepairObjectsQuery(parameters).ToList();
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="repairObject">Сущность внешней системы</param>
        /// <param name="workingPlan">Ris сущность</param>
        protected override void UpdateRisEntity(RepairObject repairObject, WorkingPlan workingPlan)
        {
            workingPlan.ExternalSystemEntityId = repairObject.Id;
            workingPlan.ExternalSystemName = "gkh";
            workingPlan.Year = (short)repairObject.RepairProgram.Period.DateStart.Year;
            workingPlan.WorkList = this.workListById.Get(repairObject.Id);
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Например, подготовить словари с данными
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.Container.UsingForResolved<IDomainService<WorkList>>(
                (c, domain) =>
                {
                    this.workListById = domain.GetAll()
                        .Where(x => x.Contragent.Id == this.Contragent.Id)
                        .ToDictionary(x => x.ExternalSystemEntityId);
                });
        }

        /// <summary>
        /// Метод получения запроса для выбора объектов текущего ремонта
        /// </summary>
        /// <returns>Запрос</returns>
        private IQueryable<RepairObject> GetRepairObjectsQuery(DynamicDictionary parameters)
        {
            var repairObjectDomain = this.Container.ResolveDomain<RepairObject>();
            var manOrgContractRealityObjectDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();
            var workListDomain = this.Container.ResolveDomain<WorkList>();

            var selectedRepairObjectsParam = parameters.GetAs<string>("selectedRepairObjects");
            var all = selectedRepairObjectsParam.ToUpper() == "ALL";
            var selectedRepairObjectIds = selectedRepairObjectsParam.ToLongArray();

            try
            {
                var currentContragent = this.Contragent;
                var workListQuery = workListDomain.GetAll();
                    //TODO: при переходе в боевые условия убрать комментирование условия
                    //.Where(x => x.Guid != null);

                // Найти объекты недвижимости, у которых заведены действующие договоры управления =>
                // => В договоре должно быть НЕ заполнено поле "Основание расторжения" И его "Дата окончания управления" > Текущая дата или = null
                var realityObjectIds = manOrgContractRealityObjectDomain.GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == currentContragent.GkhId)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value > DateTime.Today)
                    .Where(x => x.ManOrgContract.TerminateReason == null || x.ManOrgContract.TerminateReason == "")
                    .Select(x => x.RealityObject.Id);

                // Найти объекты текущего ремонта
                var repairObjects = repairObjectDomain.GetAll()
                    .Where(x => x.RepairProgram != null
                        && x.RepairProgram.TypeProgramRepairState == TypeProgramRepairState.Active
                        || x.RepairProgram.TypeProgramRepairState == TypeProgramRepairState.New)
                    .Where(x => x.RepairProgram.Period != null)
                    .Where(x => x.RealityObject != null && realityObjectIds.Any(y => x.RealityObject.Id == y))
                    .WhereIf(!all && selectedRepairObjectIds.IsNotEmpty(), x => selectedRepairObjectIds.Contains(x.RepairProgram.Id))
                    .Where(x => workListQuery.Any(y => y.ExternalSystemEntityId == x.Id));

                return repairObjects;
            }
            finally
            {
                this.Container.Release(repairObjectDomain);
                this.Container.Release(manOrgContractRealityObjectDomain);
                this.Container.Release(workListDomain);
            }
        }
    }
}
