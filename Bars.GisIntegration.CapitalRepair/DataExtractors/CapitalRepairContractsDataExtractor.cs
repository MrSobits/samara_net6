namespace Bars.GisIntegration.CapitalRepair.DataExtractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Экстрактор данных договоров на выполнение работ (оказание услуг) по капитальному ремонту
    /// </summary>
    public class CapitalRepairContractsDataExtractor : BaseDataExtractor<RisCrContract, BuildContract>
    {
        private long regOperatorContragentId;

        private Dictionary<BuildContract, string> contracts;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<BuildContract> GetExternalEntities(DynamicDictionary parameters)
        {
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();
            var regOperatorDomain = this.Container.ResolveDomain<RegOperator>();
            var buildContractDomain = this.Container.ResolveDomain<BuildContract>();
            var risCrPlanDomain = this.Container.ResolveDomain<RisCrPlan>();

            try
            {
                //Региональный оператор всегда один, поэтому FirstOrDefault()
                var regOperatorContragent = regOperatorDomain.GetAll().FirstOrDefault();

                if (regOperatorContragent == null)
                    throw new Exception("В системе не обнаружен региональный оператор!");
                if (regOperatorContragent.Contragent == null)
                    throw new Exception("В системе не обнаружен контрагент регионального оператора!");

                this.regOperatorContragentId = regOperatorContragent.Contragent.Id;

                if (!risContragentDomain.GetAll().Any(x => x.GkhId == this.regOperatorContragentId))
                    throw new Exception("Не получены данные для регионального оператора из ГИС");

                var selectedPlans = parameters.GetAs("selectedList", string.Empty);
                var selectedIds = selectedPlans.ToUpper() == "ALL" ? null : selectedPlans.ToLongArray();


                var buildContracts = buildContractDomain.GetAll()

                    // Берем только типы договора = "На СМР" И "На лифты";
                    .Where(x => x.TypeContractBuild == TypeContractBuild.Smr || x.TypeContractBuild == TypeContractBuild.Lift)

                    //Контрагент подрядчика - обязателен, так как является исполнителем
                    .Where(x => x.Builder.Contragent != null)

                    //берем только тех контрагентов (подрядчиков), которые были отправлены ранее в ГИС
                    .Where(x => risContragentDomain.GetAll().Any(c => c.GkhId == x.Builder.Contragent.Id && c.OrgPpaGuid != null && c.OrgPpaGuid != ""));

                var plans = risCrPlanDomain.GetAll().WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id));

                this.contracts =
                    (from contract in buildContracts
                        from plan in plans
                        where plan.StartMonthYear <= contract.DateStartWork && plan.EndMonthYear >= contract.DateEndWork
                            && plan.MunicipalityCode == contract.ObjectCr.RealityObject.Municipality.Oktmo.ToString()
                        select new {contract, plan.Guid})
                        .ToDictionary(x => x.contract, x => x.Guid);
                return this.contracts.Keys.ToList();
            }
            finally
            {
                this.Container.Release(buildContractDomain);
                this.Container.Release(risCrPlanDomain);
                this.Container.Release(risContragentDomain);
                this.Container.Release(regOperatorDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(BuildContract externalEntity, RisCrContract risEntity)
        {
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();
            var estimateCalculationDomain = this.Container.ResolveDomain<EstimateCalculation>();

            try
            {
                risEntity.PlanGUID = this.contracts[externalEntity];
                risEntity.ExternalSystemName = "gkh";
                risEntity.ExternalSystemEntityId = externalEntity.Id;
                risEntity.Number = externalEntity.DocumentNum;
                risEntity.Date = externalEntity.DocumentDateFrom;
                risEntity.StartDate = externalEntity.DateStartWork;
                risEntity.EndDate = externalEntity.DateEndWork;
                risEntity.Sum = externalEntity.Sum;
                
                //Заказчиком всегда является регоператор
                risEntity.Customer = risContragentDomain.GetAll()
                        .FirstOrDefault(x => x.GkhId == this.regOperatorContragentId)?.OrgRootEntityGuid;

                //Исполнителем является подрядная организация
                risEntity.Performer = risContragentDomain.GetAll()
                    .FirstOrDefault(x => x.GkhId == externalEntity.Builder.Contragent.Id)?.OrgRootEntityGuid;

#if DEBUG
                //для отладки
                risEntity.Customer = "testCustomer";
                risEntity.Performer = "testPerformer";
#endif
                
                //передавать значение "true", если в разделе "Сметный расчет по работе" отсутствуют записи с Наименованиями работ из раздела "Договоры"
                risEntity.OutlayMissing = !estimateCalculationDomain.GetAll().Any(x => x.TypeWorkCr == externalEntity.TypeWork);
            }
            finally
            {
                this.Container.Release(risContragentDomain);
                this.Container.Release(estimateCalculationDomain);
            }
        }
    }
}