namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.States;
    using B4.Utils;
    using Domain;
    using Entities.Dicts;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;
    using Utils;
    using GkhCr.DomainService;
    using GkhCr.Entities;
    using GkhCr.Enums;
    using GkhCr.Modules.ClaimWork.Entities;
    using GkhCr.Modules.ClaimWork.Enums;

    using Castle.Windsor;
    using Modules.ClaimWork.DomainService;

    public class BuilderViolatorService : IBuilderViolatorService
    {
        public IWindsorContainer Container { get; set; }

        public IRepository<BuilderViolator> builderViolatorDomain { get; set; }

        public IRepository<ViolClaimWork> violDomain { get; set; }
        
        public IRepository<BuilderViolatorViol> violBuilderViolatorDomain { get; set; }

        public IDataResult AddViolations(BaseParams baseParams)
        {
            var violatorId = baseParams.Params.GetAs("violatorId", 0L);
            var violIds = baseParams.Params.GetAs("violIds", new long[0]);

            var violator = builderViolatorDomain.GetAll().FirstOrDefault(x => x.Id == violatorId);

            if (violator == null)
            {
                return new BaseDataResult(false, "Не удалось определить договор нарушителя по Id " + violatorId.ToStr());
            }

            var listToSave = new List<BuilderViolatorViol>();

            var currentIds = violBuilderViolatorDomain.GetAll()
                .Where(x => x.BuilderViolator.Id == violatorId)
                .Select(x => x.Violation.Id)
                .Distinct()
                .ToList();

            foreach (var id in violIds)
            {
                if (currentIds.Contains(id))
                {
                    continue;
                }

                listToSave.Add(new BuilderViolatorViol
                {
                    Violation = new ViolClaimWork { Id = id },
                    BuilderViolator = violator
                });
            }

            if (listToSave.Any())
            {
                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        listToSave.ForEach(x => violBuilderViolatorDomain.Save(x));

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Метод расчета количества дней просрочки
        /// </summary>
        public void CalculationDays(BuilderViolator violator)
        {
            if (violator.StartingDate.HasValue)
            {
                violator.CountDaysDelay = (int)DateTime.Now.Subtract(violator.StartingDate.ToDateTime()).TotalDays;
            }
            else
            {
                violator.CountDaysDelay = 0;
            }
        }

        /// <summary>
        /// метод получат список в реестре Подрядчиков нарушивших условие договора
        /// </summary>
        public IList GetList(BaseParams baseParams, bool isPaging,  ref int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();
            
            var builClaimService = Container.Resolve<IDomainService<BuildContractClaimWork>>();

            try
            {
                var data = builderViolatorDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.BuildContract.ObjectCr.RealityObject.Municipality.Name,
                        Settlement = x.BuildContract.ObjectCr.RealityObject.MoSettlement.Name,
                        Builder = x.BuildContract.Builder.Contragent.Name,
                        x.BuildContract.Builder.Contragent.Inn,
                        x.BuildContract.DocumentNum,
                        x.BuildContract.DocumentDateFrom,
                        x.BuildContract.DateEndWork,
                        x.CreationType,
                        x.CountDaysDelay,
                        x.StartingDate,
                        IsClaimWorking = builClaimService.GetAll().Any(y => y.BuildContract.Id == x.BuildContract.Id && !y.State.FinalState)
                    })
                    .Filter(loadParams, Container);

                totalCount = data.Count();

                return data.Order(loadParams).Paging(loadParams).ToList();
            }
            finally
            {
                Container.Release(builClaimService);
            }
        }

        /// <summary>
        /// Метод очистки реестра подрядчиков нарушивших условие договора
        /// </summary>
        public IDataResult Clear(BaseParams baseParams)
        {

            var sessions = Container.Resolve<ISessionProvider>();

            using (Container.Using(sessions))
            {
                using (var stSession = sessions.OpenStatelessSession())
                {
                    using (var tr = stSession.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        stSession.CreateSQLQuery("delete from cr_builder_violator_viol where BUILDER_VIOLATOR_ID in (select id from cr_builder_violator where type_creation = 10)").ExecuteUpdate();
                        stSession.CreateSQLQuery("delete from cr_builder_violator where type_creation = 10").ExecuteUpdate();

                        try
                        {
                            tr.Commit();
                        }
                        catch
                        {
                            tr.Rollback();
                            throw;
                        }
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Метод формирования реестра подрядчиков нарушивших условие договора
        /// </summary>
        public IDataResult MakeNew(BaseParams baseParams)
        {
            try
            {
                // получаем текущие договора на данный момент считающиеся нарушителями
                var currentData = builderViolatorDomain.GetAll()
                    .AsEnumerable()
                    .GroupBy(x => x.BuildContract.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());
                
                // Теперь получаем договора по суммам расходящиеся (то есть договора нарушители)
                var violatorContracts = GetNotValidContracts();

                var listToSave = new List<BuilderViolator>();
                var listViolsToSave = new List<BuilderViolatorViol>();
                var listToRemove = new List<BuilderViolator>();
                var listViolsToRemove = new List<BuilderViolatorViol>();

                // поулчаем нарушение претензионки с кодом 1
                var defaultViol = violDomain.GetAll().FirstOrDefault(x => x.Code == "1");

                foreach (var contract in violatorContracts)
                {
                    if (currentData.ContainsKey(contract.Id))
                    {
                        // Недобавляем те договора, котоыре уже существуют как нарушители в реестре
                        foreach (var violator in currentData[contract.Id])
                        {
                            // ставим новую дату если она изменилась
                            if (violator.StartingDate != contract.DateEndWork)
                            {
                                violator.StartingDate = contract.DateEndWork;

                                listToSave.Add(violator);     
                            }
                        }

                        currentData.Remove(contract.Id);
                    }
                    else
                    {
                        var rec = new BuilderViolator
                        {
                            BuildContract = contract,
                            CreationType = BuildContractCreationType.Auto,
                            StartingDate = contract.DateEndWork
                        };

                        listToSave.Add(rec);

                        if (defaultViol != null)
                        {
                            listViolsToSave.Add(new BuilderViolatorViol
                            {
                                BuilderViolator = rec,
                                Violation = defaultViol
                            });
                        }    
                    }
                }

                // Если ы этом списке остались записи то Автоматические - удаляем, а ручные - изменяем и актуализируем чтобы потом пересчитать 
                foreach (var kvp in currentData)
                {
                    foreach (var value in kvp.Value)
                    {
                        if (value.CreationType == BuildContractCreationType.Auto)
                        {
                            listToRemove.Remove(value);
                        }
                        else
                        {
                            if (value.StartingDate != value.BuildContract.DateEndWork)
                            {
                                value.StartingDate = value.BuildContract.DateEndWork;

                                listToSave.Add(value);
                            }
                        }
                    }
                }

                var removeIds = listToRemove.Select(x => x.Id).ToList();

                if (removeIds.Any())
                {
                    listViolsToRemove =
                        violBuilderViolatorDomain.GetAll().Where(x => removeIds.Contains(x.BuilderViolator.Id)).ToList();
                }

                foreach (var item in listToSave)
                {
                    CalculationDays(item);
                }

                if (listToSave.Any())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var rem in listViolsToRemove)
                            {
                                violBuilderViolatorDomain.Delete(rem.Id);
                            }

                            foreach (var rem in listToRemove)
                            {
                                builderViolatorDomain.Delete(rem.Id);
                            }

                            foreach (var item in listToSave)
                            {
                                builderViolatorDomain.Save(item);
                            }

                            foreach (var item in listViolsToSave)
                            {
                                violBuilderViolatorDomain.Save(item);
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

                return new BaseDataResult(); 
            }
            finally
            {
                
            }
            
        }

        private List<BuildContract> GetNotValidContracts()
        {
            /*
             При нажатии на кнопку «Сформировать» в реестре должны добавиться записи, где 1) сумма по актам выполненных работ < суммы по договору и 2) текущая дата > даты окончания работ из договора подряда
                Суммы соотносятся следующим образом:
            Если в договоре подряда указаны виды работ в гриде «Виды работ».
                Сумма по актам выполненных работ хотя бы по одной работе меньше суммы по этой работе в договоре подряда.
            Если в договоре подряда не указаны виды работ.
            
            Соотносим договор подряда и виды работ по полю «Тип договора» и кодам работ:
            На СМР – 1, 12, 13, 16, 17, 18, 19, 2, 20-29, 3-6, 999
            На приборы – 7-11
            На лифты – 14, 15
            На энергообследование – 30
            */
            var codeByType = new CodesByTypeContract();

            var buildContractDomain = Container.Resolve<IDomainService<BuildContract>>();
            var actDomain = Container.Resolve<IDomainService<PerformedWorkAct>>();
            var buildContractTwDomain = Container.Resolve<IDomainService<BuildContractTypeWork>>();
            var workDomain = Container.Resolve<IDomainService<Work>>();
            var allContracts = new Dictionary<long, BuildContractProxy>();
            var allActs = new Dictionary<long, PerformedActProxy>();
            var TypeWorkContracts = new Dictionary<string, List<BuildContractProxy>>();
            var сontractsByCode = new Dictionary<string, List<BuildContractProxy>>();
            var ActsById = new Dictionary<string, List<PerformedActProxy>>();
            var ActsByCode = new Dictionary<string, List<PerformedActProxy>>();
            try
            {
               // Сначала обрабатываем договора у которых есть виды работ 
                // Получаем все контракты
                var dateNow = DateTime.Now;
                allContracts = buildContractDomain.GetAll()
                    .Where(x => x.Sum > 0 && x.ObjectCr != null)
                    .Where(x => x.DateEndWork.HasValue && x.DateEndWork.Value < dateNow ) // поулчаем только те контракты которые типа уже завершились
                    .Select(x => new BuildContractProxy
                    {
                        Id = x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        Sum = x.Sum.HasValue ? x.Sum.Value : 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                // Получаем все акты выполненных работ
                allActs = actDomain.GetAll()
                    .Where(x => x.Sum > 0)
                    .Where(x => x.ObjectCr != null && x.TypeWorkCr != null)
                    .Select(x => new PerformedActProxy
                    {
                        Id = x.Id,
                        ObjectCrId = x.ObjectCr.Id,
                        Sum = x.Sum.HasValue ? x.Sum.Value : 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());
                
                // Формируем список по объекту КР и виду работы все акты
                actDomain.GetAll()
                .Where(x => x.Sum > 0)
                .Where(x => x.ObjectCr != null && x.TypeWorkCr != null)
                .Select(x => new
                {
                    actId = x.Id,
                    workCode = x.TypeWorkCr.Work.Code,
                    actSum = x.Sum.HasValue ? x.Sum.Value : 0,
                    objectCrId = x.ObjectCr.Id,
                    typeWorkId = x.TypeWorkCr.Id,
                    x.DateFrom
                })
                .OrderBy(x => x.DateFrom)
                .ToList()
                .ForEach(x =>
                {
                    PerformedActProxy proxy = null;
                    if ( allActs.ContainsKey(x.actId) )
                    {
                        proxy = allActs[x.actId];
                    }

                    if (proxy == null)
                    {
                        return;
                    }

                    var key = x.objectCrId + "_" + x.typeWorkId;

                    if ( !ActsById.ContainsKey(key) )
                    {
                        ActsById.Add(key, new List<PerformedActProxy> { proxy });
                    }
                    else
                    {
                        ActsById[key].Add(proxy);
                    }

                    var keyByCode = x.objectCrId + "_" + x.workCode;

                    if (!ActsByCode.ContainsKey(keyByCode))
                    {
                        ActsByCode.Add(keyByCode, new List<PerformedActProxy> { proxy });
                    }
                    else
                    {
                        ActsByCode[keyByCode].Add(proxy);
                    }
                });

                // Идем через вида работы договора и поулчаем список всех сумм по контрактам в группировке объкета кр и вида работы
                buildContractTwDomain.GetAll()
                    .Where(x => x.BuildContract.Sum > 0 && x.BuildContract != null)
                    .Where(x => x.BuildContract.DateEndWork.HasValue && x.BuildContract.DateEndWork.Value < dateNow ) // поулчаем только те контракты которые типа уже завершились
                    .Select(x => new
                    {
                        contractId = x.BuildContract.Id,
                        contractSum = x.BuildContract.Sum,
                        objectCrId = x.BuildContract.ObjectCr.Id,
                        typeWorkId = x.TypeWork.Id,
                        x.BuildContract.DateStartWork
                    })
                    .OrderBy(x => x.DateStartWork)
                    .ToList()
                    .ForEach(x =>
                    {
                        var key = x.objectCrId + "_" + x.typeWorkId;

                        BuildContractProxy proxy = null;
                        if ( allContracts.ContainsKey(x.contractId) )
                        {
                            proxy = allContracts[x.contractId];
                        }

                        if (proxy == null)
                        {
                            return;
                        }

                        if ( !TypeWorkContracts.ContainsKey(key) )
                        {
                            TypeWorkContracts.Add(key, new List<BuildContractProxy> { proxy });
                        }
                        else
                        {
                            TypeWorkContracts[key].Add(proxy);
                        }
                    });

                //Поулчаем договора у которых нет видов работ и по Типу определям каким кодам он соответсвует
                buildContractDomain.GetAll()
                    .Where(x => !buildContractTwDomain.GetAll().Any(y => y.BuildContract.Id == x.Id))
                    .Where(x => x.Sum > 0 )
                    .Where(x => x.DateEndWork.HasValue && x.DateEndWork.Value < dateNow) // поулчаем только те контракты которые типа уже завершились
                    .Select(x => new
                    {
                        contractId = x.Id,
                        contractSum = x.Sum,
                        objectCrId = x.ObjectCr.Id,
                        type = x.TypeContractBuild,
                        x.DateStartWork
                    })
                    .OrderBy(x => x.DateStartWork)
                    .ToList()
                    .ForEach(x =>
                    {
                        BuildContractProxy proxy = null;
                        if (allContracts.ContainsKey(x.contractId))
                        {
                            proxy = allContracts[x.contractId];
                        }

                        if (proxy == null)
                        {
                            return;
                        }

                        var codes = codeByType.getCodesByType(x.type);

                        // для каждого из кодов по типу создаем связку
                        foreach ( var code in codes)
                        {
                            var key = x.objectCrId + "_" + code;

                            if (!сontractsByCode.ContainsKey(key))
                            {
                                сontractsByCode.Add(key, new List<BuildContractProxy> { proxy });
                            }
                            else
                            {
                                сontractsByCode[key].Add(proxy);
                            }
                        }
                        
                    });

                // Теперь списываем суммы по актам из договоров у которых есть виды работ
                foreach (var actKvp in ActsById)
                {
                    if (TypeWorkContracts.ContainsKey(actKvp.Key))
                    {
                        foreach (var act in actKvp.Value)
                        {
                            // если сумма уже исчерпа на то нечего значит делат ьпо этому акту
                            if (act.Sum <= 0)
                            {
                                continue;
                            }

                            // начинаем списывать из догвооров данную сумму 
                            foreach (var contract in TypeWorkContracts[actKvp.Key])
                            {
                                if (contract.Sum <= 0)
                                {
                                    continue;
                                }
                                
                                if ( contract.Sum >= act.Sum )
                                {
                                    contract.Sum -= act.Sum;
                                    act.Sum = 0;
                                }
                                else
                                {
                                    contract.Sum -= act.Sum;
                                    act.Sum -= contract.Sum;
                                }
                            }    
                        }
                    }
                }

                // Теперь списываем суммы по актам из договоров у которых нет видов работ но по Кодам связанных через Тип
                foreach (var actKvp in ActsByCode)
                {
                    if (сontractsByCode.ContainsKey(actKvp.Key))
                    {
                        foreach (var act in actKvp.Value)
                        {
                            // если сумма уже исчерпа на то нечего значит делать по этому акту
                            if (act.Sum <= 0)
                            {
                                continue;
                            }

                            // начинаем списывать из догвооров данную сумму 
                            foreach (var contract in сontractsByCode[actKvp.Key])
                            {
                                if (contract.Sum <= 0)
                                {
                                    continue;
                                }

                                if (contract.Sum >= act.Sum)
                                {
                                    contract.Sum -= act.Sum;
                                    act.Sum = 0;
                                }
                                else
                                {
                                    contract.Sum -= act.Sum;
                                    act.Sum -= contract.Sum;
                                }
                            }
                        }
                    }
                }

                // получаем только те договора, которые остались с положительной суммой
                var notValidIds = allContracts.Values
                    .Where(x => x.Sum > 0) 
                    .Select(x => x.Id)
                    .ToList();

                return buildContractDomain.GetAll().Where(x => notValidIds.Contains(x.Id)).ToList();
            }
            finally
            {
                Container.Release(buildContractDomain);
                Container.Release(actDomain);
                Container.Release(buildContractTwDomain);
                Container.Release(workDomain);
            }
            
        }

        /// <summary>
        /// Метод проверки на возможность создания претензеонных работ 
        /// </summary>
        public IDataResult ValidateToCreateClaimWorks(BaseParams baseParams)
        {
            var bvDomain = Container.ResolveDomain<BuilderViolator>();
            var bcDomain = Container.ResolveDomain<BuildContractClaimWork>();
            var stateProvider = Container.Resolve<IStateProvider>();
            var stateDomain = Container.ResolveDomain<State>();

            try
            {
                var entityInfo = stateProvider.GetStatefulEntityInfo(typeof(BuildContractClaimWork));
                var defaultState =
                    stateDomain.GetAll().FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.StartState);

                if (defaultState == null)
                {
                    return new BaseDataResult(false, "Отсутсвует начальный статус для реестра подрячиков, нарушивших условие договора");
                }

                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

                // поулчаем те договора котоыре выбраны галочками
                var contracts =
                    bvDomain.GetAll().WhereIf(ids.Length > 0, x => ids.Contains(x.Id)).Select(x => x.BuildContract.Id).ToList();

                // првоеряем, по выбранным договорам ведется ли претензеонная работа
                var hasClaimWork =
                    bcDomain.GetAll().Where(x => contracts.Contains(x.BuildContract.Id) && !x.State.FinalState)
                    .Select(x => new { x.BuildContract.DocumentNum, x.BuildContract.DocumentDateFrom }).ToList();

                if (hasClaimWork.Count > 0)
                {
                    return new BaseDataResult(true, " По следующим договорам подряда {0} ведется претензионная работа. Обновить данные по претензеонной работе?".FormatUsing(
                                                    hasClaimWork.Select(x => string.Format("№{0}{1}", x.DocumentNum, x.DocumentDateFrom.HasValue ? " от "+x.DocumentDateFrom.Value.ToShortDateString() : string.Empty)).AggregateWithSeparator(", ") ));
                }
            }
            finally
            {
                Container.Release(bvDomain);
                Container.Release(bcDomain);
                Container.Release(stateProvider);
                Container.Release(stateDomain);
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// метод формирвоания претензионных работ по выбранным записям в реестре Подрядчиков нарушителей
        /// Либо создаются новые либо заменяюстя старые записи 
        /// </summary>
        public IDataResult CreateClaimWorks(BaseParams baseParams)
        {
            var bvDomain = Container.Resolve<IRepository<BuilderViolator>>();
            var bvViolDomain = Container.Resolve<IRepository<BuilderViolatorViol>>();
            var bcDomain = Container.Resolve<IRepository<BuildContractClaimWork>>();
            var bcViolDomain = Container.Resolve<IRepository<BuildContractClwViol>>();
            var lawsuitInfoService = Container.Resolve<ILawsuitInfoService>();

            var stateProvider = Container.Resolve<IStateProvider>();
            var stateDomain = Container.ResolveDomain<State>();

            try
            {
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();
                
                var entityInfo = stateProvider.GetStatefulEntityInfo(typeof(BuildContractClaimWork));
                var defaultState =
                    stateDomain.GetAll().FirstOrDefault(x => x.TypeId == entityInfo.TypeId && x.StartState);

                if (defaultState == null)
                {
                    return new BaseDataResult(false, "Отсутсвует начальный статус для реестра подрячиков, нарушивших условие договора");
                }

                var filterQuery = bvDomain.GetAll()
                    .WhereIf(ids.Length > 0, x => ids.Contains(x.Id));

                var claimWorkHasReviewDate = lawsuitInfoService.GetClaimWorkQueryHasReviewDate();

                var claimWorkFilters = bcDomain.GetAll()
                    .Where(x => !x.State.FinalState && !x.IsDebtPaid)
                    .Where(x => !claimWorkHasReviewDate.Any(y => y.Id == x.Id));

                var existClaimWorks = claimWorkFilters
                    .GroupBy(x => x.BuildContract.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var existClaimWorkViols = bcViolDomain.GetAll()
                    .Where(x => claimWorkFilters.Any(y => y.Id == x.ClaimWork.Id))
                    .Select(x => x.Id )
                    .ToList();

                var violations = bvViolDomain.GetAll()
                    .Where(x => filterQuery.Any(y => y.Id == x.BuilderViolator.Id))
                    .GroupBy(x => x.BuilderViolator.Id)
                    .ToDictionary(x => x.Key, y => y.ToList());

                var claimWorksViolToSave = new List<BuildContractClwViol>();
                var claimWorksToSave = new List<BuildContractClaimWork>();

                filterQuery
                .Select(x => new
                {
                    x.Id,
                    x.CountDaysDelay,
                    x.CreationType,
                    x.StartingDate,
                    buildContract = x.BuildContract.Id,
                    x.BuildContract.DocumentNum,
                    x.BuildContract.DocumentDateFrom,
                    x.BuildContract.ObjectCr.RealityObject
                })
                .ToList()
                .ForEach(x =>
                {
                    var claimWork = existClaimWorks.Get(x.buildContract);

                    BuildContractClaimWork rec;

                    if (claimWork == null)
                    {
                        rec = new BuildContractClaimWork
                        {
                            ClaimWorkTypeBase = ClaimWorkTypeBase.BuildContract,
                            BuildContract = new BuildContract {Id = x.buildContract},
                            BaseInfo = string.Format("Договор № {0}{1}",
                                x.DocumentNum,
                                x.DocumentDateFrom.HasValue
                                    ? string.Format(" от {0}", x.DocumentDateFrom.ToDateString())
                                    : string.Empty),
                            RealityObject = x.RealityObject,
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            ObjectVersion = 0,
                            CountDaysDelay = x.CountDaysDelay,
                            StartingDate = x.StartingDate
                        };
                    }
                    else
                    {
                        rec = claimWork;
                    }

                    rec.CreationType = x.CreationType;
                    rec.State = defaultState;

                    claimWorksToSave.Add(rec);

                    if (violations.ContainsKey(x.Id))
                    {
                        foreach (var viol in violations[x.Id])
                        {
                            claimWorksViolToSave.Add(new BuildContractClwViol
                            {
                                ClaimWork = rec,
                                Note = viol.Note,
                                Violation = viol.Violation
                            });
                        }
                    }
                        
                });

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        foreach (var item in existClaimWorkViols)
                        {
                            bcViolDomain.Delete(item);
                        }

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                TransactionHelper.InsertInManyTransactions(Container, claimWorksToSave, claimWorksToSave.Count, true, true);

                TransactionHelper.InsertInManyTransactions(Container, claimWorksViolToSave, claimWorksViolToSave.Count, true, true);

                return new BaseDataResult();
            }
            finally
            {
                Container.Release(bvDomain);
                Container.Release(bvViolDomain);
                Container.Release(bcDomain);
                Container.Release(bcViolDomain);
                Container.Release(stateProvider);
                Container.Release(stateDomain);
                Container.Release(lawsuitInfoService);
            }
        }

        private class CodesByTypeContract
        {
            private string[] smr;

            private string[] device;

            private string[] lift;

            private string[] energySurvey;

            public CodesByTypeContract()
            {
                smr = new[] { "1", "12", "13", "16", "17", "18", "19", "2", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "3", "4", "5", "6", "999" };
                device = new[] { "7", "8", "9", "10", "11" };
                lift = new[] { "14", "15" };
                energySurvey = new[] { "30" };
            }

            public string[] getCodesByType(TypeContractBuild type)
            {
                var result = new string[0];
                switch (type)
                {
                    case TypeContractBuild.Smr: { result = smr; } break;
                    case TypeContractBuild.Device: { result = device; } break;
                    case TypeContractBuild.Lift: { result = lift; } break;
                    case TypeContractBuild.EnergySurvey: { result = energySurvey; } break;
                }

                return result;
            }
        }
        private class BuildContractProxy
        {
            public virtual long Id { get; set; }

            public virtual long ObjectCrId { get; set; }

            public virtual decimal Sum { get; set; }
        }

        private class PerformedActProxy
        {
            public virtual long Id { get; set; }

            public virtual long ObjectCrId { get; set; }

            public virtual decimal Sum { get; set; }
        }
    }
}