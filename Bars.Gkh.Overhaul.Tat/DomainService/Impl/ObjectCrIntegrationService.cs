namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Domain.ProxyEntity;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.B4.DomainService.BaseParams;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class ObjectCrIntegrationService : IObjectCrIntegrationService
    {

        public IWindsorContainer Container { get; set; }

        public IRepository<TypeWorkCr> TypeWorkCrRepo { get; set; } //тут нужен именно IRepository потому, что нужны именно все работы, включая и удаленные у которых IsActive = false

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<ProgramVersion> progVersionDomain { get; set; }

        public IDomainService<TypeWorkCrVersionStage1> referencesDomain { get; set; }

        public IDomainService<StructuralElementWork> StrElWorksDomain { get; set; }

        public IDomainService<FinanceSourceWork> finSourceDomainService { get; set; }

        /// <summary>
        /// В объекте КР получаем список возможных на выбор объектов 
        /// </summary>
        public virtual IDataResult GetListWorksForObjectCr(BaseParams baseParams, ref int totalCount)
        {
            var objectCrId = baseParams.Params.GetAs("objectCrId", 0L);
            var financeSourceId = baseParams.Params.GetAs("financeSourceId", 0L);

            var objectCr = this.ObjectCrDomain.Load(objectCrId);

            if (objectCr == null)
            {
                return new BaseDataResult(false, "Не найден объект КР.");
            }

            // получаем основную и утвержденную версию по данному МО
            var currentVersion = this.progVersionDomain.GetAll().FirstOrDefault(x => x.IsMain && x.Municipality.Id == objectCr.RealityObject.Municipality.Id && x.State.FinalState);

            if (currentVersion == null)
            {
                return new BaseDataResult(false, string.Format("Для МО '{0}' не найдена основная и утвержденная версия ДПКР.", objectCr.RealityObject.Municipality.Name));
            }

            // Запрос получения существующих в Объекте кр работ
            var currentWorkQuery = this.TypeWorkCrRepo.GetAll()
                              .Where(y => y.ObjectCr.Id == objectCrId)
                              .Where(y => y.Work.TypeWork == TypeWork.Work);

            // фильтруем типы работ по источникам финансирования
            var availableWorkTypes = this.finSourceDomainService.GetAll()
                    .Where(y => y.Work.TypeWork == TypeWork.Work)
                    .Where(x => x.FinanceSource.Id == financeSourceId).Select(x => x.Work.Id);

            // Запрос получения текущих ссылок на 1 этап ДПКР 
            var curentRererences = this.referencesDomain.GetAll()
                .Where(y => y.TypeWorkCr.ObjectCr.RealityObject.Id == objectCr.RealityObject.Id)
                .Where(y => !(y.TypeWorkCr.ObjectCr.ProgramCr.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Hidden
                        && y.TypeWorkCr.ObjectCr.ProgramCr.TypeProgramStateCr == TypeProgramStateCr.Close))
                .Where(y => y.TypeWorkCr.IsActive);

            // получаем те работы, которые еще не добавлены в объект КР
            var jobsQuery = this.StrElWorksDomain.GetAll()
                                .Where(x => x.Job.Work.TypeWork == TypeWork.Work)
                                .Where(x => !currentWorkQuery.Any(y => y.Work.Id == x.Job.Work.Id)
                                && (financeSourceId == 0 || availableWorkTypes.Contains(x.Job.Work.Id))); 
            
            // получаем Query для 1 этапа
            var dataQuery = this.VersionStage1Domain.GetAll()
                                   .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == currentVersion.Id)
                                   .Where(x => x.RealityObject.Id == objectCr.RealityObject.Id)
                                   .Where(x => jobsQuery.Any(y => y.StructuralElement.Id == x.StrElement.Id))
                                   .Where(x => !curentRererences.Any(y => y.Stage1Version.Id == x.Id));

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

            // Для итоговой выборки необходимо получить только первое вхождение КЭ по году в ДПКР
            // Поскольку каждый КЭ может встречаться много раз в ДПКР в разных годах
            // То сортируем по Году и берем только уникальные значения
            var firstStructEls =
                dataQuery.Select(
                    x =>
                    new
                        {
                            x.Id,
                            roId = x.RealityObject.Id,
                            stId = x.StrElement.Id,
                            x.Stage2Version.Stage3Version.CorrectYear,
                            x.Stage2Version.Stage3Version.IndexNumber
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.roId + "_" + x.stId)
                    .ToDictionary(
                        x => x.Key, y => y.OrderBy(x => x.CorrectYear).ThenBy(x => x.IndexNumber).FirstOrDefault());

            var firstIds = firstStructEls.Values.Select(x => x.Id).ToList();

            /*
             * В итоге должны получить те записи ДПКР, виды работ которых еще не добавили в Объект КР
               и при этом получит только ближайший плановый ремонт каждого КЭ и сгруппировать по {Год}
             */
            var stage1List =
                dataQuery.Where(x => firstIds.Contains(x.Id))
                         .Select(
                             x =>
                             new VersionRecordStage1Proxy
                             {
                                 Stage1Id = x.Id,
                                 CorrectionYear = x.Stage2Version.Stage3Version.CorrectYear,
                                 RealityObjectId = x.RealityObject.Id,
                                 StructElementName = x.StrElement.Name,
                                 StructElementId = x.StrElement.Id,
                                 CeoName = x.Stage2Version.CommonEstateObject.Name,
                                 Sum = x.Sum,
                                 Volume = x.Volume
                             })
                         .AsEnumerable()
                         .Select(
                            x => new
                            {
                                Id = jobsDict[x.StructElementId].WorkId + "_" + x.RealityObjectId + "_" + x.CorrectionYear,
                                x.Stage1Id,
                                jobsDict[x.StructElementId].WorkId,
                                jobsDict[x.StructElementId].WorkName,
                                x.CorrectionYear,
                                x.RealityObjectId,
                                x.CeoName,
                                x.StructElementName,
                                x.Sum,
                                x.Volume
                            }              
                          );

            var loadParam = this.GetLoadParam(baseParams);

            var resultList = stage1List.ToList().AsQueryable().Filter(loadParam, this.Container);

            totalCount = resultList.Count();

            return new BaseDataResult(resultList.Paging(loadParam).ToList());
        }
        
        protected virtual LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
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
                    return new BaseDataResult(false, string.Format("Не удалось получить источник  финансирования по Id {0}", finSrcId));
                }

                var stage1 = st1Service.FirstOrDefault(x => x.Id == stage1Id);

                if (stage1 == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить записи ДПКР с Id {0}", stage1Id));
                }

                var newType = new TypeWorkCr
                {
                    ObjectCr = objectCr,
                    Work = work,
                    IsActive = true,
                    IsDpkrCreated = false,
                    FinanceSource = finSrc,
                    YearRepair = objectCr.ProgramCr.Period.DateStart.Year,
                    Sum = stage1.Sum,
                    Volume = stage1.Volume
                };

                var typeWorkCrVerSt1 = new TypeWorkCrVersionStage1
                {
                    TypeWorkCr = newType,
                    Stage1Version = new VersionRecordStage1 { Id = stage1Id },
                    Sum = stage1.Sum,
                    Volume = stage1.Volume
                };


                using (var tr = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        typeWorkCrService.Save(newType);

                        var hist = historyService.HistoryAfterCreation(newType);
                        hist.YearRepair = year;

                        historyDomain.Update(hist);

                        typeWorkCrStage1Service.Save(typeWorkCrVerSt1);

                        tr.Commit();
                    }
                    catch (Exception exc)
                    {
                        tr.Rollback();
                        throw;
                    }
                }

                return new BaseDataResult(new {newType.Id} );
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
