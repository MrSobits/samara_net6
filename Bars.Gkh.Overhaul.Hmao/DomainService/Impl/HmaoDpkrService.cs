using Bars.Gkh.Authentification;
using Bars.GkhCr.Enums;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;
    using Enums;
    using NHibernate;
    using Overhaul.Entities;

    /// <summary>
    /// Домен сервис для ДПКР ХМАО
    /// </summary>
    public class HmaoDpkrService : IDpkrService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ObjectCr> objectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> typeWorkCrDomain { get; set; }

        public IDomainService<ProgramCr> programCrDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> correctionDomain { get; set; }

        public IDomainService<ProgramVersion> progVersionDomain { get; set; }

        public IDomainService<VersionRecordStage2> stage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> stage1Domain { get; set; }

        public IDomainService<StructuralElementWork> StrElWorksDomain { get; set; }

        public IDomainService<FinanceSource> finSourceDomain { get; set; }

        public IDomainService<MonitoringSmr> monitoringSmrDomain { get; set; }

        public IDomainService<State> stateDomain { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ProgramCrChangeJournal> ProgChangeJournalDomain { get; set; }

        /// <summary>
        /// Получение субсидии по МО 
        /// </summary>
        public IDataResult CreateProgramCrByDpkr(BaseParams baseParams)
        {
            var programCrId = baseParams.Params.GetAs<long>("programCrId");

            var user = UserManager.GetActiveOperator();

            var currentVersions = progVersionDomain.GetAll()
                                 .Where(x => x.IsMain && x.Municipality != null)
                                 .AsEnumerable()
                                 .GroupBy(x => x.Municipality.Id)
                                 .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            if (!currentVersions.Any())
            {
                return new BaseDataResult(false, "Не задана основная ни по одному МО");
            }

            if (programCrId < 1)
            {
                return new BaseDataResult(false, "Передан неверный идентификатор Программы КР");
            }

            var currentObjectCrMunicipalityIds =
                objectCrDomain.GetAll()
                              .Where(x => x.ProgramCr.Id == programCrId)
                              .Select(x => x.RealityObject.Municipality.Id)
                              .Distinct()
                              .ToList();

            var noExistIds = currentVersions.Keys.Where(x => !currentObjectCrMunicipalityIds.Contains(x)).ToList();

            if (!noExistIds.Any())
            {
                return new BaseDataResult(false, "По всем МО уже было проведено формирование программы из региональной программы");
            }

            var programCr = programCrDomain.GetAll().FirstOrDefault(x => x.Id == programCrId);

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

            // получаем те версии по которым еще непроводилась выгрузка из краткосрочки
            var versionIds = currentVersions.Values
                .Where(x => noExistIds.Contains(x.Municipality.Id))
                .Select(x => x.Id).Distinct().ToList();

            var queryCorrection = correctionDomain.GetAll()
                .Where(x => versionIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Id))
                .Where(x => x.PlanYear >= yearStart && x.PlanYear <= yearEnd).AsQueryable();

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

            var correctionYearsDict = correctionDomain.GetAll()
                                .Where(x => queryCorrection.Any(y => y.Id == x.Id))
                                .Select(x => new { St2Id = x.Stage2.Id, CorrectionYear = x.PlanYear })
                                .AsEnumerable()
                                .GroupBy(x => x.St2Id)
                                .ToDictionary(x => x.Key, y => y.Select(z => z.CorrectionYear).FirstOrDefault());

            // получаем те данные которые будем превращать в записи работ по объекту КР
            var data = stage1Domain.GetAll()
                                   .Where(x => queryCorrection.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                                   .Select(x => new
                                       {
                                           x.Id,
                                           roId = x.RealityObject.Id,
                                           st2Id = x.Stage2Version.Id,
                                           strId = x.StructuralElement.StructuralElement.Id,
                                           volume = x.Volume>0 ? x.Volume : x.StructuralElement.Volume,
                                           sum = x.Sum > 0 ? x.Sum + x.SumService : x.Stage2Version.Sum,
                                           x.Stage2Version.Stage3Version.Year,
                                           x.RealityObject.AreaLiving,
                                           x.RealityObject.AreaMkd,
                                           x.RealityObject.AreaLivingNotLivingMkd,
                                           x.StructuralElement.StructuralElement.CalculateBy,
                                           x.StructuralElement.StructuralElement.UnitMeasure
                                       })
                                   .AsEnumerable()
                                   .GroupBy(x => x.roId)
                                   .ToDictionary(x => x.Key, y => y.ToList());

            var finSource = this.finSourceDomain.GetAll().FirstOrDefault(x => x.Code == "1");

            var firstStateObjectCr = stateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "cr_object");

            var firstStateSmr = stateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "cr_obj_monitoring_cmp");

            var listObjectCrToSave = new List<ObjectCr>();
            var listWorkTypeCrToSave = new List<TypeWorkCr>();
            var listSmrToSave = new List<MonitoringSmr>();
            var listTypeWorkStage1ToSave = new List<TypeWorkCrVersionStage1>();

            foreach (var kvp in data)
            {
                var newObjectCr = new ObjectCr(new RealityObject { Id = kvp.Key }, programCr)
                {
                    State = firstStateObjectCr
                };

                // Необходимо чтобы в КР ушли работы схлопнутые по виду работ
                // Поэтому делаю общий словарь чтобы неполучилось задвоения
                var currentWorks = new Dictionary<long, TypeWorkCr>();

                listObjectCrToSave.Add(newObjectCr);

                var smr = new MonitoringSmr { ObjectCr = newObjectCr, State = firstStateSmr };

                listSmrToSave.Add(smr);

                // теперь для каждой полученной записи создаем запись работы по объекту КР
                foreach (var item in kvp.Value)
                {

                    if (!jobs.ContainsKey(item.strId))
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
                        Stage1Version = stage1Domain.Load(item.Id),
                        Sum = item.sum,
                        Volume = item.CalculateBy == PriceCalculateBy.Volume ? item.volume :
                                 item.CalculateBy == PriceCalculateBy.TotalArea ? item.AreaMkd.ToDecimal() :
                                 item.CalculateBy == PriceCalculateBy.LivingArea ? item.AreaLiving.ToDecimal() :
                                 item.CalculateBy == PriceCalculateBy.AreaLivingNotLivingMkd ? item.AreaLivingNotLivingMkd.ToDecimal() : 0,
                        UnitMeasure = item.UnitMeasure,
                        CalcBy = item.CalculateBy
                    };

                    listTypeWorkStage1ToSave.Add(newTypeWorkSt1);
                }
            }

            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

            try
            {
                using (var tr = session.BeginTransaction())
                {
                    try
                    {
                        listObjectCrToSave.ForEach(x => session.Insert(x));
                        listSmrToSave.ForEach(x => session.Insert(x));
                        listWorkTypeCrToSave.ForEach(x => session.Insert(x));
                        listTypeWorkStage1ToSave.ForEach(x => session.Insert(x));

                        ProgChangeJournalDomain.Save(new ProgramCrChangeJournal
                                                        {
                                                            ProgramCr = programCr,
                                                            ChangeDate = DateTime.Now,
                                                            TypeChange = TypeChangeProgramCr.FromDpkr,
                                                            UserName = user != null ? user.Name : "Администратор",
                                                            MuCount = versionIds.Count
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