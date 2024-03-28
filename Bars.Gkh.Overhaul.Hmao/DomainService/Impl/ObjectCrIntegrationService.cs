namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с Объект КР
    /// </summary>
    public class ObjectCrIntegrationService : IObjectCrIntegrationService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Репозиторий для Вид работы КР
        /// Тут нужен именно IRepository потому, что нужны именно все работы, включая и удаленные у которых IsActive = false
        /// </summary>
        public IRepository<TypeWorkCr> TypeWorkCrRepo { get; set; }

        /// <summary>
        /// Домен сервис для Версия Этапа 1
        /// </summary>
        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        /// <summary>
        /// Домен сервис для Сущность, содержащая данные, необходимые при учете корректировки ДПКР
        /// </summary>
        public IDomainService<DpkrCorrectionStage2> CorrectionDomain { get; set; }

        /// <summary>
        /// Домен сервис для Объект КР
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        /// <summary>
        /// Домен сервис для Версия программы
        /// </summary>
        public IDomainService<ProgramVersion> progVersionDomain { get; set; }

        /// <summary>
        /// Домен сервис для Связь записи первого этапа версии и видов работ
        /// </summary>
        public IDomainService<TypeWorkCrVersionStage1> referencesDomain { get; set; }

        /// <summary>
        /// Домен сервис для Работа по конструктивному элементу
        /// </summary>
        public IDomainService<StructuralElementWork> StrElWorksDomain { get; set; }

        /// <summary>
        /// Домен сервис для Работы источника финансирования по КР
        /// </summary>
        public IDomainService<FinanceSourceWork> finSourceDomainService { get; set; }

        /// <summary>
        /// В объекте КР получаем список возможных на выбор объектов 
        /// </summary>
        public virtual IDataResult GetListWorksForObjectCr(BaseParams baseParams, ref int totalCount)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var financeSourceId = baseParams.Params.GetAs("financeSourceId", 0L);

            var objectCr = this.ObjectCrDomain.Get(objectCrId);

            if (objectCr == null)
            {
                return new BaseDataResult(false, "Не найден объект КР.");
            }

            var moSettlementId = objectCr.RealityObject.MoSettlement.Return(x => x.Id);

            // получаем основную и версию по МО
            var currentVersion = this.progVersionDomain.GetAll()
                .Where(x => x.Municipality.Id == objectCr.RealityObject.Municipality.Id || x.Municipality.Id == moSettlementId)
                .FirstOrDefault(x => x.IsMain);

            if (currentVersion == null)
            {
                return new BaseDataResult(false, string.Format("Для МО '{0}' не найдена основная версия ДПКР.", objectCr.RealityObject.Municipality.Name));
            }


            // Запрос получения текущих ссылок на 1 этап ДПКР

            // Необходимо дать возможность выбора другого КЭ по ООИ, который ранее был удален,
            // т.е. берем только актуальные записи (которые не помечены как удаленные)
            //var curentRererences = this.referencesDomain.GetAll()
            //    .Where(y => y.TypeWorkCr.ObjectCr.RealityObject.Id == objectCr.RealityObject.Id)
            //    .Where(y => !(y.TypeWorkCr.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Hidden
            //            && y.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Close))
            //    .Where(y => y.TypeWorkCr.IsActive);

            // фильтруем типы работ по источникам финансирования
            var availableWorkTypes = this.finSourceDomainService.GetAll()
                .Where(y => y.Work.TypeWork == TypeWork.Work)
                .Where(x => x.FinanceSource.Id == financeSourceId).Select(x => x.Work.Id);

            // получаем те работы, которые еще не добавлены в объект КР
            var jobsQuery = this.StrElWorksDomain.GetAll()
                .Where(x => x.Job.Work.TypeWork == TypeWork.Work)
                .Where(x => (financeSourceId == 0 || availableWorkTypes.Contains(x.Job.Work.Id)));

            // получаем Query для 1 этапа (добавление работ теперь на уровне КЭ)
            var dataQuery = this.VersionStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == currentVersion.Id)
                .Where(x => x.RealityObject.Id == objectCr.RealityObject.Id)
                .Where(x => jobsQuery.Any(y => y.StructuralElement.Id == x.StructuralElement.StructuralElement.Id));
               // .Where(x => !curentRererences.Any(y => y.Stage1Version.StructuralElement.StructuralElement.Id == x.StructuralElement.StructuralElement.Id));


            var jobsDict = jobsQuery
                .Select(x => new
                {
                    x.Id,
                    WorkId = x.Job.Work.Id,
                    WorkName = x.Job.Work.Name,
                    strElId = x.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.strElId)
                .ToDictionary(x => x.Key, y => y.First());

            var correctionYearsDict = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == currentVersion.Id)
                .Where(x => x.Stage2.Stage3Version.RealityObject.Id == objectCr.RealityObject.Id)
                .Select(x => new {St2Id = x.Stage2.Id, CorrectionYear = x.PlanYear})
                .AsEnumerable()
                .GroupBy(x => x.St2Id)
                .ToDictionary(x => x.Key, y => y.Select(z => z.CorrectionYear).FirstOrDefault());

            // Для итоговой выборки необходимо получить только первое вхождение КЭ по году в ДПКР
            // Поскольку каждый КЭ может встречаться много раз в ДПКР в разных годах
            // То сортируем по Году и берем только уникальные значения
            var firstStructEls = dataQuery
                .Select(x => new
                {
                    x.Id,
                    roStrElId = x.StructuralElement.Id,
                    St2Id = x.Stage2Version.Id,
                    x.Stage2Version.Stage3Version.Year,
                    x.Stage2Version.Stage3Version.IndexNumber
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.roStrElId,
                    CorrectYear = correctionYearsDict.ContainsKey(x.St2Id) ? correctionYearsDict[x.St2Id] : x.Year,
                    x.IndexNumber
                })
                .AsEnumerable()
                .GroupBy(x => x.roStrElId) // группируем по КЭ Дома
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.CorrectYear).ThenBy(x => x.IndexNumber).FirstOrDefault());

            var firstIds = firstStructEls.Values.Select(x => x.Id).ToList();

            /*
             * В итоге должны получить те записи ДПКР, виды работ которых еще не добавили в Объект КР
               и при этом получит ьтолько ближайший плановый ремонт каждого КЭ и сгруппировать по {Год}
             */
            var stage1 = dataQuery
                .Where(x => firstIds.Contains(x.Id))
                .Select(x => new
                {
                    Stage1Id = x.Id,
                    St2Id = x.Stage2Version.Id,
                    x.Year,
                    RealityObjectId = x.RealityObject.Id,
                    StructElementName = x.StructuralElement.StructuralElement.Name,
                    StructElementId = x.StructuralElement.StructuralElement.Id,
                    CeoName = x.Stage2Version.CommonEstateObject.Name,
                    x.Sum,
                    x.Volume
                })
                .AsEnumerable()
                .Select(x => new
                {
                    Id = jobsDict[x.StructElementId].WorkId + "_" + x.RealityObjectId + "_" + (correctionYearsDict.Get(x.St2Id, (int?)null) ?? x.Year),
                    x.Stage1Id,
                    jobsDict[x.StructElementId].WorkId,
                    jobsDict[x.StructElementId].WorkName,
                    CorrectionYear = correctionYearsDict.Get(x.St2Id, (int?)null) ?? x.Year,
                    x.RealityObjectId,
                    x.StructElementName,
                    x.CeoName,
                    x.Sum,
                    x.Volume
                });

            var loadParam = baseParams.GetLoadParam();

            var resultList = stage1.ToList().AsQueryable().Filter(loadParam, this.Container);

            totalCount = resultList.Count();

            return new BaseDataResult(resultList.Paging(loadParam).ToList());
        }

        /// <summary>
        /// В объекте КР получаем список возможных на выбор объектов 
        /// </summary>
        public virtual IDataResult AddWorks(BaseParams baseParams)
        {
            var typeWorkCrService = this.Container.Resolve<IRepository<TypeWorkCr>>();
            var typeWorkCrStage1Service = this.Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();
            var workService = this.Container.Resolve<IDomainService<Work>>();
            var ocrService = this.Container.Resolve<IDomainService<ObjectCr>>();
            var st1Service = this.Container.Resolve<IDomainService<VersionRecordStage1>>();
            var finSrcService = this.Container.Resolve<IDomainService<FinanceSource>>();
            var historyService = this.Container.Resolve<ITypeWorkCrHistoryService>();
            var historyDomain = this.Container.Resolve<IDomainService<TypeWorkCrHistory>>();

            try
            {
                var workId = baseParams.Params.GetAs("workId", 0L);
                var objectCrId = baseParams.Params.GetAs("objectCrId", 0L);
                var stage1Id = baseParams.Params.GetAs("stage1Id", 0L);
                var finSrcId = baseParams.Params.GetAs("finSrcId", 0L);
                var year = baseParams.Params.GetAs("year", 0);
                var newYear = baseParams.Params.GetAs<int?>("newYear");

                var work = workService.Load(workId);

                if (work == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить вид работы по Id {0}", workId));
                }

                var objectCr = ocrService.Load(objectCrId);

                if (objectCr == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить объект КР по Id {0}", objectCrId));
                }

                var finSrc = finSrcService.Load(finSrcId);

                if (finSrc == null)
                {
                  //  return new BaseDataResult(false, string.Format("Не удалось получить источник  финансирвоания по Id {0}", finSrcId));
                }

                var dateStart = objectCr.ProgramCr.Period.DateStart;
                var dateEnd = objectCr.ProgramCr.Period.DateEnd;
                var periodName = objectCr.ProgramCr.Period.Name;
                var programName = objectCr.ProgramCr.Name;

                if (newYear.HasValue && (dateStart.Year > newYear || (dateEnd.HasValue && dateEnd.Value.Year < newYear)))
                {
                    return new BaseDataResult(false,
                        string.Format("Год ремонта не может выходить за период '{0}' краткосрочной программы '{1}'", periodName, programName));
                }

                var stage1 = st1Service.FirstOrDefault(x => x.Id == stage1Id);

                if (stage1 == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить записи ДПКР с Id {0}", stage1Id));
                }

                TypeWorkCr typeWork;

                // Если такой Вид работ уже создан, используем его
                // Иначе создаем новый

                var existTypeWork = typeWorkCrService.GetAll()
                    .Where(x => x.IsActive)
                    .Where(x => x.ObjectCr.Id == objectCr.Id)
                    .Where(x => x.YearRepair == objectCr.ProgramCr.Period.DateStart.Year)
                    .FirstOrDefault(x => x.Work == work);

                if (existTypeWork == null)
                {
                    typeWork = new TypeWorkCr
                    {
                        ObjectCr = objectCr,
                        Work = work,
                        IsActive = true,
                        FinanceSource = finSrc,
                        YearRepair = objectCr.ProgramCr.Period.DateStart.Year
                    };
                }
                else
                {
                    typeWork = existTypeWork;
                }

                var verStage1 = new TypeWorkCrVersionStage1
                {
                    TypeWorkCr = typeWork,
                    Stage1Version = new VersionRecordStage1 {Id = stage1Id },
                    Sum = stage1.Sum,
                    Volume = stage1.Volume,
                    UnitMeasure = new UnitMeasure
                    {
                        Id = stage1.StructuralElement.StructuralElement.UnitMeasure.Id 
                    
                    }
                };

                typeWork.Sum += verStage1.Sum;
                typeWork.Volume += verStage1.Volume;

                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        if (typeWork.Id > 0)
                        {
                            typeWorkCrService.Update(typeWork);
                        }
                        else
                        {
                            typeWorkCrService.Save(typeWork);
                        }

                        var hist = historyService.HistoryAfterCreation(typeWork, newYear);

                        if (hist != null)
                        {
                            hist.YearRepair = year;

                            historyDomain.Update(hist);
                        }
                        
                        typeWorkCrStage1Service.Save(verStage1);

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult(new { typeWork.Id});
            }
            finally
            {
                this.Container.Release(historyDomain);
                this.Container.Release(typeWorkCrService);
                this.Container.Release(typeWorkCrStage1Service);
                this.Container.Release(workService);
                this.Container.Release(ocrService);
                this.Container.Release(st1Service);
                this.Container.Release(finSrcService);
                this.Container.Release(historyService);
            }
        }
    }
}
