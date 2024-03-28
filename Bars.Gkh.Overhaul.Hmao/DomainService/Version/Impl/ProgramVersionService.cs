namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    using NHibernate.Linq;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;
    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис версии программы
    /// </summary>
    public partial class ProgramVersionService : IProgramVersionService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис ДПКР
        /// </summary>
        public ILongProgramService LongProgramService { get; set; }

        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }

        /// <summary>
        /// Изменить данные версии
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult ChangeVersionData(BaseParams baseParams)
        {
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var versionSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versionSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var publishRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var structElementWorkService = this.Container.ResolveDomain<StructuralElementWork>();
            var typeWorkCrDomain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var ownerDecisionDomain = this.Container.ResolveDomain<ChangeYearOwnerDecision>();

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();

            try
            {
                var newIndex = baseParams.Params.GetAs<int>("newIndex");
                var reCalcSum = baseParams.Params.GetAs<bool>("reCalcSum");
                var newYear = baseParams.Params.GetAs<int>("newYear");
                var Remark = baseParams.Params.GetAs<string>("Remark");
                var versionRecId = baseParams.Params.GetAs<long>("versionRecId");
                var versSt1Recs = baseParams.Params.GetAs<VersSt1Proxy[]>("newPlanYearRecs");
                var hasOwnerDecision = baseParams.Params.GetAs<bool>("HasOwnerDecision");
                var changeBasisTypeString = baseParams.Params.GetAs<string>("changeBasisType");
                var changeBasisType = (ChangeBasisType)changeBasisTypeString.Split(',')[0].ToInt();

                var startYear = config.ProgrammPeriodStart;
                var endYear = config.ProgrammPeriodEnd;
                var typeCorrection = config.SubsidyConfig.TypeCorrection;
                var isWorkPriceFirstYear = config.WorkPriceCalcYear == WorkPriceCalcYear.First;
                var servicePercent = config.ServiceCost;

                if (!(startYear <= newYear && newYear <= endYear))
                {
                    return BaseDataResult.Error($"Плановый год ремонта должен быть в интервале указанном в настройках ДПКР {startYear}-{endYear}");
                }

                if(versSt1Recs.Any(x => !(startYear <= x.PlanYear && x.PlanYear <= endYear)))
                {
                    return BaseDataResult.Error($"Плановый год ремонта должен быть в интервале указанном в настройках ДПКР {startYear}-{endYear}");
                }

                var versionRecordItem = versionRecordDomain.GetAll().FirstOrDefault(x => x.Id == versionRecId);
                versionRecordItem.Remark = Remark;
                if (versionRecordItem == null)
                {
                    return BaseDataResult.Error("Ошибка при получении записи");
                }

                bool isChangeNumber = versionRecordItem.IndexNumber != newIndex;
                bool isChangeYear = versionRecordItem.Year != newYear || versSt1Recs.Length > 0;

                var publishYearInfo = publishRecordDomain.GetAll()
                    .Where(x => x.Stage2 != null)
                    .Where(x => x.Stage2.Stage3Version.Id == versionRecordItem.Id)
                    .Select(
                        x => new
                        {
                            x.Stage2.Id,
                            x.PublishedYear
                        })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id, y => y.PublishedYear);

                var publishYearByStage2Dict = versionSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.Id == versionRecordItem.Id)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Stage3Version.IsDividedRec,
                            x.Stage3Version.PublishYearForDividedRec
                        })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id, y => y.IsDividedRec ? y.PublishYearForDividedRec : publishYearInfo.Get(y.Id));

                if (isChangeYear)
                {
                    // Если есть решение собственников, игнорируем проверку на опубликованный год
                    if (!hasOwnerDecision)
                    {
                        // Если год плановый поменяли и при этом Есть Опубликованная программа  в которой есть данная запись
                        // То новый плановый год не должен превышать год опубликованный                    
                        if (publishYearByStage2Dict.Count > 0)
                        {
                            var publishedYear = publishYearByStage2Dict.SafeMax(x => x.Value);

                            if (publishedYear > 0 && (newYear > publishedYear || versSt1Recs.SafeMax(x => x.PlanYear) > publishedYear))
                            {
                                // отключаем проверку на плановый год по просьбе Тюмени
                             //   return BaseDataResult.Error($"Плановый год ремонта не может быть больше {publishedYear}");
                            }
                        }
                    }

                    // Если по КЭ есть ссылка в Объектах капитального ремонта, то выводить сообщение = Конструктивный элемент включен в долгосрочную программу. 
                    // Изменение планового года ремонта элемента недоступно. 
                    // TypeWorkCrVersionStage1->VersionRecordStage1->VersionRecordStage2->VersionRecord
                    // Убираем проверку
                    var exportProgramCrs = typeWorkCrDomain.GetAll()
                        .Where(x => x.Stage1Version.Stage2Version.Stage3Version.Id == versionRecordItem.Id)
                        .Where(
                            x => x.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr != TypeProgramStateCr.Close
                                && x.TypeWorkCr.ObjectCr.ProgramCr.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden)
                        .Select(x => x.TypeWorkCr.ObjectCr.ProgramCr.Name)
                        .ToArray();

                    //if (exportProgramCrs.Length > 0)
                    //{
                    //    return BaseDataResult.Error("Конструктивный элемент включен в краткосрочную программу: " +
                    //        $"'{exportProgramCrs.AggregateWithSeparator(", ")}'. Изменение планового года ремонта элемента недоступно.");
                    //}
                }

                var listToSave = new List<VersionRecord>();
                var listSt1ToSave = new List<VersionRecordStage1>();
                var listSt2ToSave = new List<VersionRecordStage2>();
                var currIndex = versionRecordItem.IndexNumber;

                // если новый плановый год больше последнего года ДПКР, то ставим номер = 0 и корректируем нумерацию
                if (endYear < newYear)
                {
                    newIndex = 0;

                    // если значение номера быдл положительное значит нужн оуплотнить все номера относителньо номер который исключается
                    if (currIndex > 0)
                    {
                        var data =
                            versionRecordDomain.GetAll()
                                .Where(x => x.ProgramVersion.Id == versionRecordItem.ProgramVersion.Id)
                                .Where(x => x.IndexNumber > currIndex)
                                .ToList();

                        foreach (var record in data)
                        {
                            record.IndexNumber -= 1;
                            record.Remark = Remark;
                            listToSave.Add(record);
                        }
                    }
                }
                else if (newIndex > 0 && isChangeNumber)
                {
                    var isMoveUp = currIndex > newIndex;

                    var data =
                        versionRecordDomain.GetAll()
                            .Where(x => x.ProgramVersion.Id == versionRecordItem.ProgramVersion.Id)
                            .Where(x => x.IndexNumber <= Math.Max(currIndex, newIndex))
                            .Where(x => x.IndexNumber >= Math.Min(currIndex, newIndex))
                            .Where(x => x.IndexNumber > 0)
                            .ToList();

                    foreach (var record in data)
                    {
                        record.Remark = Remark;
                        if (record.IndexNumber != currIndex)
                        {
                            record.IndexNumber += isMoveUp ? 1 : -1;
                        }

                        listToSave.Add(record);
                    }
                }

                // если необходимо пересчитать сумму то пересчитываем по алгоритму расчета
                if (reCalcSum)
                {
                    var dictNotExistPrices = new Dictionary<long, string>();

                    var st1List = versionSt1Domain.GetAll()
                        .Where(x => x.Stage2Version.Stage3Version.Id == versionRecordItem.Id)
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    st2Id = x.Stage2Version.Id,
                                    x.Sum,
                                    x.SumService,
                                    x.Stage2Version.Stage3Version.Year,
                                    municipalityId =
                                        x.RealityObject.Municipality != null ? x.RealityObject.Municipality.Id : 0,
                                    settlementId =
                                        x.RealityObject.MoSettlement != null ? x.RealityObject.MoSettlement.Id : 0,
                                    RealityObjectId = x.RealityObject.Id,
                                    SeId = x.StructuralElement.StructuralElement.Id,
                                    CapitalGroupId =
                                        x.RealityObject.CapitalGroup != null ? x.RealityObject.CapitalGroup.Id : 0,
                                    x.StructuralElement.StructuralElement.CalculateBy,
                                    x.StructuralElement.Volume,
                                    x.RealityObject.AreaLiving,
                                    x.RealityObject.AreaMkd,
                                    x.RealityObject.AreaLivingNotLivingMkd
                                })
                        .ToList();

                    var st1Dict =
                        versionSt1Domain.GetAll()
                            .Where(x => x.Stage2Version.Stage3Version.Id == versionRecordItem.Id)
                            .ToDictionary(x => x.Id);

                    var dictStructElWork = structElementWorkService.GetAll()
                        .Select(
                            x => new
                            {
                                SeId = x.StructuralElement.Id,
                                JobId = x.Job.Id,
                                JobName = x.Job.Name
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.SeId)
                        .ToDictionary(x => x.Key, y => y.GroupBy(x => x.JobId).ToDictionary(x => x.Key, z => z.Select(x => x.JobName).First()));

                    foreach (var objectElement in st1List)
                    {
                        var oldSum = objectElement.Sum;

                        var workPriceYear = isWorkPriceFirstYear ? startYear : newYear;

                        var newSum = this.LongProgramService.GetDpkrCost(
                            objectElement.municipalityId,
                            objectElement.settlementId,
                            workPriceYear,
                            objectElement.SeId,
                            objectElement.RealityObjectId,
                            objectElement.CapitalGroupId,
                            objectElement.CalculateBy,
                            objectElement.Volume,
                            objectElement.AreaLiving,
                            objectElement.AreaMkd,
                            objectElement.AreaLivingNotLivingMkd);

                        if (!newSum.HasValue)
                        {
                            var jobs = dictStructElWork.ContainsKey(objectElement.SeId)
                                ? dictStructElWork[objectElement.SeId]
                                : new Dictionary<long, string>();

                            // если не найдена расценка, добавляем ее в dictionary
                            foreach (var job in jobs)
                            {
                                if (!dictNotExistPrices.ContainsKey(job.Key))
                                {
                                    dictNotExistPrices.Add(job.Key, job.Value);
                                }
                            }
                        }
                        else if (oldSum != newSum.Value)
                        {
                            var st1 = st1Dict[objectElement.Id];
                            st1.Sum = newSum.Value;
                            st1.SumService = (newSum.Value * (servicePercent / 100M)).RoundDecimal(2);

                            if (!listSt1ToSave.Any(x => x.Id == st1.Id))
                            {
                                listSt1ToSave.Add(st1);
                            }

                            var st2 = versionSt2Domain.Load(objectElement.st2Id);
                            st2.Sum += newSum.Value - oldSum;

                            if (!listSt2ToSave.Any(x => x.Id == st2.Id))
                            {
                                listSt2ToSave.Add(st2);
                            }

                            versionRecordItem.Sum = st1.Sum + st1.SumService;
                        }
                    }
                }

                var stage1Changes = new Dictionary<long, Tuple<int, int>>();

                versionSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.Id == versionRecId)
                    .ForEach(
                        x =>
                        {
                            if (x.Year != newYear)
                            {
                                stage1Changes.Add(x.Id, Tuple.Create(x.Year, newYear));

                                x.Year = newYear;
                                listSt1ToSave.Add(x);
                            }
                        });

                this.InTransaction(
                    session =>
                    {
                        listToSave.ForEach(
                            x =>
                            {
                                if (x.Id > 0)
                                {
                                    session.Update(x);
                                }
                                else
                                {
                                    session.Insert(x);
                                }
                            });

                        // если номер 0, то не меняем номер   
                        var isChanged = false;
                        if (newIndex != 0 || endYear < newYear)
                        {
                            versionRecordItem.IndexNumber = newIndex;
                            isChanged = true;
                        }

                        // [#42695] если выбран способ корректировки = С фиксацией года,
                        // то год фиксируется при любом сохранении, даже если его не меняли
                        if (newYear != versionRecordItem.Year || typeCorrection == TypeCorrection.WithFixYear)
                        {
                            versionRecordItem.IsChangedYear = true;
                            isChanged = true;
                        }

                        if (isChanged)
                        {
                            versionRecordItem.Changes = $"Изменено: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
                        }

                        listSt1ToSave.ForEach(x => { versionSt1Domain.Update(x); });

                        listSt2ToSave.ForEach(x => { versionSt2Domain.Update(x); });

                        versionRecordItem.Year = newYear;
                        versionRecordItem.IsManuallyCorrect = true;
                        versionRecordItem.Remark = Remark;
                        versionRecordItem.ChangeBasisType = changeBasisType;

                        if (hasOwnerDecision)
                        {
                            versionRecordItem.FixedYear = true;
                        }

                        session.Update(versionRecordItem);
                    });

                if (config.ActualizeConfig.TransferSingleStructEl == TypeUsage.Used)
                {
                    this.ChangeYearByStructElem(versSt1Recs, versionRecordItem, publishYearByStage2Dict, stage1Changes);
                }

                if (hasOwnerDecision)
                {
                    this.SaveOwnerDecision(baseParams, stage1Changes);
                }

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(versionRecordDomain);
                this.Container.Release(publishRecordDomain);
                this.Container.Release(versionSt1Domain);
                this.Container.Release(versionSt2Domain);
                this.Container.Release(structElementWorkService);
                this.Container.Release(typeWorkCrDomain);
                this.Container.Release(ownerDecisionDomain);
            }
        }

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult MakeNewVersion(BaseParams baseParams)
        {
            var muDomain = this.Container.Resolve<IRepository<Municipality>>();

            try
            {
                var muId = baseParams.Params.GetAs<long>("muId");
                var municipality = muDomain.Get(muId);

                if (municipality == null)
                {
                    return new BaseDataResult(false, "Не удалось получить муниципальное образование");
                }

                var versionData = new VersionData
                {
                    Date = baseParams.Params.GetAs<DateTime>("Date"),
                    Name = baseParams.Params.GetAs<string>("Name"),
                    IsMain = baseParams.Params.GetAs<bool>("IsMain"),
                    Municipality = municipality
                };

                var result = this.MakeNewVersion(versionData);

                if (result == null)
                {
                    return new BaseDataResult(false, "Нет данных для сохранения");
                }

                this.InTransaction(
                    session =>
                    {
                        session.Insert(result.ProgramVersion);

                        result.VersionsForUpdate.ForEach(session.Update);

                        result.VersionParams.ForEach(x => session.Insert(x));

                        result.Stage3Records.ForEach(x => session.Insert(x));

                        result.Stage2Records.ForEach(x => session.Insert(x));

                        result.Stage1Records.ForEach(x => session.Insert(x));
                    });

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(
                    false,
                    $"Во время сохранения произошла ошибка: {"Во время сохранения произошла ошибка: " + e.Message}<br>Внутренняя ошибка: {(e.InnerException != null ? e.InnerException.Message : null)}");
            }
            finally
            {
                this.Container.Release(muDomain);
            }
        }

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult MakeNewVersionAll(BaseParams baseParams)
        {
            var gkhParamService = this.Container.Resolve<IGkhParams>();
            var muDomain = this.Container.Resolve<IRepository<Municipality>>();

            try
            {
                var gkhParams = gkhParamService.GetParams();

                var moLevel = gkhParams.ContainsKey("MoLevel")
                    ? gkhParams["MoLevel"].To<MoLevel>()
                    : MoLevel.MunicipalUnion;

                var municipalities = muDomain.GetAll()
                    .AsEnumerable()
                    .Where(x => x.Level.ToMoLevel(this.Container) == moLevel);

                var programVersionForSave = new List<ProgramVersion>();
                var programVersionForUpdate = new List<ProgramVersion>();
                var versionParamsForSave = new List<VersionParam>();
                var stage3ForSave = new List<VersionRecord>();
                var stage2ForSave = new List<VersionRecordStage2>();
                var stage1ForSave = new List<VersionRecordStage1>();

                foreach (var municipality in municipalities)
                {
                    var versionData = new VersionData
                    {
                        Date = DateTime.Now.Date,
                        Name = municipality.Name + " " + DateTime.Now.Date.ToShortDateString(),
                        IsMain = true,
                        Municipality = municipality
                    };

                    var result = this.MakeNewVersion(versionData);

                    if (result != null)
                    {
                        programVersionForSave.Add(result.ProgramVersion);

                        programVersionForUpdate.AddRange(result.VersionsForUpdate);

                        versionParamsForSave.AddRange(result.VersionParams);

                        stage3ForSave.AddRange(result.Stage3Records);

                        stage2ForSave.AddRange(result.Stage2Records);

                        stage1ForSave.AddRange(result.Stage1Records);
                    }
                }

                this.InTransaction(
                    session =>
                    {
                        programVersionForSave.ForEach(x => session.Insert(x));

                        programVersionForUpdate.ForEach(session.Update);

                        versionParamsForSave.ForEach(x => session.Insert(x));

                        stage3ForSave.ForEach(x => session.Insert(x));

                        stage2ForSave.ForEach(x => session.Insert(x));

                        stage1ForSave.ForEach(x => session.Insert(x));
                    });

                return new BaseDataResult();
            }
            catch (Exception e)
            {
                return new BaseDataResult(
                    false,
                    $"Во время сохранения произошла ошибка: {"Во время сохранения произошла ошибка: " + e.Message}<br>Внутренняя ошибка: {(e.InnerException != null ? e.InnerException.Message : null)}");
            }
            finally
            {
                this.Container.Release(gkhParamService);
                this.Container.Release(muDomain);
            }
        }

        /// <summary>
        /// Получить список записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var result =
                this.GetDeletedEntriesQuery(baseParams)
                    .ToList()
                    .Distinct(x => x.Id)
                    .AsQueryable()
                    .OrderBy(x => x.IndexNumber);

            return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
        }

        /// <summary>
        /// Получить запрос записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IQueryable<DeletedEntriesDto> GetDeletedEntriesQuery(BaseParams baseParams)
        {
            var actualizeService = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                // Теперь получаем те записи 3го этапа которые есть в списке полученных из 1го этапа
                var result = actualizeService.GetDeletedEntriesQueryable(baseParams)
                    .Select(
                        x =>
                            new DeletedEntriesDto
                            {
                                Id = x.Stage2Version.Stage3Version.Id,
                                RealityObject = x.Stage2Version.Stage3Version.RealityObject.Address,
                                CommonEstateObjects = x.Stage2Version.Stage3Version.CommonEstateObjects,
                                Year = x.Stage2Version.Stage3Version.Year,
                                IndexNumber = x.Stage2Version.Stage3Version.IndexNumber,
                                Sum = x.Stage2Version.Stage3Version.Sum,
                                IsChangedYear = x.Stage2Version.Stage3Version.IsChangedYear
                            })
                    .ToList()
                    .Distinct(x => x.Id)
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return result;
            }
            finally
            {
                this.Container.Release(actualizeService);
            }
        }

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult GetAddEntriesList(BaseParams baseParams)
        {
            var actualizeService = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                //выдаются конструктивные элементы, еще не добавленные

                var result = actualizeService.GetAddEntriesQueryable(baseParams)
                    .Select(
                        x => new
                        {
                            x.Id,
                            RealityObject = x.RealityObject.Address,
                            CommonEstateObject = x.StructuralElement.Group.CommonEstateObject.Name,
                            StructuralElement = x.StructuralElement.Name
                        });

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                this.Container.Release(actualizeService);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию стоимости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult GetActualizeSumEntriesList(BaseParams baseParams)
        {
            var actualizeService = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var loadParams = this.GetLoadParam(baseParams);

                var result = actualizeService.GetActualizeSumEntriesQueryable(baseParams);

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                this.Container.Release(actualizeService);
            }
        }

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult GetActualizeYearEntriesList(BaseParams baseParams)
        {
            var actualizeService = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var result = actualizeService.GetActualizeYearEntriesQueryable(baseParams)
                    .Select(
                        x => new
                        {
                            x.Id,
                            RealityObject = x.RealityObject.Address,
                            CommonEstateObject = x.CommonEstateObjects,
                            x.Sum,
                            x.Year,
                            x.Changes,
                            x.IsChangedYear
                        });

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                this.Container.Release(actualizeService);
            }
        }

        /// <summary>
        /// Получить список записей на добавление актуализации изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult GetActualizeYearChangeEntriesList(BaseParams baseParams)
        {
            var actualizeService = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var result = actualizeService.GetActualizeYearChangeEntriesQueryable(baseParams)
                    .Select(
                        x => new
                        {
                            x.Id,
                            RealityObject = x.RealityObject.Address,
                            CommonEstateObject = x.CommonEstateObjects,
                            x.Sum,
                            x.Year,
                            x.Changes,
                            x.IsChangedYear
                        });

                return new ListDataResult(result.Order(loadParams).Paging(loadParams).ToList(), result.Count());
            }
            finally
            {
                this.Container.Release(actualizeService);
            }
        }

        /// <inheritdoc />
        public IDataResult ListMainVersions(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var muIds = baseParams.Params.GetAs<string>("municipalityId").ToLongArray();

            return this.ProgramVersionDomain.GetAll()
                .Where(x => x.IsMain)
                .WhereIfContains(muIds.IsNotEmpty(), x => x.Municipality.Id, muIds)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    x.Name,
                    x.VersionDate,
                    x.IsMain
                })
                .ToListDataResult(loadParams);
        }

        /// <summary>
        /// Копировать версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult CopyVersion(BaseParams baseParams)
        {
            var newName = baseParams.Params.GetAs<string>("newName");
            var newDate = baseParams.Params.GetAs<DateTime>("newDate");
            var versionId = baseParams.Params.GetAs<long>("versionId");

            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var versionRecSt3Domain = this.Container.ResolveDomain<VersionRecord>();
            var versionRecSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versionRecSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var dpkrCorrectionDomain = this.Container.ResolveDomain<DpkrCorrectionStage2>();
            var publishProgDomain = this.Container.ResolveDomain<PublishedProgram>();
            var publishProgRecDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var subsidyRecVersDomain = this.Container.ResolveDomain<SubsidyRecordVersion>();
            var shortProgramDifitsitDomain = this.Container.ResolveDomain<ShortProgramDifitsit>();
            var shortProgramRecordDomain = this.Container.ResolveDomain<ShortProgramRecord>();
            var versionParamDomain = this.Container.ResolveDomain<VersionParam>();
            var typeWorkSt1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var fileManager = this.Container.Resolve<IFileManager>();        

            try
            {     
                var versSt3Recs = versionRecSt3Domain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .Select(x=> new {
                        x.Changes,
                        x.CommonEstateObjects,                     
                        x.Id,    
                        x.Point,
                        x.IndexNumber,                    
                        x.IsChangedYear, 
                        x.IsManuallyCorrect,
                        x.ObjectVersion,
                        RealityObject = new {x.RealityObject.Id, x.RealityObject.Address},
                        x.Show,
                        x.StoredCriteria,
                        x.SubProgram,
                        x.Sum,
                        x.Year,
                        x.YearCalculated
                    })
                    .ToList();

                var versSt2Recs = versionRecSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
                    .GroupBy(x => x.Stage3Version.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var versSt1Recs = versionRecSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .GroupBy(x => x.Stage2Version.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var typeWorkSt1Recs = typeWorkSt1Domain.GetAll()
                    .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.Id == versionId)
                    .GroupBy(x => x.Stage1Version.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var correctionRecs = dpkrCorrectionDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                    .GroupBy(x => x.Stage2.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var publishProg = publishProgDomain.GetAll().FirstOrDefault(x => x.ProgramVersion.Id == versionId);

                var publishProgRecs = publishProgRecDomain.GetAll()
                    .Where(x => x.Stage2 != null)
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                    .GroupBy(x => x.Stage2.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var shortProgRecs = shortProgramRecordDomain.GetAll()
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                    .GroupBy(x => x.Stage2.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var subsidyRecs = subsidyRecVersDomain.GetAll()
                    .Where(x => x.Version.Id == versionId)
                    .ToList();

                var shortProgDifisitRecs = shortProgramDifitsitDomain.GetAll()
                    .Where(x => x.Version.Id == versionId)
                    .ToList();

                var versionParams = versionParamDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == versionId)
                    .ToList();

                var copiedVersion = programVersionDomain.Get(versionId);

                var session = sessionProvider.OpenStatelessSession();

                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        var newVersion = new ProgramVersion
                        {
                            Name = newName,
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            Municipality = copiedVersion.Municipality,
                            VersionDate = newDate,
                            State = copiedVersion.State,
                            IsMain = false,
                            ActualizeDate = copiedVersion.ActualizeDate,
                            ParentVersion = copiedVersion
                        };

                        session.Insert(newVersion);

                        PublishedProgram newPublishProgram = null;
                        if (publishProg != null)
                        {
                            newPublishProgram = new PublishedProgram()
                            {
                                ProgramVersion = newVersion,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                State = publishProg.State,
                                EcpSigned = publishProg.EcpSigned,
                                FileSign = this.ReCreateFile(publishProg.FileSign, fileManager),
                                FileXml = this.ReCreateFile(publishProg.FileXml, fileManager),
                                FilePdf = this.ReCreateFile(publishProg.FilePdf, fileManager),
                                FileCertificate = this.ReCreateFile(publishProg.FileCertificate, fileManager),
                                SignDate = publishProg.SignDate
                            };

                            session.Insert(newPublishProgram);
                        }

                        foreach (var versSt3 in versSt3Recs)
                        {
                            var newVersSt3 = new VersionRecord
                            {
                                ProgramVersion = newVersion,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                RealityObject = new RealityObject{ Id = versSt3.RealityObject.Id, Address = versSt3.RealityObject.Address },
                                Year = versSt3.Year,
                                YearCalculated = versSt3.YearCalculated,
                                CommonEstateObjects = versSt3.CommonEstateObjects,
                                Sum = versSt3.Sum,
                                ObjectVersion = versSt3.ObjectVersion,
                                IsChangedYear = versSt3.IsChangedYear,
                                IndexNumber = versSt3.IndexNumber,
                                Point = versSt3.Point,
                                StoredCriteria = versSt3.StoredCriteria,
                                IsManuallyCorrect = versSt3.IsManuallyCorrect,
                                Changes = versSt3.Changes,
                                Show = versSt3.Show,
                                SubProgram = versSt3.SubProgram,
                            };
                            if (versSt3.SubProgram)
                            {

                            }

                            session.Insert(newVersSt3);

                            var tempVersSt2Recs = versSt2Recs.Get(versSt3.Id) ?? new VersionRecordStage2[0];

                            foreach (var versSt2 in tempVersSt2Recs)
                            {
                                var newVersSt2 = new VersionRecordStage2
                                {
                                    Stage3Version = newVersSt3,
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectEditDate = DateTime.Now,
                                    CommonEstateObjectWeight = versSt2.CommonEstateObjectWeight,
                                    CommonEstateObject = versSt2.CommonEstateObject,
                                    Sum = versSt2.Sum
                                };

                                session.Insert(newVersSt2);

                                var tempVersSt1Recs = versSt1Recs.Get(versSt2.Id) ?? new VersionRecordStage1[0];

                                foreach (var versSt1 in tempVersSt1Recs)
                                {
                                    var newVersSt1 = new VersionRecordStage1
                                    {
                                        Stage2Version = newVersSt2,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now,
                                        RealityObject = versSt1.RealityObject,
                                        StructuralElement = versSt1.StructuralElement,
                                        Year = versSt1.Year,
                                        Sum = versSt1.Sum,
                                        SumService = versSt1.SumService,
                                        Volume = versSt1.Volume,
                                        VersionRecordState = versSt1.VersionRecordState,
                                        StateChangeDate = versSt1.StateChangeDate
                                    };

                                    session.Insert(newVersSt1);

                                    var tempTypeWorkSt1Recs = typeWorkSt1Recs.Get(versSt1.Id) ?? new TypeWorkCrVersionStage1[0];

                                    foreach (var typeWorkSt1 in tempTypeWorkSt1Recs)
                                    {
                                        var newTypeWorkSt1 = new TypeWorkCrVersionStage1
                                        {
                                            Stage1Version = newVersSt1,
                                            ObjectCreateDate = DateTime.Now,
                                            ObjectEditDate = DateTime.Now,
                                            TypeWorkCr = typeWorkSt1.TypeWorkCr,
                                            UnitMeasure = typeWorkSt1.UnitMeasure,
                                            CalcBy = typeWorkSt1.CalcBy,
                                            Volume = typeWorkSt1.Volume,
                                            Sum = typeWorkSt1.Sum
                                        };

                                        session.Insert(newTypeWorkSt1);
                                    }
                                }

                                var tempCorrectionRecs = correctionRecs.Get(versSt2.Id) ?? new DpkrCorrectionStage2[0];

                                foreach (var correction in tempCorrectionRecs)
                                {
                                    var newCorrection = new DpkrCorrectionStage2()
                                    {
                                        Stage2 = newVersSt2,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now,
                                        RealityObject = correction.RealityObject,
                                        PlanYear = correction.PlanYear,
                                        YearCollection = correction.YearCollection,
                                        OwnersMoneyBalance = correction.OwnersMoneyBalance,
                                        HasCredit = correction.HasCredit,
                                        BudgetFundBalance = correction.BudgetFundBalance,
                                        BudgetRegionBalance = correction.BudgetRegionBalance,
                                        BudgetMunicipalityBalance = correction.BudgetMunicipalityBalance,
                                        OtherSourceBalance = correction.OtherSourceBalance,
                                        FundBudgetNeed = correction.FundBudgetNeed,
                                        RegionBudgetNeed = correction.RegionBudgetNeed,
                                        MunicipalityBudgetNeed = correction.MunicipalityBudgetNeed,
                                        OtherSourcesBudgetNeed = correction.OtherSourcesBudgetNeed,
                                        OwnersMoneyNeed = correction.OwnersMoneyNeed,
                                        IsOwnerMoneyBalanceCalculated = correction.IsOwnerMoneyBalanceCalculated
                                    };

                                    session.Insert(newCorrection);
                                }

                                var tempPublishProgRecs = publishProgRecs.Get(versSt2.Id) ?? new PublishedProgramRecord[0];

                                foreach (var publishProgRec in tempPublishProgRecs)
                                {
                                    var newPublishProgRec = new PublishedProgramRecord
                                    {
                                        Stage2 = newVersSt2,
                                        RealityObject = newVersSt2.Stage3Version.RealityObject,
                                        PublishedProgram = newPublishProgram,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now,
                                        Sum = publishProgRec.Sum,
                                        IndexNumber = publishProgRec.IndexNumber,
                                        Locality = publishProgRec.Locality,
                                        Street = publishProgRec.Street,
                                        House = publishProgRec.House,
                                        Housing = publishProgRec.Housing,
                                        Address = publishProgRec.Address,
                                        CommissioningYear = publishProgRec.CommissioningYear,
                                        CommonEstateobject = publishProgRec.CommonEstateobject,
                                        Wear = publishProgRec.Wear,
                                        LastOverhaulYear = publishProgRec.LastOverhaulYear,
                                        PublishedYear = publishProgRec.PublishedYear
                                    };

                                    session.Insert(newPublishProgRec);
                                }

                                var tempShortProgRecs = shortProgRecs.Get(versSt2.Id) ?? new ShortProgramRecord[0];

                                foreach (var shortProgRec in tempShortProgRecs)
                                {
                                    var newShortProgRec = new ShortProgramRecord
                                    {
                                        Stage2 = newVersSt2,
                                        ObjectCreateDate = DateTime.Now,
                                        ObjectEditDate = DateTime.Now,
                                        RealityObject = shortProgRec.RealityObject,
                                        Year = shortProgRec.Year,
                                        OwnerSumForCr = shortProgRec.OwnerSumForCr,
                                        BudgetFcr = shortProgRec.BudgetFcr,
                                        BudgetOtherSource = shortProgRec.BudgetOtherSource,
                                        BudgetRegion = shortProgRec.BudgetRegion,
                                        BudgetMunicipality = shortProgRec.BudgetMunicipality,
                                        Difitsit = shortProgRec.Difitsit
                                    };

                                    session.Insert(newShortProgRec);
                                }
                            }
                        }

                        foreach (var subsidyVersRec in subsidyRecs)
                        {
                            var newSubsidyVersRec = new SubsidyRecordVersion
                            {
                                Version = newVersion,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                SubsidyRecord = subsidyVersRec.SubsidyRecord,
                                BudgetCr = subsidyVersRec.BudgetCr,
                                CorrectionFinance = subsidyVersRec.CorrectionFinance,
                                BalanceAfterCr = subsidyVersRec.BalanceAfterCr,
                                SubsidyYear = subsidyVersRec.SubsidyYear,
                                BudgetRegion = subsidyVersRec.BudgetRegion,
                                BudgetMunicipality = subsidyVersRec.BudgetMunicipality,
                                BudgetFcr = subsidyVersRec.BudgetFcr,
                                BudgetOtherSource = subsidyVersRec.BudgetOtherSource,
                                PlanOwnerCollection = subsidyVersRec.PlanOwnerCollection,
                                PlanOwnerPercent = subsidyVersRec.PlanOwnerPercent,
                                NotReduceSizePercent = subsidyVersRec.NotReduceSizePercent,
                                OwnerSumForCr = subsidyVersRec.OwnerSumForCr,
                                DateCalcOwnerCollection = subsidyVersRec.DateCalcOwnerCollection
                            };

                            session.Insert(newSubsidyVersRec);
                        }

                        foreach (var shortProgDifisit in shortProgDifisitRecs)
                        {
                            var newShortProgramDefisit = new ShortProgramDifitsit
                            {
                                Version = newVersion,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                Municipality = shortProgDifisit.Municipality,
                                Year = shortProgDifisit.Year,
                                Difitsit = shortProgDifisit.Difitsit,
                                BudgetRegionShare = shortProgDifisit.BudgetRegionShare,
                            };

                            session.Insert(newShortProgramDefisit);
                        }

                        foreach (var versionParam in versionParams)
                        {
                            var newVersionParam = new VersionParam
                            {
                                ProgramVersion = newVersion,
                                ObjectCreateDate = DateTime.Now,
                                ObjectEditDate = DateTime.Now,
                                Municipality = versionParam.Municipality,
                                Weight = versionParam.Weight,
                                Code = versionParam.Code,
                            };

                            session.Insert(newVersionParam);
                        }

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                this.Container.Release(versionRecSt3Domain);
                this.Container.Release(versionRecSt2Domain);
                this.Container.Release(versionRecSt1Domain);
                this.Container.Release(dpkrCorrectionDomain);
                this.Container.Release(publishProgDomain);
                this.Container.Release(publishProgRecDomain);
                this.Container.Release(subsidyRecVersDomain);
                this.Container.Release(shortProgramDifitsitDomain);
                this.Container.Release(shortProgramRecordDomain);
                this.Container.Release(versionParamDomain);
                this.Container.Release(programVersionDomain);
                this.Container.Release(typeWorkSt1Domain);
                this.Container.Release(sessionProvider);
                this.Container.Release(fileManager);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Список для массового изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult ListForMassChangeYear(BaseParams baseParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();

            var periodEndYear = config.ProgrammPeriodEnd;
            var typeCorrection = config.SubsidyConfig.TypeCorrection;
            var versionId = baseParams.Params.GetAsId("versionId");
            var ids = baseParams.Params.GetAs<string>("Id").ToLongArray();

            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var loadParams = this.GetLoadParam(baseParams);

            try
            {
                var data = versionRecordDomain.GetAll()
                    .Where(x => x.IndexNumber > 0 || x.Year <= periodEndYear)
                    .WhereIf(typeCorrection == TypeCorrection.WithFixYear, x => !x.IsManuallyCorrect)
                    .WhereIf(ids.Any(), x => ids.Contains(x.Id))
                    .WhereIf(!ids.Any(), x => x.ProgramVersion.Id == versionId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            Municipality = x.RealityObject.Municipality.Name,
                            RealityObject = x.RealityObject.Address,
                            x.CommonEstateObjects,
                            x.Year,
                            x.IndexNumber,
                            x.IsChangedYear,
                            x.Point,
                            x.Sum,
                            x.Changes
                        })
                    .Filter(loadParams, this.Container)
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Year)
                    .OrderThenIf(loadParams.Order.Length == 0, true, x => x.IndexNumber);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(versionRecordDomain);
            }
        }

        /// <summary>
        /// Массовое изменение года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public IDataResult MassChangeYear(BaseParams baseParams)
        {
            var recIds = baseParams.Params.GetAs<string>("recIds").ToLongArray();
            var versionId = baseParams.Params.GetAsId("versionId");
            var newYear = baseParams.Params.GetAs<int>("newYear");

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();

            var useFixationPublishedYears = config.SubsidyConfig.UseFixationPublishedYears;

            var publishedProgRecDomain = this.Container.ResolveDomain<PublishedProgramRecord>();
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var listNum = new List<int>();
            try
            {
                var listToUpd = new List<VersionRecord>();
                var publishedYears = publishedProgRecDomain.GetAll()
                    .Where(x => x.Stage2 != null)
                    .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                    .Select(x => new {x.Stage2.Stage3Version.Id, x.PublishedYear})
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.SafeMin(x => x.PublishedYear));

                var recForChange = versionRecordDomain.GetAll()
                    .Where(x => recIds.Contains(x.Id))
                    .ToList();

                foreach (var rec in recForChange)
                {
                    if (useFixationPublishedYears == TypeUsage.Used && publishedYears.ContainsKey(rec.Id) && publishedYears[rec.Id] < newYear)
                    {
                        listNum.Add(rec.IndexNumber);
                        continue;
                    }

                    rec.IsChangedYear = true;
                    rec.Year = newYear;
                    rec.Changes = $"Изменено: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
                    rec.IsManuallyCorrect = true;

                    listToUpd.Add(rec);
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToUpd, 1000, true, true);
            }
            finally
            {
                this.Container.Release(publishedProgRecDomain);
                this.Container.Release(versionRecordDomain);
            }

            if (listNum.Any())
            {
                return
                    new BaseDataResult(
                        "У некоторых записей нельзя изменить год, так как год опубликованной программы меньше чем изменяемый год. Номера записей: "
                            + listNum.OrderBy(x => x).Select(x => x.ToStr()).AggregateWithSeparator(", "));
            }

            return new BaseDataResult("Год успешно изменен");
        }

        private void ChangeYearByStructElem(VersSt1Proxy[] records, VersionRecord versionRecord, Dictionary<long, 
                                            int> publishYearByStage2Dict, Dictionary<long, Tuple<int, int>> stage1Changes)
        {
            var versSt1Domain = this.Container.ResolveDomain<VersionRecordStage1>();
            var versSt2Domain = this.Container.ResolveDomain<VersionRecordStage2>();
            var versRecordDomain = this.Container.ResolveDomain<VersionRecord>();

            try
            {
                var st1Ids = records.Select(x => x.Id).ToArray();

                var planYearById = records.ToDictionary(x => x.Id, y => y.PlanYear);

                var st2WithNoTransferSe = versSt1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.Id == versionRecord.Id)
                    .Where(x => !st1Ids.Contains(x.Id))
                    .Select(x => x.Stage2Version.Id).ToHashSet();

                var st1Recs = versSt1Domain.GetAll()
                    .Fetch(x => x.Stage2Version)
                    .Where(x => st1Ids.Contains(x.Id))
                    .AsEnumerable()
                    .Select(
                        x =>
                        {
                            var newYear = planYearById.Get(x.Id);

                            if (stage1Changes.ContainsKey(x.Id))
                            {
                                stage1Changes[x.Id] = Tuple.Create(stage1Changes[x.Id].Item1, newYear);
                            }
                            else
                            {
                                stage1Changes.Add(x.Id, Tuple.Create(x.Year, newYear));
                            }

                            x.Year = newYear;

                            return x;
                        })
                    .GroupBy(x => x.Stage2Version.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                var st3List = new HashSet<VersionRecord>();

                if (!st2WithNoTransferSe.Any())
                {
                    versionRecord.Year = records.SafeMin(x => x.PlanYear);
                    versionRecord.Sum = 0M;
                    versionRecord.CommonEstateObjects = string.Empty;
                    versionRecord.IsManuallyCorrect = true;
                    versionRecord.IsChangedYear = true;
                    versionRecord.Changes = $"Изменено: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}";
                    st3List.Add(versionRecord);
                }

                var st2List = new HashSet<VersionRecordStage2>();
                var st1List = new List<VersionRecordStage1>();
                foreach (var keyPair in st1Recs)
                {
                    var newSt2 = st2WithNoTransferSe.Contains(keyPair.Key)
                        ? null
                        : keyPair.Value.Select(x => x.Stage2Version).First();

                    keyPair.Value
                        .GroupBy(x => x.Year)
                        .ForEach(
                            x =>
                            {
                                foreach (var st1 in x)
                                {
                                    var tempSt3 = st3List.FirstOrDefault(y => y.Year == st1.Year) ?? new VersionRecord
                                    {
                                        ProgramVersion = versionRecord.ProgramVersion,
                                        Year = st1.Year,
                                        IsChangedYear = true,
                                        Point = versionRecord.Point,
                                        StoredCriteria = versionRecord.StoredCriteria,
                                        StoredPointParams = versionRecord.StoredPointParams,
                                        IsManuallyCorrect = true,
                                        CommonEstateObjects = string.Empty,
                                        RealityObject = versionRecord.RealityObject,
                                        IsDividedRec = true,
                                        PublishYearForDividedRec = publishYearByStage2Dict.Get(st1.Stage2Version.Id),
                                        Changes = $"Изменено: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}"
                                    };

                                    tempSt3.Sum += st1.Sum;

                                    var tempSt2 = newSt2 ?? new VersionRecordStage2
                                    {
                                        CommonEstateObjectWeight = st1.Stage2Version.CommonEstateObjectWeight,
                                        Sum = x.SafeSum(y => y.Sum + y.SumService),
                                        CommonEstateObject = st1.Stage2Version.CommonEstateObject,
                                        Stage3Version = tempSt3
                                    };

                                    if (!tempSt3.CommonEstateObjects.Contains(tempSt2.CommonEstateObject.Name))
                                    {
                                        tempSt3.CommonEstateObjects = tempSt3.CommonEstateObjects.IsEmpty()
                                            ? tempSt2.CommonEstateObject.Name
                                            : "{0}, {1}".FormatUsing(
                                                tempSt3.CommonEstateObjects,
                                                tempSt2.CommonEstateObject.Name);
                                    }

                                    newSt2 = tempSt2;

                                    newSt2.Stage3Version = tempSt3;
                                    st1.Stage2Version = newSt2;

                                    st3List.Add(tempSt3);
                                    st2List.Add(tempSt2);
                                    st1List.Add(st1);
                                }

                                newSt2 = null;
                            });
                }

                TransactionHelper.InsertInManyTransactions(this.Container, st3List, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, st2List, 1000, true, true);
                TransactionHelper.InsertInManyTransactions(this.Container, st1List, 1000, true, true);

                st2List.Clear();

                var otherStage1Query = versSt1Domain.GetAll().Where(x => !st1Ids.Contains(x.Id))
                    .Where(x => x.Stage2Version.Stage3Version.Id == versionRecord.Id);

                if (otherStage1Query.Any())
                {
                    versionRecord.CommonEstateObjects = string.Empty;
                    versionRecord.Sum = 0M;

                    otherStage1Query
                        .AsEnumerable()
                        .GroupBy(x => x.Stage2Version)
                        .ForEach(
                            x =>
                            {
                                x.Key.Sum = x.SafeSum(y => y.Sum);

                                versionRecord.Sum += x.Key.Sum;
                                versionRecord.CommonEstateObjects = versionRecord.CommonEstateObjects.IsEmpty()
                                    ? x.Key.CommonEstateObject.Name
                                    : $"{versionRecord.CommonEstateObjects}, {x.Key.CommonEstateObject.Name}";

                                st2List.Add(x.Key);
                            });

                    if (st2List.Count > 0)
                    {
                        TransactionHelper.InsertInManyTransactions(this.Container, st2List, 1000, true, true);
                        versRecordDomain.Update(versionRecord);
                    }
                }
            }
            finally
            {
                this.Container.Release(versSt1Domain);
                this.Container.Release(versSt2Domain);
                this.Container.Release(versRecordDomain);
            }
        }

        /// <summary>
        /// Пересоздать файл
        /// </summary>
        /// <param name="fileInfo">Описание файла</param>
        /// <param name="fileManager">Менеджер файлов</param>
        /// <returns></returns>
        protected FileInfo ReCreateFile(FileInfo fileInfo, IFileManager fileManager)
        {
            if (fileInfo == null)
            {
                return null;
            }

            var pathDir =
                new DirectoryInfo(
                    this.Container.Resolve<IConfigProvider>().GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"].GetAs(
                        "FileDirectory",
                        string.Empty));
            var path = Path.Combine(pathDir.FullName, $"{fileInfo.Id}.{fileInfo.Extention}");
            if (File.Exists(path))
            {
                var fileInfoStream = new MemoryStream(File.ReadAllBytes(path));
                var newFileInfo = fileManager.SaveFile(fileInfoStream, $"{fileInfo.Name}.{fileInfo.Extention}");
                return newFileInfo;
            }

            return null;
        }

        /// <summary>
        /// Получить параметры запроса
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        protected virtual LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }

        private class VersionData
        {
            public Municipality Municipality { get; set; }

            public DateTime Date { get; set; }

            public string Name { get; set; }

            public bool IsMain { get; set; }
        }

        /// <summary>
        /// Прокси версии 1 этапа
        /// </summary>
        private class VersSt1Proxy
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Плановый год
            /// </summary>
            public int PlanYear { get; set; }
        }

        private class MunicipalityVersion
        {
            public ProgramVersion ProgramVersion { get; set; }

            public IEnumerable<ProgramVersion> VersionsForUpdate { get; set; }

            public IEnumerable<VersionParam> VersionParams { get; set; }

            public IEnumerable<VersionRecord> Stage3Records { get; set; }

            public IEnumerable<VersionRecordStage2> Stage2Records { get; set; }

            public IEnumerable<VersionRecordStage1> Stage1Records { get; set; }
        }
    }
}