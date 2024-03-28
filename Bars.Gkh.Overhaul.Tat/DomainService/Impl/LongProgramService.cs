namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Overhaul.Tat.ProgrammPriorityParams;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Microsoft.CSharp.RuntimeBinder;
    using Binder = Microsoft.CSharp.RuntimeBinder.Binder;

    public class LongProgramService : ILongProgramService
    {
        /// <summary>
        /// Поле, для хранения "вычислителей" балла очередности
        /// </summary>
        private IProgrammPriorityParam[] evaluators;

        #region Dependency Injection Members
        public IDomainService<RealityObjectStructuralElementInProgrammStage2> Stage2Service { get; set; }
        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1Service { get; set; }
        public IDomainService<RealityObjectStructuralElementInProgrammStage3> Stage3Service { get; set; }
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectedDomain { get; set; }
        public IDomainService<SubsidyMunicipality> SubsidyMuService { get; set; }
        public IDomainService<DpkrGroupedYear> DpkrPublishDomain { get; set; }
        public IDomainService<VersionRecordStage2> VersionStage2Domain { get; set; }
        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }
        public IDomainService<EmergencyObject> EmergencyDomain { get; set; }
        public IDomainService<VersionRecord> VersionRecDomain { get; set; }
        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }
        public IDomainService<PublishedProgram> PublishedProgramDomain { get; set; }
        public IDomainService<Municipality> MunicipalityDomain { get; set; }
        public ISessionProvider SessionProvider { get; set; }
        public IWindsorContainer Container { get; set; }
        public IRealityObjectStructElementService roStrElService { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IStage2Service StagesService { get; set; } 
        #endregion

        /// <summary>
        /// Свойство, для получения "вычислителей" балла очередности
        /// </summary>
        private IEnumerable<IProgrammPriorityParam> Evaluators
        {
            get
            {
                return evaluators ?? (evaluators = Container.ResolveAll<IProgrammPriorityParam>());
            }
        }

        public IDataResult MakeStage1(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("moId");

            if (moId == 0)
            {
                return new BaseDataResult(false, "Не задан параметр \"Муниципальное образование\"");
            }

            var currOp = Container.Resolve<IGkhUserManager>().GetActiveOperator();

#warning некорректная проверка на МО у оператора
            if (currOp != null)
            {
                var municipalityIds = Container.Resolve<IGkhUserManager>().GetMunicipalityIds();

                var moAvailableIds =
                    Container.Resolve<IDomainService<Municipality>>().GetAll()
                        .Where(x => municipalityIds.Contains(x.Id))
                        .Select(x => x.Id)
                        .ToList();

                if (!moAvailableIds.Contains(moId))
                {
                    return new BaseDataResult(false, "Некорректно задан параметр \"Муниципальное образование\""); 
                }
            }

            // Получаем параметры из конфига
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;
            
            // получаем записи 1 этапа
            var listToSave = this.StagesService.GetStage1(startYear, endYear, moId);

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        session.CreateSQLQuery(@"DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG 
                                                WHERE ID IN (SELECT s1.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG s1
                                                LEFT JOIN OVRHL_RO_STRUCT_EL ro_se ON s1.RO_SE_ID = ro_se.ID
                                                LEFT JOIN GKH_REALITY_OBJECT ro ON ro_se.RO_ID = ro.ID
                                                LEFT JOIN GKH_DICT_MUNICIPALITY mu ON ro.MUNICIPALITY_ID = mu.ID
                                                WHERE mu.ID = :municipalityId)")
                               .SetParameter("municipalityId", moId)
                               .ExecuteUpdate();
                        session.CreateSQLQuery(@"DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 
                                                WHERE ID IN (SELECT s2.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_2 s2
                                                LEFT JOIN GKH_REALITY_OBJECT ro ON s2.RO_ID = ro.ID
                                                LEFT JOIN GKH_DICT_MUNICIPALITY mu ON ro.MUNICIPALITY_ID = mu.ID
                                                WHERE mu.ID = :municipalityId)")
                               .SetParameter("municipalityId", moId)
                               .ExecuteUpdate();
                        session.CreateSQLQuery(@"DELETE FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 
                                                WHERE ID IN (SELECT s3.ID FROM OVRHL_RO_STRUCT_EL_IN_PRG_3 s3
                                                LEFT JOIN GKH_REALITY_OBJECT ro ON s3.RO_ID = ro.ID
                                                LEFT JOIN GKH_DICT_MUNICIPALITY mu ON ro.MUNICIPALITY_ID = mu.ID
                                                WHERE mu.ID = :municipalityId)")
                               .SetParameter("municipalityId", moId)
                               .ExecuteUpdate();

                        listToSave.ForEach(x => session.Insert(x));

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult();
        }

        public IDataResult MakeStage2(BaseParams baseParams)
        {
            //перенесено в Stage2Service
            throw new NotImplementedException();
        }

        public IDataResult CreateDpkrForPublish(BaseParams baseParams)
        {
            var sessionProvider = Container.Resolve<ISessionProvider>();
            try
            {
                var moId = baseParams.Params.GetAs<long>("mo_id");

                var config = Container.GetGkhConfig<OverhaulTatConfig>();

                var period = config.PublicationPeriod;

                var isMass = baseParams.Params.ContainsKey("isMass") && baseParams.Params.GetAs<bool>("isMass");

                if (moId <= 0 && !isMass)
                {
                    return new BaseDataResult(false, "Не удалось получить муниципальное образование");
                }

                if (period == 0)
                {
                    return new BaseDataResult(false, "Не найден параметр \" Период для публикации \"");
                }

                // получаем основную версию
                var versions = ProgramVersionDomain.GetAll()
                    .Where(x => x.IsMain)
                    .WhereIf(!isMass, x => x.Municipality.Id == moId);

                if (!versions.Any())
                {
                    return new BaseDataResult(false, "Не задана основная версия");
                }

                var session = sessionProvider.OpenStatelessSession();

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        foreach (var programVersion in versions)
                        {
                            // Удаляем записи опубликованной программы именно этой основной версии
                            session.CreateSQLQuery(string.Format("delete from OVRHL_PUBLISH_PRG_REC where PUBLISH_PRG_ID in (" +
                                "select id from OVRHL_PUBLISH_PRG where version_id = {0})", programVersion.Id)).ExecuteUpdate();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                // Проверяем Существует ли нужный статус и если нет то создаем новый
                var firstState = StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_published_program" && x.StartState);

                if (firstState == null)
                {
                    firstState = new State
                    {
                        Name = "Черновик",
                        Code = "Черновик",
                        StartState = true,
                        TypeId = "ovrhl_published_program"
                    };

                    StateDomain.Save(firstState);
                }

                var publishPrograms = PublishedProgramDomain.GetAll()
                    .Where(x => versions.Any(y => y.Id == x.ProgramVersion.Id))
                    .ToList();

                // Грохаем существующую запись опубликованной программы поскольку
                // у нее могли существоват ьуже ненужные подписи ЭЦП
                if (publishPrograms.Any())
                {
                    publishPrograms.ForEach(x => PublishedProgramDomain.Delete(x.Id));
                }

                var listPubProgsToSave = new List<PublishedProgram>();
                var listRecordsToSave = new List<PublishedProgramRecord>();

                // Получаем записи корректировки и поним создаем опубликованную программу
                var dataCorrections =
                    DpkrCorrectedDomain.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .Select(x => new
                        {
                            x.Id,
                            st2Id = x.Stage2.Id,
                            Year = x.PlanYear,
                            x.Stage2.Stage3Version.IndexNumber,
                            Locality = x.RealityObject.FiasAddress.PlaceName,
                            Street = x.RealityObject.FiasAddress.StreetName,
                            x.RealityObject.FiasAddress.House,
                            x.RealityObject.FiasAddress.Housing,
                            Address = x.RealityObject.FiasAddress.AddressName,
                            CommonEstateobject = x.Stage2.CommonEstateObject.Name,
                            CommissioningYear = x.RealityObject.BuildYear.HasValue ? x.RealityObject.BuildYear.Value : 0,
                            versionId = x.Stage2.Stage3Version.ProgramVersion.Id,
                            x.Stage2.Sum
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.versionId)
                        .ToDictionary(x => x.Key, y => y.ToArray());

                foreach (var programVersion in versions)
                {
                    var publishProg = new PublishedProgram
                    {
                        State = firstState,
                        ProgramVersion = programVersion
                    };

                    listPubProgsToSave.Add(publishProg);

                    var tempCorrection = dataCorrections.ContainsKey(programVersion.Id)
                                             ? dataCorrections[programVersion.Id]
                                             : null;

                    if (tempCorrection != null)
                    {
                        foreach (var rec in tempCorrection)
                        {
                            var newRec = new PublishedProgramRecord
                            {
                                PublishedProgram = publishProg,
                                Stage2 = new VersionRecordStage2 { Id = rec.st2Id },
                                PublishedYear = rec.Year,
                                IndexNumber = rec.IndexNumber,
                                Locality = rec.Locality,
                                Street = rec.Street,
                                House = rec.House,
                                Housing = rec.Housing,
                                Address = rec.Address,
                                CommonEstateobject = rec.CommonEstateobject,
                                CommissioningYear = rec.CommissioningYear,
                                Sum = rec.Sum
                            };

                            listRecordsToSave.Add(newRec);
                        }
                    }
                }

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        listPubProgsToSave.Where(x => x.Id == 0).ForEach(x => session.Insert(x));
                        listRecordsToSave.ForEach(x => session.Insert(x));

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
            catch (Exception exc)
            {
                return new BaseDataResult(false, string.Format("Ошибка очередности:{0}", exc.Message));
            }
            finally
            {
                Container.Release(sessionProvider);

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Получение параметров очередности ДПКР 
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public IDataResult GetParams(BaseParams baseParams)
        {
            var parameters =
                Container.ResolveAll<IProgrammPriorityParam>().Select(x => new { Id = x.Code, x.Name, x.Code });

            return new ListDataResult(parameters, parameters.Count());
        }

        /// <summary>
        /// Установка очередности ДПКР
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат</returns>
        public IDataResult SetPriority(BaseParams baseParams)
        {
            throw new NotImplementedException();

            /*

            var recs = baseParams.Params.GetAs<object[]>("records");
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var dict = new Dictionary<string, int>();
            if (recs == null)
            {
                return new BaseDataResult(false, "Не указаны параметры!");
            }

            for (int i = 0; i < recs.Length; i++)
            {
                var dd = recs[i] as DynamicDictionary;
                if (dd == null)
                {
                    continue;
                }

                var code = dd.Get("Code", string.Empty);

                if (dict.ContainsKey(code))
                {
                    return new BaseDataResult(false, "Параметры должны быть уникальными!");
                }

                dict.Add(code, dd.GetAs<long>("Order"));
            }

            var currPriorityParamsService = Container.Resolve<IDomainService<CurrentPrioirityParams>>();

            this.InTransaction(() =>
            {
                foreach (var value in currPriorityParamsService.GetAll())
                {
                    currPriorityParamsService.Delete(value.Id);
                }

                foreach (var value in dict)
                {
                    currPriorityParamsService.Save(new CurrentPrioirityParams
                    {
                        Code = value.Key,
                        Order = value.Value
                    });
                }
            });

            // Получаем параметры из конфига
            var endYear = OverhaullParamProvider.GetOverhaulParam<int>("ProgrammPeriodEnd", Container);

            // Такой словарик нужен для того чтобы быстро получать элементы при сохранении уже из готовых данных по Id
            var dictStage3 = Stage3Service.GetAll().ToDictionary(x => x.Id);

            // Внимательно - дергаем из сервиса 2 этапа, потому что 3 этап по сути является дубляжом 2 этапа!
            // RoCeoKey тут получаю поскольку в дальнейшем много раз используется этот ключ и чтобы каждый раз его не формирвоать 
            // пытаюсь выиграть в милисекундах
            var muStage3 = Stage2Service.GetAll()
                .Where(x => x.Stage3.RealityObject != null && x.CommonEstateObject != null)
                .Select(x => new Stage3Order
                {
                    Id = x.Stage3.Id,
                    RealityObject = x.Stage3.RealityObject,
                    Year = x.Stage3.Year,
                    Stage3 = x.Stage3,
                    CeoId = x.CommonEstateObject.Id,
                    RoCeoKey = x.Stage3.RealityObject.Id + "_" + x.CommonEstateObject.Id
                }).ToList();

            // RoStrElKey тут получаю поскольку в дальнейшем много раз используется этот ключ и чтобы каждый раз его не формирвоать 
            // пытаюсь выиграть в милисекундах
            var tempDataList = Stage1Service.GetAll()
                             .Where(x => x.StructuralElement != null && x.StructuralElement.RealityObject != null)
                             .Where(x => Stage3Service.GetAll().Any(y => y.Id == x.Stage2.Stage3.Id))
                             .Select(
                                 x =>
                                 new
                                     {
                                         RealityObjectId = x.StructuralElement.RealityObject.Id,
                                         StrElId = x.StructuralElement.Id,
                                         x.Stage2.Stage3.Id,
                                         x.StructuralElement.LastOverhaulYear,
                                         x.StructuralElement.StructuralElement.LifeTime,
                                         x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                                         x.StructuralElement.RealityObject.BuildYear,
                                         x.Year,
                                         CeoId = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Id,
                                         St3Year = x.Stage2.Stage3.Year,
                                         RoStrElKey = x.StructuralElement.RealityObject.Id + "_" + x.StructuralElement.Id
                                     })
                             .ToList();

            var years = Stage2Service.GetAll()
                    .Where(x => x.RealityObject != null && x.CommonEstateObject != null)
                    .Select(
                        x =>
                            new
                            {
                                Stage3Id = x.Stage3.Id,
                                CeoId = x.CommonEstateObject.Id,
                                RoId = x.RealityObject.Id,
                                x.Year,
                                RoCeoKey = x.RealityObject.Id + "_" + x.CommonEstateObject.Id
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.RoCeoKey)
                    .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Stage3Id, y => y.Year));

            // Для реализации задачи необходимо Брать реальный год предыдущего кап ремонта
            // То есть требуется организавать словарик который будет очень быстро получать год капремонта по 
            // конструктивным элементам, котоыре также выполняются по дому но при этом имеют меньший год кап ремонта
            // Цель такая чтобы получить быстро Реальный год капремонта
            // Key = "IdДома_IdКонстрЭлемента" , Value = " Года упорядоченные по убыванию"
            var dictYearsCr = tempDataList
                            .GroupBy(x => x.RoStrElKey)
                            .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.Year)
                                                            .Select(x => x.Year)
                                                            .ToList());

            var realLastOvrhlLifeTimeList =
                tempDataList.Select(x => new
                {
                    x.Id,
                    x.BuildYear,
                    LastOverhaulYear =
                        dictYearsCr.ContainsKey(x.RoStrElKey) && dictYearsCr[x.RoStrElKey].Any(y => y < x.Year)
                            ? dictYearsCr[x.RoStrElKey].First(y => y < x.Year)
                            : x.LastOverhaulYear != x.BuildYear.ToInt()
                                ? x.LastOverhaulYear
                                : 0,
                    LifeTime = x.LifeTimeAfterRepair > 0 && dictYearsCr.ContainsKey(x.RoStrElKey)
                               && dictYearsCr[x.RoStrElKey].Any(y => y < x.Year)
                        ? x.LifeTimeAfterRepair
                        : x.LifeTime
                    /* Пожалуста такое больше не пишите, поскольку по массиву из 100 тысяч хначений бегает очень медленно
                        LastOverhaulYear = tempDataList.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                            ? tempDataList.OrderByDescending(y => y.Year).First(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year).Year
                            : x.LastOverhaulYear,
                        LifeTime = x.LifeTimeAfterRepair > 0 && tempDataList.Any(y => x.RealityObjectId == y.RealityObjectId && y.StrElId == x.StrElId && y.Year < x.Year)
                        ? x.LifeTimeAfterRepair
                        : x.LifeTime
                        
                })
                    .ToList();

            var lifetimes = realLastOvrhlLifeTimeList
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Max(y => y.LifeTime));

            // вообщем тут идет замут такой что год последнего кап ремонта может быть = 0
            // В таком слчае мы берем ГодПостройкиДома, но и Год Постройки дома может быть = 0
            // В таком случае мы берем Год окончания программы
            var yearWithlifetimes =
                realLastOvrhlLifeTimeList
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key,
                        x => x.Max(y => y.LastOverhaulYear > 0 // Если Год Последнего Кап Ремонта > 0 то берем его
                            ? y.LastOverhaulYear + y.LifeTime
                            : y.BuildYear.HasValue && y.BuildYear.Value > 0
                                // Иначе , Если Год постройки дома > 0 то берем его
                                ? y.BuildYear.Value + y.LifeTime
                                : endYear // Иначе Берем просто ГодокОнчанияПрограммы
                            ));

            var weights = Stage2Service.GetAll()
                .Where(x => Stage3Service.GetAll().Any(y => y.Id == x.Stage3.Id))
                .Select(x => new
                {
                    x.Stage3.Id,
                    x.CommonEstateObject.Weight
                })
                .AsEnumerable()
                .ToDictionary(x => x.Id, y => y.Weight);

            var lastOverhaulYears = realLastOvrhlLifeTimeList
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.Max(y => y.LastOverhaulYear > 0 ? y.LastOverhaulYear : 0));

            try
            {
                // Незнаю зачем здесь делают Parallel поскольку тут вообще милисекунды
                // Но ладно нестал убирать пусть так будет
                Parallel.ForEach(muStage3,
                    item => //foreach (var stage3 in muStage3)
                    {
                        var key = item.RoCeoKey;
                        var injections = new
                        {
                            item.Id,
                            item.RealityObject,
                            item.Year,
                            OvrhlYears = lastOverhaulYears,
                            Lifetimes = lifetimes,
                            Weights = weights,
                            YearWithLifetimes = yearWithlifetimes,
                            Years = years.ContainsKey(key) ? years[key] : new Dictionary<int, int>()
                        };

                        CalculateOrder(item, dict.Keys, injections);
                    });

                // По-умолчанию сначала сортируем по плановому году 
                var proxyList2 = muStage3.OrderBy(x => x.Year);
                foreach (var item in dict.OrderBy(x => x.Value))
                {
                    var eval = this.ResolveEvaluator(item.Key, new object());
                    if (eval != null)
                    {
                        proxyList2 = eval.Asc ? proxyList2.ThenBy(x => x.OrderDict[item.Key]) : proxyList2.ThenByDescending(x => x.OrderDict[item.Key]);
                    }
                }

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var index = 1;
                        foreach (var item in proxyList2)
                        {
                            if (!dictStage3.ContainsKey(item.Id))
                                continue;

                            var st3 = dictStage3[item.Id];

                            this.FillStage3Criteria(st3, item.OrderDict);

                            st3.IndexNumber = index++;

                            session.Update(st3);
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            return new BaseDataResult();
*/
        }

        /// <summary>
        /// Заполнение критериев сортировки
        /// </summary>
        /// <param name="st3Item">Объект ДПКР</param>
        /// <param name="orderDict">Словарь данных приоритезации</param>
        private void FillStage3Criteria(RealityObjectStructuralElementInProgrammStage3 st3Item, Dictionary<string, decimal> orderDict)
        {
            st3Item.StoredCriteria = st3Item.StoredCriteria ?? new List<StoredPriorityParam>();
            st3Item.StoredCriteria.Clear();

            foreach (var param in orderDict)
            {
                st3Item.StoredCriteria.Add(new StoredPriorityParam
                {
                    Criterion = param.Key,
                    Value = param.Value.ToStr()
                });
            }
        }

        /// <summary>
        /// Вычисление параметра приоритезации
        /// </summary>
        /// <param name="stage3">Объект ДПКР</param>
        /// <param name="keys">Названия параметров</param>
        /// <param name="injections">Свойства, необходимые для расчетов параметров</param>
        private void CalculateOrder(Stage3Order stage3, IEnumerable<string> keys, object injections)
        {
            foreach (var key in keys)
            {
                var eval = ResolveEvaluator(key, injections);

                if (eval != null)
                {
                    if (!stage3.OrderDict.ContainsKey(key))
                    {
                        stage3.OrderDict.Add(key, eval.GetValue(null));
                    }
                }
            }
        }

        public IDataResult ListDetails(BaseParams baseParams)
        {
            var stage3Id = baseParams.Params.GetAs<long>("st3Id");

            var data = Stage1Service.GetAll()
                .Where(x => x.Stage2.Stage3.Id == stage3Id)
                .Select(x => new
                {
                    x.Id,
                    Stage2Year = x.Stage2.Year,
                    Stage2Sum = x.Stage2.Sum,
                    Stage2CeoName = x.Stage2.CommonEstateObject.Name,
                    x.Sum,
                    x.ServiceCost,
                    StructElementName = x.StructuralElement.StructuralElement.Name,
                    UnitMeasureName = x.StructuralElement.StructuralElement.UnitMeasure.Name,
                    x.StructuralElement.Volume,
                    x.Year
                })
                .ToArray();

            var result = data
                .GroupBy(x => new {Year = x.Stage2Year, Sum = x.Stage2Sum, Name = x.Stage2CeoName})
                .Select(x =>
                    new
                    {
                        x.Key.Year,
                        ServiceAndWorkSum = x.Key.Sum,
                        x.Key.Name,
                        leaf = false,
                        WorkSum = x.Sum(y => y.Sum),
                        ServiceSum = x.Sum(y => y.ServiceCost),
                        Children = x.Select(y =>
                            new
                            {
                                y.Id,
                                Name = y.StructElementName,
                                WorkSum = y.Sum,
                                ServiceSum = y.ServiceCost,
                                Measure = y.UnitMeasureName,
                                y.Volume,
                                y.Year,
                                leaf = true,
                                ServiceAndWorkSum = y.Sum + y.ServiceCost
                            })
                            .ToList()
                    })
                .ToList();

            return new BaseDataResult(new { Children = result });
        }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("st3Id");

            var stage3 = this.Stage3Service.Get(id);

            if (stage3 == null)
            {
                throw new ArgumentNullException(@"Не найдена информация по 3 этапу ДПКР");
            }

            var stage1Data = this.Stage1Service.GetAll()
                .Where(x => x.Stage2.Stage3.Id == id 
                    && this.Stage2Service.GetAll().Any(y => y.Id == x.Stage2.Id))
                .Select(x => new
                {
                    x.ServiceCost,
                    x.Sum
                });

            var workSum = stage1Data.Any() ? stage1Data.Sum(x => x.Sum) : 0;
            var serviceSum = stage1Data.Any() ? stage1Data.Sum(x => x.ServiceCost) : 0;

            var proxy = new Stage3Proxy
            {
                Address = stage3.RealityObject.Address,
                IndexNumber = stage3.IndexNumber,
                Point = stage3.Point,
                ServiceAndWorkSum = workSum + serviceSum,
                Year = stage3.Year,
                WorkSum = workSum,
                ServiceSum = serviceSum
            };

            return new BaseDataResult(proxy);
        }

        public IDataResult ListWorkTypes(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("st3Id");

            var stage3 = this.Stage3Service.Get(id);

            if (stage3 == null)
            {
                throw new ArgumentNullException(@"Не найдена информация по 3 этапу ДПКР");
            }

            var stage1StrElIds = this.Stage1Service.GetAll()
                .Where(x => this.Stage2Service.GetAll()
                    .Any(y => y.Id == x.Stage2.Id && y.Stage3.Id == id));

            var strElWorkDomain = Container.Resolve<IDomainService<StructuralElementWork>>();
            var workPriceDomain = Container.Resolve<IDomainService<WorkPrice>>();
            var roStrelWorkDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            var roStrEls = roStrelWorkDomain.GetAll()
                .Where(x =>
                           stage1StrElIds.Any(y => y.StructuralElement.StructuralElement.Id == x.StructuralElement.Id)
                           && x.RealityObject.Id == stage3.RealityObject.Id)
                .ToList();

            /* Получили связки КЭ - работа для КЭ из первого этапа */
            var strElWorks = strElWorkDomain.GetAll()
                .Where(x => stage1StrElIds.Any(y => y.StructuralElement.StructuralElement.Id == x.StructuralElement.Id))
                .GroupBy(x => x.StructuralElement)
                .ToDictionary(x => x.Key, y => y.ToList());

            var workPrices = workPriceDomain.GetAll().Select(x => new { x.Job.Id, x.NormativeCost }).ToList();

            var result = new List<WorkTypeProxy>();
            foreach (var strElWork in strElWorks.Keys)
            {
                var roStrEl = roStrEls.FirstOrDefault(x => x.StructuralElement.Id == strElWork.Id);
                foreach (var elWork in strElWorks[strElWork])
                {
                    result.Add(new WorkTypeProxy
                    {
                        StructElement = strElWork.Name,
                        WorkKind = elWork.Job.Work.Name,
                        WorkType = elWork.Job.Work.TypeWork.ToStr(),
                        Volume = roStrEl.Return(x => x.Volume),
                        Sum = roStrEl.Return(x => x.Volume) * workPrices.FirstOrDefault(x => x.Id == elWork.Job.Id).Return(x => x.NormativeCost)
                    });
                }
            }

            return new ListDataResult(result, result.Count());
        }

        public IDataResult MakeNewVersion(BaseParams baseParams)
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            var muId = baseParams.Params.GetAs<long>("Municipality");
            var date = baseParams.Params.GetAs<DateTime>("Date");
            var name = baseParams.Params.GetAs<string>("Name");
            var isMain = baseParams.Params.GetAs<bool>("IsMain");

            if (muId == 0)
            {
                return new BaseDataResult(false, "Не задан параметр \"Муниципальное образование\"");
            }

            // создаем версию 
            var municipality = MunicipalityDomain.Get(muId);
            if (municipality == null)
            {
                return new BaseDataResult(false, "Некорректно задан параметр \"Муниципальное образование\"");
            }

            if (isMain && ProgramVersionDomain.GetAll().Any(x => x.Municipality.Id == muId && x.IsMain && x.State.FinalState))
            {
                return new BaseDataResult(false, "Существует основная утвержденная версия программы. Создание новой основной версии не доступно.");
            }

            var paramsService = Container.Resolve<IDomainService<CurrentPrioirityParams>>();

            var stage3Records =
                this.Stage3Service.GetAll()
                    .Where(x => x.RealityObject.Municipality.Id == muId)
                    .Select(x => new
                    {
                        x.Id,
                        RealityObjectId = x.RealityObject.Id,
                        x.Year,
                        x.CommonEstateObjects,
                        x.Sum,
                        x.IndexNumber,
                        x.Point,
                        x.StoredCriteria
                    })
                    .ToList();

            var stage2Records = this.Stage2Service.GetAll()
                .Where(x => x.RealityObject.Municipality.Id == muId)
                .Select(x => new
                {
                    x.Id,
                    CommonEstateObjectId = x.CommonEstateObject.Id,
                    x.CommonEstateObject.Weight,
                    x.Sum,
                    Stage3Id = x.Stage3.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => y.First());

            var stage1Records = this.Stage1Service.GetAll()
                .Where(x => x.Stage2.RealityObject.Municipality.Id == muId)
                .Where(x => x.Stage2 != null)
                .Select(x => new
                {
                    x.Id,
                    Stage2Id = x.Stage2.Id,
                    RealityObjectId = x.StructuralElement.RealityObject.Id,
                    x.Year,
                    RoStructuralElementId = x.StructuralElement.Id,
                    StrElId = x.StructuralElement.StructuralElement.Id,
                    x.ServiceCost,
                    x.Sum,
                    x.StructuralElement.Volume
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage2Id)
                .ToDictionary(x => x.Key, y => y.ToList());

            var stage3Params = paramsService.GetAll()
                .Select(x => new {x.Code, x.Order})
                .ToList();

            var version = new ProgramVersion { Name = name, VersionDate = date, IsMain = isMain, Municipality = municipality };

            // Массивы на сохранение
            var ver3L = new List<VersionRecord>();
            var ver2L = new List<VersionRecordStage2>();
            var ver1L = new List<VersionRecordStage1>();
            var verParams = new List<VersionParam>();

            foreach (var stage3 in stage3Params)
            {
                verParams.Add(
                    new VersionParam
                    {
                        ProgramVersion = version,
                        Code = stage3.Code,
                        Weight = stage3.Order
                    });
            }

            // Parallel тратит ресурсы на синхронизацию объектов
            // Ктому же ненужные условия Where когда можно просто сделать Dictionary
            // Итого 10 секунд на подготовку данных и 2 мин. на сохранение транзакции из 110 тыщ записей 1 этапа, 110 тыщ 2 этапа, 110 тыщ 3 этапа 

            // Это ник чему
            //Parallel.ForEach(stage3Records,
            // stage3 => 
            foreach (var stage3 in stage3Records)
            {
                var ver3 = new VersionRecord
                {
                    ProgramVersion = version,
                    RealityObject = new RealityObject { Id = stage3.RealityObjectId },
                    Year = stage3.Year,

                    CorrectYear = stage3.Year,
                    CommonEstateObjects = stage3.CommonEstateObjects,
                    Sum = stage3.Sum,
                    IndexNumber = stage3.IndexNumber,
                    Point = stage3.Point,
                    StoredCriteria = stage3.StoredCriteria,
                    TypeDpkrRecord = TypeDpkrRecord.CalcRecord // поумолчанию ставим что поле Тип= Расчитанное 
                };

                ver3L.Add(ver3);

                if (stage2Records.ContainsKey(stage3.Id))
                {
                    var st2Rec = stage2Records[stage3.Id];

                    var st2Version = new VersionRecordStage2
                    {
                        CommonEstateObject = new CommonEstateObject { Id = st2Rec.CommonEstateObjectId },
                        Stage3Version = ver3,
                        CommonEstateObjectWeight = st2Rec.Weight,
                        Sum = st2Rec.Sum,
                        TypeDpkrRecord = TypeDpkrRecord.CalcRecord // поумолчанию ставим что поле Тип= Расчитанное
                    };

                    ver2L.Add(st2Version);

                    /*
                    Parallel.ForEach(stage1Records.Where(x => x.Stage2Id == st2Rec.Id),
                        st1 => //foreach (var st1 in stage1Records.Where(x => x.Stage2Id == st2Rec.Id))
                            ver1L.Add(new VersionRecordStage1
                            {
                                RealityObject = new RealityObject
                                {
                                    Id = st1.RealityObjectId
                                },
                                Stage2Version = st2Version,
                                Year = st1.Year,
                                StructuralElement = new RealityObjectStructuralElement
                                {
                                    Id = st1.StructuralElementId
                                }
                            }));
                    */

                    if (stage1Records.ContainsKey(st2Rec.Id))
                    {
                        var st1data = stage1Records[st2Rec.Id];
                        foreach (var st1 in st1data)
                        {
                            ver1L.Add(new VersionRecordStage1
                            {
                                RealityObject = new RealityObject { Id = st1.RealityObjectId },
                                Stage2Version = st2Version,
                                Year = st1.Year,
                                StructuralElement = new RealityObjectStructuralElement { Id = st1.RoStructuralElementId },
                                Sum = st1.Sum,
                                SumService = st1.ServiceCost,
                                Volume = st1.Volume,
                                StrElement = new StructuralElement {Id = st1.StrElId},
                                TypeDpkrRecord = TypeDpkrRecord.CalcRecord // поумолчанию ставим что поле Тип= Расчитанное
                            });
                        }
                    }
                }
            }

            var versionUpdates = new List<ProgramVersion>();
            if (isMain)
            {
                versionUpdates.AddRange(
                    ProgramVersionDomain.GetAll()
                        .Where(x => x.Municipality.Id == municipality.Id)
                        .Where(x => x.IsMain)
                        .ToList());
            }

            //проставляем начальный статус у версии
            Container.Resolve<IStateProvider>().SetDefaultState(version);

            // простые объекты + 3 этап
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // Обновляем версии которые были IsMain == true но стали IsMain = false
                    versionUpdates.ForEach(x =>
                    {
                        x.IsMain = false;
                        session.Update(x);
                    });

                    // Сохраняем новую версию
                    session.Insert(version);

                    // сохраняем параметры версии
                    verParams.ForEach(x => session.Insert(x));

                    // сохраняем 3й этап
                    ver3L.ForEach(x => session.Insert(x));

                    ver2L.ForEach(x => session.Insert(x));

                    ver1L.ForEach(x => session.Insert(x));

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

            Container.Resolve<ISessionProvider>().CloseCurrentSession();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return new BaseDataResult(true);
        }

        /// <summary>
        /// Обновляет сумму по работе в первом этапе.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Объект с результатом выполнения операции</returns>
        public IDataResult UpdateWorkSum(BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAs<long>("objectId");
            var modifiedRecords = baseParams.Params.GetAs<WorkSumProxy[]>("records");

            using (var tx = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var stage1Records = Stage1Service.GetAll()
                        .Where(x => x.Stage2.Stage3.Id == objectId)
                        .ToDictionary(x => x.Id);

                    foreach (var record in modifiedRecords)
                    {
                        var stage1Element = stage1Records[record.Id];

                        stage1Element.Sum = record.WorkSum;
                        stage1Element.ServiceCost = record.ServiceSum;

                        Stage1Service.Update(stage1Element);
                    }

                    var stage2Records = Stage2Service.GetAll().Where(x => x.Stage3.Id == objectId);

                    foreach (var record in stage2Records)
                    {
                        record.Sum =
                            stage1Records.Values
                                .Where(x => x.Stage2.Id == record.Id)
                                .Sum(x => x.Sum + x.ServiceCost);

                        Stage2Service.Update(record);
                    }

                    var stage3Record = Stage3Service.Get(objectId);

                    stage3Record.Sum = stage2Records.Sum(x => x.Sum);

                    Stage3Service.Update(stage3Record);

                    tx.Commit();
                }
                catch (ValidationException e)
                {
                    tx.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Копировать суммы из сохраненной версии
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Объект с результатом выполнения операции</returns>
        public IDataResult CopyFromVersion(BaseParams baseParams)
        {
            //перенесено в Stage2Service
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод, для выполнения действий в транзации 
        /// </summary>
        /// <param name="action">Действие</param>
        protected virtual void InTransaction(Action action)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
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

        /// <summary>Открыть транзакцию</summary>
        /// <returns>Экземпляр IDataTransaction</returns>
        protected virtual IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        /// <summary>
        /// Получить вычислитель свойства программы
        /// </summary>
        /// <param name="paramCode">Код вычислителя</param>
        /// <param name="stage3">Анонимный объект для Property injection</param>
        /// <returns>Вычислитель</returns>
        private IProgrammPriorityParam ResolveEvaluator(string paramCode, object stage3)
        {
            return this.Container.Resolve<IProgrammPriorityParam>(paramCode, new Arguments
            {
                {"stage3", stage3}
            });
        }
    }

    public class WorkSumProxy
    {
        public int Id { get; set; }

        public decimal WorkSum { get; set; }

        public decimal ServiceSum { get; set; }
    }

    public class Stage3Proxy
    {
        public string Address;

        public string Name;

        public int Year;

        public decimal WorkSum;

        public decimal ServiceSum;

        public decimal ServiceAndWorkSum;

        public string Measure;

        public decimal Volume;

        public decimal Point;

        public int IndexNumber;

        public List<Stage3Proxy> Children;

        public bool leaf;
    }

    public class WorkTypeProxy
    {
        public string StructElement;

        public string WorkType;

        public string WorkKind;

        public decimal Volume;

        public decimal Sum;
    }

    internal class Stage3Order
    {
        public int Id;

        public RealityObject RealityObject;

        public int Year;

        public RealityObjectStructuralElementInProgrammStage3 Stage3;

        public Dictionary<string, decimal> OrderDict = new Dictionary<string, decimal>();

        public int CeoId;

        public string RoCeoKey;
    }

    internal static class AccessorCache
    {
        private static readonly Hashtable accessors = new Hashtable();

        private static readonly Hashtable callSites = new Hashtable();

        /// <summary>
        /// Создание "точки вызова" метода получения свойства динамического объекта
        /// </summary>
        /// <param name="name">Наименование свойства</param>
        private static CallSite<Func<CallSite, object, object>> GetCallSiteLocked(string name)
        {
            var callSite = (CallSite<Func<CallSite, object, object>>)callSites[name];
            if (callSite == null)
            {
                callSites[name] = callSite = CallSite<Func<CallSite, object, object>>.Create(
                            Binder.GetMember(CSharpBinderFlags.None, name, typeof(AccessorCache),
                            new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));
            }
            return callSite;
        }

        /// <summary>
        /// Получение делегата обращения к свойству динамического объекта
        /// </summary>
        /// <param name="name">Наименование свойства</param>
        internal static Func<dynamic, object> GetAccessor(string name)
        {
            Func<dynamic, object> accessor = (Func<dynamic, object>)accessors[name];
            if (accessor == null)
            {
                lock (accessors)
                {
                    accessor = (Func<dynamic, object>)accessors[name];
                    if (accessor == null)
                    {
                        if (name.IndexOf('.') >= 0)
                        {
                            string[] props = name.Split('.');
                            CallSite<Func<CallSite, object, object>>[] arr = Array.ConvertAll(props, GetCallSiteLocked);
                            accessor = target =>
                            {
                                object val = (object)target;
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    var cs = arr[i];
                                    val = cs.Target(cs, val);
                                }
                                return val;
                            };
                        }
                        else
                        {
                            var callSite = GetCallSiteLocked(name);
                            accessor = target =>
                            {
                                return callSite.Target(callSite, (object)target);
                            };
                        }
                        accessors[name] = accessor;
                    }
                }
            }
            return accessor;
        }
    }
}