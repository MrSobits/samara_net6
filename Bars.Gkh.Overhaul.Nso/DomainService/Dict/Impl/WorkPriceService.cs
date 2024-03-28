namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Utils;
    using B4.IoC;

    public class NsoWorkPriceService : NsoWorkPriceService<NsoWorkPrice>
    {
    }

    public class NsoWorkPriceService<T>: Bars.Gkh.Overhaul.DomainService.Impl.WorkPriceService<T>
       where T : NsoWorkPrice, new ()
    {
        public override IList GetListView(BaseParams baseParams, bool paging, ref int totalCount)
        {
            var gkhParams = Container.Resolve<IGkhParams>();
            var userManager = Container.Resolve<IGkhUserManager>();
            var municipalityRepository = Container.Resolve<IRepository<Municipality>>();

            try
            {
                var loadParam = GetLoadParam(baseParams);
                var year = loadParam.Filter.GetAs<long>("year");
                var um = loadParam.Filter.GetAs<long>("unitmeasure");
                var municipalityId = loadParam.Filter.GetAs<long>("municipal");

                var appParams = gkhParams.GetParams();

                var workPriceMoLevel = appParams.ContainsKey("WorkPriceMoLevel") && !string.IsNullOrEmpty(appParams["WorkPriceMoLevel"].To<string>())
                    ? appParams["WorkPriceMoLevel"].To<WorkpriceMoLevel>()
                    : WorkpriceMoLevel.MunicipalUnion;

                var operMuIds = userManager.GetMunicipalityIds();

                var muIds = municipalityRepository.GetAll()
                             .WhereIf(operMuIds.Count > 0, x => operMuIds.Contains(x.Id))
                             .Select(x => new { x.Id, x.Level })
                             .AsEnumerable()
                             .WhereIf(workPriceMoLevel != WorkpriceMoLevel.Both, x => x.Level.ToWorkpriceMoLevel(Container) == workPriceMoLevel)
                             .Select(x => x.Id)
                             .ToList();


                var queryable =
                    DomainService.GetAll()
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
                                 .Filter(loadParam, Container);

                totalCount = queryable.Count();

                queryable = queryable
                    .Order(loadParam)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Year);

                return paging ? queryable.Paging(loadParam).ToList() : queryable.ToList();
            }
            finally
            {
                Container.Release(gkhParams);
                Container.Release(userManager);
                Container.Release(municipalityRepository);
            }
        }

        public override IDataResult ListByToMunicipality(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var ids = baseParams.Params.GetAs("Id", string.Empty);
            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var data = DomainService.GetAll()
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
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult AddWorkPricesByMunicipality(BaseParams baseParams)
        {
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();

            try
            {
                var copyFromId = baseParams.Params.GetAs<long>("copyFromId");

                if (copyFromId == 0)
                {
                    return new BaseDataResult(false, "Не указано муниципальное образование из которого копирвоать");
                }

                var copyToMunicipalityIds = baseParams.Params.GetAs("copyToMunicipalityIds", new long[]{});

                if (copyToMunicipalityIds.Count() == 0)
                {
                    return new BaseDataResult(false, "Не указаны муниципальные образования в которые необходимо копирвоать работы");
                }

                var workPriceIds = baseParams.Params.GetAs("objectIds", new long[] { }) ?? new long[] { };
                
                // убрал это условие, чтобы копировались все работы, если не одна не выбрана
                //if (workPriceIds.Count() == 0)
                //{
                //    return new BaseDataResult(false, "Не указаны которые необходимо скопирвоать");
                //}

                var listToSave = new List<T>();

                // Получаем запоси Откуда над окопировать
                var copeFromQueryable = DomainService.GetAll()
                                 .Where(x => x.Municipality.Id == copyFromId)
                                 .WhereIf(workPriceIds.Count() > 0, x => workPriceIds.Contains(x.Id));

                var copyFromWorkPrices =
                    copeFromQueryable.AsEnumerable()
                                     .GroupBy(
                                         x =>
                                         x.Job.Id + "_" + x.Year + "_" + (x.CapitalGroup != null ? x.CapitalGroup.Id.ToStr() : string.Empty)
                                         + "_" + (x.RealEstateType != null ? x.RealEstateType.Id.ToStr() : string.Empty))
                                     .ToDictionary(x => x.Key, y => y.First());

                // Получаем записи сущиствующие в МО в Которые надо скопировать
                var existWorkPrices =
                    DomainService.GetAll()
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

                var session = Container.Resolve<ISessionProvider>().OpenStatelessSession();

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
                Container.Release(serviceMunicipality);
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

        protected override T NewWorkPrice(T oldWorkPrice, int startYear, int currentYear, decimal percent)
        {
            return new T
            {
                Municipality = oldWorkPrice.Municipality,
                Job = oldWorkPrice.Job,
                Year = currentYear,
                NormativeCost = GetPriceByYears(startYear, currentYear, oldWorkPrice.NormativeCost, percent),
                SquareMeterCost = oldWorkPrice.SquareMeterCost,
                CapitalGroup = oldWorkPrice.CapitalGroup,
                RealEstateType = oldWorkPrice.RealEstateType,
            };
        }

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