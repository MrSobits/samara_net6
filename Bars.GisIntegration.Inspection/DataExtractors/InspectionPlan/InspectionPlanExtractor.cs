namespace Bars.GisIntegration.Inspection.DataExtractors.InspectionPlan
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Экстрактор данных по планам проверок юридического лица
    /// </summary>
    public class InspectionPlanExtractor : BaseDataExtractor<InspectionPlan, PlanJurPersonGji>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PlanJurPersonGji> GetExternalEntities(DynamicDictionary parameters)
        {
            long[] selectedIds;

            var selectedList = parameters.GetAs("selectedList", string.Empty);
            if (selectedList.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedList.ToLongArray();
            }

            var planJurPersonGjiDomain = this.Container.ResolveDomain<PlanJurPersonGji>();
            var baseJurPersonDomain = this.Container.ResolveDomain<BaseJurPerson>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();

            try
            {
                var disposals = disposalDomain.GetAll()
                    .Where(x => x.DateEnd.HasValue && x.DateEnd < DateTime.Now);

                var inspectionPlanIds = baseJurPersonDomain.GetAll()
                    .Where(x => x.Plan != null && x.Contragent != null)
                    .Where(x => disposals.Any(y => y.Inspection.Id == x.Id))
                    //.Where(x => x.Contragent.Id == this.Contragent.GkhId)
                    .Select(x => x.Plan.Id)
                    .Distinct();

                return planJurPersonGjiDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .WhereIf(selectedIds == null, x => inspectionPlanIds.Contains(x.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(planJurPersonGjiDomain);
                this.Container.Release(baseJurPersonDomain);
                this.Container.Release(disposalDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PlanJurPersonGji externalEntity, InspectionPlan risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.Year = externalEntity.DateStart?.Year;
            risEntity.ApprovalDate = externalEntity.DateApproval ?? DateTime.MinValue;
            risEntity.UriRegistrationNumber = externalEntity.UriRegistrationNumber;
        }
    }
}
