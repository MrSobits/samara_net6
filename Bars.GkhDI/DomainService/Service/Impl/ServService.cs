namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    using Gkh.Authentification;
    using Gkh.Domain;
    using Gkh.DomainService;
    using Gkh.Enums;

    using Ionic.Zip;
    using Ionic.Zlib;

    using FileInfo = B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для работы с услугами
    /// </summary>
    public class ServService : IServService
    {
#warning людям со слабой психикой не смотреть, переписать

        protected StringBuilder message { get; set; }

        #region services
        public IFileManager FileManager { get; set; }

        public IRepository<ManOrgContractRealityObject> ManOrgContractRoRepository { get; set; }

        public IRepository<CommunalService> CommunalRepository { get; set; }

        public IRepository<HousingService> HousingRepository { get; set; }

        public IRepository<RepairService> RepairRepository { get; set; }

        public IRepository<CapRepairService> CapRepairRepository { get; set; }

        public IRepository<ControlService> ControlRepository { get; set; }

        public IRepository<AdditionalService> AdditionalRepository { get; set; }

        public IRepository<BaseService> BaseRepository { get; set; }

        public IRepository<TariffForConsumers> TariffForConsumRepository { get; set; }

        public IRepository<TariffForRso> TariffForRsoRepository { get; set; }

        public IRepository<ProviderService> ProviderServiceRepository { get; set; }

        public IRepository<CostItem> CostItemRepository { get; set; }

        public IRepository<WorkRepairDetail> WorkRepairDetailRepository { get; set; }

        public IRepository<WorkRepairList> WorkRepairListRepository { get; set; }

        public IRepository<WorkRepairTechServ> WorkRepairTechServRepository { get; set; }

        public IRepository<PlanWorkServiceRepair> PlanWorkServiceRepairRepository { get; set; }

        public IRepository<PlanWorkServiceRepairWorks> PlanWorkServiceRepairWorksRepository { get; set; }

        public IRepository<WorkCapRepair> WorkCapRepairRepository { get; set; }

        public IRepository<DisclosureInfoRealityObj> DisclosureInfoRealObjRepository { get; set; }

        public IRepository<PeriodDi> PeriodDiRepository { get; set; }

        public IRepository<ConsumptionNormsNpa> ConsumptionNormsNpaRepository { get; set; }

        public IDomainService<TemplateService> TemplateServiceDomain { get; set; }

        #endregion

        #region словари данных

        protected Dictionary<long, PeriodDi> DictPeriodDi = new Dictionary<long, PeriodDi>();

        protected Dictionary<long, string> DictRealObj = new Dictionary<long, string>(); // int - realityObjId

        protected Dictionary<long, List<BaseService>> DictBaseService = new Dictionary<long, List<BaseService>>(); // int - realityObjId

        protected Dictionary<long, List<DisclosureInfoRealityObj>> DictDisclosureInfoRealObj = new Dictionary<long, List<DisclosureInfoRealityObj>>(); // int - realityObjId

        protected Dictionary<long, List<CommunalService>> DictCommunalService = new Dictionary<long, List<CommunalService>>(); // int - disclosureInfoRealityObjId

        protected Dictionary<long, List<HousingService>> DictHousingService = new Dictionary<long, List<HousingService>>(); // int - disclosureInfoRealityObjId

        protected Dictionary<long, List<AdditionalService>> DictAdditionalService = new Dictionary<long, List<AdditionalService>>(); // int - disclosureInfoRealityObjId

        protected Dictionary<long, List<ControlService>> DictControlService = new Dictionary<long, List<ControlService>>(); // int - disclosureInfoRealityObjId

        protected Dictionary<long, List<RepairService>> DictRepairService = new Dictionary<long, List<RepairService>>();// int - disclosureInfoRealityObjId

        protected Dictionary<long, List<CapRepairService>> DictCapRepairService = new Dictionary<long, List<CapRepairService>>(); // int - disclosureInfoRealityObjId

        protected Dictionary<long, List<TariffForConsumers>> DictTariffForConsumers = new Dictionary<long, List<TariffForConsumers>>(); // int - baseServiceId

        protected Dictionary<long, List<TariffForRso>> DictTariffForRso = new Dictionary<long, List<TariffForRso>>(); // int - baseServiceId

        protected Dictionary<long, List<CostItem>> DictCostItem = new Dictionary<long, List<CostItem>>(); // int - baseServiceId

        protected Dictionary<long, List<ProviderService>> DictProviderService = new Dictionary<long, List<ProviderService>>(); // int - baseServiceId

        protected Dictionary<long, List<WorkCapRepair>> DictWorkCapRepair = new Dictionary<long, List<WorkCapRepair>>(); // int - baseServiceId

        protected Dictionary<long, List<WorkRepairDetail>> DictWorkRepairDetail = new Dictionary<long, List<WorkRepairDetail>>(); // int - baseServiceId

        protected Dictionary<long, List<WorkRepairList>> DictWorkRepairList = new Dictionary<long, List<WorkRepairList>>(); // int - baseServiceId

        protected Dictionary<long, List<WorkRepairTechServ>> DictWorkRepairTechServ = new Dictionary<long, List<WorkRepairTechServ>>(); // int - baseServiceId

        protected Dictionary<long, List<ConsumptionNormsNpa>> DictConsumptionNormsNpa = new Dictionary<long, List<ConsumptionNormsNpa>>(); // int - baseServiceId

        protected Dictionary<long, Dictionary<long, PlanWorkServiceRepairWorks>> DictPlanWorkServiceRepairWorks = new Dictionary<long, Dictionary<long, PlanWorkServiceRepairWorks>>(); // int - baseServiceId => int - GroupWorkPprId

        protected List<CommunalService> ListSaveCommunalService = new List<CommunalService>();

        protected List<HousingService> ListSaveHousingService = new List<HousingService>();

        protected List<AdditionalService> ListSaveAdditionalService = new List<AdditionalService>();

        protected List<ControlService> ListSaveControlService = new List<ControlService>();

        protected List<RepairService> ListSaveRepairService = new List<RepairService>();

        protected List<CapRepairService> ListSaveCapRepairService = new List<CapRepairService>();

        protected List<TariffForConsumers> ListSaveTariffForConsumers = new List<TariffForConsumers>();

        protected List<TariffForRso> ListSaveTariffForRso = new List<TariffForRso>();

        protected List<CostItem> ListSaveCostItem = new List<CostItem>();

        protected List<WorkCapRepair> ListSaveWorkCapRepair = new List<WorkCapRepair>();

        protected List<ProviderService> ListSaveProviderService = new List<ProviderService>();

        protected List<WorkRepairDetail> ListSaveWorkRepairDetail = new List<WorkRepairDetail>();

        protected List<WorkRepairList> ListSaveWorkRepairList = new List<WorkRepairList>();

        protected List<WorkRepairTechServ> ListSaveWorkRepairTechServ = new List<WorkRepairTechServ>();

        protected List<PlanWorkServiceRepair> ListSavePlanWorkServiceRepair = new List<PlanWorkServiceRepair>();

        protected List<PlanWorkServiceRepairWorks> ListSavePlanWorkServiceRepairWorks = new List<PlanWorkServiceRepairWorks>();

        protected List<DisclosureInfoRealityObj> ListSaveDisclosureInfoRealityObj = new List<DisclosureInfoRealityObj>();

        protected List<ConsumptionNormsNpa> ListSaveConsumptionNormsNpa = new List<ConsumptionNormsNpa>();

        #endregion
        
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Копирует услуги между периодами
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult CopyService(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var logOperationDomain = Container.ResolveDomain<LogOperation>();
            var fileManager = Container.Resolve<IFileManager>();
            var manOrgRoService = Container.Resolve<IManagingOrgRealityObjectService>();

            try
            {
                var logOperation = new LogOperation
                {
                    StartDate = DateTime.UtcNow,
                    OperationType = LogOperationType.CopyServByRo,
                    User = userManager.GetActiveUser()
                };

                var realityObjIds = baseParams.Params.GetAs<string>("realityObjIds").ToLongArray();
                var copingServiceId = baseParams.Params.GetAsId("copingServiceId");
                var copingServiceKind = baseParams.Params.GetAs<KindServiceDi>("copingServiceKind");

                var baseService = BaseRepository.Get(copingServiceId);

                var initParams = this.GetInitParams(copingServiceKind);

                var listRealityObjIds = realityObjIds.Select(x => x).ToList();
                listRealityObjIds.Add(baseService.DisclosureInfoRealityObj.RealityObject.Id);

                this.InitCollections(new List<long> { baseService.DisclosureInfoRealityObj.PeriodDi.Id }, initParams, baseService.DisclosureInfoRealityObj.ManagingOrganization.Id, listRealityObjIds);

                var manOrg =
                    manOrgRoService.GetCurrentManOrg(baseService.DisclosureInfoRealityObj.RealityObject)
                        .Return(x => x.ManagingOrganization)
                        .Return(x => x.Contragent)
                        .Return(x => x.Name);

                message = new StringBuilder();

                foreach (var roId in realityObjIds)
                {
                    // не копируем услуги по в дома в которых уже есть такие услуги
                    if (DictBaseService.ContainsKey(roId))
                    {
                        var existService = DictBaseService[roId].Any(x => x.DisclosureInfoRealityObj.PeriodDi.Id == baseService.DisclosureInfoRealityObj.PeriodDi.Id
                                                                                && x.TemplateService.Id == baseService.TemplateService.Id);

                        if (existService)
                        {
                            if (DictRealObj.ContainsKey(roId))
                            {
                                message.AppendFormat("{0};\n", DictRealObj[roId]);
                            }

                            continue;
                        }
                    }

                    switch (copingServiceKind)
                    {
                        case KindServiceDi.Communal:
                            {
                                this.CopyCommunalService(baseService, 0, roId);
                            }

                            break;

                        case KindServiceDi.Housing:
                            {
                                this.CopyHousingService(baseService, roId);
                            }

                            break;

                        case KindServiceDi.Repair:
                            {
                                this.CopyRepairService(baseService, roId);
                            }

                            break;

                        case KindServiceDi.CapitalRepair:
                            {
                                this.CopyCapRepairService(baseService, roId);
                            }

                            break;

                        case KindServiceDi.Additional:
                            {
                                this.CopyAdditionalService(baseService, roId);
                            }

                            break;

                        case KindServiceDi.Managing:
                            {
                                this.CopyControlService(baseService, roId);
                            }

                            break;
                    }
                }

                this.SaveServices();

                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };


                var finalStr = new StringBuilder();
                finalStr.AppendFormat("Копирование услуг дома {0} из отчетного периода {1} в отчетный период {2} по УО = {3}\n",  baseService.DisclosureInfoRealityObj.RealityObject.Address,
                    baseService.DisclosureInfoRealityObj.PeriodDi.Name, baseService.DisclosureInfoRealityObj.PeriodDi.Name, manOrg);

                if (message.Length > 0)
                {
                    finalStr.Append("Услуга \"{0}\" не скопирована в следующие дома:\n".FormatUsing(baseService.TemplateService.Name));
                }
                else
                {
                    finalStr.Append("Услуга \"{0}\" успешно скопирована".FormatUsing(baseService.TemplateService.Name));
                }

                using (var logFile = new MemoryStream())
                {
                    var log = Encoding.GetEncoding(1251).GetBytes(finalStr.ToString() + message);

                    logsZip.AddEntry(string.Format("{0}.csv", logOperation.OperationType.GetEnumMeta().Display), log);

                    logsZip.Save(logFile);

                    var logFileInfo = fileManager.SaveFile(logFile, string.Format("{0}.zip", logOperation.OperationType.GetEnumMeta().Display));

                    logOperation.LogFile = logFileInfo;
                }

                logOperation.EndDate = DateTime.UtcNow;
                logOperation.Comment = "УО = {0}. Копирование услуг из дома в дом".FormatUsing(manOrg);

                logOperationDomain.Save(logOperation);

                return new BaseDataResult { Success = true, Message = message.Length > 0 ? 
                    "Услуга \"{0}\" скопирована не во все дома.".FormatUsing(baseService.TemplateService.Name) :
                    "Услуга \"{0}\" успешно скопирована.".FormatUsing(baseService.TemplateService.Name)
                    , Data = logOperation.LogFile.Id };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Возвращает количество обязательных услуг
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetCountMandatory(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoRealityObjId = baseParams.Params["disclosureInfoRealityObjId"].ToLong();

                // берем все различные шаблонные услуги с пометкой обязательности
                var mandatoryTemplateServiceIdList = this.TemplateServiceDomain.GetAll()
                    .Where(x => x.IsMandatory)
                    .AsEnumerable()
                    .Distinct(x => x.Id);

                // берем услуге по дому, у которых берем различные шаблонные услуги
                var serviceList =
                    Container.Resolve<IDomainService<BaseService>>().GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObjId)
                    .AsEnumerable()
                    .Distinct(x => x.TemplateService.Id);

                var countDifficit = mandatoryTemplateServiceIdList.Count() - serviceList.Count();
                countDifficit = countDifficit >= 0 ? countDifficit : 0;

                return new BaseDataResult(countDifficit)
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Возвращает информацию о периоде раскрытия
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>

        public IDataResult GetInfoServPeriod(BaseParams baseParams)
        {
            try
            {
                var disclosureInfoId = baseParams.Params["disclosureInfoId"].ToLong();
                var disclosureInfo = this.Container.Resolve<IDomainService<DisclosureInfo>>().Load(disclosureInfoId);

                return new BaseDataResult(new
                {
                    Period = new
                    {
                        disclosureInfo.PeriodDi.Id,
                        disclosureInfo.PeriodDi.Name
                    },
                    ManOrg = new
                    {
                        disclosureInfo.ManagingOrganization.Id,
                        disclosureInfo.ManagingOrganization.Contragent.Name
                    }
                })
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Копирует услуги между периодами массово
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult CopyServPeriod(BaseParams baseParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var logOperationDomain = Container.ResolveDomain<LogOperation>();
            var manOrgDomain = Container.ResolveDomain<ManagingOrganization>();
            var fileManager = Container.Resolve<IFileManager>();

            try
            {
                var logOperation = new LogOperation
                {
                    StartDate = DateTime.UtcNow,
                    OperationType = LogOperationType.CopyServByPeriod,
                    User = userManager.GetActiveUser()
                };

                var manorgId = baseParams.Params["manorgId"].ToLong();
                var periodCurrentId = baseParams.Params["periodCurrentId"].ToLong();
                var periodFromId = baseParams.Params["periodFromId"].ToLong();

                var periodFrom = PeriodDiRepository.Get(periodFromId);
                var manOrg = manOrgDomain.Get(manorgId);

                var realityObjIds = ManOrgContractRoRepository.GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization.Id == manorgId)

                    // нач2>=нач1 и кон1>=нач2
                    // или
                    // нач1>=нач2 и кон2>=нач1
                    // кон1 или кон2 == null считаем что null соответ + бесконечность
                    .Where(x =>
                        ((x.ManOrgContract.StartDate.HasValue && periodFrom.DateStart.HasValue
                                && (x.ManOrgContract.StartDate.Value >= periodFrom.DateStart.Value)
                                || !periodFrom.DateStart.HasValue)
                            && (x.ManOrgContract.StartDate.HasValue && periodFrom.DateEnd.HasValue
                                && (periodFrom.DateEnd.Value >= x.ManOrgContract.StartDate.Value)
                                || !periodFrom.DateEnd.HasValue))
                        || ((x.ManOrgContract.StartDate.HasValue && periodFrom.DateStart.HasValue &&
                                (periodFrom.DateStart.Value >= x.ManOrgContract.StartDate.Value) || !x.ManOrgContract.StartDate.HasValue)
                            && (x.ManOrgContract.StartDate.HasValue && periodFrom.DateEnd.HasValue
                                && (x.ManOrgContract.EndDate.Value >= periodFrom.DateStart.Value)
                                || !x.ManOrgContract.EndDate.HasValue)))
                    .Select(x => x.RealityObject.Id)
                    .AsEnumerable()
                    .Distinct()
                    .ToList();

                var initParams = new InitParams { All = true };
                this.InitCollections(new List<long> { periodCurrentId, periodFromId }, initParams, manorgId, realityObjIds);

                var periodCurrent = DictPeriodDi.Get(periodCurrentId);

                var existKeys = new Dictionary<string, long>();
                foreach (var kvp in DictBaseService)
                {

                    foreach (var currentService in kvp.Value)
                    {
                        if (currentService.DisclosureInfoRealityObj.PeriodDi.Id != periodCurrentId)
                        {
                            continue;
                        }

                        // ДОМ_УСЛУГА - берем только существующие в текущем периоде услуги 
                        var key = string.Format("{0}_{1}", kvp.Key, currentService.TemplateService.Id);
                        if (!existKeys.ContainsKey(key))
                        {
                            existKeys.Add(key, kvp.Key);
                        }
                    }
                }

                var messageMain = new StringBuilder();

                foreach (var realityObjId in realityObjIds)
                {

                    // Нужно копироват ьтолько те услуги которых еще нет
                    // Для этог опроверяем есть лии нет услуга с таким ключем всписке уже существующих
                    /*

                    if (DictBaseService.ContainsKey(realityObjId))
                    {
                        var existService = DictBaseService[realityObjId].Any(x => x.DisclosureInfoRealityObj.PeriodDi.Id == periodCurrentId);

                        if (existService)
                        {
                            if (DictRealObj.ContainsKey(realityObjId.ToLong()))
                            {
                                message.AppendFormat("{0};<br/>", DictRealObj[realityObjId.ToLong()]);
                            }

                            continue;
                        }
                    }
                    */

                    var messageRo = new StringBuilder();

                    var currentDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realityObjId)
                        ? DictDisclosureInfoRealObj[realityObjId].FirstOrDefault(x => x.PeriodDi.Id == periodFromId)
                        : null;
                    if (currentDisclosureInfoRealObj == null)
                    {
                        continue;
                    }

                    var communalServices = DictCommunalService.Get(currentDisclosureInfoRealObj.Id);
                    if (communalServices != null)
                    {
                        foreach (var communalService in communalServices)
                        {
                            var key = string.Format("{0}_{1}", realityObjId, communalService.TemplateService.Id);

                            if (!existKeys.ContainsKey(key))
                            {
                                if (!this.CopyCommunalService(communalService, manorgId, realityObjId, periodCurrentId))
                                {
                                    messageRo.AppendFormat("\t{0} - {1};\n", communalService.TemplateService.Name, "Не скопирована");
                                }
                            }
                            else
                            {
                                // логируем - услуга уже существует
                                messageRo.AppendFormat("\t{0} - {1};\n", communalService.TemplateService.Name, "Уже существует");
                            }
                        }
                    }

                    var housingServices = DictHousingService.Get(currentDisclosureInfoRealObj.Id);
                    if (housingServices != null)
                    {
                        foreach (var housingService in housingServices)
                        {
                            var key = string.Format("{0}_{1}", realityObjId, housingService.TemplateService.Id);

                            if (!existKeys.ContainsKey(key))
                            {
                                if (!this.CopyHousingService(housingService, realityObjId, periodCurrentId))
                                {
                                    messageRo.AppendFormat("\t{0} - {1};\n", housingService.TemplateService.Name, "Не скопирована");
                                }
                            }
                            else
                            {
                                // логируем - услуга уже существует
                                messageRo.AppendFormat("\t{0} - {1};\n", housingService.TemplateService.Name, "Уже существует");
                            }
                        }
                    }

                    var repairServices = DictRepairService.Get(currentDisclosureInfoRealObj.Id);
                    if (repairServices != null)
                    {
                        foreach (var repairService in repairServices)
                        {
                            var key = string.Format("{0}_{1}", realityObjId, repairService.TemplateService.Id);

                            if (!existKeys.ContainsKey(key))
                            {
                                if (!this.CopyRepairService(repairService, realityObjId, periodCurrentId))
                                {
                                    messageRo.AppendFormat("\t{0} - {1};\n", repairService.TemplateService.Name, "Не скопирована");
                                }
                            }
                            else
                            {
                                // логируем - услуга уже существует
                                messageRo.AppendFormat("\t{0} - {1};\n", repairService.TemplateService.Name, "Уже существует");
                            }
                        }
                    }

                    var capRepairServices = DictCapRepairService.Get(currentDisclosureInfoRealObj.Id);
                    if (capRepairServices != null)
                    {
                        foreach (var capRepairService in capRepairServices)
                        {
                            var key = string.Format("{0}_{1}", realityObjId, capRepairService.TemplateService.Id);

                            if (!existKeys.ContainsKey(key))
                            {
                                if (!this.CopyCapRepairService(capRepairService, realityObjId, periodCurrentId))
                                {
                                    messageRo.AppendFormat("\t{0} - {1};\n", capRepairService.TemplateService.Name, "Не скопирована");
                                }
                            }
                            else
                            {
                                // логируем - услуга уже существует
                                messageRo.AppendFormat("\t{0} - {1};\n", capRepairService.TemplateService.Name, "Уже существует");
                            }
                        }
                    }

                    var controlServices = DictControlService.Get(currentDisclosureInfoRealObj.Id);
                    if (controlServices != null)
                    {
                        foreach (var controlService in controlServices)
                        {
                            var key = string.Format("{0}_{1}", realityObjId, controlService.TemplateService.Id);

                            if (!existKeys.ContainsKey(key))
                            {
                                if (!this.CopyControlService(controlService, realityObjId, periodCurrentId))
                                {
                                    messageRo.AppendFormat("\t{0} - {1};\n", controlService.TemplateService.Name, "Не скопирована");
                                }
                            }
                            else
                            {
                                // логируем - услуга уже существует
                                messageRo.AppendFormat("\t{0} - {1};\n", controlService.TemplateService.Name, "Уже существует");
                            }
                        }
                    }

                    var additionalServices = DictAdditionalService.Get(currentDisclosureInfoRealObj.Id);
                    if (additionalServices != null)
                    {
                        foreach (var additionalService in additionalServices)
                        {
                            var key = string.Format("{0}_{1}", realityObjId, additionalService.TemplateService.Id);

                            if (!existKeys.ContainsKey(key))
                            {
                                if (!this.CopyAdditionalService(additionalService, realityObjId, periodCurrentId))
                                {
                                    messageRo.AppendFormat("\t{0} - {1};\n", additionalService.TemplateService.Name, "Не скопирована");
                                }
                            }
                            else
                            {
                                // логируем - услуга уже существует
                                messageRo.AppendFormat("\t{0} - {1};\n", additionalService.TemplateService.Name, "Уже существует");
                            }
                        }
                    }

                    if (messageRo.Length > 0)
                    {
                        messageMain.AppendFormat("{0}:\n", DictRealObj.Get(realityObjId));
                        messageMain.AppendLine(messageRo.ToString());
                    }

                }

                this.SaveServices();

                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                var finalStr = new StringBuilder();
                finalStr.AppendFormat("Копирование услуг из отчетного периода {0} в отчетный период {1} по УО = {2}\n",
                    periodFrom.Name,
                    periodCurrent.Name,
                    manOrg.Contragent.Name);

                if (messageMain.Length > 0)
                {
                    finalStr.Append("Часть услуг нескопированы, поскольку уже существуют. Подробно:");
                }
                else
                {
                    finalStr.Append("Все услуги скопированы успешно.");
                }

                using (var logFile = new MemoryStream())
                {
                    var log = Encoding.GetEncoding(1251).GetBytes(finalStr.ToString() + messageMain);

                    logsZip.AddEntry(string.Format("{0}.csv", logOperation.OperationType.GetEnumMeta().Display), log);

                    logsZip.Save(logFile);

                    var logFileInfo = fileManager.SaveFile(logFile, string.Format("{0}.zip", logOperation.OperationType.GetEnumMeta().Display));

                    logOperation.LogFile = logFileInfo;
                }

                logOperation.EndDate = DateTime.UtcNow;
                logOperation.Comment = "УО = {0}.Копирование услуг из периода в период".FormatUsing(manOrg.Contragent.Name);
                logOperationDomain.Save(logOperation);

                return new BaseDataResult
                {
                    Success = true,
                    Message = messageMain.Length > 0
                        ? "Часть услуг нескопированы, поскольку уже существуют."
                        : "Все услуги скопированы успешно.",
                    Data = logOperation.LogFile.Id
                };

            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <inheritdoc/>>
        public IDataResult CopyUninhabitablePremisesPeriod(BaseParams baseParams)
        {
            var nonResidentialPlacementRepository = this.Container.ResolveRepository<NonResidentialPlacement>();
            var periodDiRepository = this.Container.ResolveRepository<PeriodDi>();

            using (this.Container.Using(nonResidentialPlacementRepository, periodDiRepository))
            {
                var manorgId = baseParams.Params["manorgId"].ToLong();
                var periodCurrentId = baseParams.Params["periodCurrentId"].ToLong();
                var periodFromId = baseParams.Params["periodFromId"].ToLong();
                var periodDiCurrent = periodDiRepository.GetAll().FirstOrDefault(w => w.Id == periodCurrentId);

                var nonResidentialPlacementQueryable = nonResidentialPlacementRepository.GetAll()
                    .Where(w => w.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId
                        && periodFromId == w.DisclosureInfoRealityObj.PeriodDi.Id);

                var fromDisclosureInfoRealityObjDictionary = this.DisclosureInfoRealObjRepository.GetAll()
                    .Where(w => w.ManagingOrganization.Id == manorgId
                        && periodFromId == w.PeriodDi.Id)
                    .ToDictionary(k => new { k.RealityObject, k.ManagingOrganization }.GetHashCode());

                var currentDisclosureInfoRealityObjDictionary = this.DisclosureInfoRealObjRepository.GetAll()
                    .Where(w => w.ManagingOrganization.Id == manorgId
                        && periodCurrentId == w.PeriodDi.Id)
                    .ToDictionary(k => new { k.RealityObject, k.ManagingOrganization }.GetHashCode());

                var nonResidentialPlacement = nonResidentialPlacementQueryable
                    .GroupBy(g => g.DisclosureInfoRealityObj)
                    .ToArray()
                    .Select(s => new
                    {
                        s.Key.RealityObject,
                        s.Key.ManagingOrganization,
                        s.Key.NonResidentialPlacement
                    });

                // Обновление существующих объектов недвижимости
                foreach (var elem in currentDisclosureInfoRealityObjDictionary)
                {
                    var index = new { elem.Value.RealityObject, elem.Value.ManagingOrganization }.GetHashCode();

                    if (fromDisclosureInfoRealityObjDictionary.TryGetValue(index, out var disclosureInfoRealityObj))
                    {
                        elem.Value.NonResidentialPlacement = disclosureInfoRealityObj.NonResidentialPlacement;
                    }
                }

                // Добавление недостающих DisclosureInfoRealityObj
                foreach (var elem in nonResidentialPlacement)
                {
                    var index = new { elem.RealityObject, elem.ManagingOrganization }.GetHashCode();

                    if (!currentDisclosureInfoRealityObjDictionary.TryGetValue(index, out var disclosureInfoRealityObj))
                    {
                        disclosureInfoRealityObj = new DisclosureInfoRealityObj
                        {
                            PeriodDi = periodDiCurrent,
                            RealityObject = elem.RealityObject,
                            ManagingOrganization = elem.ManagingOrganization
                        };

                        currentDisclosureInfoRealityObjDictionary.Add(index, disclosureInfoRealityObj);
                    }

                    disclosureInfoRealityObj.NonResidentialPlacement = elem.NonResidentialPlacement;
                }

                // Перенос данных
                var nonResidentialPlacementList = nonResidentialPlacementQueryable
                    .ToArray()
                    .Select(s =>
                    {
                        var index = new { s.DisclosureInfoRealityObj.RealityObject, s.DisclosureInfoRealityObj.ManagingOrganization }.GetHashCode();
                        currentDisclosureInfoRealityObjDictionary.TryGetValue(index, out var disclosureInfoRealityObj);

                        return new NonResidentialPlacement
                        {
                            Id = 0,
                            DisclosureInfoRealityObj = disclosureInfoRealityObj,
                            ObjectCreateDate = DateTime.UtcNow,

                            Area = s.Area,
                            ContragentName = s.ContragentName,
                            DateEnd = s.DateEnd,
                            DateStart = s.DateStart,
                            DocumentDateApartment = s.DocumentDateApartment,
                            DocumentDateCommunal = s.DocumentDateCommunal,
                            DocumentNameApartment = s.DocumentNameApartment,
                            DocumentNameCommunal = s.DocumentNameCommunal,
                            DocumentNumApartment = s.DocumentNumApartment,
                            DocumentNumCommunal = s.DocumentNumCommunal,
                            ExternalId = s.ExternalId,
                            ImportEntityId = s.ImportEntityId,
                            ObjectEditDate = s.ObjectEditDate,
                            ObjectVersion = s.ObjectVersion,
                            TypeContragentDi = s.TypeContragentDi
                        };
                    })
                    .ToArray();

                // Копирование
                this.InTransaction(currentDisclosureInfoRealityObjDictionary.Values, this.DisclosureInfoRealObjRepository);
                this.InTransaction(nonResidentialPlacementList, nonResidentialPlacementRepository);

                return new BaseDataResult(true, "Объекты успешно скопированы");
            }
        }

        /// <inheritdoc/>>
        public IDataResult CopyCommonAreasPeriod(BaseParams baseParams)
        {
            var infoAboutUseCommonFacilitiesRepository = this.Container.Resolve<IRepository<InfoAboutUseCommonFacilities>>();
            var periodDiRepository = this.Container.ResolveRepository<PeriodDi>();

            using (this.Container.Using(infoAboutUseCommonFacilitiesRepository, periodDiRepository))
            {
                var manorgId = baseParams.Params["manorgId"].ToLong();
                var periodCurrentId = baseParams.Params["periodCurrentId"].ToLong();
                var periodFromId = baseParams.Params["periodFromId"].ToLong();
                var periodDiCurrent = periodDiRepository.GetAll().FirstOrDefault(w => w.Id == periodCurrentId);

                var infoAboutUseCommonFacilitiesQueryable = infoAboutUseCommonFacilitiesRepository.GetAll()
                    .Where(w => w.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId
                        && periodFromId == w.DisclosureInfoRealityObj.PeriodDi.Id);

                var fromDisclosureInfoRealityObjDictionary = this.DisclosureInfoRealObjRepository.GetAll()
                    .Where(w => w.ManagingOrganization.Id == manorgId
                        && periodFromId == w.PeriodDi.Id)
                    .ToDictionary(k => new { k.RealityObject, k.ManagingOrganization }.GetHashCode());

                var currentDisclosureInfoRealityObjDictionary = this.DisclosureInfoRealObjRepository.GetAll()
                    .Where(w => w.ManagingOrganization.Id == manorgId
                        && periodCurrentId == w.PeriodDi.Id)
                    .ToDictionary(k => new { k.RealityObject, k.ManagingOrganization }.GetHashCode());

                var infoAboutUseCommonFacilities = infoAboutUseCommonFacilitiesQueryable
                    .GroupBy(g => g.DisclosureInfoRealityObj)
                    .ToArray()
                    .Select(s => new
                    {
                        s.Key.RealityObject,
                        s.Key.ManagingOrganization,
                        s.Key.PlaceGeneralUse
                    });

                // Обновление существующих объектов недвижимости
                foreach (var elem in currentDisclosureInfoRealityObjDictionary)
                {
                    var index = new { elem.Value.RealityObject, elem.Value.ManagingOrganization }.GetHashCode();

                    if (fromDisclosureInfoRealityObjDictionary.TryGetValue(index, out var disclosureInfoRealityObj))
                    {
                        elem.Value.PlaceGeneralUse = disclosureInfoRealityObj.PlaceGeneralUse;
                    }
                }

                // Добавление недостающих DisclosureInfoRealityObj
                foreach (var elem in infoAboutUseCommonFacilities)
                {
                    var index = new { elem.RealityObject, elem.ManagingOrganization }.GetHashCode();

                    if (!currentDisclosureInfoRealityObjDictionary.TryGetValue(index, out var disclosureInfoRealityObj))
                    {
                        disclosureInfoRealityObj = new DisclosureInfoRealityObj
                        {
                            PeriodDi = periodDiCurrent,
                            RealityObject = elem.RealityObject,
                            ManagingOrganization = elem.ManagingOrganization
                        };

                        currentDisclosureInfoRealityObjDictionary.Add(index, disclosureInfoRealityObj);
                    }

                    disclosureInfoRealityObj.PlaceGeneralUse = elem.PlaceGeneralUse;
                }

                // Изменение
                var infoAboutUseCommonFacilitiesList = infoAboutUseCommonFacilitiesQueryable
                    .ToArray()
                    .Select(s =>
                    {
                        var index = new { s.DisclosureInfoRealityObj.RealityObject, s.DisclosureInfoRealityObj.ManagingOrganization }.GetHashCode();
                        currentDisclosureInfoRealityObjDictionary.TryGetValue(index, out var disclosureInfoRealityObj);

                        return new InfoAboutUseCommonFacilities
                        {
                            Id = 0,
                            DisclosureInfoRealityObj = disclosureInfoRealityObj,
                            ProtocolFile = this.ReCreateFile(s.ProtocolFile, this.FileManager),
                            ContractFile = this.ReCreateFile(s.ContractFile, this.FileManager),
                            ObjectCreateDate = DateTime.UtcNow,

                            AppointmentCommonFacilities = s.AppointmentCommonFacilities,
                            AreaOfCommonFacilities = s.AreaOfCommonFacilities,
                            BirthDate = s.BirthDate,
                            BirthPlace = s.BirthPlace,
                            Comment = s.Comment,
                            ContractDate = s.ContractDate,
                            ContractNumber = s.ContractNumber,
                            ContractSubject = s.ContractSubject,
                            CostByContractInMonth = s.CostByContractInMonth,
                            CostContract = s.CostContract,
                            DateEnd = s.DateEnd,
                            DateStart = s.DateStart,
                            DayMonthPeriodIn = s.DayMonthPeriodIn,
                            DayMonthPeriodOut = s.DayMonthPeriodOut,
                            ExternalId = s.ExternalId,
                            From = s.From,
                            Gender = s.Gender,
                            ImportEntityId = s.ImportEntityId,
                            Inn = s.Inn,
                            IsLastDayMonthPeriodIn = s.IsLastDayMonthPeriodIn,
                            IsLastDayMonthPeriodOut = s.IsLastDayMonthPeriodOut,
                            IsNextMonthPeriodIn = s.IsNextMonthPeriodIn,
                            IsNextMonthPeriodOut = s.IsNextMonthPeriodOut,
                            KindCommomFacilities = s.KindCommomFacilities,
                            Lessee = s.Lessee,
                            LesseeType = s.LesseeType,
                            Name = s.Name,
                            Number = s.Number,
                            ObjectVersion = s.ObjectVersion,
                            Ogrn = s.Ogrn,
                            Patronymic = s.Patronymic,
                            SigningContractDate = s.SigningContractDate,
                            Snils = s.Snils,
                            Surname = s.Surname,
                            TypeContract = s.TypeContract
                        };
                    })
                    .ToArray();

                // Копирование
                this.InTransaction(currentDisclosureInfoRealityObjDictionary.Values, this.DisclosureInfoRealObjRepository);
                this.InTransaction(infoAboutUseCommonFacilitiesList.Where(w => w.Id == 0), infoAboutUseCommonFacilitiesRepository);

                return new BaseDataResult(true, "Объекты успешно скопированы");
            }
        }

        /// <summary>
        /// Возвращает список не добавленных обязательных услуг
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        public IDataResult GetUnfilledMandatoryServs(BaseParams baseParams)
        {
            var disclosureInfoRealityObjId = baseParams.Params["disclosureInfoRealityObjId"].ToLong();

            var unfilledMandatoryServs = this.GetUnfilledMandatoryServsNameList(disclosureInfoRealityObjId);

            var message = string.Empty;

            if (unfilledMandatoryServs.Count > 0)
            {
                message = unfilledMandatoryServs.Aggregate(message, (current, str) => current + string.Format(" - {0}<br>", str));
                if (!string.IsNullOrEmpty(message))
                {
                    message = string.Format("Не добавлены следующие услуги: <br>{0}", message);
                }

                return new BaseDataResult { Success = true, Message = message };
            }

            return new BaseDataResult { Success = true, Message = "Все обязательные услуги заполнены" };
        }

        /// <summary>
        /// Возвращает список не добавленных обязательных услуг
        /// </summary>
        /// <param name="disclosureInfoRealityObjId">Идентификатор DisclosureInfoRealityObj"/></param>
        /// <returns>Список названий услуг</returns>
        public IList<string> GetUnfilledMandatoryServsNameList(long disclosureInfoRealityObjId)
        {
            var diRoDomain = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var tehPassportDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();
            var baseServiceDomain = this.Container.Resolve<IDomainService<BaseService>>();

            try
            {
                var realObjDi = diRoDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoRealityObjId);
                var realObj = realObjDi?.RealityObject;

                var isNotChute = false;
                if (realObj != null)
                {
                    isNotChute = tehPassportDomain.GetAll()
                        .Any(x => x.TehPassport.RealityObject.Id == realObj.Id && x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value == "0");
                }

                var fillMandatoryServs = baseServiceDomain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.Id == realObjDi.Id && x.TemplateService.IsMandatory)
                    .Select(x => x.TemplateService.Id)
                    .Distinct()
                    .ToList();


                var result = this.TemplateServiceDomain.GetAll()
                    .Where(x => x.IsMandatory && !fillMandatoryServs.Contains(x.Id))
                    .Where(x =>
                        (x.ActualYearStart.HasValue && realObjDi.PeriodDi.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                        && (x.ActualYearEnd.HasValue && realObjDi.PeriodDi.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue))
                    .WhereIf(realObj != null && realObj.NumberLifts == 0, x => !(x.Code == "27" || x.Code == "28"))
                    .WhereIf(isNotChute, x => x.Code != "7")
                    .Select(x => x.Name)
                    .ToList();

                return result;
            }
            finally
            {
                this.Container.Release(diRoDomain);
                this.Container.Release(tehPassportDomain);
                this.Container.Release(baseServiceDomain);
            }
        }

        public IDictionary<long, string[]> GetUnfilledMandatoryServsNameList(IQueryable<DisclosureInfoRealityObj> diRoQuery)
        {
            var templateServiceDomain = this.Container.Resolve<IDomainService<TemplateService>>();
            var tehPassportDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();
            var baseServiceDomain = this.Container.Resolve<IDomainService<BaseService>>();

            using (this.Container.Using(templateServiceDomain, tehPassportDomain, baseServiceDomain))
            {
                var mandatoryTemplateServices = templateServiceDomain.GetAll()
                    .Where(x => x.IsMandatory)
                    .ToArray();

                var isNotChute = tehPassportDomain.GetAll()
                    .Where(x => diRoQuery.Any(y => y.RealityObject.Id == x.TehPassport.RealityObject.Id))
                    .Where(x => x.FormCode == "Form_3_7" && x.CellCode == "1:3" && x.Value == "0")
                    .Select(x => x.TehPassport.RealityObject.Id)
                    .ToHashSet();

                var existsMandatoryServices = baseServiceDomain.GetAll()
                    .Where(x => diRoQuery.Any(y => y.Id == x.DisclosureInfoRealityObj.Id))
                    .Select(x => new
                    {
                        x.DisclosureInfoRealityObj.Id,
                        ServiceId = x.TemplateService.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.ServiceId)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                return diRoQuery
                    .Select(x => new { x.Id, x.PeriodDi.DateStart, x.RealityObject.NumberLifts, RoId = x.RealityObject.Id })
                    .AsEnumerable()
                    .ToDictionary(
                        x => x.Id,
                        y => mandatoryTemplateServices
                            .Where(x => x.IsMandatory && !existsMandatoryServices.Get(y.Id).AllOrEmpty().Contains(x.Id))
                            .WhereIf(y.NumberLifts == 0, x => !(x.Code == "27" || x.Code == "28"))
                            .WhereIf(isNotChute.Contains(y.RoId), x => x.Code != "7")
                            .Where(x =>
                                (x.ActualYearStart.HasValue && y.DateStart.Value.Year >= x.ActualYearStart.Value || !x.ActualYearStart.HasValue)
                                && (x.ActualYearEnd.HasValue && y.DateStart.Value.Year <= x.ActualYearEnd.Value || !x.ActualYearEnd.HasValue))
                            .Select(x => x.Name)
                            .ToArray());
            }
        }

        #region Методы инициализации коллекций

        /// <summary>
        ///  Инициализируем коллекции
        /// </summary>
        /// <param name="periodIds">Периоды</param>
        /// <param name="initParams">Список параметров для кэширования</param>
        protected virtual void InitCollections(List<long> periodIds, InitParams initParams, long manorgId, List<long> realityObjIds = null)
        {
            DictPeriodDi = PeriodDiRepository.GetAll().Where(x => periodIds.Contains(x.Id)).ToDictionary(x => x.Id);

            // IRepository для того что бы получать дома вне зависимости от ограничений.
            // Например при копирование из 2012 в 2013 период должны учитываться дома с которыми был договор в 2012 году, а в 2013 нет
            DictRealObj = this.Container.Resolve<IRepository<RealityObject>>()
                .GetAll()
                .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Address })
                .AsEnumerable()
                .GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.Select(z => z.Address).FirstOrDefault());


            DictBaseService = BaseRepository
                .GetAll()
                .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.DisclosureInfoRealityObj.RealityObject.Id).ToDictionary(x => x.Key, y => y.ToList());

            DictDisclosureInfoRealObj = DisclosureInfoRealObjRepository
                .GetAll()
                .Where(x => periodIds.Contains(x.PeriodDi.Id))
                .Where(x => x.ManagingOrganization.Id == manorgId)
                .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            if (initParams.All || initParams.CommunalServ)
            {
                DictCommunalService = CommunalRepository
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id).ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.HousingServ)
            {
                DictHousingService = HousingRepository
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.AdditionalServ)
            {
                DictAdditionalService = AdditionalRepository
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.ControlServ)
            {
                DictControlService = ControlRepository
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.RepairServ)
            {
                DictRepairService = RepairRepository
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                DictPlanWorkServiceRepairWorks = PlanWorkServiceRepairWorksRepository.GetAll()
                    .Where(x => x.PlanWorkServiceRepair.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.PlanWorkServiceRepair.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.PlanWorkServiceRepair.DisclosureInfoRealityObj.RealityObject.Id))
                    .Select(x => new
                        {
                            BaseServiceId = (long?)x.PlanWorkServiceRepair.BaseService.Id,
                            GroupWorkPprId = (long?)x.WorkRepairList.GroupWorkPpr.Id,
                            x.Cost,
                            x.DataComplete,
                            x.DateComplete,
                            x.DateEnd,
                            x.DateStart,
                            PeriodicityId = (long?)x.PeriodicityTemplateService.Id,
                            x.ReasonRejection,
                            x.FactCost
                        })
                    .AsEnumerable()
                    .Where(x => x.BaseServiceId != null)
                    .Where(x => x.GroupWorkPprId != null)
                    .GroupBy(x => x.BaseServiceId.Value)
                    .ToDictionary(
                        x => x.Key,
                        x => x.GroupBy(y => y.GroupWorkPprId.Value)
                              .ToDictionary(
                                  y => y.Key,
                                  y => y.Select(z => new PlanWorkServiceRepairWorks
                                        {
                                            Cost = z.Cost,
                                            DataComplete = z.DataComplete,
                                            DateComplete = z.DateComplete,
                                            DateEnd = z.DateEnd,
                                            DateStart = z.DateStart,
                                            PeriodicityTemplateService = z.PeriodicityId.HasValue ? new PeriodicityTemplateService { Id = z.PeriodicityId.Value } : null,
                                            ReasonRejection = z.ReasonRejection,
                                            FactCost = z.FactCost
                                        })
                                        .First()));
            }

            if (initParams.All || initParams.CapRepairServ)
            {
                DictCapRepairService = CapRepairRepository
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.TariffConsumers)
            {
                DictTariffForConsumers = TariffForConsumRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.TariffRso)
            {
                DictTariffForRso = TariffForRsoRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.CostItem)
            {
                DictCostItem = CostItemRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.ProviderService)
            {
                DictProviderService = ProviderServiceRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.WorkCapRepair)
            {
                DictWorkCapRepair = WorkCapRepairRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }

            InitWorkRepairList(periodIds, initParams, manorgId, realityObjIds);
            InitWorkRepairDetail(periodIds, initParams, manorgId, realityObjIds);

            if (initParams.All || initParams.WorkRepairTechServ)
            {
                DictWorkRepairTechServ = WorkRepairTechServRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id).ToDictionary(x => x.Key, y => y.ToList());
            }

            if (initParams.All || initParams.ConsumptionNormsNpa)
            {
                DictConsumptionNormsNpa = ConsumptionNormsNpaRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id).ToDictionary(x => x.Key, y => y.ToList());
            }

        }

        protected virtual void InitWorkRepairDetail(List<long> periodIds, InitParams initParams, long manorgId, List<long> realityObjIds = null)
        {
            if (initParams.All || initParams.WorkRepairDetail)
            {
                DictWorkRepairDetail = WorkRepairDetailRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
            }
        }

        protected virtual void InitWorkRepairList(List<long> periodIds, InitParams initParams, long manorgId, List<long> realityObjIds = null)
        {
            if (initParams.All || initParams.WorkRepairList)
            {
                DictWorkRepairList = WorkRepairListRepository
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.ManagingOrganization.Id == manorgId)
                    .Where(x => periodIds.Contains(x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id))
                    .WhereIf(realityObjIds != null, x => realityObjIds.Contains(x.BaseService.DisclosureInfoRealityObj.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.Id).ToDictionary(x => x.Key, y => y.ToList());
            }
        }

        #endregion Методы инициализации коллекций

        #region Формирование списков на создание

        protected virtual bool CopyCommunalService(BaseService oldService, long manorgId, long realObjId = 0, long periodId = 0)
        {
            if (!DictCommunalService.ContainsKey(oldService.DisclosureInfoRealityObj.Id))
            {
                return false;
            }

            var oldCommunalService = DictCommunalService[oldService.DisclosureInfoRealityObj.Id].FirstOrDefault(x => x.Id == oldService.Id);

            if (oldCommunalService == null)
            {
                return false;
            }

            if (realObjId == 0)
            {
                realObjId = oldCommunalService.DisclosureInfoRealityObj.RealityObject.Id;
            }

            if (manorgId == 0)
            {
                manorgId = oldCommunalService.DisclosureInfoRealityObj.ManagingOrganization.Id;
            }

            if (periodId == 0)
            {
                periodId = oldCommunalService.DisclosureInfoRealityObj.PeriodDi.Id;
            }

            var newDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realObjId) ? DictDisclosureInfoRealObj[realObjId].FirstOrDefault(x => x.PeriodDi.Id == periodId) : null;

            if (newDisclosureInfoRealObj == null)
            {
                newDisclosureInfoRealObj = ListSaveDisclosureInfoRealityObj.FirstOrDefault(x => x.PeriodDi.Id == periodId && x.RealityObject.Id == realObjId);
                
                if (newDisclosureInfoRealObj == null)
                {
                    newDisclosureInfoRealObj = new DisclosureInfoRealityObj
                                        {
                                            Id = 0,
                                            PeriodDi = new PeriodDi { Id = periodId },
                                            RealityObject = new RealityObject { Id = realObjId },
                                            ManagingOrganization = new ManagingOrganization {Id = manorgId }
                    };

                    ListSaveDisclosureInfoRealityObj.Add(newDisclosureInfoRealObj);
                }
            }

            var newCommunalService = new CommunalService
            {
                Id = 0,
                TemplateService = oldCommunalService.TemplateService,
                DisclosureInfoRealityObj = newDisclosureInfoRealObj,
                Provider = oldCommunalService.Provider,
                Profit = oldCommunalService.Profit,
                UnitMeasure = oldCommunalService.UnitMeasure,
                TypeOfProvisionService = oldCommunalService.TypeOfProvisionService,
                VolumePurchasedResources = oldCommunalService.VolumePurchasedResources,
                PricePurchasedResources = oldCommunalService.PricePurchasedResources,
                KindElectricitySupply = oldCommunalService.KindElectricitySupply,
                TariffForConsumers = oldCommunalService.TariffForConsumers,
                TariffIsSetForDi = oldCommunalService.TariffIsSetForDi,
                DateStartTariff = oldCommunalService.DateStartTariff,
                ConsumptionNormLivingHouse = oldCommunalService.ConsumptionNormLivingHouse,
                UnitMeasureLivingHouse = oldCommunalService.UnitMeasureLivingHouse,
                AdditionalInfoLivingHouse = oldCommunalService.AdditionalInfoLivingHouse,
                ConsumptionNormHousing = oldCommunalService.ConsumptionNormHousing,
                UnitMeasureHousing = oldCommunalService.UnitMeasureHousing,
                AdditionalInfoHousing = oldCommunalService.AdditionalInfoHousing
            };
            ListSaveCommunalService.Add(newCommunalService);

            this.CopyTariffForConsumers(newCommunalService, oldCommunalService.Id);
            this.CopyTariffForRso(newCommunalService, oldCommunalService.Id);
            this.CopyProviderService(newCommunalService, oldCommunalService.Id);
            this.CopyConsumptionNormsNpa(newCommunalService, oldCommunalService.Id);

            return true;
        }

        protected virtual bool CopyHousingService(BaseService oldService, long realObjId = 0, long periodId = 0)
        {
            if (!DictHousingService.ContainsKey(oldService.DisclosureInfoRealityObj.Id))
            {
                return false;
            }

            var oldHousingService = DictHousingService[oldService.DisclosureInfoRealityObj.Id].FirstOrDefault(x => x.Id == oldService.Id);

            if (oldHousingService == null)
            {
                return false;
            }

            if (realObjId == 0)
            {
                realObjId = oldHousingService.DisclosureInfoRealityObj.RealityObject.Id;
            }

            if (periodId == 0)
            {
                periodId = oldHousingService.DisclosureInfoRealityObj.PeriodDi.Id;
            }

            if (!DictDisclosureInfoRealObj.ContainsKey(realObjId))
            {
                return false;
            }

            var newDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realObjId) ? DictDisclosureInfoRealObj[realObjId].FirstOrDefault(x => x.PeriodDi.Id == periodId) : null;

            if (newDisclosureInfoRealObj == null)
            {
                newDisclosureInfoRealObj = ListSaveDisclosureInfoRealityObj.FirstOrDefault(x => x.PeriodDi.Id == periodId && x.RealityObject.Id == realObjId);

                if (newDisclosureInfoRealObj == null)
                {
                    newDisclosureInfoRealObj = new DisclosureInfoRealityObj
                    {
                        Id = 0,
                        PeriodDi = new PeriodDi { Id = periodId },
                        RealityObject = new RealityObject { Id = realObjId },
                        ManagingOrganization = oldService.DisclosureInfoRealityObj.ManagingOrganization
                    };

                    ListSaveDisclosureInfoRealityObj.Add(newDisclosureInfoRealObj);
                }
            }

            var newHousingService = new HousingService
            {
                Id = 0,
                TemplateService = oldHousingService.TemplateService,
                DisclosureInfoRealityObj = newDisclosureInfoRealObj,
                Provider = oldHousingService.Provider,
                Profit = oldHousingService.Profit,
                UnitMeasure = oldHousingService.UnitMeasure,
                TypeOfProvisionService = oldHousingService.TypeOfProvisionService,
                Periodicity = oldHousingService.Periodicity,
                Equipment = oldHousingService.Equipment,
                ProtocolNumber = oldHousingService.ProtocolNumber,
                ProtocolFrom = oldHousingService.ProtocolFrom,
                TariffForConsumers = oldHousingService.TariffForConsumers,
                TariffIsSetForDi = oldHousingService.TariffIsSetForDi,
                DateStartTariff = oldHousingService.DateStartTariff,
                Protocol = this.ReCreateFile(oldHousingService.Protocol, this.FileManager)
            };

            ListSaveHousingService.Add(newHousingService);

            this.CopyTariffForConsumers(newHousingService, oldHousingService.Id);
            this.CopyCostItem(newHousingService, oldHousingService.Id);
            this.CopyProviderService(newHousingService, oldHousingService.Id);

            return true;
        }

        public bool CopyAdditionalService(BaseService oldService, long realObjId = 0, long periodId = 0)
        {
            if (!DictAdditionalService.ContainsKey(oldService.DisclosureInfoRealityObj.Id))
            {
                return false;
            }

            var oldAdditionalService = DictAdditionalService[oldService.DisclosureInfoRealityObj.Id].FirstOrDefault(x => x.Id == oldService.Id);

            if (oldAdditionalService == null)
            {
                return false;
            }

            if (realObjId == 0)
            {
                realObjId = oldAdditionalService.DisclosureInfoRealityObj.RealityObject.Id;
            }

            if (periodId == 0)
            {
                periodId = oldAdditionalService.DisclosureInfoRealityObj.PeriodDi.Id;
            }

            var newDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realObjId) ? DictDisclosureInfoRealObj[realObjId].FirstOrDefault(x => x.PeriodDi.Id == periodId) : null;

            if (newDisclosureInfoRealObj == null)
            {
                newDisclosureInfoRealObj = ListSaveDisclosureInfoRealityObj.FirstOrDefault(x => x.PeriodDi.Id == periodId && x.RealityObject.Id == realObjId);

                if (newDisclosureInfoRealObj == null)
                {
                    newDisclosureInfoRealObj = new DisclosureInfoRealityObj
                    {
                        Id = 0,
                        PeriodDi = new PeriodDi { Id = periodId },
                        RealityObject = new RealityObject { Id = realObjId },
                        ManagingOrganization = oldService.DisclosureInfoRealityObj.ManagingOrganization
                    };

                    ListSaveDisclosureInfoRealityObj.Add(newDisclosureInfoRealObj);
                }
            }

            var newAdditionalService = new AdditionalService
            {
                Id = 0,
                TemplateService = oldAdditionalService.TemplateService,
                DisclosureInfoRealityObj = newDisclosureInfoRealObj,
                Provider = oldAdditionalService.Provider,
                Profit = oldAdditionalService.Profit,
                UnitMeasure = oldAdditionalService.UnitMeasure,
                Periodicity = oldAdditionalService.Periodicity,
                Document = oldAdditionalService.Document,
                DocumentNumber = oldAdditionalService.DocumentNumber,
                DocumentFrom = oldAdditionalService.DocumentFrom,
                DateStart = oldAdditionalService.DateStart,
                DateEnd = oldAdditionalService.DateEnd,
                Total = oldAdditionalService.Total,
                TariffForConsumers = oldAdditionalService.TariffForConsumers,
                TariffIsSetForDi = oldAdditionalService.TariffIsSetForDi,
                DateStartTariff = oldAdditionalService.DateStartTariff
            };

            ListSaveAdditionalService.Add(newAdditionalService);

            this.CopyTariffForConsumers(newAdditionalService, oldAdditionalService.Id);
            this.CopyProviderService(newAdditionalService, oldAdditionalService.Id);

            return true;
        }

        protected virtual bool CopyControlService(BaseService oldService, long realObjId = 0, long periodId = 0)
        {
            if (!DictControlService.ContainsKey(oldService.DisclosureInfoRealityObj.Id))
            {
                return false;
            }

            var oldControlService = DictControlService[oldService.DisclosureInfoRealityObj.Id].FirstOrDefault(x => x.Id == oldService.Id);

            if (oldControlService == null)
            {
                return false;
            }

            if (realObjId == 0)
            {
                realObjId = oldControlService.DisclosureInfoRealityObj.RealityObject.Id;
            }

            if (periodId == 0)
            {
                periodId = oldControlService.DisclosureInfoRealityObj.PeriodDi.Id;
            }

            var newDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realObjId) ? DictDisclosureInfoRealObj[realObjId].FirstOrDefault(x => x.PeriodDi.Id == periodId) : null;

            if (newDisclosureInfoRealObj == null)
            {
                newDisclosureInfoRealObj = ListSaveDisclosureInfoRealityObj.FirstOrDefault(x => x.PeriodDi.Id == periodId && x.RealityObject.Id == realObjId);

                if (newDisclosureInfoRealObj == null)
                {
                    newDisclosureInfoRealObj = new DisclosureInfoRealityObj
                    {
                        Id = 0,
                        PeriodDi = new PeriodDi { Id = periodId },
                        RealityObject = new RealityObject { Id = realObjId },
                        ManagingOrganization = oldService.DisclosureInfoRealityObj.ManagingOrganization
                    };

                    ListSaveDisclosureInfoRealityObj.Add(newDisclosureInfoRealObj);
                }
            }

            var newControlService = new ControlService
            {
                Id = 0,
                TemplateService = oldControlService.TemplateService,
                DisclosureInfoRealityObj = newDisclosureInfoRealObj,
                Provider = oldControlService.Provider,
                Profit = oldControlService.Profit,
                UnitMeasure = oldControlService.UnitMeasure,
                TariffForConsumers = oldControlService.TariffForConsumers,
                TariffIsSetForDi = oldControlService.TariffIsSetForDi,
                DateStartTariff = oldControlService.DateStartTariff
            };

            ListSaveControlService.Add(newControlService);

            this.CopyTariffForConsumers(newControlService, oldControlService.Id);
            this.CopyProviderService(newControlService, oldControlService.Id);

            return true;
        }

        protected virtual bool CopyCapRepairService(BaseService oldService, long realObjId = 0, long periodId = 0)
        {
            if (!DictCapRepairService.ContainsKey(oldService.DisclosureInfoRealityObj.Id))
            {
                return false;
            }

            var oldCapRepairService = DictCapRepairService[oldService.DisclosureInfoRealityObj.Id].FirstOrDefault(x => x.Id == oldService.Id);

            if (oldCapRepairService == null)
            {
                return false;
            }

            if (realObjId == 0)
            {
                realObjId = oldCapRepairService.DisclosureInfoRealityObj.RealityObject.Id;
            }

            if (periodId == 0)
            {
                periodId = oldCapRepairService.DisclosureInfoRealityObj.PeriodDi.Id;
            }

            var newDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realObjId) ? DictDisclosureInfoRealObj[realObjId].FirstOrDefault(x => x.PeriodDi.Id == periodId) : null;

            if (newDisclosureInfoRealObj == null)
            {
                newDisclosureInfoRealObj = ListSaveDisclosureInfoRealityObj.FirstOrDefault(x => x.PeriodDi.Id == periodId && x.RealityObject.Id == realObjId);

                if (newDisclosureInfoRealObj == null)
                {
                    newDisclosureInfoRealObj = new DisclosureInfoRealityObj
                    {
                        Id = 0,
                        PeriodDi = new PeriodDi { Id = periodId },
                        RealityObject = new RealityObject { Id = realObjId },
                        ManagingOrganization = oldService.DisclosureInfoRealityObj.ManagingOrganization
                    };

                    ListSaveDisclosureInfoRealityObj.Add(newDisclosureInfoRealObj);
                }
            }

            var newCapRepairService = new CapRepairService
            {
                Id = 0,
                TemplateService = oldCapRepairService.TemplateService,
                DisclosureInfoRealityObj = newDisclosureInfoRealObj,
                Provider = oldCapRepairService.Provider,
                Profit = oldCapRepairService.Profit,
                UnitMeasure = oldCapRepairService.UnitMeasure,
                TypeOfProvisionService = oldCapRepairService.TypeOfProvisionService,
                RegionalFund = oldCapRepairService.RegionalFund,
                TariffForConsumers = oldCapRepairService.TariffForConsumers,
                TariffIsSetForDi = oldCapRepairService.TariffIsSetForDi,
                DateStartTariff = oldCapRepairService.DateStartTariff
            };

            ListSaveCapRepairService.Add(newCapRepairService);

            this.CopyTariffForConsumers(newCapRepairService, oldCapRepairService.Id);
            this.CopyWorkCapRepair(newCapRepairService, oldCapRepairService.Id);
            this.CopyProviderService(newCapRepairService, oldCapRepairService.Id);

            return true;
        }

        protected virtual bool CopyRepairService(BaseService oldService, long realObjId = 0, long periodId = 0)
        {
            if (!DictRepairService.ContainsKey(oldService.DisclosureInfoRealityObj.Id))
            {
                return false;
            }

            var oldRepairService = DictRepairService[oldService.DisclosureInfoRealityObj.Id].FirstOrDefault(x => x.Id == oldService.Id);

            if (oldRepairService == null)
            {
                return false;
            }

            if (realObjId == 0)
            {
                realObjId = oldRepairService.DisclosureInfoRealityObj.RealityObject.Id;
            }

            if (periodId == 0)
            {
                periodId = oldRepairService.DisclosureInfoRealityObj.PeriodDi.Id;
            }

            var newDisclosureInfoRealObj = DictDisclosureInfoRealObj.ContainsKey(realObjId) ? DictDisclosureInfoRealObj[realObjId].FirstOrDefault(x => x.PeriodDi.Id == periodId) : null;

            if (newDisclosureInfoRealObj == null)
            {
                newDisclosureInfoRealObj = ListSaveDisclosureInfoRealityObj.FirstOrDefault(x => x.PeriodDi.Id == periodId && x.RealityObject.Id == realObjId);

                if (newDisclosureInfoRealObj == null)
                {
                    newDisclosureInfoRealObj = new DisclosureInfoRealityObj
                    {
                        Id = 0,
                        PeriodDi = new PeriodDi { Id = periodId },
                        RealityObject = new RealityObject { Id = realObjId },
                        ManagingOrganization = oldService.DisclosureInfoRealityObj.ManagingOrganization
                    };

                    ListSaveDisclosureInfoRealityObj.Add(newDisclosureInfoRealObj);
                }
            }

            var newRepairService = new RepairService
            {
                Id = 0,
                TemplateService = oldRepairService.TemplateService,
                DisclosureInfoRealityObj = newDisclosureInfoRealObj,
                Provider = oldRepairService.Provider,
                Profit = oldRepairService.Profit,
                UnitMeasure = oldRepairService.UnitMeasure,
                TypeOfProvisionService = GetTypeOfProvisionService(oldRepairService, periodId),
                TariffForConsumers = oldRepairService.TariffForConsumers,
                TariffIsSetForDi = oldRepairService.TariffIsSetForDi,
                DateStartTariff = oldRepairService.DateStartTariff,
                ScheduledPreventiveMaintanance = oldRepairService.ScheduledPreventiveMaintanance,
                SumWorkTo = oldRepairService.SumWorkTo,
                SumFact = oldRepairService.SumFact,
                DateStart = oldRepairService.DateStart,
                DateEnd = oldRepairService.DateEnd,
                ProgressInfo = oldRepairService.ProgressInfo,
                RejectCause = oldRepairService.RejectCause
            };

            ListSaveRepairService.Add(newRepairService);

            this.CopyTariffForConsumers(newRepairService, oldRepairService.Id);
            this.CopyWorkRepairList(newRepairService, oldRepairService.Id);
            this.CopyWorkRepairDetail(newRepairService, oldRepairService.Id);
            this.CopyWorkRepairTechServ(newRepairService, oldRepairService.Id);
            this.CopyProviderService(newRepairService, oldRepairService.Id);

            return true;
        }

        protected virtual TypeOfProvisionServiceDi GetTypeOfProvisionService(RepairService oldService, long newPeriodId)
        {
            return oldService.TypeOfProvisionService;
        }

        protected virtual void CopyTariffForConsumers(BaseService newService, long oldServiceId)
        {
            if (!DictTariffForConsumers.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var tariff in DictTariffForConsumers[oldServiceId])
            {
                ListSaveTariffForConsumers.Add(new TariffForConsumers
                {
                    BaseService = newService,
                    DateStart = tariff.DateStart,
                    DateEnd = tariff.DateEnd,
                    TariffIsSetFor = tariff.TariffIsSetFor,
                    OrganizationSetTariff = tariff.OrganizationSetTariff,
                    TypeOrganSetTariffDi = tariff.TypeOrganSetTariffDi,
                    Cost = tariff.Cost,
                    CostNight = tariff.CostNight,
                });
            }
        }

        protected virtual void CopyConsumptionNormsNpa(BaseService newService, long oldServiceId)
        {
            if (!DictConsumptionNormsNpa.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var consumptionNormsNpa in DictConsumptionNormsNpa[oldServiceId])
            {
                ListSaveConsumptionNormsNpa.Add(new ConsumptionNormsNpa
                {
                    BaseService = newService,
                    NpaDate = consumptionNormsNpa.NpaDate,
                    NpaNumber = consumptionNormsNpa.NpaNumber,
                    NpaAcceptor = consumptionNormsNpa.NpaAcceptor,
                });
            }
        }

        protected virtual void CopyTariffForRso(BaseService newService, long oldServiceId)
        {
            if (!DictTariffForRso.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var tariff in DictTariffForRso[oldServiceId])
            {
                this.ListSaveTariffForRso.Add(new TariffForRso
                {
                    BaseService = newService,
                    DateStart = tariff.DateStart,
                    DateEnd = tariff.DateEnd,
                    NumberNormativeLegalAct = tariff.NumberNormativeLegalAct,
                    DateNormativeLegalAct = tariff.DateNormativeLegalAct,
                    OrganizationSetTariff = tariff.OrganizationSetTariff,
                    Cost = tariff.Cost,
                    CostNight = tariff.CostNight
                });
            }
        }

        protected virtual void CopyCostItem(BaseService newService, long oldServiceId)
        {
            if (!DictCostItem.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var costItem in DictCostItem[oldServiceId])
            {
                this.ListSaveCostItem.Add(new CostItem
                {
                    BaseService = newService,
                    Name = costItem.Name,
                    Count = costItem.Count,
                    Sum = costItem.Sum,
                    Cost = costItem.Cost
                });
            }
        }

        protected virtual void CopyProviderService(BaseService newService, long oldServiceId)
        {
            if (!DictProviderService.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var provider in DictProviderService[oldServiceId])
            {
                this.ListSaveProviderService.Add(new ProviderService
                {
                    BaseService = newService,
                    Provider = provider.Provider,
                    DateStartContract = provider.DateStartContract,
                    Description = provider.Description,
                    IsActive = provider.IsActive,
                    NumberContract = provider.NumberContract
                });
            }
        }

        protected virtual void CopyWorkCapRepair(BaseService newService, long oldServiceId)
        {
            if (!DictWorkCapRepair.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var workCr in DictWorkCapRepair[oldServiceId])
            {
                this.ListSaveWorkCapRepair.Add(new WorkCapRepair
                {
                    BaseService = newService,
                    Work = workCr.Work,
                    PlannedVolume = workCr.PlannedVolume,
                    PlannedCost = workCr.PlannedCost,
                    FactedVolume = workCr.FactedVolume,
                    FactedCost = workCr.FactedCost
                });
            }
        }

        protected virtual void CopyWorkRepairList(RepairService newService, long oldServiceId)
        {
            var planWorkServiceRepair = this.CreatePlanWorkServiceRepair(newService, oldServiceId);

            if (!DictWorkRepairList.ContainsKey(oldServiceId))
            {
                return;
            }

            foreach (var workRl in DictWorkRepairList[oldServiceId])
            {
                var workRepairList = new WorkRepairList
                {
                    BaseService = newService,
                    GroupWorkPpr = workRl.GroupWorkPpr,
                    PlannedVolume = workRl.PlannedVolume,
                    PlannedCost = workRl.PlannedCost,
                    FactVolume = workRl.FactVolume,
                    FactCost = workRl.FactCost,
                    DateStart = workRl.DateStart,
                    DateEnd = workRl.DateEnd
                };

                this.ListSaveWorkRepairList.Add(workRepairList);

                if (planWorkServiceRepair != null)
                    {
                    // Создаем ППР для плана на основе ППР услуги
                    var planWorkServiceRepairWork = new PlanWorkServiceRepairWorks
                {
                            PlanWorkServiceRepair = planWorkServiceRepair,
                            WorkRepairList = workRepairList,
                            Cost = workRepairList.PlannedCost,
                            FactCost = workRepairList.FactCost,
                            DateStart = workRepairList.DateStart,
                            DateEnd = workRepairList.DateEnd
                };

                    // Если в копируемом периоде были ППР плана, то перезатираем значения выше
                    if (DictPlanWorkServiceRepairWorks.ContainsKey(oldServiceId)
                        && DictPlanWorkServiceRepairWorks[oldServiceId].ContainsKey(workRepairList.GroupWorkPpr.Id))
                    {
                        var planWork = DictPlanWorkServiceRepairWorks[oldServiceId][workRepairList.GroupWorkPpr.Id];

                        planWorkServiceRepairWork.Cost = planWork.Cost;
                        planWorkServiceRepairWork.FactCost = planWork.FactCost;
                        planWorkServiceRepairWork.DateStart = planWork.DateStart;
                        planWorkServiceRepairWork.DateEnd = planWork.DateEnd;
                        planWorkServiceRepairWork.DateComplete = planWork.DateComplete;
                        planWorkServiceRepairWork.DataComplete = planWork.DataComplete;
                        planWorkServiceRepairWork.PeriodicityTemplateService = planWork.PeriodicityTemplateService;
                        planWorkServiceRepairWork.ReasonRejection = planWork.ReasonRejection;
                            }
                            
                    this.ListSavePlanWorkServiceRepairWorks.Add(planWorkServiceRepairWork);
                        }
                    }
                    }

        protected virtual void CopyWorkRepairDetail(BaseService newService, long oldServiceId)
                    {
            if (!DictWorkRepairDetail.ContainsKey(oldServiceId))
                        {
                return;
                    }

            foreach (var workRd in DictWorkRepairDetail[oldServiceId])
                    {
                this.ListSaveWorkRepairDetail.Add(new WorkRepairDetail
                        {
                    BaseService = newService,
                    WorkPpr = workRd.WorkPpr
                });
                        }
                    }

        protected virtual void CopyWorkRepairTechServ(BaseService newService, long oldServiceId)
                    {
            if (!DictWorkRepairTechServ.ContainsKey(oldServiceId))
                        {
                return;
                    }

            foreach (var workRd in DictWorkRepairTechServ[oldServiceId])
                    {
                this.ListSaveWorkRepairTechServ.Add(new WorkRepairTechServ
                        {
                    BaseService = newService,
                    WorkTo = workRd.WorkTo
                });
                        }
                    }

        protected virtual PlanWorkServiceRepair CreatePlanWorkServiceRepair(RepairService newService, long oldServiceId)
        {
            if (newService.TypeOfProvisionService == TypeOfProvisionServiceDi.ServiceProvidedMo)
                    {
                var planWorkServiceRepair = new PlanWorkServiceRepair
                        {
                        BaseService = newService,
                        DisclosureInfoRealityObj = newService.DisclosureInfoRealityObj
                    };

                ListSavePlanWorkServiceRepair.Add(planWorkServiceRepair);

                return planWorkServiceRepair;
            }

            return null;
        }

        #endregion Формирование списков на создание

        #region Сохранение

        protected void SaveServices()
        {
            this.InTransaction(ListSaveDisclosureInfoRealityObj, DisclosureInfoRealObjRepository);
            this.InTransaction(ListSaveCommunalService, CommunalRepository);
            this.InTransaction(ListSaveHousingService, HousingRepository);
            this.InTransaction(ListSaveAdditionalService, AdditionalRepository);
            this.InTransaction(ListSaveControlService, ControlRepository);
            this.InTransaction(ListSaveRepairService, RepairRepository);
            this.InTransaction(ListSaveCapRepairService, CapRepairRepository);

            SaveTariff();

            this.InTransaction(ListSaveCostItem, CostItemRepository);
            this.InTransaction(ListSaveWorkCapRepair, WorkCapRepairRepository);
            this.InTransaction(ListSaveProviderService, ProviderServiceRepository);
            
            SaveWorkRepairList();

            this.InTransaction(ListSavePlanWorkServiceRepair, PlanWorkServiceRepairRepository);
            this.InTransaction(ListSavePlanWorkServiceRepairWorks, PlanWorkServiceRepairWorksRepository);

            SaveWorkRepairDetail();

            this.InTransaction(ListSaveWorkRepairTechServ, WorkRepairTechServRepository);
            this.InTransaction(ListSaveConsumptionNormsNpa, ConsumptionNormsNpaRepository);

            // чистим коллекции
            DictDisclosureInfoRealObj.Clear();
            DictAdditionalService.Clear();
            DictControlService.Clear();
            DictCommunalService.Clear();
            DictHousingService.Clear();
            DictRepairService.Clear();
            DictCapRepairService.Clear();
            DictConsumptionNormsNpa.Clear();

            ListSaveDisclosureInfoRealityObj.Clear();
            ListSaveAdditionalService.Clear();
            ListSaveCapRepairService.Clear();
            ListSaveCommunalService.Clear();
            ListSaveControlService.Clear();
            ListSaveHousingService.Clear();
            ListSaveRepairService.Clear();
            ListSaveConsumptionNormsNpa.Clear();

            DictCostItem.Clear();
            this.ListSaveCostItem.Clear();

            DictWorkCapRepair.Clear();
            this.ListSaveWorkCapRepair.Clear();

            DictProviderService.Clear();
            this.ListSaveProviderService.Clear();

            DictWorkRepairTechServ.Clear();
            this.ListSaveWorkRepairTechServ.Clear();
        }

        protected virtual void SaveTariff()
        {
            this.InTransaction(ListSaveTariffForConsumers, TariffForConsumRepository);
            this.InTransaction(ListSaveTariffForRso, TariffForRsoRepository);

                    DictTariffForConsumers.Clear();
            ListSaveTariffForConsumers.Clear();

                    DictTariffForRso.Clear();
            ListSaveTariffForRso.Clear();
        }

        protected virtual void SaveWorkRepairList()
                    {
            this.InTransaction(ListSaveWorkRepairList, WorkRepairListRepository);

            DictWorkRepairList.Clear();
            this.ListSaveWorkRepairList.Clear();
        }

        protected virtual void SaveWorkRepairDetail()
                        {
            this.InTransaction(ListSaveWorkRepairDetail, WorkRepairDetailRepository);

            DictWorkRepairDetail.Clear();
            this.ListSaveWorkRepairDetail.Clear();
                    }

        #endregion Сохранение

        protected FileInfo ReCreateFile(FileInfo fileInfo, IFileManager fileManager)
            {
                try
                {
                if (fileInfo == null)
                        {
                    return null;
                    }

                var fileInfoStream = fileManager.GetFile(fileInfo);
                var newFileInfo = fileManager.SaveFile(fileInfoStream, string.Format("{0}.{1}", fileInfo.Name, fileInfo.Extention));
                return newFileInfo;
                }
            catch
                    {
                return null;
                    }
                    }

        #region Транзакция

        private IDataTransaction BeginTransaction()
                        {
            return Container.Resolve<IDataTransaction>();
                    }

        protected void InTransaction(IEnumerable<IEntity> list, IRepository repos)
        {
            if (!list.Any())
            {
                return;
            }

            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id.ToLong() > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
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

        #endregion

        #region init params

        public class InitParams
        {
            public bool CommunalServ = false;

            public bool HousingServ = false;

            public bool RepairServ = false;

            public bool CapRepairServ = false;

            public bool AdditionalServ = false;

            public bool ControlServ = false;

            public bool TariffConsumers = false;

            public bool TariffRso = false;

            public bool CostItem = false;

            public bool ProviderService = false;

            public bool WorkCapRepair = false;

            public bool WorkRepairList = false;

            public bool WorkRepairDetail = false;

            public bool WorkRepairTechServ = false;

            public bool ConsumptionNormsNpa = false;

            public bool All = false;
        }

        private InitParams GetInitParams(KindServiceDi kindServiceDi)
        {
            var result = new InitParams();

            switch (kindServiceDi)
            {
                case KindServiceDi.Communal:
                    result = new InitParams
                    {
                        CommunalServ = true,
                        TariffConsumers = true,
                        TariffRso = true,
                        ProviderService = true,
                        ConsumptionNormsNpa = true
                    };
                    break;

                case KindServiceDi.Housing:
                    result = new InitParams
                    {
                        HousingServ = true,
                        TariffConsumers = true,
                        CostItem = true,
                        ProviderService = true
                    };
                    break;

                case KindServiceDi.Repair:
                    result = new InitParams
                    {
                        RepairServ = true,
                        TariffConsumers = true,
                        WorkRepairList = true,
                        WorkRepairDetail = true,
                        WorkRepairTechServ = true,
                        ProviderService = true
                    };
                    break;

                case KindServiceDi.CapitalRepair:
                    result = new InitParams
                    {
                        CapRepairServ = true,
                        TariffConsumers = true,
                        WorkCapRepair = true,
                        ProviderService = true
                    };
                    break;

                case KindServiceDi.Managing:
                    result = new InitParams
                    {
                        ControlServ = true,
                        TariffConsumers = true,
                        ProviderService = true
                    };
                    break;

                case KindServiceDi.Additional:
                    result = new InitParams
                    {
                        AdditionalServ = true,
                        TariffConsumers = true,
                        ProviderService = true
                    };
                    break;
            }

            return result;
        }

        #endregion
    }
}
