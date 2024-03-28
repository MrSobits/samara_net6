namespace Bars.GisIntegration.CapitalRepair.DataExtractors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Экстрактор данных работ договоров на выполнение работ (оказание услуг) по капитальному ремонту
    /// </summary>
    public class CapitalRepairWorksDataExtractor : BaseDataExtractor<RisCrWork, BuildContractTypeWork>
    {
        private Dictionary<BuildContractTypeWork, RisCrContract> contracts;

        private IDictionary workDict;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<BuildContractTypeWork> GetExternalEntities(DynamicDictionary parameters)
        {
            var risCrContracts = parameters.GetAs<IEnumerable<RisCrContract>>("risCrContracts");         

            var buildContractTypeWorkDomain = this.Container.ResolveDomain<BuildContractTypeWork>();

            try
            {
                this.contracts =
                    (from risContract in risCrContracts
                        join gkhContract in buildContractTypeWorkDomain.GetAll()
                            on risContract.ExternalSystemEntityId equals gkhContract.Id
                        select new {risContract, gkhContract}).ToDictionary(x => x.gkhContract, x => x.risContract);


                return this.contracts.Keys.ToList();
            }
            finally
            {
                this.Container.Release(buildContractTypeWorkDomain);
            }
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.workDict = this.DictionaryManager.GetDictionary("WorkTypeDictionary");
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(BuildContractTypeWork externalEntity, RisCrWork risEntity)
        {
            risEntity.ExternalSystemName = "gkh";
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.EndMonthYear = externalEntity.TypeWork.DateEndWork.GetValueOrDefault().ToString("yyyy-MM");

            risEntity.Cost = externalEntity.BuildContract.Sum;
            risEntity.CostPlan = externalEntity.TypeWork.Sum;
            risEntity.Volume = externalEntity.TypeWork.Volume;
            risEntity.OtherUnit = externalEntity.TypeWork.Work.UnitMeasure.Name;
            risEntity.EndDate = externalEntity.TypeWork.DateEndWork;
            risEntity.StartDate = externalEntity.TypeWork.DateStartWork;
            risEntity.Contract = this.contracts[externalEntity];
            risEntity.ApartmentBuildingFiasGuid = externalEntity.BuildContract.ObjectCr.RealityObject.HouseGuid;

            var work = this.workDict.GetDictionaryRecord(externalEntity.TypeWork.Work.Id);
            risEntity.WorkKindCode = work?.GisCode;
            risEntity.WorkKindGuid = work?.GisGuid;

            var planWork = this.GetPlanWork(risEntity);
            risEntity.WorkPlanGUID = planWork?.Guid;
        }

        /// <summary>
        /// Сопоставление работы по договору с плановыми работами, полученными из ГИС
        /// </summary>
        /// <param name="work">Работа по договору</param>
        /// <returns>Соответствующая работа по плану или null</returns>
        private RisCrPlanWork GetPlanWork(RisCrWork work)
        {
            throw new NotImplementedException();
        }
    }
}