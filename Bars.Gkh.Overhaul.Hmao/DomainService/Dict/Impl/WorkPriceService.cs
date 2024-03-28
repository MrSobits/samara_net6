
namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Расценка по работе
    /// </summary>
    public class HmaoWorkPriceService : HmaoWorkPriceService<HmaoWorkPrice>
    {
    }

    /// <summary>
    /// Расценка по работе
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HmaoWorkPriceService<T> : Bars.Gkh.Overhaul.DomainService.Impl.WorkPriceService<T>
       where T : HmaoWorkPrice, new()
    {
        /// <summary>
        /// Расценка по работе
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="paging"></param>
        /// <param name="totalCount"></param>
        /// <returns>Результат получения списка</returns>
        public override IList GetListView(BaseParams baseParams, bool paging, ref int totalCount)
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

                var workPriceMoLevel = appParams.ContainsKey("WorkPriceMoLevel") && !string.IsNullOrEmpty(appParams["WorkPriceMoLevel"].To<string>())
                    ? appParams["WorkPriceMoLevel"].To<WorkpriceMoLevel>() == WorkpriceMoLevel.Both 
                        ? WorkpriceMoLevel.Settlement
                        : appParams["WorkPriceMoLevel"].To<WorkpriceMoLevel>()
                    : WorkpriceMoLevel.MunicipalUnion;

                var operMuIds = userManager.GetMunicipalityIds();

                var muIds = municipalityRepository.GetAll()
                             .WhereIf(operMuIds.Count > 0, x => operMuIds.Contains(x.Id))
                             .Select(x => new { x.Id, x.Level })
                             .AsEnumerable()
                             .WhereIf(workPriceMoLevel != WorkpriceMoLevel.MunicipalUnion, x => x.Level.ToWorkpriceMoLevel(Container) == workPriceMoLevel)
                             .Select(x => x.Id)
                             .ToList();

                var queryable =
                    this.DomainService.GetAll()
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
                                             ParentMo = x.Municipality.ParentMo.Name ?? x.Municipality.Name,
                                             CapitalGroup = x.CapitalGroup != null ? x.CapitalGroup.Name : null,
                                             RealEstateType = x.RealEstateType != null ? x.RealEstateType.Name : null
                                         })
                                 .Filter(loadParam, this.Container);

                totalCount = queryable.Count();

                queryable = queryable
                    .Order(loadParam)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.ParentMo)
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
        /// Вовзаращает список расценок заданного МО
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список расценок заданного МО</returns>
        public override IDataResult ListByFromMunicipality(BaseParams baseParams)
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
                    CapitalGroup = x.CapitalGroup.Name,
                    RealEstateType = x.RealEstateType != null ? x.RealEstateType.Name : null
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

	    public override IDataResult ListByToMunicipality(BaseParams baseParams)
	    {
		    var loadParams = baseParams.GetLoadParam();

			var ids = baseParams.Params.GetAs("Id", string.Empty);
			var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

		    var data = this.DomainService.GetAll()
				.WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
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
        public override IDataResult AddWorkPricesByMunicipality(BaseParams baseParams)
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

                if (workPriceIds.Count() == 0)
                {
                    return new BaseDataResult(false, "Не указаны которые необходимо скопирвоать");
                }

                var listToSave = new List<T>();

                // Получаем запоси Откуда над окопировать
                var copeFromQueryable = this.DomainService.GetAll()
                                 .Where(x => x.Municipality.Id == copyFromId)
                                 .WhereIf(workPriceIds.Count() > 0, x => workPriceIds.Contains(x.Id));

                var copyFromWorkPrices =
                    copeFromQueryable.AsEnumerable()
                                     .GroupBy(
                                         x =>
                                         x.Job.Id + "_" + x.Year + "_" + (x.CapitalGroup != null ? x.CapitalGroup.Id.ToStr() : string.Empty)
                                         + "_" + (x.RealEstateType != null ? x.RealEstateType.Id.ToStr() : string.Empty))
                                     .ToDictionary(x => x.Key, y => y.First());

                // Получаем записи сущиствующие в МО в Которые над оскопировать
                var existWorkPrices =
                    this.DomainService.GetAll()
                        .Where(x => x.Municipality.Id != copyFromId)
                        .Where(x => copyToMunicipalityIds.Contains(x.Municipality.Id))
                        .AsEnumerable()
                        .GroupBy(
                            x =>
                            x.Municipality.Id + "_" + x.Job.Id + "_" + x.Year + "_"
                            + (x.CapitalGroup != null ? x.CapitalGroup.Id.ToStr() : string.Empty) + "_"
                            + (x.RealEstateType != null ? x.RealEstateType.Id.ToStr() : string.Empty))
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
                                Year = kvp.Value.Year,
                                CapitalGroup = kvp.Value.CapitalGroup,
                                RealEstateType = kvp.Value.RealEstateType
                            };

                            existWorkPrices.Add(newKey, wp);
                        }

                        wp.NormativeCost = kvp.Value.NormativeCost;
                        wp.SquareMeterCost = kvp.Value.SquareMeterCost;

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
        /// Добавить Массово Расценки по работе
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult DoMassAddition(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<int>("Year");
            var from = baseParams.Params.GetAs<int>("From");
            var to = baseParams.Params.GetAs<int>("To");
            var coef = baseParams.Params.GetAs<decimal>("InflCoef") / 100;
            var municipalityId = baseParams.Params.GetAs<long>("Municipality");

            if (municipalityId == 0)
            {
                return new BaseDataResult { Success = false, Message = "Invalid arguments" };
            }

            if (year >= from || from > to)
            {
                return new BaseDataResult { Success = false, Message = "Invalid arguments" };
            }

            var workPriceList = this.DomainService.GetAll().Where(x => x.Year == year && x.Municipality.Id == municipalityId).ToList();

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
                muDomain.Load(municipalityId);
            }

            var existWorkPrices = this.DomainService.GetAll()
                                .Where(x => x.Year >= from && x.Year <= to && x.Municipality.Id == municipalityId)
                                .GroupBy(x => x.Year)
                                .ToDictionary(x => x.Key);

            var listToSave = new List<T>();

            Enumerable.Range(from, to - from + 1)
                        .ForEach(
                            e =>
                                workPriceList.ForEach(
                                    x =>
                                    {
                                        var existWorkPrice = existWorkPrices.ContainsKey(e) ? 
                                            existWorkPrices[e]
                                            .FirstOrDefault(y => y.Job.Id == x.Job.Id &&
                                                y.CapitalGroup?.Id == x.CapitalGroup?.Id &&
                                                y.RealEstateType?.Id == x.RealEstateType?.Id)
                                            : null;

                                        if (existWorkPrice == null)
                                        {
                                            listToSave.Add(this.NewWorkPrice(x, from, e, coef));
                                        }
                                        else
                                        {
                                            existWorkPrice.NormativeCost = this.GetPriceByYears(from, e, x.NormativeCost, coef);
                                            existWorkPrice.SquareMeterCost = x.SquareMeterCost != null ? this.GetPriceByYears(from, e, x.SquareMeterCost.Value, coef) : (decimal?)null;

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
        protected override T NewWorkPrice(T oldWorkPrice, int startYear, int currentYear, decimal percent)
        {
            return new T
            {
                Municipality = oldWorkPrice.Municipality,
                Job = oldWorkPrice.Job,
                Year = currentYear,
                NormativeCost = this.GetPriceByYears(startYear, currentYear, oldWorkPrice.NormativeCost, percent),
                SquareMeterCost = oldWorkPrice.SquareMeterCost !=null ? this.GetPriceByYears(startYear, currentYear, oldWorkPrice.SquareMeterCost.Value, percent) : (decimal?) null,
                CapitalGroup = oldWorkPrice.CapitalGroup,
                RealEstateType = oldWorkPrice.RealEstateType,
            };
        }

        /// <summary>
        /// Проверка на дубликаты
        /// </summary>
        /// <param name="workPrices"></param>
        /// <returns>Результат выполнения</returns>
        protected override string ValidateMassAddition(List<T> workPrices)
        {
            var duplicate = workPrices.GroupBy(x => new { x.Municipality, x.Year, x.CapitalGroup, x.Job, x.RealEstateType }).Where(x => x.Count() > 1).ToList();

            if (duplicate.Any())
            {
                var result = "Следующие работы имеют дублирующие записи расценок в базовом году: "
                             + string.Join(
                                 "; ",
                                 duplicate.Select(
                                     x =>
                                     string.Format(
                                         "{0}, группа капитальности: {1}, тип дома: {2}",
                                         x.Key.Job.Name,
                                         x.Key.CapitalGroup.Return(y => y.Name, "Не задано"),
                                         x.Key.RealEstateType.Return(y => y.Name, "Не задано"))))
                             + ". Массовое добавление отменено.";

                return result;
            }

            return string.Empty;
        }
    }
}
