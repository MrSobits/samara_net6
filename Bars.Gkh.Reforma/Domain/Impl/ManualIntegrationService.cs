namespace Bars.Gkh.Reforma.Domain.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Tasks;
    using Bars.GkhDi.DomainService;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    using global::Quartz;

    /// <summary>
    /// Сервис для работы с выборочной интеграцией Реформы.ЖКХ
    /// </summary>
    public class ManualIntegrationService : IManualIntegrationService
    {
        #region Public Properties
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис УО в период раскрытия информации
        /// </summary>
        public IDomainService<DisclosureInfo> DisclosureInfoDomain { get; set; }

        /// <summary>
        /// Домен сервис периодов интеграции с Реформой ЖКХ
        /// </summary>
        public IDomainService<ReportingPeriodDict> ReportingPeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис "Синхронизируемый жилой дом"
        /// </summary>
        public IDomainService<RefRealityObject> RefRealityObjectDomain { get; set; }

        /// <summary>
        /// Домен-сервис "Связь деятельности УО и деятельности УО в доме"
        /// </summary>
        public IDomainService<DisclosureInfoRelation> DisclosureInfoRelationDomain { get; set; }

        /// <summary>
        /// Сервис для получения управляемых домов УО
        /// </summary>
        public IDiRealityObjectViewModelService DiRealityObjectViewModelService { get; set; }
        #endregion

        /// <summary>
        /// Добавить в очередь задачу интеграции УО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult ScheduleManorgIntegrationTask(BaseParams baseParams)
        {
            var disclosureInfoId = baseParams.Params.GetAsId("disclosureInfoId");
            var disclosureInfo = this.DisclosureInfoDomain.Get(disclosureInfoId);

            if (!disclosureInfo.IsNotNull())
            {
                return BaseDataResult.Error("Не найдена управляющая организация в активном периоде раскрытия информации");
            }

            var periodIntegration = this.ReportingPeriodDomain.GetAll().FirstOrDefault(x => x.PeriodDi.Id == disclosureInfo.PeriodDi.Id && x.Is_988 && x.Synchronizing);
            if (periodIntegration.IsNull())
            {
                return BaseDataResult.Error("Не найден активный отчётный период с Реформой, связанный с текущим периодом интеграции и относящийся к форме 988 ПП РФ");
            }

            if (!ManOrgService.Instance.IsSynchronizable(disclosureInfo.ManagingOrganization))
            {
                return BaseDataResult.Error("Невозможно запустить выборочную интеграцию по текущей управляющей организации. Дождитесь плановой интеграции с Реформой");
            }


            var dictionary = DynamicDictionary.Create();
            dictionary.Add("ManagingOrganizationId", disclosureInfo.ManagingOrganization.Id);
            dictionary.Add("PeriodExternalId", periodIntegration.ExternalId);
            dictionary.Add("PeriodId", disclosureInfo.PeriodDi.Id);

            this.ScheduleTask<SetCompanyProfile988Task>(dictionary);

            return new BaseDataResult();
        }

        /// <summary>
        /// Вернуть список управляемых домов для выполнения выборочной интеграции
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операци</returns>
        public IDataResult ListManagedRealityObjects(BaseParams baseParams)
        {
            var disclosureInfoId = baseParams.Params.GetAsId("disclosureInfoId");
            var loadParams = baseParams.GetLoadParam();
            var disclosureInfo = this.DisclosureInfoDomain.Get(disclosureInfoId);

            if (!disclosureInfo.IsNotNull())
            {
                return new ListDataResult();
            }

            var queryManagedRealityObjects = this.DiRealityObjectViewModelService.GetManagedRealityObjects(disclosureInfo);
            var data = this.RefRealityObjectDomain.GetAll()
                .Where(x => queryManagedRealityObjects.Any(y => x.RealityObject.Id == y))
                .Select(x => new { x.Id, x.RealityObject.Address, x.ExternalId })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }

        /// <summary>
        /// Добавить в очередь задачу интеграции домов по УО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult ScheduleRobjectIntegrationTask(BaseParams baseParams)
        {
            var disclosureInfoId = baseParams.Params.GetAsId("disclosureInfoId");
            var manOrgId = baseParams.Params.GetAsId("manOrgId");
            var disclosureInfo = this.DisclosureInfoDomain.Get(disclosureInfoId);

            if (!disclosureInfo.IsNotNull())
            {
                return BaseDataResult.Error("Не найдена управляющая организация в активном периоде раскрытия информации");
            }

            var periodIntegration = this.ReportingPeriodDomain.GetAll().FirstOrDefault(x => x.PeriodDi.Id == disclosureInfo.PeriodDi.Id && x.Is_988 && x.Synchronizing);
            if (periodIntegration.IsNull())
            {
                return BaseDataResult.Error("Не найден активный отчётный период с Реформой, связанный с текущим периодом интеграции и относящийся к форме 988 ПП РФ");
            }

            if (!ManOrgService.Instance.IsSynchronizable(disclosureInfo.ManagingOrganization))
            {
                return BaseDataResult.Error("Невозможно запустить выборочную интеграцию по текущей управлющей организации. Дождитесь плановой интеграции с Реформой");
            }
            
            var dictionary = DynamicDictionary.Create();
            dictionary.Add("RealityObjectIds", this.ExtractRealityObjectIds(baseParams, disclosureInfo));
            dictionary.Add("PeriodDiId", disclosureInfo.PeriodDi.Id);
            dictionary.Add("ManOrgId", manOrgId);

            this.ScheduleTask<SetHouseProfile988Task>(dictionary);
            return new BaseDataResult();
        }

        /// <summary>
        /// Извлечь синхронизируемые дома 
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроска</param>
        /// <param name="disclosureInfo">УО в период раскрытия информации</param>
        /// <returns></returns>
        private IList<long> ExtractRealityObjectIds(BaseParams baseParams, DisclosureInfo disclosureInfo)
        {
            var selectedRefObjectIds = baseParams.Params.GetAs<IEnumerable<long>>("refRoIds");

            var queryManagedRealityObjects = this.DiRealityObjectViewModelService.GetManagedRealityObjects(disclosureInfo);

            return this.RefRealityObjectDomain.GetAll()
                .Where(x => queryManagedRealityObjects.Any(y => x.RealityObject.Id == y))
                .WhereIf(selectedRefObjectIds.IsNotEmpty(), x => selectedRefObjectIds.Contains(x.Id))
                .Select(x => x.RealityObject.Id)
                .ToList();
        }

        /// <summary>
        /// Поставить в очередь задачу в планировщик
        /// </summary>
        /// <typeparam name="TTask">Тип задачи</typeparam>
        /// <param name="params">Параметры</param>
        private void ScheduleTask<TTask>(DynamicDictionary @params) where TTask : class, ITask
        {
            var scheduler = this.Container.Resolve<IScheduler>("ReformaTaskScheduler");
            var job = JobBuilder.Create<TaskJob<TTask>>().UsingJobData(new JobDataMap().Apply(@params)).Build();
            var trigger = TriggerBuilder.Create().WithIdentity(Guid.NewGuid().ToString()).StartNow().Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}