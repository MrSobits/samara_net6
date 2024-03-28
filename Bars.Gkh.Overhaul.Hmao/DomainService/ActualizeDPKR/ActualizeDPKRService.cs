using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities.Version;
using Bars.Gkh.Overhaul.Hmao.Enum;
using Bars.Gkh.Overhaul.Hmao.Helpers;
using Bars.Gkh.Utils;
using Bars.GkhCr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.Gkh.Overhaul.Hmao.Services.ActualizeDPKR
{
    public class ActualizeDPKRService : IActualizeDPKRService
    {
        #region Properties

        public IUserIdentity UserIdentity { get; set; }

        public IGkhUserManager UserManager { get; set; }

        public IFileManager FileManager { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<DPKRActualCriterias> DPKRActualCriteriasDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public IDomainService<VersionRecordStage2> Stage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<VersionActualizeLog> VersionActualizeLogDomain { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Список домов на добавление
        /// </summary>
        List<VersionRecordWithReasonAdd> housesToAdd = new List<VersionRecordWithReasonAdd>();

        /// <summary>
        /// Начальный год, для которого был построен список домов на добавление
        /// </summary>
        short startYearCachedAdd;

        /// <summary>
        /// Версия, для которой был построен список домов на добавление
        /// </summary>
        ProgramVersion AddProgramVersion;

        /// <summary>
        /// Список домов на удаление
        /// </summary>
        List<VersionRecordWithReasonDelete> housesToDelete = new List<VersionRecordWithReasonDelete>();

        /// <summary>
        /// Начальный год, для которого был построен список домов на удаление
        /// </summary>
        short startYearCachedDelete;

        /// <summary>
        /// Версия, для которой был построен список домов на удаление
        /// </summary>
        ProgramVersion DeleteProgramVersion;

        #endregion

        #region public methods

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        public List<VersionRecordWithReasonView> GetAddEntriesList(ProgramVersion version, short startYear)
        {
            ActualizeAddCache(version, startYear);

            return housesToAdd.Where(x => !x.Disable)
            .Select(x => new VersionRecordWithReasonView
            {
                Id = x.Id,
                Address = x.house.Address,
                CommonEstateObject = x.commonEstateObject.Name,
                StructuralElement = x.structuralElement.Name,
                Year = x.year,
                Reasons = x.NeedToSave ? "Дом подходит под условия" : "Запись подходит под условия",
                RoseName = x.rose.Name
            }).ToList();
        }

        /// <summary>
        /// Получить список записей на удаление
        /// </summary>
        public List<VersionRecordWithReasonView> GetDeleteEntriesList(ProgramVersion version, short startYear)
        {
            ActualizeDeleteCache(version, startYear);

            return housesToDelete
                .Where(x => !x.Disable)
                .Where(x => !x.Passed)
                .Select(x => new VersionRecordWithReasonView
            {
                Id = x.Id,
                Address = x.house.Address,
                CommonEstateObject = x.commonEstateObject.Name,
                StructuralElement = x.structuralElement.Name,
                Year = x.VersionRecordStage1.Stage2Version.Stage3Version.Year,
                Reasons = x.Reasons
            }).ToList();
        }

        /// <summary>
        /// Удалить запись из списка на добавление
        /// </summary>
        /// <param name="Id"></param>
        public void RemoveHouseForAdd(long Id)
        {
            housesToAdd.Where(x => x.Id == Id).ForEach(x => x.Disable = true);
        }

        /// <summary>
        /// Удалить запись из списка на удаление
        /// </summary>
        /// <param name="Id"></param>
        public void RemoveHouseForDelete(long Id)
        {
            housesToDelete.Where(x => x.Id == Id).ForEach(x => x.Disable = true);
        }

        /// <summary>
        /// Удалить выбранные записи из списков
        /// </summary>
        /// <param name="version"></param>
        /// <param name="selectedAddId"></param>
        /// <param name="selectedDeleteId"></param>
        public void RemoveSelected(ProgramVersion version, long[] selectedAddId, long[] selectedDeleteId)
        {
            if (housesToDelete != null)
                foreach (long id in selectedDeleteId)
                {
                    housesToDelete.Where(x => x.Id == id).ForEach(x => x.Disable = true);
                }

            if (housesToAdd != null)
                foreach (long id in selectedAddId)
                {
                    housesToAdd.Where(x => x.Id == id).ForEach(x => x.Disable = true);
                }
        }

        /// <summary>
        /// Актуализировать записи
        /// </summary>
        /// <param name="version">Версия программы</param>
        /// <param name="startYear">Год начала актуализации; все добавленные работы должны быть после него</param>
        /// <param name="selectedAddId">Список id записей на добавление. Null означает все</param>
        /// <param name="selectedDeletedId">Список id записей на удаление. Null означает все</param>
        public void Actualize(ProgramVersion version, short startYear, long[] selectedAddId = null, long[] selectedDeletedId = null)
        {
            int count = 0;
            StringBuilder log = new StringBuilder();
            log.Append($"Id дома;Адрес;Id ООИ;ООИ;Id КЭ;КЭ;Начальный год;Год планового ремонта;Действие;Причина\n");

            //список stage3, которым надо обновить флажок видимости
            var changedStage3 = new Dictionary<long, VersionRecord>();

            //-----обрабатываем список на удаление-----
            ActualizeDeleteCache(version, startYear);

            var selectedHousesToDelete = housesToDelete
                .Where(x => !x.Disable)
                .Where(x => !x.Passed)
                .WhereIf(selectedDeletedId != null, x => selectedDeletedId.Contains(x.Id))
                .ToList();

            selectedHousesToDelete
            .ForEach(x =>
            {
                //изменяем статус
                x.VersionRecordStage1.VersionRecordState = VersionRecordState.NonActual;
                x.VersionRecordStage1.StateChangeDate = DateTime.Now;
                Stage1Domain.Update(x.VersionRecordStage1);

                if (!changedStage3.ContainsKey(x.VersionRecordStage1.Stage2Version.Stage3Version.Id))
                    changedStage3.Add(x.VersionRecordStage1.Stage2Version.Stage3Version.Id, x.VersionRecordStage1.Stage2Version.Stage3Version);

                housesToDelete.Remove(x);
                log.Append($"{x.house.Id};{x.house.Address};{x.commonEstateObject.Id};{x.commonEstateObject.Name};{x.structuralElement.Id};{x.structuralElement.Name};{startYear};{x.year};Удалено;{x.Reasons}\n");
                count++;
            });

            //-----обрабатываем список на добавление-----
            ActualizeAddCache(version, startYear);

            var selectedHousesToAdd = housesToAdd
                .Where(x => !x.Disable)
                .WhereIf(selectedAddId != null, x => selectedAddId.Contains(x.Id))
                .ToList();

            selectedHousesToAdd.ForEach(x =>
                {
                    if (x.NeedToSave)
                    {
                        var stage1 = MakeNewVersionRecords(x);

                        //сохраняем
                        if (stage1.Stage2Version.Stage3Version.Id == 0)
                            VersionRecordDomain.Save(stage1.Stage2Version.Stage3Version);

                        if (stage1.Stage2Version.Id == 0)
                            Stage2Domain.Save(stage1.Stage2Version);

                        Stage1Domain.Save(stage1);

                        if (!changedStage3.ContainsKey(stage1.Stage2Version.Stage3Version.Id))
                            changedStage3.Add(stage1.Stage2Version.Stage3Version.Id, stage1.Stage2Version.Stage3Version);
                    }
                    else
                    {
                        //изменяем существующую
                        x.versionRecordStage1.VersionRecordState = VersionRecordState.Actual;
                        x.versionRecordStage1.StateChangeDate = DateTime.Now;
                        Stage1Domain.Update(x.versionRecordStage1);

                        //выкидываем из подпрограммы
                        if (x.versionRecordStage1.RealityObject.IsSubProgram)
                        {
                            x.versionRecordStage1.RealityObject.IsSubProgram = false;
                            RealityObjectDomain.Update(x.versionRecordStage1.RealityObject);
                        }

                        if (!changedStage3.ContainsKey(x.versionRecordStage1.Stage2Version.Stage3Version.Id))
                            changedStage3.Add(x.versionRecordStage1.Stage2Version.Stage3Version.Id, x.versionRecordStage1.Stage2Version.Stage3Version);
                    }

                    housesToAdd.Remove(x);

                    log.Append($"{x.house.Id};{x.house.Address};{x.commonEstateObject.Id};{x.commonEstateObject.Name};{x.structuralElement.Id};{x.structuralElement.Name};{startYear};{x.year};Добавлено;{(x.NeedToSave ? "КЭ дома подошел под условия" : "Скрытая запись версии подошла под условие")}\n");
                    count++;
                });

            changedStage3.ForEach(x => ActualizeStage3(x.Value));

            Operator thisOperator = UserManager.GetActiveOperator();

            VersionActualizeLogDomain.Save(new VersionActualizeLog
            {
                ProgramVersion = version,
                Municipality = version.Municipality,
                UserName = thisOperator.Name,
                DateAction = DateTime.Now,
                ActualizeType = VersionActualizeType.ActualizeYear,
                CountActions = count,
                LogFile = FileManager.SaveFile($"ActualizeLog_{DateTime.Now.ToString("dd_MMM_yyyy-HH_mm_ss")}.csv", Encoding.UTF8.GetBytes(log.ToString())),
            });
        }

        /// <summary>
        /// Очистить списки на добавление и удаление
        /// </summary>
        public void ClearCache()
        {
            AddProgramVersion = null;
            DeleteProgramVersion = null;
        }


        #endregion

        #region Private methods

        private void ActualizeAddCache(ProgramVersion version, short startYear)
        {
            if (AddProgramVersion == null || AddProgramVersion.Id != version.Id || startYearCachedAdd != startYear)
            {
                housesToAdd.Clear();
                AddSuitableHouses(version, startYear, 2050);
             //   AddRecords(version, startYear, 2044);

                startYearCachedAdd = startYear;
                AddProgramVersion = version;
            }
        }

        private void ActualizeDeleteCache(ProgramVersion version, short startYear)
        {
            if (DeleteProgramVersion == null || DeleteProgramVersion.Id != version.Id || startYearCachedDelete != startYear)
            {
                housesToDelete = AddHousesToDelete(version, startYear);

                startYearCachedDelete = startYear;
                DeleteProgramVersion = version;
            }
        }

        /// <summary>
        /// Подбирает подходящие из скрытых записей
        /// </summary>
        private void AddRecords(ProgramVersion version, short startYear, short endYear)
        {
            //получаем все скрытые записи в этой версии
            var versionRecordQuery = Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.VersionRecordState == VersionRecordState.NonActual)
                .Where(x => x.Year >= startYear);

            //фильтруем
            var now = DateTime.Now;
            var actualCriterias = DPKRActualCriteriasDomain.GetAll().Where(x => x.DateStart <= now && x.DateEnd >= now);
            foreach (var criterias in actualCriterias)
            {
                versionRecordQuery = ApplyCriterias(versionRecordQuery, criterias);
            }

            IEnumerable<VersionRecordStage1> versionRecords = versionRecordQuery.ToList();
            foreach (var criterias in actualCriterias)
                versionRecords = ApplySoftCriterias(versionRecords, criterias);

            //добавляем в список
            foreach (var versionRecord in versionRecords)
            {
                var year = CorrectWorkYear(versionRecord.StructuralElement, startYear, null);
                if (year > endYear)
                    continue;

                housesToAdd.Add(new VersionRecordWithReasonAdd(versionRecord, year));
            }
        }

        /// <summary>
        /// Подбирает подходящие дома из не включенных в версию
        /// </summary>
        private void AddSuitableHouses(ProgramVersion version, short startYear, short endYear)
        {
            //получаем все активные фильтры
            var now = DateTime.Now;
            var actualCriterias = DPKRActualCriteriasDomain.GetAll().Where(x => x.DateStart <= now && x.DateEnd >= now).ToList();
            if (actualCriterias.Count == 0)
                return; //без критериев оно виснет на полдня

            //получаем все домокэ
            var housesList = RealityObjectStructuralElementDomain.GetAll()
              //  .Where(x=> x.RealityObject.Id == 130121)
                .Where(x => x.RealityObject.Municipality.Id == version.Municipality.Id);

            //фильтруем дома
            foreach (var criterias in actualCriterias)
                housesList = ApplyCriterias(housesList, criterias);

            IEnumerable<RealityObjectStructuralElement> housesKe = housesList.ToList();
            foreach (var criterias in actualCriterias)
            {
                housesKe = ApplySoftCriterias(housesKe, criterias);
            }

            //словарь дом - кэ, которые уже в версии
            var kecache = Stage1Domain.GetAll()
                        .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                         .Where(x => x.Stage2Version.Stage3Version.Show == true)
                        .Where(x => x.StructuralElement.StructuralElement != null)
                        .Select(x => new
                        {
                            RealityObjectId = x.RealityObject.Id,
                            StructuralElementId = x.StructuralElement.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RealityObjectId)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.StructuralElementId).Distinct().ToHashSet());
            

            //var kecache = Stage1Domain.GetAll()
            //           .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
            //           .Where(x => x.StructuralElement.StructuralElement != null)
            //           .Select(x => new
            //           {
            //               RealityObjectId = x.RealityObject.Id,
            //               StructuralElementId = x.StructuralElement.StructuralElement.Id
            //           })
            //           .AsEnumerable()
            //           .GroupBy(x => x.RealityObjectId)
            //           .ToDictionary(x => x.Key, y => y.Select(z => z.StructuralElementId).Distinct().ToHashSet());

            //дополняем КЭ    
            foreach (var rose in housesKe)
            {
                if (kecache.ContainsKey(rose.RealityObject.Id) && kecache[rose.RealityObject.Id].Contains(rose.Id))
                    continue;

                //получаем правильный год
                var year = CorrectWorkYear(rose, startYear, null);
                if (year == 0)
                    continue;

                housesToAdd.Add(new VersionRecordWithReasonAdd(
                      rose.RealityObject,
                      rose.StructuralElement,
                      rose.StructuralElement.Group.CommonEstateObject,
                      year,
                      rose
                  ));


                try
                {
                   

                    if (rose.StructuralElement.LifeTimeAfterRepair == 0)
                        rose.StructuralElement.LifeTimeAfterRepair = rose.StructuralElement.LifeTime;

                    decimal times = (2050 - year) /rose.StructuralElement.LifeTimeAfterRepair;
                    times = decimal.Truncate(times);
                    if (times > 0)
                    {
                        for (int y = 0; y <= times; y++)
                        {
                            short lifeTine = (Int16)(rose.StructuralElement.LifeTimeAfterRepair);
                            year = (Int16)(year + lifeTine);

                            housesToAdd.Add(new VersionRecordWithReasonAdd(
                            rose.RealityObject,
                            rose.StructuralElement,
                            rose.StructuralElement.Group.CommonEstateObject,
                            year,
                            rose));
                        }
                    }
                }
                catch(Exception e)
                {
                  
                }

                //housesToAdd.Add(new VersionRecordWithReasonAdd(
                //            rose.RealityObject,
                //            rose.StructuralElement,
                //            rose.StructuralElement.Group.CommonEstateObject,
                //            year,
                //            rose
                //        ));
            }
        }

        /// <summary>
        /// Создать новую stage1 для добавления
        /// </summary>
        private VersionRecordStage1 MakeNewVersionRecords(VersionRecordWithReasonAdd addedRecord)
        {
            //stage3
            var record = new VersionRecord
            {
                ProgramVersion = AddProgramVersion,
                RealityObject = addedRecord.house,
                Year = addedRecord.year,
                YearCalculated = addedRecord.year,
                CommonEstateObjects = "" //пересчитается в рамках актуализации stage3
            };

            //stage2
            var stage2 = new VersionRecordStage2
            {
                Stage3Version = record,
                Sum = record.Sum,
                CommonEstateObjectWeight = 0,
                CommonEstateObject = addedRecord.commonEstateObject
            };

            var housecr = addedRecord.rose;
            //связка с structel
            //var housecr = RealityObjectStructuralElementDomain.GetAll()
            //    .Where(x => x.RealityObject.Id == addedRecord.house.Id)
            //    .Where(x => x.StructuralElement.Id == addedRecord.structuralElement.Id)
            //    .Where(x => !x.State.FinalState)
            //    .Where(x => x.StructuralElement.Group.CommonEstateObject != null)
            //    .First();

            return new VersionRecordStage1
            {
                Stage2Version = stage2,
                RealityObject = addedRecord.house,
                StructuralElement = housecr,
                Year = addedRecord.year,
                Sum = stage2.Sum,
                StateChangeDate = DateTime.Now,
                VersionRecordState = VersionRecordState.Actual,
                Volume = 0
            };
        }

        /// <summary>
        /// Получить год следующего кап.ремонта, не входящего в currentYears (если они заданы)
        /// </summary>
        private short CorrectWorkYear(RealityObjectStructuralElement element, short startYear, IEnumerable<int> currentYears)
        {
            bool isRepaired = element.Repaired;
            if (element.RealityObject.Id == 15665)
            {
                string str = "";
            }
            short startDate = (short)(element.RealityObject.DateCommissioning.HasValue ? element.RealityObject.DateCommissioning.Value.Year : (element.RealityObject.BuildYear.HasValue ? element.RealityObject.BuildYear.Value : 1900));
            //год начала отсчета 
            short year = startDate;
            if (element.LastOverhaulYear >= year || isRepaired)
            {
                year = (short)element.LastOverhaulYear;
            }
               

            if (element.StructuralElement.LifeTimeAfterRepair == 0)
                element.StructuralElement.LifeTimeAfterRepair = element.StructuralElement.LifeTime;

            while (year < startYear || (currentYears!= null && currentYears.Contains(year)))
            {
                if (!isRepaired)
                {
                    if (element.StructuralElement.LifeTime == 0)
                    {
                        //Писать в лог такие дома
                        return year;
                    }
                    if (year + element.StructuralElement.LifeTime < startYear)
                    {
                        year = startYear;
                    }
                    else
                    {
                        year += (short)element.StructuralElement.LifeTime;
                    }
                  
                    isRepaired = true;
                }
                else
                {
                    year += (short)element.StructuralElement.LifeTimeAfterRepair;
                }
            }

            return year;
        }

        /// <summary>
        /// Проверяет активные дома в версии
        /// </summary>
        private List<VersionRecordWithReasonDelete> AddHousesToDelete(ProgramVersion version, short startYear)
        {
            //получаем все записи домов в версии
            var versionRecords = Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id)
                .Where(x => x.Stage2Version.Stage3Version.Show)
                //.Where(x => x.VersionRecordState != VersionRecordState.NonActual)
                .Select(x => new VersionRecordWithReasonDelete
                {
                    Id = x.Stage2Version.Stage3Version.Id,
                    VersionRecordStage1 = x,
                    //для вывода
                    house = x.RealityObject,
                    structuralElement = x.StructuralElement.StructuralElement,
                    commonEstateObject = x.StructuralElement.StructuralElement.Group.CommonEstateObject,
                    year = (short)x.Year,
                    //для фильтрации
                    structuralElementStateId = x.StructuralElement.State.Id,
                    structElementCount = (RealityObjectStructuralElementDomain.GetAll().Where(y=> y.RealityObject.Id == x.RealityObject.Id).Count()),
                    yearRepair = TypeWorkCrDomain.GetAll().Where(y => y.ObjectCr.RealityObject.Id == x.RealityObject.Id).Select(y => y.YearRepair).FirstOrDefault()
                }).OrderBy(x=> x.house).ToList();

            //получаем все активные фильтры
            var now = DateTime.Now;
            var actualCriterias = DPKRActualCriteriasDomain.GetAll().Where(x => x.DateStart <= now && x.DateEnd >= now);

            //применяем фильтры
            foreach (var criterias in actualCriterias)
            {
                versionRecords = ApplyCriteriasInverted(versionRecords, criterias);
            }

            return versionRecords;
        }

        /// <summary>
        /// Проверяет записи домов по фильтру и формирует причину непрохождения
        /// </summary>
        private List<VersionRecordWithReasonDelete> ApplyCriteriasInverted(List<VersionRecordWithReasonDelete> versionRecords, DPKRActualCriterias criterias)
        {
            // Допустимые статусы
            if (criterias.Status != null)
                versionRecords.Where(x => x.house.State.Id != criterias.Status.Id)
                              .ForEach(x => { x.Passed = false; x.Reasons += $"cтатус дома не соответствует {criterias.Status.Name}; "; });

            // Допустимый тип дома
            if (criterias.TypeHouse != Enums.TypeHouse.NotSet)
                versionRecords.Where(x => x.house.TypeHouse != criterias.TypeHouse)
                .ForEach(x => { x.Passed = false; x.Reasons += $"тип дома не соответствует {EnumToTextHelper.TypeHouseToString(criterias.TypeHouse)}; "; });

            // Допустимое состояние дома
            if (criterias.ConditionHouse != Enums.ConditionHouse.NotSelected)
                versionRecords.Where(x => x.house.ConditionHouse != criterias.ConditionHouse)
                    .ForEach(x => { x.Passed = false; x.Reasons += $"состояние дома не соответствует {EnumToTextHelper.ConditionHouseToString(criterias.ConditionHouse)}; "; });

            // Количество квартир
            if (criterias.IsNumberApartments)
                switch (criterias.NumberApartmentsCondition)
                {
                    case Condition.Lower:
                        versionRecords.Where(x => x.house.NumberApartments >= criterias.NumberApartments)
                            .ForEach(x => { x.Passed = false; x.Reasons += $"Количество квартир больше или равно {criterias.NumberApartments}; "; });
                        break;
                    case Condition.Equal:
                        versionRecords.Where(x => x.house.NumberApartments != criterias.NumberApartments)
                            .ForEach(x => { x.Passed = false; x.Reasons += $"Количество квартир не равно {criterias.NumberApartments}; "; });
                        break;
                    case Condition.Greater:
                        versionRecords.Where(x => x.house.NumberApartments <= criterias.NumberApartments)
                            .ForEach(x => { x.Passed = false; x.Reasons += $"Количество квартир меньше или равно {criterias.NumberApartments}; "; });
                        break;
                }

            // Год последнего капитального ремонта
            if (criterias.IsYearRepair)
                switch (criterias.YearRepairCondition)
                {
                    case Condition.Lower:
                        versionRecords.Where(x => x.yearRepair.HasValue && x.yearRepair.Value >= criterias.YearRepair)
                            .ForEach(x =>
                            {
                                x.Passed = false;
                                x.Reasons += $"Год последнего капитального ремонта старше или равен {criterias.YearRepair}; ";
                            });
                        break;
                    case Condition.Equal:
                        versionRecords.Where(x => x.yearRepair != criterias.YearRepair)
                            .ForEach(x =>
                            {
                                x.Passed = false;
                                x.Reasons += $"Год последнего капитального ремонта не равен {criterias.YearRepair}; ";
                            });
                        break;
                    case Condition.Greater:
                        versionRecords.Where(x => x.yearRepair.HasValue && x.yearRepair.Value >= criterias.YearRepair)
                            .ForEach(x =>
                            {
                                x.Passed = false;
                                x.Reasons += $"Год последнего капитального ремонта младше или равен {criterias.YearRepair}; ";
                            });
                        break;
                }

            // Учитывать признак «Ремонт не целесообразен»
            if (criterias.CheckRepairAdvisable)
                versionRecords.Where(x => x.house.IsRepairInadvisable)
                    .ForEach(x => { x.Passed = false; x.Reasons += $"Ремонт не целесообразен; "; });

            // Учитывать признак «Дом не участвует в КР»
            if (criterias.CheckInvolvedCr)
                versionRecords.Where(x => x.house.IsNotInvolvedCr)
                    .ForEach(x => { x.Passed = false; x.Reasons += $"Дом не участвует в КР; "; });

            // Количество КЭ
            if (criterias.IsStructuralElementCount)
            {
                switch (criterias.StructuralElementCountCondition)
                {
                    case Condition.Lower:
                        versionRecords.Where(x => x.structElementCount >= criterias.StructuralElementCount)
                            .ForEach(x => { x.Passed = false; x.Reasons += $"Количество КЭ больше или равно {criterias.StructuralElementCount}; "; });
                        break;
                    case Condition.Equal:
                        versionRecords.Where(x => x.structElementCount != criterias.StructuralElementCount)
                            .ForEach(x => { x.Passed = false; x.Reasons += $"Количество КЭ не равно {criterias.StructuralElementCount}; "; });
                        break;
                    case Condition.Greater:
                        versionRecords.Where(x => x.structElementCount <= criterias.StructuralElementCount)
                            .ForEach(x => { x.Passed = false; x.Reasons += $"Количество КЭ меньше или равно {criterias.StructuralElementCount}; "; });
                        break;
                }
            }

            // Допустимый статус КЭ
            if (criterias.SEStatus != null)
                versionRecords.Where(x => criterias.SEStatus.Id != x.structuralElementStateId)
                              .ForEach(x => { x.Passed = false; x.Reasons += $"cтатус КЭ не соответствует {criterias.SEStatus.Name}; "; });

            return versionRecords;
        }

        /// <summary>
        /// Отбирает дома, подходящие под условия
        /// Чистка списка быстрее, чем поэлементная
        /// </summary>
        private IQueryable<RealityObjectStructuralElement> ApplyCriterias(IQueryable<RealityObjectStructuralElement> records, DPKRActualCriterias criterias)
        {
            // Допустимые статусы
            if (criterias.Status != null)
                records = records.Where(x => criterias.Status.Id == x.RealityObject.State.Id);
            if (criterias.SEStatus != null)
                records = records.Where(x => criterias.SEStatus.Id == x.State.Id);

            // Допустимый тип дома
            if (criterias.TypeHouse != Enums.TypeHouse.NotSet)
                records = records.Where(x => x.RealityObject.TypeHouse == criterias.TypeHouse);

            // Допустимое состояние дома
            if (criterias.ConditionHouse != Enums.ConditionHouse.NotSelected)
                records = records.Where(x => x.RealityObject.ConditionHouse == criterias.ConditionHouse);

            // Количество квартир
            if (criterias.IsNumberApartments)
                switch (criterias.NumberApartmentsCondition)
                {
                    case Condition.Lower:
                        records = records.Where(x => x.RealityObject.NumberApartments < criterias.NumberApartments);
                        break;
                    case Condition.Equal:
                        records = records.Where(x => x.RealityObject.NumberApartments == criterias.NumberApartments);
                        break;
                    case Condition.Greater:
                        records = records.Where(x => x.RealityObject.NumberApartments > criterias.NumberApartments);
                        break;
                }

            // Учитывать признак «Ремонт не целесообразен»
            if (criterias.CheckRepairAdvisable)
                records = records.Where(x => !x.RealityObject.IsRepairInadvisable);

            // Учитывать признак «Дом не участвует в КР»
            if (criterias.CheckInvolvedCr)
                records = records.Where(x => !x.RealityObject.IsNotInvolvedCr);

            return records;
        }

        /// <summary>
        /// Отбирает записи програм, подходящие под условия
        /// </summary>
        private IQueryable<VersionRecordStage1> ApplyCriterias(IQueryable<VersionRecordStage1> records, DPKRActualCriterias criterias)
        {
            // Допустимые статусы
            if (criterias.Status != null)
                records = records.Where(x => criterias.Status.Id == x.RealityObject.State.Id);

            // Допустимый тип дома
            if (criterias.TypeHouse != Enums.TypeHouse.NotSet)
                records = records.Where(x => x.RealityObject.TypeHouse == criterias.TypeHouse);

            // Допустимое состояние дома
            if (criterias.ConditionHouse != Enums.ConditionHouse.NotSelected)
                records = records.Where(x => x.RealityObject.ConditionHouse == criterias.ConditionHouse);

            // Количество квартир
            if (criterias.IsNumberApartments)
                switch (criterias.NumberApartmentsCondition)
                {
                    case Condition.Lower:
                        records = records.Where(x => x.RealityObject.NumberApartments < criterias.NumberApartments);
                        break;
                    case Condition.Equal:
                        records = records.Where(x => x.RealityObject.NumberApartments == criterias.NumberApartments);
                        break;
                    case Condition.Greater:
                        records = records.Where(x => x.RealityObject.NumberApartments > criterias.NumberApartments);
                        break;
                }

            //статус КЭ
            if (criterias.SEStatus!= null)
                records = records.Where(x => criterias.SEStatus.Id == x.StructuralElement.State.Id);

            // Учитывать признак «Ремонт не целесообразен»
            if (criterias.CheckRepairAdvisable)
                records = records.Where(x => !x.RealityObject.IsRepairInadvisable);

            // Учитывать признак «Дом не участвует в КР»
            if (criterias.CheckInvolvedCr)
                records = records.Where(x => !x.RealityObject.IsNotInvolvedCr);

            return records;
        }

        /// <summary>
        /// Отбирает дома, подходящие под условия, которые нельзя описать NHibernate
        /// </summary>
        private IEnumerable<RealityObjectStructuralElement> ApplySoftCriterias(IEnumerable<RealityObjectStructuralElement> records, DPKRActualCriterias criterias)
        {
            // Год последнего капитального ремонта
            if (criterias.IsYearRepair)
                switch (criterias.YearRepairCondition)
                {
                    case Condition.Lower:
                        records = records.Where(x => GetYearRepair(x.RealityObject) < criterias.YearRepair);
                        break;
                    case Condition.Equal:
                        records = records.Where(x => GetYearRepair(x.RealityObject) == criterias.YearRepair);
                        break;
                    case Condition.Greater:
                        records = records.Where(x => GetYearRepair(x.RealityObject) > criterias.YearRepair);
                        break;
                }

            // Количество КЭ
            if (criterias.IsStructuralElementCount)
            {
                switch (criterias.StructuralElementCountCondition)
                {
                    case Condition.Lower:
                        records = records.Where(x => GetStructElementCount(x.RealityObject) < criterias.StructuralElementCount);
                        break;
                    case Condition.Equal:
                        records = records.Where(x => GetStructElementCount(x.RealityObject) == criterias.StructuralElementCount);
                        break;
                    case Condition.Greater:
                        records = records.Where(x => GetStructElementCount(x.RealityObject) > criterias.StructuralElementCount);
                        break;
                }
            }

            return records;
        }

        /// <summary>
        /// Отбирает записи, подходящие под условия, которые нельзя описать NHibernate
        /// </summary>
        private IEnumerable<VersionRecordStage1> ApplySoftCriterias(IEnumerable<VersionRecordStage1> records, DPKRActualCriterias criterias)
        {
            // Год последнего капитального ремонта
            if (criterias.IsYearRepair)
                switch (criterias.YearRepairCondition)
                {
                    case Condition.Lower:
                        records = records.Where(x => GetYearRepair(x.RealityObject) < criterias.YearRepair);
                        break;
                    case Condition.Equal:
                        records = records.Where(x => GetYearRepair(x.RealityObject) == criterias.YearRepair);
                        break;
                    case Condition.Greater:
                        records = records.Where(x => GetYearRepair(x.RealityObject) > criterias.YearRepair);
                        break;
                }

            // Количество КЭ
            if (criterias.IsStructuralElementCount)
            {
                switch (criterias.StructuralElementCountCondition)
                {
                    case Condition.Lower:
                        records = records.Where(x => GetStructElementCount(x.RealityObject) < criterias.StructuralElementCount);
                        break;
                    case Condition.Equal:
                        records = records.Where(x => GetStructElementCount(x.RealityObject) == criterias.StructuralElementCount);
                        break;
                    case Condition.Greater:
                        records = records.Where(x => GetStructElementCount(x.RealityObject) > criterias.StructuralElementCount);
                        break;
                }
            }

            return records;
        }

        private int GetYearRepair(RealityObject house)
        {
            return TypeWorkCrDomain.GetAll().Where(x => x.ObjectCr.RealityObject == house).Max(x => x.YearRepair) ?? 0;
        }

        private int GetStructElementCount(RealityObject house)
        {
            return RealityObjectStructuralElementDomain.GetAll().Where(x => x.RealityObject == house).Count();
        }

        /// <summary>
        /// Приведение в порядок stage3 после правок stage1
        /// </summary>
        /// <param name="record"></param>
        private void ActualizeStage3(VersionRecord record)
        {
            var stages1 = Stage1Domain.GetAll().Where(x => x.Stage2Version.Stage3Version.Id == record.Id);

            bool show = false;

            stages1.ForEach(x =>
            {
                if (x.VersionRecordState == VersionRecordState.Actual)
                    show = true;
            });

            record.Show = show;
            record.CommonEstateObjects = stages1.Select(x => x.StructuralElement.StructuralElement.Group.CommonEstateObject.Name).Distinct().AggregateWithSeparator(", ");

            VersionRecordDomain.Update(record);
        }
        #endregion

        /// <summary>
        /// Запись в программе для вывода в списки фильтрации всяких актуализаций КР
        /// </summary>
        private class VersionRecordWithReasonAdd
        {
            static int nextid = 0;

            internal VersionRecordWithReasonAdd(RealityObject house, StructuralElement structuralElement, CommonEstateObject commonEstateObject, short year, RealityObjectStructuralElement rose)
            {
                Id = nextid++;

                this.house = house;
                this.structuralElement = structuralElement;
                this.commonEstateObject = commonEstateObject;
                this.year = year;
                this.rose = rose;

                NeedToSave = true;
            }

            internal VersionRecordWithReasonAdd(VersionRecordStage1 versionRecord, short year)
            {
                Id = nextid++;

                this.versionRecordStage1 = versionRecord;
                this.house = versionRecord.RealityObject;
                this.structuralElement = versionRecord.StructuralElement.StructuralElement;
                this.commonEstateObject = versionRecord.StructuralElement.StructuralElement.Group.CommonEstateObject;
                this.year = year;
                this.rose = versionRecord.StructuralElement;

                NeedToSave = false;
            }

            /// <summary>
            /// Это не id сущности в базе!!! Это только для фронта
            /// </summary>
            internal int Id;

            /// <summary>
            /// Нужно ли создать новый stage1?
            /// </summary>
            internal bool NeedToSave = false;

            /// <summary>
            /// Запись в программе - если NeedToSave = false
            /// </summary>
            internal VersionRecordStage1 versionRecordStage1;

            internal RealityObject house;

            internal StructuralElement structuralElement;

            internal CommonEstateObject commonEstateObject;

            internal short year;

            internal RealityObjectStructuralElement rose;

            internal bool Disable = false;
        }

        /// <summary>
        /// Запись в программе для вывода в списки фильтрации всяких актуализаций КР
        /// </summary>
        public class VersionRecordWithReasonDelete
        {
            static int nextid = 0;

            internal VersionRecordWithReasonDelete()
            {
              //  Id = nextid++;
            }

            /// <summary>
            /// Это не id сущности в базе!!! Это только для фронта
            /// </summary>
            internal long Id;

            /// <summary>
            /// Запись в программе
            /// </summary>
            internal VersionRecordStage1 VersionRecordStage1;

            internal RealityObject house;

            internal StructuralElement structuralElement;

            internal CommonEstateObject commonEstateObject;

            internal short year;

            /// <summary>
            /// Прошел ли условия?
            /// </summary>
            internal bool Passed = true;

            /// <summary>
            /// Причины непрохождения условий
            /// </summary>
            internal string Reasons = "";

            internal bool Disable = false;

            //кэш для ускорения отбора
            internal long structuralElementStateId;
            internal int structElementCount;
            internal int? yearRepair;
        }

        public class VersionRecordWithReasonView
        {
            public long Id { get; set; }

            public string Address { get; set; }

            public int Year { get; set; }

            public string Reasons { get; set; }

            public string StructuralElement { get; set; }

            public string CommonEstateObject { get; set; }

            public string RoseName { get; set; }
        }

        private class CacheRecord
        {
            public long RoId { get; set; }
            public int Year { get; set; }
        }
    }
}
