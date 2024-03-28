using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.GkhCr.Entities;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    public class MakeKPKRFromDPKRService : IMakeKPKRFromDPKRService
    {
        #region Properties

        public IDomainService<ProgramCr> ProgramCrDomain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IDomainService<Period> PeriodDomain { get; set; }

        public IDomainService<Work> WorkDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<FinanceSource> finSourceDomain { get; set; }

        public IDomainService<StructuralElementWork> StructuralElementWorkDomain { get; set; }

        public IDomainService<MaxSumByYear> MaxSumByYearDomain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> RealityObjectStructuralElementInProgrammStage3Domain { get; set; }

        public IDomainService<PSDWorks> PSDWorksDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Public methods

        public IDataResult MoveTypeWork(BaseParams baseParams, Int64 programId, Int64 typeworkToMoveId)
        {

            var objectCrDomain = Container.ResolveDomain<ObjectCr>();
            var workDomain = Container.ResolveDomain<Work>();
            var roDomain = Container.ResolveDomain<RealityObject>();
            var programDomain = Container.ResolveDomain<ProgramCr>();
            var typeWorkRepo = Container.ResolveRepository<TypeWorkCr>();

            if (typeworkToMoveId > 0 && programId > 0)
            {
                var program = programDomain.Get(programId);
                var typeWorkCr = typeWorkRepo.Get(typeworkToMoveId);
                var realityObject = typeWorkCr.ObjectCr.RealityObject;
                var objectCrInNewPr = objectCrDomain.GetAll()
                    .Where(x => x.RealityObject == realityObject)
                    .Where(x => x.ProgramCr.Id == programId).FirstOrDefault();
                var currentProgramName = typeWorkCr.ObjectCr.ProgramCr.Name;
                var currentYear = typeWorkCr.ObjectCr.ProgramCr.Period.DateStart.Year;
                if (objectCrInNewPr != null)
                {
                    typeWorkCr.ObjectCr = objectCrInNewPr;
                    typeWorkCr.Description = "Перенесена из программы " + currentProgramName;
                    typeWorkRepo.Update(typeWorkCr);
                }
                else
                {
                    var objectCr = new ObjectCr(realityObject, program) { };
                    objectCrDomain.Save(objectCr);
                    typeWorkCr.ObjectCr = objectCr;
                    typeWorkCr.Description = "Перенесена из программы " + currentProgramName;
                    typeWorkRepo.Update(typeWorkCr);
                }
                //меняем год в ДПКР
                try
                {
                    int year = program.Period.DateStart.Year;
                    var structElement = StructuralElementWorkDomain.GetAll()
                   .Where(x => x.Job.Work == typeWorkCr.Work)
                   .Select(x => x.StructuralElement)
                   .FirstOrDefault();
                    if (structElement != null)
                    {
                        var versionRecord = Stage1Domain.GetAll()
                         .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                         .Where(x => x.StructuralElement.RealityObject == realityObject)
                         .Where(x => x.Stage2Version.Stage3Version.Show)
                         .Where(x => x.Stage2Version.Stage3Version.Year >= currentYear - 2 && x.Stage2Version.Stage3Version.Year <= currentYear + 2)
                         .Where(x => x.StructuralElement.StructuralElement == structElement)
                         .Select(x => x.Stage2Version.Stage3Version).FirstOrDefault();
                        if (versionRecord != null)
                        {
                            versionRecord.Year = year;
                            versionRecord.IsManuallyCorrect = true;
                            versionRecord.IsChangedYear = true;
                            versionRecord.Changes = "Изменено переносом работы в КПКР " + DateTime.Now.ToString("dd.MM.yyyy");
                            VersionRecordDomain.Update(versionRecord);
                        }
                    }
                    else
                    {
                        return new BaseDataResult(false, "Работа КПКР перенесена в новый период, ООИ в ДПКР не определен, отредактируйте версию вручную");
                    }

                    
                }
                catch
                {
                    return new BaseDataResult(false, "Работа КПКР перенесена в новый период, ООИ в ДПКР не определен, отредактируйте версию вручную");
                }
            }
            else
            {
                return new BaseDataResult(false, "Не определены новая программа или не найдена работа");
            }
            return new BaseDataResult(true);
        }

        /// <summary>
        /// Создает КПКР из ДПКР
        /// </summary>
        /// <param name="version">Версия программы</param>
        /// <param name="startYear">Год начала</param>
        /// <param name="yearCount">Количество лет для формирования</param>
        /// <param name="firstYearPSD">Формировать ПСД по всем годам в первом году КПКР</param>
        /// <param name="firstYearWithoutWork">В первый год кпкр нет работ</param>  
        public string MakeKPKR(ProgramVersion version, short startYear, byte yearCount, bool firstYearPSD, bool firstYearWithoutWork, bool SKWithWorks, bool PSDWithWorks, bool PSDNext3, bool EathWorkPSD, bool OneProgramCR)
        {

            var mkdId = VersionRecordDomain.GetAll()
                 .Where(x => x.ProgramVersion.Id == version.Id)
                 .Where(x=> !x.SubProgram)
                 .Where(x => x.Show)
                 .Select(x => new
                 {
                     x.RealityObject
                 });

            //сформируем все работы из КЭ
            var works = Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.Stage2Version.Stage3Version.Show)
                .Where(x => x.Stage2Version.Stage3Version.Year >= startYear && x.Stage2Version.Stage3Version.Year < (startYear + yearCount))
                .Where(x => x.StructuralElement.StructuralElement.Group.CommonEstateObject != null)
                .Select(x => new BlankWork
                {
                    Work = GetWorkFromStructuralElement(x.StructuralElement.StructuralElement),
                    CommonEstateObject = x.StructuralElement.StructuralElement.Group.CommonEstateObject,
                    HasPsd = firstYearPSD,
                    Volume = x.StructuralElement.Volume,
                    Sum = x.Stage2Version.Stage3Version.Sum,
                    Description = $"Работа из версии {x.Id}",
                    YearRepair = x.Stage2Version.Stage3Version.Year,
                    House = x.RealityObject
                })
                .ToList();

            works = GroupByYear(works, startYear, yearCount, firstYearWithoutWork);

            string name = $"Программа КР из программы ДПКР";

            if(!OneProgramCR)
            {
                //если ошибка, откатываем все изменения
                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        //создаем КПКР
                        for (short year = startYear; year < startYear + yearCount; year++)
                        {
                            MakeKPKR(name, year, works, firstYearPSD && year == startYear, SKWithWorks, PSDWithWorks, PSDNext3, EathWorkPSD);
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            else
            {
                //если ошибка, откатываем все изменения
                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        //создаем КПКР                     
                        MakeOneKPKR(name, startYear, yearCount, works, SKWithWorks, PSDWithWorks, PSDNext3, EathWorkPSD);
                       
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return name;
        }

        /// <summary>
        /// Создает подпрограмму КПКР из ДПКР
        /// </summary>
        /// <param name="version">Версия программы</param>
        /// <param name="startYear">Год начала</param>
        /// <param name="yearCount">Количество лет для формирования</param>
        /// <param name="firstYearPSD">Формировать ПСД по всем годам в первом году КПКР</param>
        /// <param name="firstYearWithoutWork">В первый год кпкр нет работ</param>
        /// <param name="KEIds">выбранные КЭ</param> 
        public string MakeSubKPKR(ProgramVersion version, short startYear, byte yearCount, bool firstYearPSD, bool firstYearWithoutWork, long[] KEIds)
        {
            //сформируем все работы из КЭ
            var works = Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.RealityObject.IsSubProgram)
                .Where(x => x.Stage2Version.Stage3Version.Year >= startYear && x.Stage2Version.Stage3Version.Year < (startYear + yearCount))
                .Where(x => x.StructuralElement.StructuralElement.Group.CommonEstateObject != null)
                .Where(x => KEIds.Contains(x.Id))
                .Select(x => new BlankWork
                {
                    Work = GetWorkFromStructuralElement(x.StructuralElement.StructuralElement),
                    CommonEstateObject = x.StructuralElement.StructuralElement.Group.CommonEstateObject,
                    HasPsd = firstYearPSD,
                    Volume = x.StructuralElement.Volume,
                    Sum = x.Stage2Version.Stage3Version.Sum,
                    Description = $"Работа из версии {x.Id}",
                    YearRepair = x.Stage2Version.Stage3Version.Year,
                    House = x.RealityObject
                }).ToList();

            works = GroupByYear(works, startYear, yearCount, firstYearWithoutWork);

            string name = $"Программа КР из подпрограммы ДПКР";

            //если ошибка, откатываем все изменения
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    //создаем КПКР
                    for (short year = startYear; year < startYear + yearCount; year++)
                    {
                        MakeKPKR(name, year, works, firstYearPSD && year == startYear, false, false, false, false);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return name;
        }

        /// <summary>
        /// Возвращает список КЭ в подпрограмме в выбранных годах
        /// </summary>
        /// <param name="version"></param>
        /// <param name="startYear"></param>
        /// <param name="yearCount"></param>
        /// <returns></returns>
        public List<KEWithHouse> GetKE(ProgramVersion version, short startYear, byte yearCount)
        {
            return Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.RealityObject.IsSubProgram)
                .Where(x => (x.Stage2Version.Stage3Version.Year >= startYear) && (x.Stage2Version.Stage3Version.Year < (startYear + yearCount)))
                .Select(x => new KEWithHouse
                {
                    Id = x.Id,
                    CommonEstateObject = x.StructuralElement.StructuralElement.Group.CommonEstateObject,
                    RealityObject = x.RealityObject,
                    Sum = x.Stage2Version.Stage3Version.Sum
                })
                .ToList();
        }

        /// <summary>
        /// Возвращает стоимость и допустимую стоимость выбранных КЭ
        /// </summary>
        /// <param name="version"></param>
        /// <param name="selectedKE"></param>
        /// <returns></returns>
        public SumView GetCosts(ProgramVersion version, long[] selectedKE)
        {
            var currentSumByYears = GetSumByYears(selectedKE);

            return new SumView
            {
                CurrentCost = currentSumByYears.Sum(x => x.Value),
                CurrentLimit = currentSumByYears.Sum(x => GetGrantedSum(x.Key, version)),
            };
        }

        /// <summary>
        /// Возвращает стоимости выбранных КЭ по годам
        /// </summary>
        /// <param name="version"></param>
        /// <param name="selectedKE"></param>
        /// <returns>Обертка год/сумма/допустимая сумма </returns>
        public List<SumByYearsView> GetCostsByYear(ProgramVersion version, long[] selectedKE)
        {
           var currentSumByYears = GetSumByYears(selectedKE);

            return currentSumByYears.Select(x => new SumByYearsView
            {
                Year = x.Key,
                Sum = x.Value,
                GrantedSum = GetGrantedSum(x.Key, version)
            }).ToList();
        }

        /// <summary>
        /// Проверяет выбранные работы на допустимую стоимость
        /// </summary>
        /// <param name="version"></param>
        /// <param name="selectedKE"></param>
        /// <returns></returns>
        public bool CheckCostsByYear(ProgramVersion version, long[] selectedKE)
        {
            var currentSumByYears = GetSumByYears(selectedKE);

            foreach (var sumByYear in currentSumByYears)
            {
                if (GetGrantedSum(sumByYear.Key, version) < sumByYear.Value)
                    return false;
            }

            return true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Создает программу на заданный год
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="year">Год</param>
        /// <param name="works">Список работ (в т.ч. других годов, для псд)</param>
        /// <param name="makePSD">Создать работы ПСД для всех работ</param>
        private void MakeKPKR(string name, short year, List<BlankWork> works, bool makePSD, bool SKWithWorks, bool PSDWithWorks, bool PSDNext3, bool EathWorkPSD)
        {
            if (!makePSD && !works.Where(x => x.YearRepair == year).Any())
                return;

            //ищем програму
            name = $"{name} {year}";

            ProgramCr program;

            var programs = ProgramCrDomain.GetAll().Where(x => x.Name == name);
            if (programs.Any())
            {
                program = programs.First();
            }
            else
            {
                program = new ProgramCr()//создаем программу
                {
                    TypeVisibilityProgramCr = GkhCr.Enums.TypeVisibilityProgramCr.Full,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    Name = name,
                    Period = GetYearPeriod(year),
                    Code = year.ToString(),
                    TypeProgramStateCr = GkhCr.Enums.TypeProgramStateCr.New
                };

                ProgramCrDomain.Save(program);
            };            

            //выбираем работы этого года и группируем по дому
            var worksByHouse = works
                 .Where(x => x.House != null)
                 .GroupBy(x => x.House.Id)
                 .ToDictionary(x => x.Key, y => y.ToList());

            foreach (var workByHouse in worksByHouse)
            {
                var NormalWorks = workByHouse.Value.Where(x => x.YearRepair == year);
                var PSDWorks = works.Where(x => x.House.Id == workByHouse.Key);

                if (!NormalWorks.Any() && (makePSD ? !PSDWorks.Any() : true))
                    continue;

                var house = RealityObjectDomain.Get(workByHouse.Key);

                //ищем подходящий ObjectCr
                var objectCr = ObjectCrDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == workByHouse.Key && x.ProgramCr.Id == program.Id);
                if (objectCr == null)
                {
                    //создаем ObjectCr для этого дома
                    objectCr = new ObjectCr(house, program) { };
                    ObjectCrDomain.Save(objectCr);
                }

                //псд
                TypeWorkCr psdwork = null;
                if (makePSD && !EathWorkPSD)
                {
                    psdwork = MakePSDForWork(GetPSDSum(PSDWorks), objectCr, year);

                    //работы
                    NormalWorks
                    .GroupBy(x => x.Work.Id)
                    .ToDictionary(x => x.Key)
                    .ForEach(x =>
                    {
                        var work = CloneTypeWorkIntoNewObjectCr(GroupWork(x.Value), objectCr);

                    });
                }
                else if (makePSD && EathWorkPSD)
                {
                    foreach (BlankWork bw in PSDWorks)
                    {
                        psdwork = MakePSDForEachWork(GetPSDSum(new List<BlankWork> { bw }), objectCr, year, bw);
                        if (!SKWithWorks)
                        {
                            MakeSKForEachWork(GetSKSum(new List<BlankWork> { bw }), objectCr, year, bw);
                        }
                    }

              
                }

                NormalWorks
              .GroupBy(x => x.Work.Id)
              .ToDictionary(x => x.Key)
              .ForEach(x =>
              {
                  var work = CloneTypeWorkIntoNewObjectCr(GroupWork(x.Value), objectCr);
                  if (SKWithWorks)
                  {
                      MakeSKForEachWork(GetSKSum(new List<BlankWork> { GroupWork(x.Value) }), objectCr, year, GroupWork(x.Value));
                  }
                  if (!EathWorkPSD && psdwork != null && work != null && !PSDWorksDomain.GetAll().Where(y => y.Work.Id == work.Id).Where(y => y.PSDWork.Id == psdwork.Id).Any())
                  {
                      PSDWorks newWork = new PSDWorks
                      {
                          Work = work,
                          PSDWork = psdwork,
                          ObjectCreateDate = DateTime.Now,
                          ObjectVersion = 1,
                          ObjectEditDate = DateTime.Now,
                          Cost = work.Sum.HasValue ? work.Sum.Value : 0
                      };
                      try
                      {
                          PSDWorksDomain.Save(newWork);
                      }
                      catch (Exception e)
                      {

                      }
                  }
              });

                //общая сумма
                objectCr.MaxKpkrAmount = workByHouse.Value.Where(x => x.YearRepair == year).Sum(x => x.Sum) * 0.0338m;
                if (makePSD)
                    objectCr.MaxKpkrAmount += PSDWorks.Sum(x => x.Sum) * 0.0338m;

                ObjectCrDomain.Update(objectCr);
            }
        }

        /// <summary>
        /// Создает программу на весь период
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="year">Год</param>
        /// <param name="works">Список работ (в т.ч. других годов, для псд)</param>
        /// <param name="makePSD">Создать работы ПСД для всех работ</param>
        private void MakeOneKPKR(string name, short startyear, byte yearsCount, List<BlankWork> works, bool SKWithWorks, bool PSDWithWorks, bool PSDNext3, bool EathWorkPSD)
        {

            //ищем програму
            name = $"{name} {startyear}-{startyear + yearsCount-1}";

            ProgramCr program;

            var programs = ProgramCrDomain.GetAll().Where(x => x.Name == name);
            if (programs.Any())
            {
                program = programs.First();
            }
            else
            {
                program = new ProgramCr()//создаем программу
                {
                    TypeVisibilityProgramCr = GkhCr.Enums.TypeVisibilityProgramCr.Full,
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    Name = name,
                    Period = GetPeriodForProgram(startyear, yearsCount),
                    Code = $"{startyear}_{startyear + yearsCount - 1}",
                    TypeProgramStateCr = GkhCr.Enums.TypeProgramStateCr.New
                };

                ProgramCrDomain.Save(program);
            };

            //выбираем работы этого года и группируем по дому
            var worksByHouse = works
                 .Where(x => x.House != null)
                 .GroupBy(x => x.House.Id)
                 .ToDictionary(x => x.Key, y => y.ToList());

            foreach (var workByHouse in worksByHouse)
            {         

                var house = RealityObjectDomain.Get(workByHouse.Key);

                //ищем подходящий ObjectCr
                var objectCr = ObjectCrDomain.GetAll().FirstOrDefault(x => x.RealityObject.Id == workByHouse.Key && x.ProgramCr.Id == program.Id);
                if (objectCr == null)
                {
                    //создаем ObjectCr для этого дома
                    objectCr = new ObjectCr(house, program) { };
                    ObjectCrDomain.Save(objectCr);
                }

                //псд
                TypeWorkCr psdwork = null;
                if (PSDWithWorks && !EathWorkPSD)
                {
                    psdwork = MakePSDForWork(GetPSDSum(workByHouse.Value), objectCr, startyear);

                    //работы
                    workByHouse.Value
                    .GroupBy(x => x.Work.Id)
                    .ToDictionary(x => x.Key)
                    .ForEach(x =>
                    {
                        var work = CloneTypeWorkIntoNewObjectCr(GroupWork(x.Value), objectCr);

                    });
                }
                else if (PSDWithWorks && EathWorkPSD)
                {
                    foreach (BlankWork bw in workByHouse.Value)
                    {
                        psdwork = MakePSDForEachWork(GetPSDSum(new List<BlankWork> { bw }), objectCr, startyear, bw);
                        //if (SKWithWorks)
                        //{
                        //    MakeSKForEachWork(GetSKSum(new List<BlankWork> { bw }), objectCr, startyear, bw);
                        //}
                    }


                }

                workByHouse.Value
             .GroupBy(x => x.Work.Id)
              .ToDictionary(x => x.Key)
              .ForEach(x =>
              {
                  var work = CloneTypeWorkIntoNewObjectCr(GroupWork(x.Value), objectCr);
                  if (SKWithWorks)
                  {
                      MakeSKForEachWork(GetSKSum(new List<BlankWork> { GroupWork(x.Value) }), objectCr, startyear, GroupWork(x.Value));
                  }
                  if (!EathWorkPSD && psdwork != null && work != null && !PSDWorksDomain.GetAll().Where(y => y.Work.Id == work.Id).Where(y => y.PSDWork.Id == psdwork.Id).Any())
                  {
                      PSDWorks newWork = new PSDWorks
                      {
                          Work = work,
                          PSDWork = psdwork,
                          ObjectCreateDate = DateTime.Now,
                          ObjectVersion = 1,
                          ObjectEditDate = DateTime.Now,
                          Cost = work.Sum.HasValue ? work.Sum.Value : 0
                      };
                      try
                      {
                          PSDWorksDomain.Save(newWork);
                      }
                      catch (Exception e)
                      {

                      }
                  }
              });

                //общая сумма
                objectCr.MaxKpkrAmount = workByHouse.Value.Sum(x => x.Sum);
                if (PSDWithWorks)
                    objectCr.MaxKpkrAmount += workByHouse.Value.Sum(x => x.Sum) * 0.0338m;

                ObjectCrDomain.Update(objectCr);
            }
        }


        private BlankWork GroupWork(IGrouping<long, BlankWork> value)
        {
            if (value.Count() == 0)
                return null;

            else if (value.Count() == 1)
                return value.First();

            else if (value.Count() > 1)
                return new BlankWork
                {
                    Work = value.First().Work,
                    Volume = value.Sum(y => y.Volume),
                    Sum = value.Sum(y => y.Sum),
                    Description = value.First().Description,
                    YearRepair = value.First().YearRepair,
                    House = value.First().House,
                };

            else throw new Exception("Ужс, отрицательное количество элементов");
        }

        private List<BlankWork> GroupByYear(List<BlankWork> works, short startYear, byte yearCount, bool firstYearWithoutWork)
        {
            //группировка по ООИ
            works = works
                .GroupBy(x => new { HouseId = x.House.Id, CommonEstateObjectId = x.CommonEstateObject.Id, x.YearRepair, WorkId = x.Work.Id })
                .ToDictionary(x => x.Key)
                .Select(x => MakeWorkFromGroup(x.Value.ToList()))
                .ToList();

            //в первый год нет работ
            if (firstYearWithoutWork)
                works.Where(x => x.YearRepair == startYear)
                    .Where(x => x.Work.Code != "6")
                    .ForEach(x => x.YearRepair++);

            return works;
        }

        private BlankWork MakeWorkFromGroup(List<BlankWork> value)
        {
            if (value == null)
                return null;
            else if (value.Count() == 1)
                return value.First();
            else return new BlankWork
            {
                Work = value.First().Work,
                HasPsd = value.Any(x => x.HasPsd),
                Volume = value.Sum(x => x.Volume),
                Sum = value.Sum(x => x.Sum),
                Description = value.First().Description,
                YearRepair = value.First().YearRepair,
                House = value.First().House,
                CommonEstateObject = value.First().CommonEstateObject,
            };
        }

        private TypeWorkCr CloneTypeWorkIntoNewObjectCr(BlankWork work, ObjectCr objectCR)
        {
            var typeWork = TypeWorkCrDomain.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCR.Id && x.Work.Id == work.Work.Id);
            var stageWorkCrDomain = this.Container.Resolve<IDomainService<StageWorkCr>>();
            var stageWorkCr = stageWorkCrDomain.GetAll().Where(x => x.Code == "1").FirstOrDefault();
            if (typeWork != null && stageWorkCr!= null)
            {
                //обновить?
                typeWork.StageWorkCr = stageWorkCr;
                typeWork.HasPsd = work.HasPsd;
                typeWork.Volume = work.Volume;
                typeWork.Sum = work.Sum;
                typeWork.Description = work.Description;
                typeWork.YearRepair = work.YearRepair;
                typeWork.IsActive = true;
                typeWork.IsDpkrCreated = true;
                typeWork.State = GetTypeWorkDraftState();

                TypeWorkCrDomain.Update(typeWork);
            }
            else
            {
                //создать
                typeWork = new TypeWorkCr
                {
                    ObjectCr = objectCR,
                    FinanceSource = finSourceDomain.GetAll().FirstOrDefault(y => y.Code == "1"),
                    Work = work.Work,
                    StageWorkCr = stageWorkCr,
                    HasPsd = work.HasPsd,
                    Volume = work.Volume,
                    Sum = work.Sum,
                    Description = work.Description,
                    YearRepair = work.YearRepair,
                    IsActive = true,
                    IsDpkrCreated = true,
                    State = GetTypeWorkDraftState()
                };

                TypeWorkCrDomain.Save(typeWork);
            }

            return typeWork;
        }

        /// <summary>
        /// Создает ПСД работу
        /// </summary>
        /// <param name="cost">Сумма работы</param>
        /// <param name="objectCR">ОКР</param>
        /// <param name="year">Год</param>
        private TypeWorkCr MakePSDForWork(decimal cost, ObjectCr objectCR, short year)
        {
            var psdwork = WorkDomain.GetAll().FirstOrDefault(x => x.IsPSD);//Получаем работу по ПСД (по признаку IS_PSD в базе)
            if (psdwork == null)
                throw new ApplicationException("Не найдена работа, отмеченная как ПСД");

            var typeWork = TypeWorkCrDomain.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCR.Id && x.Work.Id == psdwork.Id);
            if (typeWork == null)
            {
                typeWork = new TypeWorkCr
                {
                    ObjectCr = objectCR,
                    FinanceSource = finSourceDomain.GetAll().FirstOrDefault(y => y.Code == "1"),
                    Work = psdwork,
                    StageWorkCr = new StageWorkCr { Id = 1L },
                    HasPsd = false,
                    Volume = 0,
                    Sum = cost,
                    Description = $"ПСД для года {year}",
                    YearRepair = year,
                    IsActive = true,
                    IsDpkrCreated = true,
                    State = GetTypeWorkDraftState()
                };

                TypeWorkCrDomain.Save(typeWork);
            }

            return typeWork;
        }

        /// <summary>
        /// Создает ПСД для указанной работы
        /// </summary>
        /// <param name="cost">Сумма работы</param>
        /// <param name="objectCR">ОКР</param>
        /// <param name="year">Год</param>
        private TypeWorkCr MakePSDForEachWork(decimal cost, ObjectCr objectCR, short year, BlankWork bw)
        {
            var psdName = "ПСД разработка/" + bw.Work.Name;
            var psdwork = WorkDomain.GetAll().FirstOrDefault(x => x.Name == psdName);//Получаем работу по ПСД (по признаку IS_PSD в базе)

            if (psdwork == null)
                psdwork = WorkDomain.GetAll().FirstOrDefault(x => x.IsPSD);

            if (psdwork == null)
                throw new ApplicationException("Не найдена работа, отмеченная как ПСД");

            var typeWork = TypeWorkCrDomain.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCR.Id && x.Work.Id == psdwork.Id);
            if (typeWork == null)
            {
                typeWork = new TypeWorkCr
                {
                    ObjectCr = objectCR,
                    FinanceSource = finSourceDomain.GetAll().FirstOrDefault(y => y.Code == "1"),
                    Work = psdwork,
                    StageWorkCr = new StageWorkCr { Id = 1L },
                    HasPsd = false,
                    Volume = 0,
                    Sum = cost,
                    Description = $"ПСД для года {year}",
                    YearRepair = year,
                    IsActive = true,
                    IsDpkrCreated = true,
                    State = GetTypeWorkDraftState()
                };

                TypeWorkCrDomain.Save(typeWork);
            }

            return typeWork;
        }

        /// <summary>
        /// Создает Стройконтроль для указанной работы
        /// </summary>
        /// <param name="cost">Сумма работы</param>
        /// <param name="objectCR">ОКР</param>
        /// <param name="year">Год</param>
        private TypeWorkCr MakeSKForEachWork(decimal cost, ObjectCr objectCR, short year, BlankWork bw)
        {
            var skName = "Стройконтроль/" + bw.Work.Name;
            var skwork = WorkDomain.GetAll().FirstOrDefault(x => x.Name == skName);

            if (skwork == null)
                throw new ApplicationException("Не найдена работа, отмеченная как стройкнотроль");

            var typeWork = TypeWorkCrDomain.GetAll().FirstOrDefault(x => x.ObjectCr.Id == objectCR.Id && x.Work.Id == skwork.Id);
            if (typeWork == null)
            {
                typeWork = new TypeWorkCr
                {
                    ObjectCr = objectCR,
                    FinanceSource = finSourceDomain.GetAll().FirstOrDefault(y => y.Code == "1"),
                    Work = skwork,
                    StageWorkCr = new StageWorkCr { Id = 1L },
                    HasPsd = false,
                    Volume = 0,
                    Sum = cost,
                    Description = $"Стройконтроль для года {bw.YearRepair}",
                    YearRepair = bw.YearRepair,
                    IsActive = true,
                    IsDpkrCreated = true,
                    State = GetTypeWorkDraftState()
                };

                TypeWorkCrDomain.Save(typeWork);
            }

            return typeWork;
        }

        private State GetTypeWorkDraftState()
        {
            var state = StateDomain.GetAll().Where(x => x.TypeId.Contains("cr_type_work")).Where(x => x.Name.ToLower().Contains("черновик")).FirstOrDefault();
            if (state == null)
                throw new ApplicationException("На найден статус \"Черновик\" для TypeWork");

            return state;
        }

        private decimal GetPSDSum(IEnumerable<BlankWork> works)
        {
            if (works.Count() == 0)
                return 0;
            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().AppSettings;
            decimal pSDPerscent = 0;
            try
            {
                pSDPerscent = config.GetAs("PSDPerscent", (decimal)0, true);
            }
            catch
            {

            }
            return works.Sum(x => x.Sum) * (pSDPerscent>0? (pSDPerscent/100):0.0338m);
        }

        private decimal GetSKSum(IEnumerable<BlankWork> works)
        {
            if (works.Count() == 0)
                return 0;
            var configProvider = Container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().AppSettings;
            decimal sKPerscent = 0;
            try
            {
                sKPerscent = config.GetAs("SKPerscent", (decimal)0, true);
            }
            catch
            {

            }
            return works.Sum(x => x.Sum) * (sKPerscent > 0 ? (sKPerscent / 100) : 0.02m);
        }

        private Work GetWorkFromStructuralElement(StructuralElement structuralElement)
        {
            // получаем все работы по КЭ
            var work = StructuralElementWorkDomain.GetAll()
                    .Where(x => x.StructuralElement == structuralElement)
                    .Select(x => x.Job.Work)
                    .FirstOrDefault();

            if (work == null)
            {
                work = new Work
                {
                    Name = $"Работа из КЭ {structuralElement.Name}"
                };

                WorkDomain.Save(work);
            }

            return work;
        }

        private Period GetYearPeriod(short year)
        {
            var period = PeriodDomain.GetAll().Where(x => x.DateStart == new DateTime(year, 1, 1) && x.DateEnd == new DateTime(year, 12, 31)).FirstOrDefault();

            if (period == null)
            {
                period = new Period
                {
                    Name = year + " год",
                    DateStart = new DateTime(year, 1, 1),
                    DateEnd = new DateTime(year, 12, 31)
                };

                PeriodDomain.Save(period);
            }

            return period;
        }

        private Period GetPeriodForProgram(short startYear, byte yearsCount)
        {
            var period = PeriodDomain.GetAll().Where(x => x.DateStart == new DateTime(startYear, 1, 1) && x.DateEnd == new DateTime(startYear+yearsCount-1, 12, 31)).FirstOrDefault();

            if (period == null)
            {
                period = new Period
                {
                    Name = $"{startYear}-{startYear + yearsCount - 1}гг",
                    DateStart = new DateTime(startYear, 1, 1),
                    DateEnd = new DateTime(startYear + yearsCount - 1, 12, 31)
                };

                PeriodDomain.Save(period);
            }

            return period;
        }

        private decimal GetGrantedSum(int year, ProgramVersion version)
        {
            var maxsum = MaxSumByYearDomain.GetAll()
                .Where(x => x.Year == year)
                .ToList()
                .Where(x => x.Program == null || x.Program == version)
                .Where(x => x.Municipality == null || x.Municipality == version.Municipality)
                .FirstOrDefault();

            return maxsum == null ? 0 : maxsum.Sum;
        }

        /// <summary>
        /// Возвращает сумму стоимости выбранных работ по годам
        /// </summary>
        /// <param name="stage1Ids"></param>
        /// <returns></returns>
        private Dictionary<int, decimal> GetSumByYears(long[] stage1Ids)
        {
            return Stage1Domain.GetAll()
            .Where(x => stage1Ids.Contains(x.Id))
            .GroupBy(x => x.Stage2Version.Stage3Version.Year)
            .ToDictionary(x => x.Key, x => x.Sum(y => y.Stage2Version.Stage3Version.Sum));
        }

        #endregion

        #region Nested classes

        private class BlankWork
        {
            public Work Work { get; set; }
            public bool HasPsd { get; set; }
            public decimal Volume { get; set; }
            public decimal Sum { get; set; }
            public string Description { get; set; }
            public int YearRepair { get; set; }
            public RealityObject House { get; internal set; }
            public CommonEstateObject CommonEstateObject { get; internal set; }
        }

        public class SumByYearsView
        {
            public int Year { get; set; }
            public decimal Sum { get; set; }
            public decimal GrantedSum { get; set; }
        }

        public class KEWithHouse
        {
            public long Id;
            public CommonEstateObject CommonEstateObject;
            public RealityObject RealityObject;
            public decimal Sum;
        }

        public class SumView
        {
            public decimal CurrentCost;
            public decimal CurrentLimit;
        }

        #endregion
    }
}
