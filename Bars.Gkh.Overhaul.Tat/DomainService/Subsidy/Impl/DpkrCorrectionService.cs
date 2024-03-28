namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.IoC;
    using B4.Modules.NHibernateChangeLog;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Enum;
    using Gkh.Domain;
    using NHibernate.Linq;

    /// <summary>
    /// Сервис корректировки ДПКР
    /// </summary>
    public class DpkrCorrectionService : IDpkrCorrectionService
    {
        /// <summary>
        /// IOC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        private readonly IDomainService<DpkrCorrectionStage2> domain;

        /// <summary>
        /// Домен-сервис версии строки
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        /// <summary>
        /// Домен-сервис Муниципального образования
        /// </summary>
        public IDomainService<Municipality> MunicicpalityDomain { get; set; }

        /// <summary>
        /// Интерфейс сервиса для работы с ограничениями на выбор скорректированного года
        /// </summary>
        public IYearCorrectionConfigService YearCorrectionConfigService { get; set; }

        /// <summary>
        /// Конструктор домен-сервиса
        /// </summary>
        /// <param name="domain">данные, необходиме при учете корректировки ДПКР</param>
        public DpkrCorrectionService(IDomainService<DpkrCorrectionStage2> domain)
        {
            this.domain = domain;
        }

        /// <summary>
        /// Получение периодов актуализации программы
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполнения запроса</returns>
        public IDataResult GetActualizeYears(BaseParams baseParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulTatConfig>();
            var aStartYear = config.ActualizePeriodStart;
            var aEndYear = config.ActualizePeriodEnd;

            var result = new List<int>();
            while (aStartYear <= aEndYear)
            {
                result.Add(aStartYear);

                aStartYear++;
            }

            return new BaseDataResult(result);
        }

        /// <summary>
        /// Получение информации типа результата корректировки по муниципальному учреждению
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполнения запроса</returns>
        public IDataResult GetInfo(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var version = this.Container.Resolve<IDomainService<ProgramVersion>>().GetAll()
                    .FirstOrDefault(x => x.IsMain && x.Municipality.Id == municipalityId);

            if (version == null)
            {
                return new BaseDataResult(false, "Для выбранного муниципального образования не указана основная версия");
            }

            var data =
                this.domain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Municipality.Id == municipalityId && x.Stage2.Stage3Version.ProgramVersion.IsMain)
                    .Select(x => new { x.TypeResult, x.Id, st3Id = x.Stage2.Stage3Version.Id })
                    .ToList()
                    .GroupBy(x => x.TypeResult)
                    .Select(x => new
                    {
                        Type = x.Key.ToString(),
                        Count = x.Select(y => y.st3Id).Distinct().Count()
                    });

            return new BaseDataResult(new { versionId = version.Id, data });
        }

        /// <summary>
        /// Проверка при изменении корректировки 
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполения запроса</returns>
        public IDataResult ChangeIndexNumber(BaseParams baseParams)
        {
            var dpkrCorrectionId = baseParams.Params.GetAsId();
            var stage3Id = baseParams.Params.GetAsId("st3Id");
            var newNumber = baseParams.Params.GetAs<int>("newNumber");
            var newPlanYear = baseParams.Params.GetAs<int>("newPlanYear");
            var firstPlanYear = baseParams.Params.GetAs<int>("firstPlanYear");
            var fixedYear = baseParams.Params.GetAs<bool>("fixedYear");

            var config = this.Container.GetGkhConfig<OverhaulTatConfig>();
            if (newPlanYear < config.ActualizePeriodStart)
            {
                return new BaseDataResult(false, string.Format("Изменение записи не доступно, т.к. Скорректированный год меньше допустимого периода актуализации версии {0}", config.ActualizePeriodStart));
            }

            if (newPlanYear > config.ProgrammPeriodEnd)
            {
                return new BaseDataResult(false, "Введите дату меньше окончания периода ДПКР.");
            }

            var validateYearResult = this.YearCorrectionConfigService.IsValidYear(newPlanYear);
            if (!validateYearResult.Success)
            {
                return validateYearResult;
            }

            try
            {
                var versionRec = this.VersionRecordDomain.GetAll()
                    .FirstOrDefault(x => x.Id == stage3Id);

                var dpkrCorrectionStages2 = this.domain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.Id == stage3Id);

                if (versionRec == null || !dpkrCorrectionStages2.Any())
                {
                    return new BaseDataResult(false, "Ошибка при получении записи версии");
                }
                
                var versionRecordList = new List<VersionRecord>();
                bool isUpDirection = false;

                if (newNumber > 0)
                {
                    isUpDirection = versionRec.IndexNumber < newNumber;

                    if (versionRec.IndexNumber != newNumber)
                    {
                        if (isUpDirection)
                        {
                            versionRecordList = this.VersionRecordDomain.GetAll()
                                    .Where(x => x.IndexNumber <= newNumber && x.IndexNumber > versionRec.IndexNumber
                                                &&
                                                x.ProgramVersion.Municipality.Id ==
                                                versionRec.ProgramVersion.Municipality.Id)
                                    .ToList();
                        }
                        else
                        {
                            versionRecordList = this.VersionRecordDomain.GetAll()
                                    .Where(x => x.IndexNumber >= newNumber && x.IndexNumber < versionRec.IndexNumber
                                                &&
                                                x.ProgramVersion.Municipality.Id ==
                                                versionRec.ProgramVersion.Municipality.Id)
                                    .ToList();
                        }
                    }
                }

                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        //// Пересчитываем номера
                        foreach (var versionRecord in versionRecordList)
                        {
                            versionRec.IndexNumber = isUpDirection ? versionRecord.IndexNumber-- : versionRecord.IndexNumber++;
                            this.VersionRecordDomain.Update(versionRec);
                        }

                        if (newNumber > 0)
                        {
                            versionRec.IndexNumber = newNumber;
                        }

                        versionRec.FixedYear = fixedYear;

                        foreach (var dpkrCorrectionStage2 in dpkrCorrectionStages2)
                        {
                            dpkrCorrectionStage2.PlanYear = newPlanYear;
                            this.domain.Update(dpkrCorrectionStage2);
                        }

                        this.VersionRecordDomain.Update(versionRec);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                
                return new BaseDataResult();
            }
            finally 
            {
                this.Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
        }

        /// <summary>
        /// История изменений 
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполнения запроса</returns>
        public IDataResult GetHistory(BaseParams baseParams)
        {
            var logEntityDomain = this.Container.ResolveDomain<LogEntity>();

            try
            {
                var loadParam = baseParams.GetLoadParam();
                var dpkrCorrectionId = loadParam.Filter.GetAsId();
                var stage3Id = loadParam.Filter.GetAsId("st3Id");

                var data = logEntityDomain.GetAll()
                    .Where(x => x.ActionKind == ActionKind.Update || x.ActionKind == ActionKind.Insert)
                    .Where(x =>
                        (x.EntityType == typeof(DpkrCorrectionStage2).FullName && x.EntityId == dpkrCorrectionId) ||
                        (x.EntityType == typeof(VersionRecord).FullName && x.EntityId == stage3Id))
                    .Filter(loadParam, this.Container)
                    .Select(x => new CorrectionHistoryProxy
                    {
                        Id = x.Id,
                        EntityDescription = x.EntityDescription,
                        EntityDateChange = x.ChangeDate,
                        Ip = x.UserIpAddress,
                        UserLogin = x.UserLogin,
                        EntityTypeChange = x.ActionKind
                    });

                var excludedLogEntities = this.GetExcludeLogEntity(data);

                data = data.Where(x => !excludedLogEntities.Contains(x.Id));

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(logEntityDomain);
            }
        }

        /// <summary>
        /// Детализация истории изменений
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполнения запроса</returns>
        public IDataResult GetHistoryDetail(BaseParams baseParams)
        {
            var logEntityDomain = this.Container.ResolveDomain<LogEntity>();
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            using (this.Container.Using(logEntityDomain, logEntityPropertyDomain))
            {
                var loadParams = baseParams.GetLoadParam();
                var logEntityId = loadParams.Filter.GetAs<long>("logEntityId");
                const string PropertyName = "Неизвестный атрибут";

                var logEntity = logEntityDomain.GetAll().First(x => x.Id == logEntityId);

                var properties = new Dictionary<string, LoggedPropertyInfo>();
                var dictProperties = this.Container.Resolve<IChangeLogInfoProvider>()
                    .GetLoggedEntities()
                    .With(x => x.ToDictionary(y => y.EntityType, z => z.Properties));

                if (dictProperties != null && dictProperties.ContainsKey(logEntity.EntityType))
                {
                    properties = dictProperties[logEntity.EntityType].ToDictionary(x => x.PropertyCode, y => y);
                }


                var data = logEntityPropertyDomain.GetAll()
                    .Where(x => x.LogEntity == logEntity)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        PropertyName = properties.ContainsKey(x.PropertyCode)
                            ? properties[x.PropertyCode].DisplayName
                            : PropertyName,
                        x.NewValue,
                        x.OldValue,
                        Type = properties.ContainsKey(x.PropertyCode)
                            ? this.GetNativeType(properties[x.PropertyCode].Type).Name
                            : null,
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }

        /// <summary>
        /// Список домов для массового изменения статуса
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполнения запроса</returns>
        public IDataResult ListForMassChangeYear(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var provider = this.Container.Resolve<IDpkrCorrectionDataProvider>();
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var config = this.Container.GetGkhConfig<OverhaulTatConfig>();

            try
            {
                var ids = baseParams.Params.GetAs<string>("Id").ToLongArray();
     
                /* 
                   Поскольку записи в 3 этапе могут быть сгруппированы то в корректировке необходимо показвать записи не 2этапа а 3го
                   то есть должно быть так 'Лифт, Крыша' 
                */
                var correctionQuery = provider.GetCorrectionData(baseParams);
         
                var corrDataBySt3 = correctionQuery
                    .Select(x => new { st3Id = x.Stage2.Stage3Version.Id, x.PlanYear, x.TypeResult })
                    .AsEnumerable()
                    .GroupBy(x => x.st3Id)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        new
                        {
                            PlanYear = y.Select(z => z.PlanYear).FirstOrDefault(),
                            TypeResult = y.Select(z => z.TypeResult).FirstOrDefault()
                        });

                var st3Data = versionRecordDomain.GetAll()
                    .WhereIf(ids.Any(), x => ids.Contains(x.Id))
                    .Where(x => correctionQuery.Any(y => y.Stage2.Stage3Version.Id == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.Sum,
                        CommonEstateObjectName = x.CommonEstateObjects,
                        FirstPlanYear = x.Year,
                        x.Point,
                        x.IndexNumber
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Municipality,
                        x.RealityObject,
                        x.Sum,
                        x.CommonEstateObjectName,
                        x.FirstPlanYear,
                        x.Point,
                        x.IndexNumber,
                        PlanYear = corrDataBySt3.ContainsKey(x.Id) ? corrDataBySt3[x.Id].PlanYear : 0,
                        TypeResult = corrDataBySt3.ContainsKey(x.Id) ? corrDataBySt3[x.Id].TypeResult : TypeResultCorrectionDpkr.InLongTerm
                    })
                    .AsQueryable()
                    .Where(x => x.PlanYear >= config.ActualizePeriodStart)
                    .Filter(loadParam, this.Container);

                var count = st3Data.Count();

                return new ListDataResult(st3Data.Order(loadParam)
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.PlanYear)
                        .Paging(loadParam).ToList(), count);
                
            }
            finally
            {
                this.Container.Release(provider);
                this.Container.Release(config);
                this.Container.Release(versionRecordDomain);
            }    
        }

        /// <summary>
        /// Массовое изменение статуса
        /// </summary>
        /// <param name="baseParams">параметры клиента</param>
        /// <returns>результат выполнения запроса</returns>
        public IDataResult MassChangeYear(BaseParams baseParams)
        {
            var recIds = baseParams.Params.GetAs<string>("recIds").ToLongArray();
            var newYear = baseParams.Params.GetAs<int>("newYear");
            var config = this.Container.GetGkhConfig<OverhaulTatConfig>();

            if (newYear < config.ActualizePeriodStart)
            {
                return new BaseDataResult(false, "Введите дату больше начала периода ДПКР.");
            }

            if (newYear > config.ProgrammPeriodEnd)
            {
                return new BaseDataResult(false, "Введите дату меньше окончания периода ДПКР.");
            }

            var validateYearResult = this.YearCorrectionConfigService.IsValidYear(newYear);
            if (!validateYearResult.Success)
            {
                return validateYearResult;
            }

            var dpkrCorrectionDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            try
            {
                var correctionToUpd = new List<DpkrCorrectionStage2>();
                var versionToUpd = new HashSet<VersionRecord>();

                var recForChange = dpkrCorrectionDomain.GetAll()
                    .Fetch(x => x.Stage2)
                    .ThenFetch(x => x.Stage3Version)
                    .Where(x => recIds.Contains(x.Stage2.Stage3Version.Id))
                    .ToList();

                foreach (var rec in recForChange)
                {
                    var st3 = rec.Stage2.Stage3Version;
                    
                    st3.FixedYear = true;

                    versionToUpd.Add(st3);

                    rec.PlanYear = newYear;

                    correctionToUpd.Add(rec);
                }

                TransactionHelper.InsertInManyTransactions(this.Container, correctionToUpd, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, versionToUpd, 1000, true, true);
            }
            finally
            {
                this.Container.Release(dpkrCorrectionDomain);
            }

            return new BaseDataResult("Год успешно изменен");
        }

        private List<long> GetExcludeLogEntity(IQueryable<CorrectionHistoryProxy> data)
        {
            var logEntityPropertyDomain = this.Container.ResolveDomain<LogEntityProperty>();

            var insertedLogEntityIds = data.Where(x => x.EntityTypeChange == ActionKind.Insert).Select(x => x.Id).ToList();
            var filterLogEnityId = new List<long>();

            using (this.Container.Using(logEntityPropertyDomain))
            {
                var logEntityProperties = logEntityPropertyDomain.GetAll()
                    .Where(x => insertedLogEntityIds.Contains(x.LogEntity.Id))
                    .Select(x => new { x.Id, x.NewValue, x.LogEntity })
                    .AsEnumerable()
                    .GroupBy(x => x.LogEntity.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                foreach (var logEntityProperty in logEntityProperties)
                {
                    if (logEntityProperty.Value.All(x => x.NewValue.IsEmpty()))
                    {
                        filterLogEnityId.Add(logEntityProperty.Key);
                    }
                }
            }

            return filterLogEnityId;
        }

        private Type GetNativeType(Type type)
        {
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return type.GetGenericArguments()[0];
                }
            }

            return type;
        }

        private class CorrectionHistoryProxy
        {
            public long Id { get; set; }

            public string EntityDescription { get; set; }

            public DateTime EntityDateChange { get; set; }

            public string Ip { get; set; }

            public string UserLogin { get; set; }

            public ActionKind EntityTypeChange { get; set; }
        }
    }
}