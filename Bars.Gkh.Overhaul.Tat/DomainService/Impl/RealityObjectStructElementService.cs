namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Castle.Windsor;
    using Overhaul.Entities;

    public class RealityObjectStructElementService : IRealObjStructElementService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElement> DomainService { get; set; }

        public IDomainService<EmergencyObject> EmerObjectDomainService { get; set; }

        public IDataResult List(BaseParams baseParams)
        {
            var domainService = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            var loadParam = baseParams.GetLoadParam();

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    x.StructuralElement.Name,
                    Group = x.StructuralElement.Group.Name,
                    Object = x.StructuralElement.Group.CommonEstateObject.Name,
                    x.LastOverhaulYear,
                    x.Volume
                })
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }

        public IDataResult ListRoForMassDelete(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var serviceRoSe = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            var data = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => serviceRoSe.GetAll().Any(y => y.RealityObject.Id == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    Municipality = x.Municipality.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Address)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IQueryable<RealityObjectStructuralElement> GetUsedInLongProgram()
        {
            return DomainService.GetAll()
                .Where(x => x.State.StartState) // Попросили по требованию 57368
                .Where(x => !EmerObjectDomainService.GetAll().Any(e => e.RealityObject.Id == x.RealityObject.Id)
                            || EmerObjectDomainService.GetAll()
                                .Where(e => e.RealityObject.Id == x.RealityObject.Id)
                                .Any(e => e.ConditionHouse == ConditionHouse.Serviceable 
                                            || e.ConditionHouse == ConditionHouse.Dilapidated))
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.BlockedBuilding)
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.Individual)
                .Where(x => x.RealityObject.TypeHouse != TypeHouse.SocialBehavior)
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable
                            || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated)
                // Если у дома есть галочка "Дом не участвует в программе КР" то не берем этот дом
                .Where(x => !x.RealityObject.IsNotInvolvedCr);
        }

        //средняя производительность: на 2,5к записей тратится примерно от 7 до 11 секунд
        //ToDo: Проверить производительность с меньшим числом запросов
        public IDataResult MassDelete(BaseParams baseParams)
        {
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds").Distinct().ToArray();

            if (objectIds.Length == 0)
            {
                return new BaseDataResult(false, "Не выбраны записи для удаления");
            }

            if (objectIds.Length > 1000)
            {
                return new BaseDataResult(false, "Количество записей должно быть меньше 1000");
            }

            var listForUpdate = new LinkedList<IEntity>();
            var listForDelete = new LinkedList<IEntity>();

            var shortRecordService = Container.Resolve<IDomainService<ShortProgramRecord>>();

            var query = DomainService.GetAll()
                .Where(x => !shortRecordService.GetAll().Any(y => y.ShortProgramObject.State.FinalState && y.Stage1.StructuralElement.Id == x.Id))
                .Where(x => objectIds.Contains(x.Id));

            ProcessDpkr(query, listForUpdate, listForDelete);

            ProcessVersion(query, listForUpdate, listForDelete);

            foreach (var rose in query)
            {
                listForDelete.AddLast(rose);
            }

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            using (var tr = session.BeginTransaction())
            {
                try
                {
                    listForDelete.ForEach(session.Delete);
                    listForUpdate.ForEach(session.SaveOrUpdate);

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }

        private void ProcessDpkr(
            IQueryable<RealityObjectStructuralElement> query,
            LinkedList<IEntity> listForUpdate,
            LinkedList<IEntity> listForDelete)
        {
            var stage1Service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgramm>>();
            var stage2Service = Container.Resolve<IDomainService<RealityObjectStructuralElementInProgrammStage2>>();

#warning неправильно написано, т.к. третий этап может быть не расчитан, а список зависимостей формируется от третьего этапа

            var st3Recs = stage1Service.GetAll()
                .Where(x => query.Any(y => y.Id == x.StructuralElement.Id))
                .Select(x => x.Stage2.Stage3)
                .AsEnumerable()
                .Distinct(x => x.Id)
                .ToDictionary(x => x.Id);

            var st1IdsForDelete =
                new HashSet<long>(
                    stage1Service.GetAll()
                        .Where(x => query.Any(y => y.Id == x.StructuralElement.Id))
                        .Select(x => x.Id)
                        .Distinct());

            var st3Ids = st3Recs.Select(x => x.Key).ToList();

            foreach (var st3Id in st3Ids)
            {
                var st3Rec = st3Recs[st3Id];

                var st2Recs = stage2Service.GetAll()
                    .Where(x => x.Stage3.Id == st3Id)
                    .ToDictionary(x => x.Id);

                var st2Ids = st2Recs.Select(x => x.Key).ToList();

                foreach (var st2Id in st2Ids)
                {
                    var st2Rec = st2Recs[st2Id];

                    var st1Recs = stage1Service.GetAll()
                        .Where(x => x.Stage2.Id == st2Id)
                        .ToDictionary(x => x.Id);

                    var st1Ids = st1Recs.Select(x => x.Key).ToList();

                    foreach (var st1Id in st1Ids)
                    {
                        if (st1IdsForDelete.Contains(st1Id))
                        {
                            listForDelete.AddLast(st1Recs[st1Id]);
                            st1Recs.Remove(st1Id);
                        }
                    }

                    //если остались записи первого этапа - пересчитываем суммы
                    if (st1Recs.Any())
                    {
                        st2Rec.Sum = st1Recs.Sum(x => x.Value.Sum + x.Value.ServiceCost);
                        listForUpdate.AddLast(st2Rec);
                    }
                    // иначе добавляем второй этап в список на удаление 
                    else 
                    {
                        listForDelete.AddLast(st2Rec);
                        st2Recs.Remove(st2Id);
                    }
                }

                //если остались записи второго этапа - пересчитываем суммы
                if (st2Recs.Any())
                {
                    st3Rec.Sum = st2Recs.Sum(x => x.Value.Sum);
                    listForUpdate.AddLast(st3Rec);
                }
                //иначе добавляем третий этап в список на удаление 
                else
                {
                    listForDelete.AddLast(st3Rec);
                    st3Recs.Remove(st3Id);
                }
            }
        }

        private void ProcessVersion(
            IQueryable<RealityObjectStructuralElement> query,
            LinkedList<IEntity> listForUpdate,
            LinkedList<IEntity> listForDelete)
        {
            var stage1Service = Container.Resolve<IDomainService<VersionRecordStage1>>();
            var stage2Service = Container.Resolve<IDomainService<VersionRecordStage2>>();
            var dpkrCorrService = Container.Resolve<IDomainService<DpkrCorrectionStage2>>();
            var shortRecService = Container.Resolve<IDomainService<ShortProgramRecord>>();

            var st3Recs = stage1Service.GetAll()
                .Where(x => query.Any(y => y.Id == x.StructuralElement.Id))
                .Select(x => x.Stage2Version.Stage3Version)
                .AsEnumerable()
                .Distinct(x => x.Id)
                .ToDictionary(x => x.Id);

            var st1IdsForDelete =
                new HashSet<long>(
                    stage1Service.GetAll()
                        .Where(x => query.Any(y => y.Id == x.StructuralElement.Id))
                        .Select(x => x.Id)
                        .Distinct());

            var st3Ids = st3Recs.Select(x => x.Key).ToList();

            foreach (var st3Id in st3Ids)
            {
                var st3Rec = st3Recs[st3Id];

                var st2Recs = stage2Service.GetAll()
                    .Where(x => x.Stage3Version.Id == st3Id)
                    .ToDictionary(x => x.Id);

                var st2Ids = st2Recs.Select(x => x.Key).ToList();

                foreach (var st2Id in st2Ids)
                {
                    var st2Rec = st2Recs[st2Id];

                    var st1Recs = stage1Service.GetAll()
                        .Where(x => x.Stage2Version.Id == st2Id)
                        .ToDictionary(x => x.Id);

                    var st1Ids = st1Recs.Select(x => x.Key).ToList();

                    foreach (var st1Id in st1Ids)
                    {
                        var st1Rec = st1Recs[st1Id];

                        if (st1IdsForDelete.Contains(st1Id))
                        {
                            //идем в кратокрочную программу и собираем там записи для удаления
                            ProcessShortProgram(st1Rec, listForDelete, shortRecService);

                            listForDelete.AddLast(st1Recs[st1Id]);
                            st1Recs.Remove(st1Id);
                        }
                    }

                    if (st1Recs.Any())
                    {
                        st2Rec.Sum = st1Recs.Sum(x => x.Value.Sum + x.Value.SumService);
                        listForUpdate.AddLast(st2Rec);
                    }
                    else
                    {
                        //идем в корректировку и собираем там записи для удаления
                        ProcessDpkrCorrection(st2Rec, listForDelete, dpkrCorrService);

                        listForDelete.AddLast(st2Rec);
                        st2Recs.Remove(st2Id);
                    }
                }

                if (st2Recs.Any())
                {
                    st3Rec.Sum = st2Recs.Sum(x => x.Value.Sum);
                    listForUpdate.AddLast(st3Rec);
                }
                else
                {
                    listForDelete.AddLast(st3Rec);
                    st3Recs.Remove(st3Id);
                }
            }
        }

        private void ProcessDpkrCorrection(
            VersionRecordStage2 stage2, 
            LinkedList<IEntity> listForDelete,
            IDomainService<DpkrCorrectionStage2> domainService)
        {
            var recs = domainService.GetAll()
                .Where(x => x.Stage2.Id == stage2.Id)
                .ToList();

            foreach (var rec in recs)
            {
                listForDelete.AddLast(rec);
            }
        }

        private void ProcessShortProgram(
            VersionRecordStage1 stage1,
            LinkedList<IEntity> listForDelete,
            IDomainService<ShortProgramRecord> domainService)
        {
            var recs = domainService.GetAll()
                .Where(x => x.Stage1.Id == stage1.Id)
                .ToList();

            foreach (var rec in recs)
            {
                listForDelete.AddLast(rec);
            }
        }
    }
}