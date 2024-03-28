namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;

    using System.Linq;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class OverhaulScriptService : IOverhaulScriptService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ProgramCr> programCrDomain { get; set; }

        public IDomainService<Job> jobDomain { get; set; }

        public IDomainService<Work> workDomain { get; set; }

        public IDomainService<TatListCeoService> ceoDomain { get; set; }

        public IDomainService<StructuralElement> sructElDomain { get; set; }

        public IDomainService<StructuralElementWork> sructElWorkDomain { get; set; }

        public IDomainService<RealityObject> roDomain { get; set; }

        public IDomainService<TypeWorkCr> workCrDomain { get; set; }

        public IDomainService<ObjectCr> objectCrDomain { get; set; }

        public IDomainService<RealityObjectStructuralElement> realObjStrElDomain { get; set; }

        /// <summary>
        /// Данный метод переносит из КР Работы в Конструктивные элементы Дома
        /// </summary>
        public BaseDataResult CreateStructElements(BaseParams baseParams)
        {
            var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();
            
            var listToSave = new List<RealityObjectStructuralElement>();
            
            var listProgramNames = new List<string>() { 
                "Программа 2008, пост. 756 в ред. 852",
                "Программа 2009 ноябрь, пост.778",
                "Программа капремонта РТ 2010 ПКМРТ 1168 30.12.2010 г.",
                "Копия Моногорода пост. 777, 776 (13.11.2010)",
                "Копия Моногород  Чистополь 2010-2011 14.06.2011",
                "Программа капремонта 2011, пост. № 1122 от 30.12.2011 г.",
                "Корректировка программы капремонта 2012, пост. № 215 от 15.03.2012",
                "Корректировка Программы капремонта 2013",
                "Программа капремонта 2012 дополнительная (корректировка)",
                "Программа капремонта 2013 дополнительная",
                "Программа капремонта 2010 (приборы)",
                "Программа по приборам 2011",
                "Программа по приборам 2011, пост. 395 КМ РТ"
            };

            // получаем программы, которые вообще можно загружать
            var programs = programCrDomain.GetAll().Where(x => listProgramNames.Contains(x.Name)).Select(x => x.Id).ToList();

            var strElQuery = sructElWorkDomain.GetAll()
                                 .Select(
                                     x =>
                                     new
                                         {
                                             StrElId = x.StructuralElement.Id,
                                             StrElName = x.StructuralElement.Name,
                                             WorkId = x.Job.Work.Id,
                                             JobId = x.Job.Id,
                                             CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                                             CeoName = x.StructuralElement.Group.CommonEstateObject.Name,
                                             x.StructuralElement.LifeTime
                                         })
                                 .AsQueryable();

            // по всем структурным элементам собираем словарик с ключем вид Работы
            var dictStrEl = strElQuery
                                .AsEnumerable()
                                .GroupBy(x => x.WorkId)
                                .ToDictionary(x => x.Key, y => y.First());

            // Данный словарь для получения КЭ по наименвоанию
            var dictStrElNames = strElQuery
                                .AsEnumerable()
                                .GroupBy(x => x.StrElName.ToLower().Trim())
                                          .ToDictionary(x => x.Key, y => y.First());

            // 1 Группа КЭ будет такая (Не Крыша и Не Лифты и Не Фасад )
            // Для них алгоритм создания КЭ простой
            // необходимо по программыми и по видам работ найти последнюю запись по Дате программы и создать КЭ
            // то есть в результате должно получутся на 1 дом = 1 КЭ с данным видом
            var work1Stage = strElQuery.Where(x => x.CeoName != "Крыша" && x.CeoName != "Лифты" && x.CeoName != "Фасад")
                         .Select(x => x.WorkId)
                         .Distinct()
                         .ToList();

            // получаем Работы КР для 1 этапа
            var query = workCrDomain.GetAll()
                            .Where(x => programs.Contains(x.ObjectCr.ProgramCr.Id))
                            .Select(
                                x =>
                                new
                                    {
                                        WorkId = x.Work.Id,
                                        RoId = x.ObjectCr.RealityObject.Id,
                                        RoWorkKey = x.ObjectCr.RealityObject.Id + "_" + x.Work.Id,
                                        x.ObjectCr.ProgramCr.Period.DateEnd,
                                        RoofName = x.ObjectCr.RealityObject.RoofingMaterial.Name,
                                        WallName = x.ObjectCr.RealityObject.WallMaterial.Name
                                    })
                            .AsQueryable();

            // получаем записи только для 1 этапа
            var date1Stage = query
                        .Where(x => work1Stage.Contains(x.WorkId))
                        .AsEnumerable()
                        .GroupBy(x => x.RoWorkKey)
                        .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateEnd).First());

            int currYear = 0;
            int yearCr = 0;
            decimal wearout = 0m;

            foreach (var item in date1Stage)
            {
                // тут надо создавать КЭ для стандартных элементов
                if(!dictStrEl.ContainsKey(item.Value.WorkId))
                    continue;

                var strEl = dictStrEl[item.Value.WorkId];

                currYear = DateTime.Now.Year;
                yearCr = item.Value.DateEnd.ToDateTime().Year;
                wearout = ((decimal)(currYear - yearCr) / strEl.LifeTime) * 100;
                if (wearout < 0)
                {
                    wearout = 0;
                }

                if (wearout > 100)
                {
                    wearout = 100;
                }

                var roStrEl = new RealityObjectStructuralElement()
                                  {
                                      RealityObject =
                                          new RealityObject { Id = item.Value.RoId },
                                      StructuralElement =
                                          new StructuralElement { Id = strEl.StrElId },
                                      Volume = 0m,
                                      Repaired = true,
                                      LastOverhaulYear = yearCr,
                                      Name = strEl.StrElName,
                                      Wearout = wearout
                                  };

                listToSave.Add(roStrEl);
            }
            
            // 2 группа КЭ будет только для ООИ = Крыша
            // то необходимо для Дома добавить только 1 КЭ при этоом взять последний по Программе
            // и необходимо брать поле Материал кровли от него уже брать КЭ
            var work2Stage = strElQuery.Where(x => x.CeoName == "Крыша")
                         .Select(x => x.WorkId)
                         .Distinct()
                         .ToList();

            // делаем сопоставление материала кровли
            var dictRoofMatarial = new Dictionary<string, string>
                                       {
                                           { "наплавляемый материал", "мягкая (наплавляемая)" },
                                           { "черепица", "черепица" },
                                           { "рубероид", "рубероид" },
                                           { "шифер", "шиферная" },
                                           { "мягкая (наплавляемая)", "мягкая (наплавляемая)" },
                                           { "сталь оцинкованная", "стальная оцинкованная" },
                                           { "металлическая", "металлическая" },
                                           { "шиферная", "шиферная" }
                                       };

            // получаем записи только для 1 этапа
            var date2Stage = query
                        .Where(x => work2Stage.Contains(x.WorkId))
                        .AsEnumerable()
                        .GroupBy(x => x.RoWorkKey)
                        .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateEnd).First());

            foreach (var item in date2Stage)
            {
                // тут над осоздавать КЭ для Крыш
                var roof = item.Value.RoofName;
                if (string.IsNullOrEmpty(roof)) 
                    roof = string.Empty;

                roof = roof.ToLower().Trim();

                string roof2 = null;

                dictRoofMatarial.TryGetValue(roof, out roof2);

                if (roof2 == null) 
                    roof2 = string.Empty;

                if(!dictStrElNames.ContainsKey(roof2))
                    continue;

                var strEl = dictStrElNames[roof2];
                currYear = DateTime.Now.Year;
                yearCr = item.Value.DateEnd.ToDateTime().Year;
                wearout = ((decimal)(currYear - yearCr) / strEl.LifeTime) * 100;
                if (wearout < 0)
                {
                    wearout = 0;
                }

                if (wearout > 100)
                {
                    wearout = 100;
                }

                var roStrEl = new RealityObjectStructuralElement()
                {
                    RealityObject =
                        new RealityObject { Id = item.Value.RoId },
                    StructuralElement =
                        new StructuralElement { Id = strEl.StrElId },
                    Volume = 0m,
                    Repaired = true,
                    LastOverhaulYear = yearCr,
                    Name = strEl.StrElName,
                    Wearout = wearout
                };

                listToSave.Add(roStrEl);
            }

            // 3 этап КЭ будет только для ООИ = Фасад
            // то необходимо для Дома добавить только 1 КЭ при этоом взять последний по Программе
            // и необходимо брать поле Материал кровли от него уже брать КЭ
            var work3Stage = strElQuery
                            .Where(x => x.CeoName == "Фасад")
                            .Select(x => x.WorkId)
                            .Distinct()
                            .ToList();

            // делаем сопосталвение материала стен
            var dictWallMatarial = new Dictionary<string, string>
                                       {
                                           { "крупно-блочные", "крупно-блочный" },
                                           { "монолитные", "монолитный" },
                                           { "блочные", "блочный" },
                                           { "деревянные", "деревянный" },
                                           { "кирпичные", "кирпичный" },
                                           { "панельные", "панельный" }
                                       };

            // получаем записи только для 1 этапа
            var date3Stage = query
                        .Where(x => work3Stage.Contains(x.WorkId))
                        .AsEnumerable()
                        .GroupBy(x => x.RoWorkKey)
                        .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.DateEnd).First());

            foreach (var item in date3Stage)
            {
                // тут над осоздавать КЭ для Фасада
                var wall = item.Value.WallName;
                if (string.IsNullOrEmpty(wall))
                    wall = string.Empty;

                wall = wall.ToLower().Trim();

                string wall2 = null;

                dictWallMatarial.TryGetValue(wall, out wall2);

                if (wall2 == null)
                    wall2 = string.Empty;

                if (!dictStrElNames.ContainsKey(wall2))
                    continue;

                var strEl = dictStrElNames[wall2];
                currYear = DateTime.Now.Year;
                yearCr = item.Value.DateEnd.ToDateTime().Year;
                wearout = ((decimal)(currYear - yearCr) / strEl.LifeTime) * 100;
                if (wearout < 0)
                {
                    wearout = 0;
                }

                if (wearout > 100)
                {
                    wearout = 100;
                }

                var roStrEl = new RealityObjectStructuralElement()
                {
                    RealityObject =
                        new RealityObject { Id = item.Value.RoId },
                    StructuralElement =
                        new StructuralElement { Id = strEl.StrElId },
                    Volume = 0m,
                    Repaired = true,
                    LastOverhaulYear = yearCr,
                    Name = strEl.StrElName,
                    Wearout = wearout
                };

                listToSave.Add(roStrEl);
            }

            // 4 этап КЭ будет только для ООИ = Лифты
            // Тут сколько записей лифтов есть все добавляем
            var work4Stage = strElQuery
                            .Where(x => x.CeoName == "Лифты")
                            .Select(x => x.WorkId)
                            .Distinct()
                            .ToList();

            // получаем записи только для 1 этапа
            var date4Stage = query
                        .Where(x => work4Stage.Contains(x.WorkId))
                        .ToList();

            foreach (var item in date4Stage)
            {
                // тут над осоздавать КЭ для Лифтов
                // тут над осоздавать КЭ для стандартных элементов
                var strEl = dictStrEl[item.WorkId];

                currYear = DateTime.Now.Year;
                yearCr = item.DateEnd.ToDateTime().Year;
                wearout = ((decimal)(currYear - yearCr) / strEl.LifeTime) * 100;
                if (wearout < 0)
                {
                    wearout = 0;
                }

                if (wearout > 100)
                {
                    wearout = 100;
                }

                var roStrEl = new RealityObjectStructuralElement()
                {
                    RealityObject =
                        new RealityObject { Id = item.RoId },
                    StructuralElement =
                        new StructuralElement { Id = strEl.StrElId },
                    Volume = 0m,
                    Repaired = true,
                    LastOverhaulYear = yearCr,
                    Name = strEl.StrElName,
                    Wearout = wearout
                };

                listToSave.Add(roStrEl);
            }
            
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        listToSave.ForEach(x =>
                            {
                                if (x.Id > 0)
                                {
                                    session.Update(x);
                                }
                                else
                                {
                                    session.Insert(x);
                                }
                            });
                    
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

        public BaseDataResult UpdateVolumeStructElements(BaseParams baseParams)
        {
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var listToSave = new List<RealityObjectStructuralElement>();

            var listProgramNames = new List<string>() { 
                "Программа 2008, пост. 756 в ред. 852",
                "Программа 2009 ноябрь, пост.778",
                "Программа капремонта РТ 2010 ПКМРТ 1168 30.12.2010 г.",
                "Копия Моногорода пост. 777, 776 (13.11.2010)",
                "Копия Моногород  Чистополь 2010-2011 14.06.2011",
                "Программа капремонта 2011, пост. № 1122 от 30.12.2011 г.",
                "Корректировка программы капремонта 2012, пост. № 215 от 15.03.2012",
                "Корректировка Программы капремонта 2013",
                "Программа капремонта 2012 дополнительная (корректировка)",
                "Программа капремонта 2013 дополнительная",
                "Программа капремонта 2010 (приборы)",
                "Программа по приборам 2011",
                "Программа по приборам 2011, пост. 395 КМ РТ"
            };

            var programs = programCrDomain.GetAll().Where(x => listProgramNames.Contains(x.Name)).Select(x => x.Id).ToList();

            // Запрос на получение КЭ забитых в справочниках
            var dictStrEl =
                sructElWorkDomain.GetAll()
                                 .Select(
                                     x =>
                                     new
                                         {
                                             StrElId = x.StructuralElement.Id,
                                             StrElName = x.StructuralElement.Name,
                                             WorkId = x.Job.Work.Id,
                                             JobId = x.Job.Id,
                                             CeoId = x.StructuralElement.Group.CommonEstateObject.Id,
                                             CeoName = x.StructuralElement.Group.CommonEstateObject.Name,
                                             x.StructuralElement.LifeTime
                                         })
                                 .AsEnumerable()
                                 .GroupBy(x => x.StrElId)
                                 .ToDictionary(x => x.Key, y => y.First());

            // Получаем объекты КР последние по всем программам группируем по дому
            var dictObjectCr =
                objectCrDomain.GetAll()
                              .Where(x => programs.Contains(x.ProgramCr.Id))
                              .OrderByDescending(x => x.ProgramCr.Period.DateStart)
                              .Select(x => new { x.Id, roId = x.RealityObject.Id, })
                              .AsEnumerable()
                              .GroupBy(x => x.roId)
                              .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

            // получаем Работы КР
            var dictWorkVolume = workCrDomain.GetAll()
                            .Where(x => programs.Contains(x.ObjectCr.ProgramCr.Id))
                            .Select(x => new { ObjectCrId = x.ObjectCr.Id, WorkId = x.Work.Id, x.Volume })
                            .AsEnumerable()
                            .GroupBy(x => x.ObjectCrId)
                            .ToDictionary(
                                x => x.Key,
                                y =>
                                y.GroupBy(x => x.WorkId)
                                 .ToDictionary(x => x.Key, x => x.Select(z => z.Volume.HasValue ? z.Volume.Value : 0).First()));
            
            // Получаем текущие записи КЭ которых в доме в 1 экземпляре. Другие ненужны
            var query = realObjStrElDomain.GetAll()
                                  .Where(x => x.Volume <=0)
                                  .Where(
                                      x =>
                                      this.realObjStrElDomain.GetAll()
                                             .Count(
                                                 y =>
                                                 x.RealityObject.Id == y.RealityObject.Id
                                                 && x.StructuralElement.Id == y.StructuralElement.Id) == 1);

            var сurrStrElRo = query.Select(x => new
                                    {
                                        x.Id,
                                        roId = x.RealityObject.Id,
                                        strElId = x.StructuralElement.Id
                                    })
                                    .ToList();

            var dictCurrStrElRo = query.ToDictionary(x => x.Id);

            // Проходим по КЭ и пытаемся получить объем из выше сформированных словарей 
            foreach (var item in сurrStrElRo)
            {
                if(!dictCurrStrElRo.ContainsKey(item.Id))
                    continue;

                if(!dictObjectCr.ContainsKey(item.roId))
                    continue;

                if(!dictStrEl.ContainsKey(item.strElId))
                    continue;

                var ovjectCrId = dictObjectCr[item.roId];
                var workId = dictStrEl[item.strElId].WorkId;

                if(!dictWorkVolume.ContainsKey(ovjectCrId))
                    continue;

                if(!dictWorkVolume[ovjectCrId].ContainsKey(workId))
                    continue;

                var record = dictCurrStrElRo[item.Id];
                record.Volume = dictWorkVolume[ovjectCrId][workId];

                listToSave.Add(record);
            }

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        listToSave.ForEach(x => session.SaveOrUpdate(x));

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
    }
}
