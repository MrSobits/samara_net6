using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Entities.CommonEstateObject;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.ConfigSections;
using Bars.GkhCr.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Services.ActualizeSubDPKR
{
    public class ActualizeSubDPKRService : IActualizeSubrecordService
    {

        #region Properties

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<SubProgramCriterias> SubProgramCriteriasDomain { get; set; }

        public IRepository<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public IDomainService<VersionRecordStage2> VersionRecordStage2Domain { get; set; }

        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> stage1Service { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage2> stage2Service { get; set; }

        public IDomainService<StructuralElementGroup> StructuralElementGroupDomain { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> RealityObjectStructuralElementInProgrammStage3Domain { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// Список домов на добавление
        /// </summary>
        List<HouseWithReason> housesToAdd = null;

        long CachedVersionIdAdd = 0;

        /// <summary>
        /// Список домов на удаление
        /// </summary>
        List<HouseWithReason> housesToDelete = null;

        long CachedVersionIdDelete = 0;

        #endregion

        #region Public methods

        public List<HouseWithReasonView> GetAddEntriesList(ProgramVersion version)
        {
            ActualizeAddList(version);

            return housesToAdd.Select(x => new HouseWithReasonView
            {
                Id = x.RealityObject.Id,
                Address = x.RealityObject.Address,
                Reasons = x.Reasons
            }).ToList();
        }

        public List<HouseWithReasonView> GetDeleteEntriesList(ProgramVersion version)
        {
            ActualizeDeleteList(version);

            return housesToDelete.Select(x => new HouseWithReasonView
            {
                Id = x.RealityObject.Id,
                Address = x.RealityObject.Address,
                Reasons = x.Reasons
            }).ToList();
        }

        public void RemoveHouseForAdd(long houseId)
        {
            housesToAdd.RemoveAll(x => x.RealityObject.Id == houseId);
        }

        public void RemoveHouseForDelete(long houseId)
        {
            housesToDelete.RemoveAll(x => x.RealityObject.Id == houseId);
        }

        public void ClearCache()
        {
            housesToAdd = null;
            housesToDelete = null;
        }
        //Старый метод, комментим. ToDo: Это будет функционал регистрации
        /*
        public void Actualize(ProgramVersion version)
        {
            if (housesToAdd == null)
                housesToAdd = MakeAddList(version);

            if (housesToDelete == null)
                housesToDelete = MakeDeleteList(version);

            //удаляем флаг у всех домов, которые в списке на удаление
            housesToDelete.Select(x => x.RealityObject).ForEach(x =>
            {
                x.IsSubProgram = false;
                RealityObjectDomain.Update(x);
            });

            //ставим флаг тем домам, которые в списке на добавление
            housesToAdd.Select(x => x.RealityObject).ForEach(x =>
                 {
                     x.IsSubProgram = true;
                     RealityObjectDomain.Update(x);

                     //при добавлении дома исключаются из основной
                     VersionRecordStage1Domain.GetAll().Where(y => y.RealityObject.Id == x.Id).Where(y => y.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id).ForEach(y =>
                     {
                         if (y.VersionRecordState == Enum.VersionRecordState.Actual)
                         {
                             y.VersionRecordState = Enum.VersionRecordState.NonActual;
                             VersionRecordStage1Domain.Save(y);
                         }
                     });

                     var stage3 = RealityObjectStructuralElementInProgrammStage3Domain.GetAll()
                       .Where(y => y.RealityObject.Id == x.Id)
                       .Select(y => y.Id).ToList();

                     if (stage3 == null || stage3.Count == 0)
                     {
                         //VersionRecordStage1Domain.GetAll().Where(y => y.RealityObject.Id == x.Id).Where(y => y.Stage2Version.Stage3Version.ProgramVersion.Id == version.Id).ForEach(y =>
                         //{
                         //    var newstage3 = new RealityObjectStructuralElementInProgrammStage3
                         //    {
                         //        RealityObject = new RealityObject { Id = x.Id, Address = x.Address },
                         //        Year = 2044,
                         //        Sum = y.Stage2Version.Stage3Version.Sum,
                         //        CommonEstateObjects = y.Stage2Version.Stage3Version.CommonEstateObjects
                         //    };
                         //    RealityObjectStructuralElementInProgrammStage3Domain.Save(newstage3);
                         //});

                         var verRecords = VersionRecordDomain.GetAll()
                         .Where(y => y.RealityObject.Id == x.Id)
                         .Where(y => y.ProgramVersion.Id == version.Id)
                         .Where(y => y.Year > 2020)
                         .Where(y => y.Show == true).ToList();
                         foreach(VersionRecord verRec in verRecords)
                         {
                             var newstage3 = new RealityObjectStructuralElementInProgrammStage3
                             {
                                 RealityObject = new RealityObject { Id = x.Id, Address = x.Address },
                                 Year = 2044,
                                 Sum = verRec.Sum,
                                 CommonEstateObjects = verRec.CommonEstateObjects
                             };
                             RealityObjectStructuralElementInProgrammStage3Domain.Save(newstage3);
                             var vrst1 = VersionRecordStage1Domain.GetAll()
                             .Where(z => z.RealityObject == x)
                             .Where(z => z.Stage2Version.Stage3Version == verRec)
                             .Select(z=> z.StructuralElement).FirstOrDefault();
                             var datast2 = stage1Service.GetAll()
                             .Where(z => z.Stage2.RealityObject.Id == x.Id)
                             .Where(z=> z.StructuralElement == vrst1)
                             .Select(z=> z.Stage2).FirstOrDefault();
                             var ceo = vrst1.StructuralElement.Group.CommonEstateObject;
                             var stage1 = stage1Service.GetAll()
                             .Where(z => z.Stage2.RealityObject.Id == x.Id)
                             .Where(z => z.StructuralElement == vrst1)
                             .FirstOrDefault();
                             if (datast2 == null)
                             {
                                 var stage2 = new RealityObjectStructuralElementInProgrammStage2
                                 {
                                     StructuralElements = vrst1.StructuralElement.Name,
                                     CommonEstateObject = ceo,
                                      Year = 2044,
                                     ObjectVersion = 13,
                                     ObjectCreateDate = DateTime.Now,
                                     ObjectEditDate = DateTime.Now,
                                     RealityObject = new RealityObject { Id = x.Id, Address = x.Address },
                                     Sum = verRec.Sum,
                                     Stage3 = newstage3
                                 };
                                 stage2Service.Save(stage2);
                                 if (stage1 == null)
                                 {
                                     var newStage1 = new RealityObjectStructuralElementInProgramm
                                     {
                                         StructuralElement = vrst1,
                                         Stage2 = stage2,
                                         Year = 2044,
                                         Sum = verRec.Sum,
                                         ObjectCreateDate = DateTime.Now,
                                         ObjectEditDate = DateTime.Now,
                                         ObjectVersion = 13,
                                         ServiceCost = 0m
                                     };
                                     stage1Service.Save(newStage1);
                                 }
                                 else
                                 {
                                     stage1.Stage2 = stage2;
                                     stage1Service.Update(stage1);
                                 }

                               
                             }
                             else
                             {
                                 datast2.Stage3 = newstage3;
                                 stage2Service.Update(datast2);
                             }
                             

                         }
                     }
                     else
                     {
                         //выставить 44 год всем работам в подпрограмме
                         RealityObjectStructuralElementInProgrammStage3Domain.GetAll()
                             .Where(y => y.RealityObject.Id == x.Id)
                               .ForEach(y =>
                               {
                                   y.Year = 2044;
                                   RealityObjectStructuralElementInProgrammStage3Domain.Save(y);
                               });
                     }

                   

                     VersionRecordDomain.GetAll()
                     .Where(y => y.RealityObject.Id == x.Id)
                     .Where(y=> version.Id == y.ProgramVersion.Id)
                          .Where(y => y.Year > 2020)
                         .Where(y => y.Show == true)
                           .ForEach(y =>
                           {
                               y.Year = 2044;
                               y.Show = false;
                               VersionRecordDomain.Update(y);
                           });

                     //скрываем записи, которые включили в подпрограмму
                     //VersionRecordDomain.GetAll()
                     //.Where(z => z.ProgramVersion == version)
                     //.Where(z => z.RealityObject.Id == x.Id)
                     //.ForEach(z =>
                     //{
                     //    z.Show = false;
                     //    VersionRecordDomain.Update(z);
                     //});
                 });

            ClearCache();
        }
        */

        public void Actualize(ProgramVersion version, int endYear)
        {          

            if (housesToAdd == null)
                housesToAdd = MakeAddList(version);

            if (housesToDelete == null)
                housesToDelete = MakeDeleteList(version);

            //удаляем флаг у всех домов, которые в списке на удаление
            housesToDelete.Select(x => x.RealityObject).ForEach(x =>
            {
                x.IsSubProgram = false;
                RealityObjectDomain.Update(x);
            });

            //ставим флаг тем домам, которые в списке на добавление
            housesToAdd.Select(x => x.RealityObject).ForEach(x =>
            {
                x.IsSubProgram = true;
                RealityObjectDomain.Update(x);

                //при добавлении дома исключаются из основной
                         
                VersionRecordDomain.GetAll()
                .Where(y => y.RealityObject.Id == x.Id)
                .Where(y => version.Id == y.ProgramVersion.Id)
                     .Where(y => y.Year > DateTime.Now.Year)
                    .Where(y => y.Show == true)
                      .ForEach(y =>
                      {
                          y.Year = 2050;
                      //    y.Year = endYear;
                          y.SubProgram = true;
                          VersionRecordDomain.Update(y);
                      });

            });

            housesToDelete.Select(x => x.RealityObject).ForEach(x =>
            {
                VersionRecordDomain.GetAll()
               .Where(y => y.RealityObject.Id == x.Id)
               .Where(y => version.Id == y.ProgramVersion.Id)
                    .Where(y => y.Year > DateTime.Now.Year)
                   .Where(y => y.Show == true)
                     .ForEach(y =>
                     {
                         y.Year = endYear;
                         y.SubProgram = false;
                         VersionRecordDomain.Update(y);
                     });
            });

                ClearCache();
        }

        public void RemoveSelected(ProgramVersion version, long[] selectedAddId, long[] selectedDeleteId)
        {
            if (housesToDelete != null)
                foreach (long id in selectedDeleteId)
                {
                    housesToDelete.RemoveAll(x => x.RealityObject.Id == id);
                }

            if (housesToAdd != null)
                foreach (long id in selectedAddId)
                {
                    housesToAdd.RemoveAll(x => x.RealityObject.Id == id);
                }
        }

        #endregion

        #region Private methods

        private void ActualizeAddList(ProgramVersion version)
        {
            if (housesToAdd == null || CachedVersionIdAdd != version.Id)
            {
                housesToAdd = MakeAddList(version);
                CachedVersionIdAdd = version.Id;
            }
        }

        private void ActualizeDeleteList(ProgramVersion version)
        {
            if (housesToDelete == null || CachedVersionIdDelete != version.Id)
            {
                housesToDelete = MakeDeleteList(version);
                CachedVersionIdDelete = version.Id;
            }
        }

        private List<HouseWithReason> MakeAddList(ProgramVersion version)
        {
            var houseRecord = new List<HouseWithReason>();

            //получаем все еще не включенные дома
            var houses = VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion == version)
                .Where(x => !x.RealityObject.IsSubProgram)
                .Select(x => x.RealityObject)
                .ToList()
                .Distinct(x => x.Id)
                .ToList();

            //выбираем те, у который стоит галочка: включить в подпрограмму           
            var flagHouses = houses.Where(x => x.IncludeToSubProgram).ToList();

            flagHouses.ForEach(x =>
                {
                    houseRecord.Add(new HouseWithReason
                    {
                        RealityObject = x,
                        Reasons = "Стоит флаг: включить в подпрограмму"
                    });

                    houses.Remove(x);
                });

            //добавляем те, которые удовлетворяют условию
            foreach (var criterias in SubProgramCriteriasDomain.GetAll())
            {
                houses.Where(x => IsCriteriasSatisfy(x, criterias))
                .ForEach(x =>
                {
                    houseRecord.Add(new HouseWithReason
                    {
                        RealityObject = x,
                        Reasons = $"Удовлетворяет критерию {criterias.Name}"
                    });
                });
            }

            return houseRecord;
        }

        private List<HouseWithReason> MakeDeleteList(ProgramVersion version)
        {
            //получаем все включенные дома
            var list = VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion == version)
                .Where(x => x.RealityObject.IsSubProgram && !x.RealityObject.IncludeToSubProgram) //принудительно включаемые не обязаны проходить условия
                .Select(x => x.RealityObject)                
                .ToList()
                .Distinct(x => x.Id)
                .ToList();

            return list.Where(x => !IsCriteriasSatisfy(x, SubProgramCriteriasDomain.GetAll().ToList()))
                .Select(x => new HouseWithReason
                {
                    RealityObject = x,
                    Passed = false,
                    Reasons = $"Не удовлетворяет ни одному критерию"
                })
                .ToList();
        }

        private bool IsCriteriasSatisfy(RealityObject house, List<SubProgramCriterias> list)
        {
            foreach (var criterias in list)
            {
                if (!IsCriteriasSatisfy(house, criterias))
                    return false;
            }

            return true;
        }

        private bool IsCriteriasSatisfy(RealityObject house, SubProgramCriterias criterias)
        {
            if (criterias.IsStatusUsed && criterias.Status.Id != house.State.Id)
                return false;

            if (criterias.IsTypeHouseUsed && criterias.TypeHouse != house.TypeHouse)
                return false;

            if (criterias.IsConditionHouseUsed && criterias.ConditionHouse != house.ConditionHouse)
                return false;

            if (criterias.IsNumberApartmentsUsed)
            {
                if (house.NumberApartments != null)
                    switch (criterias.NumberApartmentsCondition)
                    {
                        case Enum.Condition.Lower:
                            if (house.NumberApartments >= criterias.NumberApartments)
                                return false;
                            break;
                        case Enum.Condition.Equal:
                            if (house.NumberApartments != criterias.NumberApartments)
                                return false;
                            break;
                        case Enum.Condition.Greater:
                            if (house.NumberApartments <= criterias.NumberApartments)
                                return false;
                            break;
                    }
            }

            if (criterias.IsYearRepairUsed)
            {
                int? lastYearRepair = GetLastYearRepair(house);

                if (lastYearRepair != null)
                    switch (criterias.YearRepairCondition)
                    {
                        case Enum.Condition.Lower:
                            if (lastYearRepair >= criterias.YearRepair)
                                return false;
                            break;
                        case Enum.Condition.Equal:
                            if (lastYearRepair != criterias.YearRepair)
                                return false;
                            break;
                        case Enum.Condition.Greater:
                            if (lastYearRepair <= criterias.YearRepair)
                                return false;
                            break;
                    }
            }

            if (criterias.IsRepairNotAdvisableUsed && criterias.RepairNotAdvisable != house.IsRepairInadvisable)
                return false;

            if (criterias.IsNotInvolvedCrUsed && criterias.NotInvolvedCr != house.IsNotInvolvedCr)
                return false;

            if (criterias.IsStructuralElementCountUsed)
            {
                switch (criterias.StructuralElementCountCondition)
                {
                    case Enum.Condition.Lower:
                        if (GetStructElementCount(house) >= criterias.StructuralElementCount)
                            return false;
                        break;
                    case Enum.Condition.Equal:
                        if (GetStructElementCount(house) != criterias.StructuralElementCount)
                            return false;
                        break;
                    case Enum.Condition.Greater:
                        if (GetStructElementCount(house) <= criterias.StructuralElementCount)
                            return false;
                        break;
                }
            }

            if (criterias.IsFloorCountUsed)
            {
                if (house.MaximumFloors != null)
                    switch (criterias.FloorCountCondition)
                    {
                        case Enum.Condition.Lower:
                            if (house.MaximumFloors >= criterias.FloorCount)
                                return false;
                            break;
                        case Enum.Condition.Equal:
                            if (house.MaximumFloors != criterias.FloorCount)
                                return false;
                            break;
                        case Enum.Condition.Greater:
                            if (house.MaximumFloors <= criterias.FloorCount)
                                return false;
                            break;
                    }
            }

            if (criterias.IsLifetimeUsed)
            {
                short? lifetime = GetLifeTime(house);

                if (lifetime != null)
                    switch (criterias.LifetimeCondition)
                    {
                        case Enum.Condition.Lower:
                            if (lifetime >= criterias.Lifetime)
                                return false;
                            break;
                        case Enum.Condition.Equal:
                            if (lifetime != criterias.Lifetime)
                                return false;
                            break;
                        case Enum.Condition.Greater:
                            if (lifetime <= criterias.Lifetime)
                                return false;
                            break;
                    }
            }

            return true;
        }

        private short? GetLifeTime(RealityObject house)
        {
            if (house.DateCommissioning == null)
                return null;

            var YearCommissioning = house.DateCommissioning.Value.Year;            

            //есть куча домов, где первые две цифры года 10 - сделать коррекцию
            if (YearCommissioning < 1100)
                YearCommissioning = YearCommissioning % 100 + 1900; //TODO: подумать, что делать, если они с 2000

            var lifeTime = DateTime.Now.Year - YearCommissioning;          
            
            if (lifeTime > 65535)
                throw new ApplicationException($"Срок службы дома больше 65535 лет");

            return (short)(lifeTime);
        }

        private int GetStructElementCount(RealityObject house)
        {
            return RealityObjectStructuralElementDomain.GetAll().Where(x => x.RealityObject == house).Count();
        }

        private int? GetLastYearRepair(RealityObject house)
        {
            return TypeWorkCrDomain.GetAll().Where(x => x.ObjectCr.RealityObject == house).Max(x => x.YearRepair);
        }

        #endregion

        #region Nested classes

        /// <summary>
        /// Дом для вывода в списки фильтрации всяких актуализаций КР
        /// </summary>
        public class HouseWithReason
        {
            /// <summary>
            /// Дом
            /// </summary>
            public RealityObject RealityObject;

            /// <summary>
            /// Прошел ли условия?
            /// </summary>
            public bool Passed = true;

            /// <summary>
            /// Причины непрохождения условий
            /// </summary>
            public string Reasons = "";
        }

        public class HouseWithReasonView
        {
            public long Id { get; set; }

            public string Address { get; set; }

            public string Reasons { get; set; }
        }

        #endregion
    }
}
