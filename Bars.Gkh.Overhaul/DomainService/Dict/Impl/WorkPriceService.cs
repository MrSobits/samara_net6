namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Authentification;
    using Bars.B4.IoC;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    using Gkh.Entities;

    using Castle.Windsor;
    using Entities;

    using Converter = Bars.B4.DomainService.BaseParams.Converter;

    // Пустышка на случай если ктото наследвоался
    /// <summary>
    /// Расценка по работе
    /// </summary>
    public class WorkPriceService : WorkPriceService<WorkPrice>
    {
        //внимание код доабвлять в Generic класс
    }

    //Generic класс для тех кто расширяет сущност ьв других регионах
    /// <summary>
    /// Расценка по работе
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WorkPriceService<T> : IWorkPriceService<T>
        where T: WorkPrice, new ()
    {
        /// <summary>
        ///  Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис
        /// </summary>
        public IDomainService<T> DomainService { get; set; }

        public IGkhParams GkhParams { get; set; }

        /// <summary>
        /// Получаем Базвый год  
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Список Базовых лет</returns>
        public IDataResult YearList(BaseParams baseParams)
        {
            var municipalityIds = baseParams.Params.GetAs("Municipality", string.Empty).ToLongArray();

			var appParams = this.GkhParams.GetParams();
			var workPriceMoLevel = appParams.ContainsKey("WorkPriceMoLevel") && !string.IsNullOrEmpty(appParams["WorkPriceMoLevel"].To<string>())
					? appParams["WorkPriceMoLevel"].To<WorkpriceMoLevel>()
					: WorkpriceMoLevel.MunicipalUnion;

			var years = this.DomainService.GetAll()
				.Where(x => municipalityIds.Contains(x.Municipality.Id) || municipalityIds.Contains(x.Municipality.ParentMo.Id))
                .Select(x => new
			    {
                    ParentMoLevel = (TypeMunicipality?)x.Municipality.ParentMo.Level,
                    x.Municipality.Level,
                    x.Year
			    })
				.AsEnumerable()
				.WhereIf(workPriceMoLevel != WorkpriceMoLevel.Both,
					x => x.Level.ToWorkpriceMoLevel(this.Container) == workPriceMoLevel 
					    || x.ParentMoLevel.HasValue && x.ParentMoLevel.Value.ToWorkpriceMoLevel(this.Container) == workPriceMoLevel)
				.Select(x => x.Year)
                .Distinct()
                .OrderBy(x => x)
                .Select(x => new {Id = x, Year = x.ToStr()})
                .ToList();

            // Добавляем пустой элемент
            years.Insert(0, new { Id = 0, Year = "-" });

            return new ListDataResult(years, years.Count);
        }

        /// <summary>
        /// Добавить Массово Расценки по работе
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Результат выполнения</returns>
        public virtual IDataResult DoMassAddition(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<int>("Year");
            var from = baseParams.Params.GetAs<int>("From");
            var to = baseParams.Params.GetAs<int>("To");
            var coef = baseParams.Params.GetAs<decimal>("InflCoef") / 100;
            var municipalityIds = baseParams.Params.GetAs("Municipality", string.Empty).ToLongArray();
            
            if (municipalityIds.IsNull())
            {
                return new BaseDataResult { Success = false, Message = "Invalid arguments" };
            }

            if (year >= from || from > to)
            {
                return new BaseDataResult { Success = false, Message = "Invalid arguments" };
            }

            var workPriceList = this.DomainService.GetAll().Where(x => x.Year == year && municipalityIds.Contains(x.Municipality.Id)).ToList();
            
            if (workPriceList.IsEmpty())
            {
                return new BaseDataResult { Success = false, Message = "Не найдены базовые расценки для добавления" };
            }

            var invalidText = this.ValidateMassAddition(workPriceList);

            if (!string.IsNullOrWhiteSpace(invalidText))
            {
                return new BaseDataResult { Success = false, Message = invalidText };
            }

            var muDomain = this.Container.Resolve<IDomainService<Municipality>>();

            using (this.Container.Using(muDomain))
            {
                muDomain.Load(municipalityIds);
            }
            
            var existWorkPrices = this.DomainService.GetAll()
                                .Where(x => x.Year >= from && x.Year <= to && municipalityIds.Contains(x.Municipality.Id))
                                .GroupBy(x => x.Year)
                                .ToDictionary(x => x.Key);

            var listToSave = new List<T>();

            Enumerable.Range(from, to - from + 1)
                        .ForEach(
                            e =>
                                workPriceList.ForEach(
                                    x =>
                                    {
                                        var existWorkPrice = existWorkPrices.ContainsKey(e) ? existWorkPrices[e].FirstOrDefault(y => y.Job.Id == x.Job.Id) : null;

                                        if (existWorkPrice == null)
                                        {
                                            listToSave.Add(this.NewWorkPrice(x, from, e, coef));
                                        }
                                        else
                                        {
                                            existWorkPrice.NormativeCost = this.GetPriceByYears(from, e, x.NormativeCost, coef);

                                            listToSave.Add(existWorkPrice);
                                        }

                                    }));

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                
                try
                {
                    foreach (var workPrice in listToSave)
                    {
                        if (workPrice.Id > 0)
                        {
                            this.DomainService.Update(workPrice);
                        }
                        else
                        {
                            this.DomainService.Save(workPrice);
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }

        /// <summary>
        ///  Расценки по работе
        /// </summary>
        /// <param name="oldWorkPrice"></param>
        /// <param name="startYear"></param>
        /// <param name="currentYear"></param>
        /// <param name="percent"></param>
        /// <returns>Результат выполнения</returns>
        protected virtual T NewWorkPrice(T oldWorkPrice, int startYear, int currentYear, decimal percent)
        {
            return new T
                {
                    Municipality = oldWorkPrice.Municipality,
                    Job = oldWorkPrice.Job,
                    Year = currentYear,
                    NormativeCost = this.GetPriceByYears(startYear, currentYear, oldWorkPrice.NormativeCost, percent),
                    SquareMeterCost = oldWorkPrice.SquareMeterCost,
                    CapitalGroup = oldWorkPrice.CapitalGroup
                };
        }

        /// <summary>
        /// Проверка на дубликаты
        /// </summary>
        /// <param name="workPrices"></param>
        /// <returns>Результат выполнения</returns>
        protected virtual string ValidateMassAddition(List<T> workPrices)
        {
            var duplicate = workPrices.GroupBy(x => new { x.Municipality, x.Year, x.CapitalGroup, x.Job }).Where(x => x.Count() > 1).ToList();

            if (duplicate.Any())
            {
                var result = "Следующие работы имеют дублирующие записи расценок в базовом году: " +
                string.Join(
                    "; ",
                    duplicate.Select(x => $"{x.Key.Job.Name}, группа капитальности: {x.Key.CapitalGroup.Return(y => y.Name, "Не задано")}"))
                    + ". Массовое добавление отменено.";

                return result;
            }

            return string.Empty;
        }

        /// <summary>
        /// Рекурсивно возвращает цену с учетом цены предыдущего года и коэффициента инфляции
        /// </summary>
        /// <param name="startYear">Год начала</param>
        /// <param name="endYear">Год окончания</param>
        /// <param name="cost">Цена</param>
        /// <param name="coef">Коэффициент инфляции</param>
        /// <returns>Цена с учетом предыдущих лет и инфляции</returns>
        protected decimal GetPriceByYears(int startYear, int endYear, decimal cost, decimal coef)
        {
            if (startYear == endYear)
            {
                return cost + cost * coef;
            }

            var prevYearCost = this.GetPriceByYears(startYear, endYear - 1, cost, coef);
            return prevYearCost + prevYearCost * coef;
        }

        /// <summary>
        /// Вовзращает уникальный список муниципалов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Уникальный список муниципальных образований</returns>
        public IDataResult MunicipalityList(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var operatorMunicipalityList = userManager.GetMunicipalityIds();

            var municipals = this.DomainService.GetAll()
                .Where(x => x.Municipality != null)
                .WhereIf(operatorMunicipalityList.Count > 0, x => operatorMunicipalityList.Contains(x.Municipality.Id))
                .Select(x => new {x.Municipality.Id, Municipal = x.Municipality.Name})
                .AsEnumerable()
                .Distinct()
                .ToList();

            return new ListDataResult(municipals, municipals.Count());
        }


#warning ListByFromMunicipality и ListByToMunicipality делают одно и тоже

        /// <summary>
        /// Вовзаращает список расценок заданного МО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список расценок заданного МО</returns>
        public virtual IDataResult ListByFromMunicipality(BaseParams baseParams)

        {
            var loadParams = baseParams.GetLoadParam();

            var municipalityId = baseParams.Params.GetAs<long>("copyFromMunicipalityId");

            var data = this.DomainService.GetAll()
                .Where(x => x.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    Job = x.Job.Name,
                    UnitMeasure = x.Job.UnitMeasure.Name,
                    x.NormativeCost,
                    x.Year,
                    CapitalGroup = x.CapitalGroup.Name
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Вовзаращает список расценок заданного МО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список расценок заданного МО</returns>
        public virtual IDataResult ListByToMunicipality(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var municipalityId = baseParams.Params.GetAs<long>("copyToMunicipalityId");

            var data = this.DomainService.GetAll()
                .Where(x => x.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    Job = x.Job.Name,
                    UnitMeasure = x.Job.UnitMeasure.Name,
                    x.NormativeCost,
                    x.Year,
                    x.Municipality
                })
                .Filter(loadParams, this.Container);
            
            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Добавляет список расценок заданному МО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public virtual IDataResult AddWorkPricesByMunicipality(BaseParams baseParams)
        {
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

            try
            {
                var copyFromId = baseParams.Params.GetAs<long>("copyFromId");

                if (copyFromId == 0)
                {
                    return new BaseDataResult(false, "Не указано муниципальное образование из которого копирвоать");
                }

                var copyToMunicipalityIds = baseParams.Params.GetAs("copyToMunicipalityIds", new long[] { });

                if (copyToMunicipalityIds.Count() == 0)
                {
                    return new BaseDataResult(false, "Не указаны муниципальные образования в которые необходимо копирвоать работы");
                }

                var workPriceIds = baseParams.Params.GetAs("objectIds", new long[] { });

                // убрал это условие, чтобы копировались все работы, если не одна не выбрана
                //if (workPriceIds.Count() == 0)
                //{
                //    return new BaseDataResult(false, "Не указаны которые необходимо скопирвоать");
                //}

                var listToSave = new List<T>();
                
                // Получаем запоси Откуда над окопировать
                var copeFromQueryable = this.DomainService.GetAll()
                                 .Where(x => x.Municipality.Id == copyFromId)
                                 .WhereIf(workPriceIds.Count() > 0, x => workPriceIds.Contains(x.Id));

                var copyFromWorkPrices =
                    copeFromQueryable.AsEnumerable()
                                     .GroupBy(
                                         x =>
                                         x.Job.Id + "_" + x.Year + "_" + (x.CapitalGroup != null ? x.CapitalGroup.Id.ToStr() : string.Empty))
                                     .ToDictionary(x => x.Key, y => y.First());

                // Получаем записи сущиствующие в МО в Которые над оскопировать
                var existWorkPrices = this.DomainService.GetAll()
                        .Where(x => x.Municipality.Id != copyFromId)
                        .Where(x => copyToMunicipalityIds.Contains(x.Municipality.Id))
                        .AsEnumerable()
                        .GroupBy(
                            x =>
                            x.Municipality.Id + "_" + x.Job.Id + "_" + x.Year + "_"
                            + (x.CapitalGroup != null ? x.CapitalGroup.Id.ToStr() : string.Empty))
                        .ToDictionary(x => x.Key, y => y.First());


                foreach (var munId in copyToMunicipalityIds)
                {
                    var municipality = serviceMunicipality.Load(munId);

                    if (munId == copyFromId || municipality == null)
                    {
                        continue;
                    }

                    foreach (var kvp in copyFromWorkPrices)
                    {
                        T wp;

                        var newKey = munId.ToStr() + "_" + kvp.Key;

                        if (existWorkPrices.ContainsKey(newKey))
                        {
                            wp = existWorkPrices[newKey];
                        }
                        else
                        {
                            wp = new T
                            {
                                Municipality = municipality,
                                Job = kvp.Value.Job,
                                Year = kvp.Value.Year
                            };

                            existWorkPrices.Add(newKey, wp);
                        }

                        wp.NormativeCost = kvp.Value.NormativeCost;
                        wp.SquareMeterCost = kvp.Value.SquareMeterCost;
                        wp.CapitalGroup = kvp.Value.CapitalGroup;

                        listToSave.Add(wp);
                    }
                }

                var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // сохраняем новые записи для выбранных МО
                        listToSave.ForEach(x =>
                        {
                            if (x.Id > 0)
                                session.Update(x);
                            else
                                session.Insert(x);
                        });

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return new BaseDataResult
                        {
                            Success = false,
                            Message = "Произошла ошибка про копировании:" + e.Message
                        };
                    }
                }

                return new BaseDataResult { Success = true, Message = "Копирование прошло успешно" };
            }
            finally
            {
                this.Container.Release(serviceMunicipality);
            }
        }

        /// <summary>
        /// Расценка по работе
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="paging"></param>
        /// <param name="totalCount"></param>
        /// <returns>Результат получения списка</returns>
        public virtual IList GetListView(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var gkhParams = this.Container.Resolve<IGkhParams>();
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var municipalityRepository = this.Container.Resolve<IRepository<Municipality>>();

            try
            {
                var loadParam = this.GetLoadParam(baseParams);
                var year = loadParam.Filter.GetAs<long>("year");
                var um = loadParam.Filter.GetAs<long>("unitmeasure");
                var municipalityId = loadParam.Filter.GetAs<long>("municipal");

                var appParams = gkhParams.GetParams();

                var moLevel = appParams.ContainsKey("WorkPriceMoLevel") && !string.IsNullOrEmpty(appParams["WorkPriceMoLevel"].To<string>())
                    ? appParams["WorkPriceMoLevel"].To<MoLevel>()
                    : MoLevel.MunicipalUnion;

                var operMuIds = userManager.GetMunicipalityIds();

                var muIds = municipalityRepository.GetAll()
                             .WhereIf(operMuIds.Count > 0, x => operMuIds.Contains(x.Id))
                             .Select(x => new { x.Id, x.Level })
                             .AsEnumerable()
                             .Where(x => x.Level.ToMoLevel(this.Container) == moLevel)
                             .Select(x => x.Id)
                             .ToList();


                var queryable = this.DomainService.GetAll()
                                 .WhereIf(year > 0, x => x.Year == year)
                                 .WhereIf(um > 0, x => x.Job.UnitMeasure.Id == um)
                                 .WhereIf(municipalityId > 0, x => x.Municipality.Id == municipalityId)
                                 .Where(x => x.Municipality == null || muIds.Contains(x.Municipality.Id))
                                 .Select(
                                     x =>
                                     new
                                         {
                                             x.Id,
                                             Job = x.Job.Name,
                                             UnitMeasure = x.Job.UnitMeasure.Name,
                                             x.NormativeCost,
                                             x.SquareMeterCost,
                                             x.Year,
                                             Municipality = x.Municipality.Name,
                                             CapitalGroup = x.CapitalGroup.Name,
                                         })
                                 .Filter(loadParam, this.Container);

                totalCount = queryable.Count();

                queryable = queryable
                    .Order(loadParam)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Year);

                return paging ? queryable.Paging(loadParam).ToList() : queryable.ToList();
            }
            finally 
            {
                this.Container.Release(gkhParams);
                this.Container.Release(userManager);
                this.Container.Release(municipalityRepository);
            }
        }

        /// <summary>
        /// Параметры загрузки списка
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns> Результат выполнения</returns>
        protected LoadParam GetLoadParam(BaseParams baseParams)
        {
            return baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
        }
    }
}