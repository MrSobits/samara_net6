using Bars.Gkh.Authentification;
using Bars.GkhCr.Enums;

namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;
    using Enums;
    using Gkh.Entities.Dicts;
    using NHibernate;
    using Overhaul.Entities;

    /// <summary>
    /// Домен сервис для ДПКР НСО
    /// </summary>
    public class NsoDpkrService : IDpkrService
    {

        public IWindsorContainer Container { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }
        
        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> CorrectionDomain { get; set; }

        public IDomainService<ProgramVersion> ProgVersionDomain { get; set; }

        public IDomainService<VersionRecordStage2> Stage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<StructuralElementWork> StrElWorksDomain { get; set; }

        public IDomainService<FinanceSource> FinSourceDomain { get; set; }

        public IDomainService<MonitoringSmr> MonitoringSmrDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }
        
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ProgramCrChangeJournal> ProgChangeJournalDomain { get; set; }
        
        /// <summary>
        /// Получение субсидии по МО 
        /// </summary>
        public IDataResult CreateProgramCrByDpkr(BaseParams baseParams)
        {
            var programCrId = baseParams.Params.GetAs<long>("programCrId");

            if (programCrId < 1)
            {
                return new BaseDataResult(false, "Передан неверный идентификатор Программы КР");
            }

            var version = ProgVersionDomain.GetAll().FirstOrDefault(x => x.IsMain);

            if (version == null)
            {
                return new BaseDataResult(false, "Не задана основная версия");
            }

            if (ObjectCrDomain.GetAll().Any(x => x.ProgramCr.Id == programCrId))
            {
                return new BaseDataResult(false, "По данной программе уже имеются объекты КР");
            }

            var programCr = ProgramCrDomain.Get(programCrId);

            if (programCr.Period == null)
            {
                return new BaseDataResult(false, "Не указан период программы");
            }

            if (!programCr.Period.DateEnd.HasValue)
            {
                return new BaseDataResult(false, "Не задана дата окончания программы");
            }

            var yearStart = programCr.Period.DateStart.Year;
            var yearEnd = programCr.Period.DateEnd.Value.Year;

            var queryCorrection = CorrectionDomain.GetAll().Where(x => x.PlanYear >= yearStart && x.PlanYear <= yearEnd);

            if (!queryCorrection.Any())
            {
                return new BaseDataResult(false, "По указанному периоду записи в региональной программе отсутствуют");
            }

            // получаем все работы по КЭ
            var jobs = StrElWorksDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    WorkId = x.Job.Work.Id,
                    strElId = x.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.strElId)
                .ToDictionary(x => x.Key, y => y.First());

            var correctionYearsDict = CorrectionDomain.GetAll()
                                .Where(x => queryCorrection.Any(y => y.Id == x.Id))
                                .Select(x => new { St2Id = x.Stage2.Id, CorrectionYear = x.PlanYear })
                                .AsEnumerable()
                                .GroupBy(x => x.St2Id)
                                .ToDictionary(x => x.Key, y => y.Select(z => z.CorrectionYear).FirstOrDefault());


            // получаем те данные которые будем превращать в записи работ по объекту КР
            var query = Stage1Domain.GetAll()
                .Where(x => queryCorrection.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                .Select(x => new
                {
                    x.Id,
                    roId = x.RealityObject.Id,
                    st2Id = x.Stage2Version.Id,
                    muId = x.RealityObject.Municipality.Id,
                    strId = x.StructuralElement.StructuralElement.Id,
                    volume = x.Volume > 0 ? x.Volume : x.StructuralElement.Volume,
                    sum = x.Sum > 0 ? x.Sum + x.SumService : x.Stage2Version.Sum,
                    x.Stage2Version.Stage3Version.Year,
                    x.RealityObject.AreaLiving,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.AreaLivingNotLivingMkd,
                    x.StructuralElement.StructuralElement.CalculateBy,
                    x.StructuralElement.StructuralElement.UnitMeasure
                })
                .AsEnumerable();

             var data = query
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, y => y.ToList());

            var muIds = query.Select(x => x.muId).Distinct().ToList();

            var finSource = FinSourceDomain.GetAll().FirstOrDefault(x => x.Code == "1");

            var firstStateObjectCr = StateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "cr_object");

            var firstStateSmr = StateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "cr_obj_monitoring_cmp");

            var listObjectCrToSave = new List<ObjectCr>();
            var listWorkTypeCrToSave = new List<TypeWorkCr>();
            var listSmrToSave = new List<MonitoringSmr>();
            var listTypeWorkStage1ToSave = new List<TypeWorkCrVersionStage1>();
            foreach (var kvp in data)
            {
                var newObjectCr = new ObjectCr(new RealityObject {Id = kvp.Key}, programCr)
                {
                    State = firstStateObjectCr
                };

                // Необходимо чтобы в КР ушли работы схлопнутые по виду работ
                // Поэтому делаю общий словарь чтобы неполучилось задвоения
                var currentWorks = new Dictionary<long, TypeWorkCr>();

                listObjectCrToSave.Add(newObjectCr);

                var smr = new MonitoringSmr { ObjectCr = newObjectCr, State = firstStateSmr};

                listSmrToSave.Add(smr);

                // теперь для каждой полученной записи создаем запись работы по объекту КР
                foreach (var item in kvp.Value)
                {
                    if(!jobs.ContainsKey(item.strId))
                        continue;

                    var job = jobs[item.strId];

                    if (!currentWorks.ContainsKey(job.WorkId))
                    {
                        var newWork = new TypeWorkCr
                        {
                            ObjectCr = newObjectCr,
                            Work = new Work { Id = job.WorkId },
                            Sum = 0,
                            CostSum = 0,
                            Volume = 0,
                            YearRepair = correctionYearsDict.ContainsKey(item.st2Id)
                                                 ? correctionYearsDict[item.st2Id]
                                                 : item.Year,
                            IsDpkrCreated = true,
                            IsActive = true,
                            FinanceSource = finSource
                        };

                        currentWorks.Add(job.WorkId, newWork);

                        listWorkTypeCrToSave.Add(newWork);
                    }

                    var work = currentWorks[job.WorkId];

                    var newTypeWorkSt1 = new TypeWorkCrVersionStage1
                    {
                        TypeWorkCr = work,
                        Stage1Version = Stage1Domain.Load(item.Id),
                        Sum = item.sum,
                        Volume = item.CalculateBy == PriceCalculateBy.Volume
                            ? item.volume
                            : item.CalculateBy == PriceCalculateBy.TotalArea
                                ? item.AreaMkd.ToDecimal()
                                : item.CalculateBy == PriceCalculateBy.LivingArea
                                    ? item.AreaLiving.ToDecimal()
                                    : item.CalculateBy == PriceCalculateBy.AreaLivingNotLivingMkd
                                        ? item.AreaLivingNotLivingMkd.ToDecimal()
                                        : 0,
                        UnitMeasure = item.UnitMeasure,
                        CalcBy = item.CalculateBy
                    };

                    listTypeWorkStage1ToSave.Add(newTypeWorkSt1);
                }
            }

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            var user = UserManager.GetActiveOperator();

            try
            {
                session.SetBatchSize(1);
                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        listObjectCrToSave.ForEach(x => session.Insert(x));
                        listSmrToSave.ForEach(x => session.Insert(x));
                        listWorkTypeCrToSave.ForEach(x => session.Insert(x));
                        listTypeWorkStage1ToSave.ForEach(x => session.Insert(x));

                        session.Insert(new ProgramCrChangeJournal
                        {
                            ProgramCr = programCr,
                            ChangeDate = DateTime.Now,
                            TypeChange = TypeChangeProgramCr.FromDpkr,
                            UserName = user != null ? user.Name : "Администратор",
                            MuCount = muIds.Count
                        });

                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, string.Format("Ошибка: {0}", exc.Message));
            }
            finally
            {
                Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            
            return new BaseDataResult(true, "Программа сформирована на основе региональной программы успешно");
        }

        public IEnumerable<RealityObjectDpkrInfo> GetOvrhlYears(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var currentYear = DateTime.Now.Date.Year;

            return Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .Where(x => x.ProgramVersion.IsMain)
                .Where(x => x.RealityObject.Municipality.Id == municipalityId)
                .Where(x => x.Year >= currentYear)
                .Select(x => new RealityObjectDpkrInfo
                {
                    RealityObject = x.RealityObject,
                    Year = x.Year,
                    Sum = x.Sum
                })
                .OrderBy(x => x.Year)
                .AsEnumerable()
                .GroupBy(x => x.RealityObject.Id)
                .Select(x => x.First());
        }
    }
}