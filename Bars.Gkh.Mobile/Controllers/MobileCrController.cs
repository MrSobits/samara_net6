namespace Bars.Gkh.Mobile.Controllers
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Microsoft.AspNetCore.Mvc;

    using RegOperator.Domain;

    // TODO: Заменить OutputCache
    //[OutputCache(CacheProfile = "mobilecr")]
    public class MobileCrController : BaseController
    {
        /// <summary>
        /// Информация о периоде программы
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetPeriodProgram(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);
            var programDomain = Container.ResolveDomain<ProgramCr>();

            try
            {
                using (Container.Using(programDomain))
                {
                    var program = programDomain.FirstOrDefault(x => x.Id == programId);
                    if (program == null)
                    {
                        return Error(string.Format("Не найдена программма с Id = {0}", programId));
                    }

                    return Success(new
                    {
                        begin = program.Period.DateStart.Year,
                        end = program.Period.DateEnd.HasValue ? program.Period.DateEnd.Value.Year : -1
                    });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        /// <summary>
        /// Список текущих программ кап.ремонта
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetCrPrograms(BaseParams baseParams)
        {
            try
            {
                var programDomain = Container.ResolveDomain<ProgramCr>();
                using (Container.Using(programDomain))
                {
                    var currentProgram = programDomain.GetAll()
                        .Where(x => x.Period.DateStart < DateTime.Now)
                        .Where(x => x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                        .FirstOrDefault(x => x.TypeProgramStateCr == TypeProgramStateCr.Active
                                             || x.TypeProgramStateCr == TypeProgramStateCr.New
                                             || x.TypeProgramStateCr == TypeProgramStateCr.Open);

                    var result = programDomain.GetAll()
                        .WhereIf(currentProgram != null, x => x.Id != currentProgram.Id)
                        .ToList()
                        .Select(x => new
                        {
                            id = x.Id,
                            name = x.Name,
                            begin = x.Period.DateStart.Year,
                            end = x.Period.DateEnd.HasValue ? x.Period.DateEnd.Value.Year : -1,
                            visibility = x.TypeVisibilityProgramCr.GetEnumMeta().Display,
                            type = x.TypeProgramCr.GetEnumMeta().Display,
                            state = x.TypeProgramStateCr.GetEnumMeta().Display
                        })
                        .ToList();

                    if (result.Any() && currentProgram != null)
                    {
                        result.Insert(0, new
                        {
                            id = currentProgram.Id,
                            name = currentProgram.Name,
                            begin = currentProgram.Period.DateStart.Year,
                            end = currentProgram.Period.DateEnd.HasValue ? currentProgram.Period.DateEnd.Value.Year : -1,
                            visibility = currentProgram.TypeVisibilityProgramCr.GetEnumMeta().Display,
                            type = currentProgram.TypeProgramCr.GetEnumMeta().Display,
                            state = currentProgram.TypeProgramStateCr.GetEnumMeta().Display
                        });
                    }

                    return Success(result);
                }
            }
            catch (Exception e)
            {
                return Error("Произошла ошибка");
            }
        }

        /// <summary>
        /// Список адресов домов по строке запроса 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult GetMuByQuery(BaseParams baseParams)
        {
            var query = baseParams.Params.GetAs("query", string.Empty, ignoreCase: true);
            var limit = baseParams.Params.GetAs("limit", 25, ignoreCase: true);
            var offset = baseParams.Params.GetAs("offset", 0, ignoreCase: true);

            var realtyDomain = Container.ResolveDomain<RealityObject>();
            using (Container.Using(realtyDomain))
            {
                try
                {
                    return Success(realtyDomain.GetAll().Where(x => x.Address.Contains(query)).Select(x => new
                    {
                        id = x.Id,
                        address = x.Address
                    }).Skip(offset).Take(limit).ToList());
                }
                catch (Exception)
                {
                    return Error("Произошла ошибка");
                }
            }
        }

        public ActionResult GetMainCr(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);

            try
            {
                var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
                var performedWorkActDomain = Container.ResolveDomain<PerformedWorkAct>();
                using (Container.Using(typeWorkCrDomain, performedWorkActDomain))
                {
                    var typeWorkCrs = typeWorkCrDomain.GetAll()
                        .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                        .Select(x => new
                        {
                            id = x.ObjectCr.ProgramCr.Id,
                            name = x.ObjectCr.ProgramCr.Name,
                            mo_id = x.ObjectCr.RealityObject.Municipality.Id,
                            mo = x.ObjectCr.RealityObject.Municipality.Name,
                            complete = x.PercentOfCompletion,
                            sum_plan = x.Sum
                        })
                        .ToList()
                        .GroupBy(x => new { x.mo_id, x.id })
                        .Select(x => new
                        {
                            x.Key,
                            x.First().mo,
                            x.First().name,
                            sum_plan = x.Sum(y => y.sum_plan.HasValue ? y.sum_plan.Value : 0M),
                            complete = x.Average(y => y.complete.HasValue ? y.complete.Value : 0M)
                        });

                    var acts = performedWorkActDomain.GetAll()
                        .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                        .Select(x => new
                        {
                            id = x.ObjectCr.ProgramCr.Id,
                            mo_id = x.ObjectCr.RealityObject.Municipality.Id,
                            sum_act = x.Sum
                        })
                        .ToList()
                        .GroupBy(x => new { x.mo_id, x.id })
                        .Select(x => new
                        {
                            x.Key,
                            sum_act = x.Sum(y => y.sum_act.HasValue ? y.sum_act.Value : 0M)
                        });

                    var data = typeWorkCrs
                        .LeftJoin(acts, typeWork => typeWork.Key, act => act.Key,
                        (typeWork, act) => new
                        {
                            typeWork.Key.id,
                            typeWork.mo,
                            typeWork.Key.mo_id,
                            typeWork.name,
                            sum_plan = decimal.Round(typeWork.sum_plan, 2),
                            complete = decimal.Round(typeWork.complete, 2),
                            sum_act = decimal.Round(act != null ? act.sum_act : 0, 2)
                        })
                        .ToList();

                    var result = data
                        .Select(x => new
                        {
                            x.id,
                            x.mo,
                            x.mo_id,
                            x.name,
                            sum_plan = string.Format("{0:0.00}", x.sum_plan),
                            complete = string.Format("{0:0.00}", x.complete),
                            sum_act = string.Format("{0:0.00}", x.sum_act)
                        })
                        .OrderBy(x => x.mo)
                        .ToList();

                    if (result.Any())
                    {
                        result.Insert(0, new
                        {
                            id = -1L,
                            mo = GetRegionName(),
                            mo_id = -1L,
                            name = string.Empty,
                            sum_plan = string.Format("{0:0.00}", decimal.Round(data.Sum(x => x.sum_plan), 2)),
                            complete = string.Format("{0:0.00}", decimal.Round(data.Average(x => x.complete), 2)),
                            sum_act = string.Format("{0:0.00}", decimal.Round(data.Sum(x => x.sum_act), 2))
                        });
                    }
                    
                    return Success(result);
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetCurrentCr(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);
            var moId = baseParams.Params.GetAs<long>("mo_id", ignoreCase: true);
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
            var archSmrDomain = Container.ResolveDomain<ArchiveSmr>();
            var moDomain = Container.ResolveDomain<Municipality>();

            try
            {
                using (Container.Using(archSmrDomain, typeWorkCrDomain, moDomain))
                {
                    var title = string.Empty;

                    if (moId > 0)
                    {
                        var mo = moDomain.FirstOrDefault(x => x.Id == moId);
                        if (mo == null)
                        {
                            return Error("Муниципальное образование не найдено");
                        }
                        title = mo.Name;
                    }
                    else
                    {
                        title = GetRegionName();
                    }

                    return Success(GetCurrentCr(
                        title,
                        typeWorkCrDomain.GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                            .WhereIf(moId > 0, x => x.ObjectCr.RealityObject.Municipality.Id == moId),
                        archSmrDomain.GetAll()
                            .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programId)
                            .WhereIf(moId > 0, x => x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id == moId)
                        /*.Where(x => x.DateChangeRec.HasValue)*/));
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetCurrentCrMo(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);
            var moId = baseParams.Params.GetAs<long>("mo_id", ignoreCase: true);
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
            var buildContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();

            using (Container.Using(typeWorkCrDomain, buildContractTypeWorkDomain))
            {
                try
                {
                    var typeWorks = typeWorkCrDomain.GetAll()
                        .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                        .Where(x => x.ObjectCr.RealityObject.Municipality.Id == moId)
                        .Select(
                            x => new
                            {
                                id = x.ObjectCr.RealityObject.Id,
                                municipality = x.ObjectCr.RealityObject.Municipality.Name,
                                address = x.ObjectCr.RealityObject.Address,
                                sum_plan = x.Sum,
                                complete = x.PercentOfCompletion,
                                objectCrId = x.ObjectCr.Id,
                                typeWork = x
                            })
                        .ToList()
                        .GroupBy(x => x.id)
                        .Select(x => new
                        {
                            id = x.Key,
                            x.First().municipality,
                            x.First().address,
                            complete = decimal.Round(x.Average(y => y.complete.HasValue ? y.complete.Value : 0), 2),
                            sum_plan = x.Sum(y => y.sum_plan.HasValue ? y.sum_plan.Value : 0),
                            objectCrs = x.Select(y => y.objectCrId).Distinct().ToArray(),
                            typeWorkIds = x.Select(y => y.typeWork.Id).Distinct().ToArray(),
                            acts = x.Any(y => y.typeWork.HasActs())
                                ? (x.All(y => y.typeWork.IsCoveredByActs()) ? "Да" : "Частично")
                                : "Нет",
                            sum_act = x.Sum(y => y.typeWork.Acts.Sum(z => z.Sum.GetValueOrDefault(0))),
                            payments = x.All(y => y.typeWork.HasActs() &&
                                                  y.typeWork.IsCoveredByActs() &&
                                                  y.typeWork.Acts.All(z => z.IsPaid()))
                                ? "Да"
                                : x.All(y => y.typeWork.Acts.All(z => !z.Payments.Any() || !z.State.FinalState))
                                    ? "Нет"
                                    : "Частично"
                        })
                        .ToList();

                    var contracts = buildContractTypeWorkDomain.GetAll()
                        .Where(x => x.BuildContract.ObjectCr.ProgramCr.Id == programId)
                        .Where(x => x.BuildContract.ObjectCr.RealityObject.Municipality.Id == moId)
                        .Select(x => new { id = x.BuildContract.ObjectCr.RealityObject.Id, typeWorkId = x.TypeWork.Id })
                        .ToList()
                        .GroupBy(x => x.id)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.typeWorkId));

                    var result = typeWorks.Select(x => new
                    {
                        x.id,
                        x.address,
                        complete = string.Format("{0:0.00}", decimal.Round(x.complete, 2)),
                        sum_plan = string.Format("{0:0.00}", decimal.Round(x.sum_plan, 2)),
                        sum_act = string.Format("{0:0.00}", decimal.Round(x.sum_act, 2)),
                        contracts =
                            contracts.ContainsKey(x.id)
                                ? (x.typeWorkIds.All(contracts[x.id].Contains) ? "Да" : "Частично")
                                : "Нет",
                        x.payments,
                        x.acts
                    })
                    .OrderBy(x => x.address)
                    .ToList();

                    if (result.Any())
                    {
                        result.Insert(
                            0,
                            new
                            {
                                id = -1L,
                                address =
                                    typeWorks.FirstOrDefault() != null
                                        ? typeWorks.FirstOrDefault().municipality
                                        : string.Empty,
                                complete =
                                    string.Format(
                                        "{0:0.00}",
                                        typeWorks.Count > 0
                                            ? decimal.Round(typeWorks.SafeSum(x => x.complete) / typeWorks.Count, 2)
                                            : 0),
                                sum_plan =
                                    string.Format("{0:0.00}", decimal.Round(typeWorks.SafeSum(x => x.sum_plan), 2)),
                                sum_act =
                                    string.Format("{0:0.00}", decimal.Round(typeWorks.SafeSum(x => x.sum_act), 2)),
                                contracts =
                                    result.All(x => x.contracts == "Да")
                                        ? "Да"
                                        : result.All(x => x.contracts == "Нет") ? "Нет" : "Частично",
                                payments =
                                    result.All(x => x.payments == "Да")
                                        ? "Да"
                                        : result.All(x => x.payments == "Нет") ? "Нет" : "Частично",
                                acts =
                                    result.All(x => x.acts == "Да")
                                        ? "Да"
                                        : result.All(x => x.acts == "Нет") ? "Нет" : "Частично"
                            });
                    }

                    return Success(result);
                }
                catch (Exception)
                {
                    return Error("Произошла ошибка.");
                }
            }
        }

        public ActionResult GetCurrentCrMkd(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);
            var realtyId = baseParams.Params.GetAs<long>("house_id", ignoreCase: true);
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
            var archSmrDomain = Container.ResolveDomain<ArchiveSmr>();
            var realtyDomain = Container.ResolveDomain<RealityObject>();
            try
            {
                using (Container.Using(archSmrDomain, typeWorkCrDomain))
                {
                    var realty = realtyDomain.Get(realtyId);
                    if (realty == null)
                    {
                        return Error("Дом не найден");
                    }

                    return Success(GetCurrentCr(
                        realty.Address,
                        typeWorkCrDomain.GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == programId)
                            .Where(x => x.ObjectCr.RealityObject.Id == realtyId),
                        archSmrDomain.GetAll()
                            .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programId)
                            .Where(x => x.TypeWorkCr.ObjectCr.RealityObject.Id == realtyId)
                            .Where(x => x.DateChangeRec.HasValue)));
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetPlanWorkByHouse(BaseParams baseParams)
        {
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);
            var realtyId = baseParams.Params.GetAs<long>("house_id", ignoreCase: true);
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
            var realtyDomain = Container.ResolveDomain<RealityObject>();
            var buildContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();
            var structElemWorkDomain = Container.ResolveDomain<StructuralElementWork>();
            var typeWorkCrVersStage1Domain = Container.ResolveDomain<TypeWorkCrVersionStage1>();

            try
            {
                using (Container.Using(typeWorkCrDomain, realtyDomain, buildContractTypeWorkDomain,
                    structElemWorkDomain, typeWorkCrVersStage1Domain))
                {
                    var realty = realtyDomain.FirstOrDefault(x => x.Id == realtyId);
                    if (realty == null)
                    {
                        return Error("Дом не найден");
                    }

                    var works = typeWorkCrDomain.GetAll()
                        .Where(x => x.ObjectCr.RealityObject.Id == realtyId)
                        .ToList();

                    var programWorks = works.Where(x => x.ObjectCr.ProgramCr.Id == programId).ToArray();

                    var contracts =
                        buildContractTypeWorkDomain.GetAll()
                            .Where(x => x.BuildContract.ObjectCr.ProgramCr.Id == programId)
                            .Where(x => x.BuildContract.ObjectCr.RealityObject.Id == realtyId)
                            .Select(x => x.TypeWork.Id)
                            .ToList();

                    return Success(new
                    {
                        address = string.Format("{0}, {1}", realty.Municipality.Name, realty.Address),
                        programs = works.Select(x => x.ObjectCr.ProgramCr).DistinctBy(x => x.Id).Select(x => new
                        {
                            id = x.Id,
                            name = x.Name
                        }),
                        works = programWorks.Select(x => new
                        {
                            name = x.Work.Name,
                            work_id = x.Work.Id,
                            vol = string.Format("{0} {1}", x.Volume.HasValue ? decimal.Round(x.Volume.Value, 2) : 0, x.Work.UnitMeasure.Name),
                            price = string.Format("{0:0.00}", x.Sum.HasValue ? decimal.Round(x.Sum.Value, 2) : 0),
                            year = x.YearRepair.HasValue ? x.YearRepair.Value : -1
                        }),
                        sum_plan = string.Format("{0:0.00}", decimal.Round(programWorks.Sum(x => x.Sum.GetValueOrDefault(0)), 2)),
                        sum_payment = string.Format("{0:0.00}", decimal.Round(programWorks.Sum(x => x.GetPaidSumByActs()), 2)),
                        sum_act = string.Format("{0:0.00}", decimal.Round(programWorks.Sum(x => x.GetSumByActs()), 2)),
                        complete = string.Format("{0:0.00}", programWorks.Any()
                            ? decimal.Round(programWorks
                                .SafeSum(x => x.PercentOfCompletion.HasValue ? x.PercentOfCompletion.Value : 0M) / programWorks.Count(), 2)
                            : 0),
                        contracts =
                            contracts.Any()
                                ? (programWorks.All(x => contracts.Contains(x.Id)) ? "Да" : "Частично")
                                : "Нет",
                        acts = programWorks.Any(y => y.HasActs())
                            ? (programWorks.All(y => y.IsCoveredByActs()) ? "Да" : "Частично")
                            : "Нет",
                        payments = programWorks.All(y => y.HasActs() &&
                                                         y.IsCoveredByActs() &&
                                                         y.Acts.All(z => z.IsPaid()))
                            ? "Да"
                            : programWorks.All(y => y.Acts.All(z => !z.Payments.Any() || !z.State.FinalState))
                                ? "Нет"
                                : "Частично"
                    });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetCurrentCrHouseWork(BaseParams baseParams)
        {
            // Переменные передаются, но не используются
            var programId = baseParams.Params.GetAs<long>("program_id", ignoreCase: true);
            var realtyId = baseParams.Params.GetAs<long>("house_id", ignoreCase: true);
            var workId = baseParams.Params.GetAs<long>("work_id", ignoreCase: true);
            var typeWorkCrDomain = Container.ResolveDomain<TypeWorkCr>();
            var buildContractTypeWorkDomain = Container.ResolveDomain<BuildContractTypeWork>();
            try
            {
                using (Container.Using(typeWorkCrDomain, buildContractTypeWorkDomain))
                {
                    var typeWork = typeWorkCrDomain.GetAll()
                        .FirstOrDefault(x => x.ObjectCr.ProgramCr.Id == programId
                            && x.ObjectCr.RealityObject.Id == realtyId
                            && x.Work.Id == workId);

                    if (typeWork != null)
                    {
                        var hasContract = buildContractTypeWorkDomain.GetAll()
                            .Any(x => x.TypeWork.Work.Id == workId
                                && x.BuildContract.ObjectCr.RealityObject.Id == realtyId
                                && x.BuildContract.ObjectCr.ProgramCr.Id == programId);

                        return Success(new
                        {
                            work = typeWork.Work.Name,
                            sum_plan = string.Format("{0:0.00}", typeWork.Sum.HasValue ? decimal.Round(typeWork.Sum.Value, 2) : 0),
                            sum_act = string.Format("{0:0.00}", decimal.Round(typeWork.GetSumByActs(), 2)),
                            sum_payment = string.Format("{0:0.00}", decimal.Round(typeWork.GetPaidSumByActs(), 2)),
                            complete = string.Format("{0:0.00}", typeWork.PercentOfCompletion.HasValue ? decimal.Round(typeWork.PercentOfCompletion.Value, 2) : 0),
                            contracts = hasContract ? "Да" : "Нет",
                            acts = typeWork.HasActs() ? (typeWork.Sum == typeWork.GetSumByActs() ? "Да" : "Частично") : "Нет",
                            payments = typeWork.HasActs() && typeWork.Acts.All(z => z.IsPaid())
                                ? "Да"
                                : (typeWork.Acts.All(z => !z.Payments.Any() || !z.State.FinalState) ? "Нет" : "Частично")
                        });
                    }

                    return Error("Не найден вид работы");
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetMoList(BaseParams baseParams)
        {
            var moDomain = Container.ResolveDomain<Municipality>();
            try
            {
                using (Container.Using(moDomain))
                {
                    var result = moDomain.GetAll()
                        .Select(x => new { name = x.Name, id = x.Id })
                        .OrderBy(x => x.name)
                        .ToList();

                    if (result.Any())
                    {
                        result.Insert(0, new { name = GetRegionName(), id = 0L });
                    }

                    return Success(result);
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }


        public ActionResult GetHouseInfo(BaseParams baseParams)
        {
            var realtyId = baseParams.Params.GetAs<long>("house_id");
            var realtyDomain = Container.ResolveDomain<RealityObject>();
            var accountOpDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            using (Container.Using(realtyDomain, accountOpDomain))
            {
                try
                {
                    var realty = realtyDomain.Get(realtyId);
                    var operations =
                        accountOpDomain.GetAll()
                            .Where(x => x.Account.RealityObject.Id == realtyId)
                            .Where(x => x.Period.IsClosed).ToList();

                    var chargedTotal = operations.Sum(x => x.ChargedTotal);
                    var paidTotal = operations.Sum(x => x.PaidTotal);
                    if (realty == null)
                    {
                        return Error("Дом не найден");
                    }
                    return Success(new
                    {
                        address = string.Format("{0}, {1}", realty.Municipality.Name, realty.Address),
                        company = realty.ManOrgs,
                        year_built = realty.BuildYear,
                        floor = realty.MaximumFloors,
                        flat_number = realty.NumberApartments,
                        total_area = string.Format("{0:0.00}", realty.AreaMkd.HasValue ? decimal.Round(realty.AreaMkd.Value, 2) : 0),
                        deterioration = string.Format("{0:0.00}", decimal.Round(realty.PhysicalWear.GetValueOrDefault(0), 2)),
                        rate_payment = string.Format("{0:0.00}", chargedTotal != 0 ? decimal.Round(100 * paidTotal / chargedTotal, 2) : 0)
                    });
                }
                catch (Exception)
                {
                    return Error("Произошла ошибка");
                }
            }
        }

        public ActionResult GetDpkrOOI(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("mo_id");
            var beginYear = baseParams.Params.GetAs<int>("begin_year");
            var endYear = baseParams.Params.GetAs<int>("end_year");
            var domainService = Container.ResolveDomain<PublishedProgramRecord>();
            var versionRecordDomain = Container.ResolveDomain<VersionRecord>();

            try
            {
                using (Container.Using(domainService, versionRecordDomain))
                {
                    var dataPublished = domainService.GetAll()
                       .WhereIf(moId > 0,
                           x =>
                               x.PublishedProgram.ProgramVersion.IsMain &&
                               x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                       .Where(x => x.PublishedYear >= beginYear && x.PublishedYear <= endYear)
                       .Where(x => versionRecordDomain.GetAll().Where(y => y.ProgramVersion.IsMain).Select(y => y.Id).Contains(x.Stage2.Stage3Version.Id))
                       .Select(x => new
                       {
                           x.Sum,
                           x.CommonEstateobject,
                           MkdId = x.Stage2.Stage3Version.RealityObject.Id
                       })
                       .ToList()
                       .OrderBy(x => x.CommonEstateobject)
                       .GroupBy(x => x.CommonEstateobject)
                       .Select(x => new
                       {
                           work = x.Key,
                           mkd = x.Select(y => y.MkdId).ToList().Distinct().Count(),
                           sum_plan = string.Format("{0:0.00}", decimal.Round(x.Select(y => y.Sum).Sum(), 2))
                       });

                    return Success(dataPublished);
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetDpkrMKDList(BaseParams baseParams)
        {
            try
            {
                var moId = baseParams.Params.GetAs<long>("mo_id");

                var publishedDomain = Container.ResolveDomain<PublishedProgramRecord>();
                var versionRecordDomain = Container.ResolveDomain<VersionRecord>();

                using (Container.Using(publishedDomain, versionRecordDomain))
                {
                    var data =
                        versionRecordDomain.GetAll()
                            .WhereIf(moId > 0, x => x.ProgramVersion.Municipality.Id == moId)
                            .Where(x => x.ProgramVersion.IsMain)
                            .Where(x => publishedDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                            .Select(x => new
                            {
                                roId = x.RealityObject.Id,
                                city = x.RealityObject.FiasAddress.PlaceName,
                                street_number = x.RealityObject.GetLocalAddress()
                            })
                            .ToList()
                            .GroupBy(x => x.roId)
                            .Select(x => x.First())
                            .OrderBy(x => x.city)
                            .ThenBy(x => x.street_number);

                    return Success(data);
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetDpkrMKD(BaseParams baseParams)
        {
            var moId = baseParams.Params.GetAs<long>("mo_id");
            var beginYear = baseParams.Params.GetAs<int>("begin_year");
            var endYear = baseParams.Params.GetAs<int>("end_year");
            var domainService = Container.ResolveDomain<PublishedProgramRecord>();
            var realityObjectDecisionsService = Container.Resolve<IRealityObjectDecisionsService>();
            var versionRecordDomain = Container.ResolveDomain<VersionRecord>();

            try
            {
                using (Container.Using(domainService, realityObjectDecisionsService, versionRecordDomain))
                {
                    var roIds = realityObjectDecisionsService.GetRobjectsFundFormation((IQueryable<long>)null)
                        .Where(x => x.Value.FirstOrDefault() != null && x.Value.FirstOrDefault().Item2 == CrFundFormationDecisionType.RegOpAccount)
                        .Select(x => x.Key)
                        .ToArray();

                    var dataPublished = domainService.GetAll()
                       .Where(x => roIds.Contains(x.Stage2.Stage3Version.RealityObject.Id))
                       .WhereIf(moId > 0,
                           x =>
                               x.PublishedProgram.ProgramVersion.IsMain &&
                               x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                       .Where(x => x.PublishedYear >= beginYear && x.PublishedYear <= endYear)
                       .Select(x => new
                       {
                           x.Sum,
                           x.PublishedYear,
                           MkdId = x.Stage2.Stage3Version.RealityObject.Id
                       })
                       .AsEnumerable()
                       .OrderBy(x => x.PublishedYear)
                       .GroupBy(x => x.PublishedYear)
                       .Select(x => new
                       {
                           year = x.Key,
                           mkd = x.Select(y => y.MkdId).Distinct().Count(),
                           sum_plan = string.Format("{0:0.00}", decimal.Round(x.Select(y => y.Sum).Sum(), 2))
                       });

                    return Success(dataPublished);
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetRatePaymentMO(BaseParams baseParams)
        {
            var begin = baseParams.Params.GetAs<DateTime>("begin");
            var end = baseParams.Params.GetAs<DateTime>("end");
            var accountOpDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var moDomain = Container.ResolveDomain<Municipality>();
            var realityObjectDecisionsService = Container.Resolve<IRealityObjectDecisionsService>();
            var gkhParams = Container.Resolve<IGkhParams>().GetParams();
            var paymPenaltiesDomain = Container.ResolveDomain<PaymentPenalties>();

            var regionName = gkhParams.ContainsKey("RegionName") ? gkhParams["RegionName"].ToString() : string.Empty;
            using (Container.Using(moDomain, accountOpDomain, realityObjectDecisionsService, paymPenaltiesDomain))
            {
                try
                {
                    var allPeriods = GetPeriods(begin, end);

                    if (!allPeriods.Any())
                    {
                        return Error("Не найдены подходящие периоды");
                    }

                    var firstPaymPenalty = paymPenaltiesDomain.GetAll()
                        .Where(x => x.DecisionType == CrFundFormationDecisionType.RegOpAccount)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd >= DateTime.Now)
                        .Where(x => x.DateStart <= DateTime.Now)
                        .OrderByDescending(x => x.DateStart)
                        .FirstOrDefault();

                    var allowedDelay = firstPaymPenalty != null ? firstPaymPenalty.Days : 0;

                    var municipalities = moDomain.GetAll().ToArray();

                    var roIds = realityObjectDecisionsService.GetRobjectsFundFormation(((IQueryable<long>)null))
                        .Where(x => x.Value.FirstOrDefault() != null && x.Value.First().Item2 == CrFundFormationDecisionType.RegOpAccount)
                        .Select(x => x.Key)
                        .ToArray();

                    var allPeriodsIds = allPeriods.Select(x => x.Id).ToArray();
                    var closedPeriodIds = allPeriods.Where(x => x.IsClosed).Select(x => x.Id).ToHashSet();

                    var operationsByMunicipality = accountOpDomain.GetAll()
                        .Where(x => allPeriodsIds.Contains(x.Period.Id))
                        .Where(x => roIds.Contains(x.Account.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.Account.RealityObject.Id,
                            MuId = x.Account.RealityObject.Municipality.Id,
                            x.ChargedTotal,
                            x.PaidTotal,
                            x.Period
                        })
                        .ToList()
                        .Select(x => new
                        {
                            x.RoId,
                            x.MuId,
                            x.ChargedTotal,
                            x.PaidTotal,
                            PeriodId = x.Period.Id,
                            PeriodStart = x.Period.StartDate,
                            PeriodEnd = x.Period.GetEndDate()
                        })
                        .GroupBy(x => x.MuId)
                        .ToDictionary(x => x.Key, x =>
                        {
                            var arrearChargedTotal =
                                x.Where(z => z.PeriodEnd <= end.AddDays(-allowedDelay))
                                    .Sum(z => z.ChargedTotal);

                            var sumChargeClosePeriods = x.Where(z => closedPeriodIds.Contains(z.PeriodId)).Sum(z => z.ChargedTotal);
                            var sumPaidTotal = x.Sum(z => z.PaidTotal);

                            var countClosedPeriods =
                                x.Where(z => closedPeriodIds.Contains(z.PeriodId))
                                    .Distinct(z => z.PeriodId)
                                    .Count();

                            return new
                            {
                                arrear = Math.Max(arrearChargedTotal - sumPaidTotal, 0m),
                                average_monthly_accural =
                                    countClosedPeriods > 0
                                        ? sumChargeClosePeriods / countClosedPeriods
                                        : 0,
                                rate_payment =
                                    arrearChargedTotal != 0
                                        ? 100 * sumPaidTotal / arrearChargedTotal
                                        : 0
                            };
                        });

                    var operationsByRegion = accountOpDomain.GetAll()
                        .Where(x => allPeriodsIds.Contains(x.Period.Id))
                        .Where(x => roIds.Contains(x.Account.RealityObject.Id))
                        .Select(x => new
                        {
                            RoId = x.Account.RealityObject.Id,
                            MuId = x.Account.RealityObject.Municipality.Id,
                            x.ChargedTotal,
                            x.PaidTotal,
                            x.Period
                        })
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.RoId,
                            x.MuId,
                            x.ChargedTotal,
                            x.PaidTotal,
                            PeriodId = x.Period.Id,
                            PeriodStart = x.Period.StartDate,
                            PeriodEnd = x.Period.GetEndDate()
                        })
                        .ToArray();

                    var arrearChargedTotalRegion = operationsByRegion
                        .Where(y => y.PeriodEnd <= end.AddDays(-allowedDelay))
                        .Sum(x => x.ChargedTotal);

                    var sumChargeClosedPeriodRegion = operationsByRegion
                        .Where(x => closedPeriodIds.Contains(x.PeriodId))
                        .Sum(x => x.ChargedTotal);

                    var countClosedPeriodsRegion =
                        operationsByRegion.Where(x => closedPeriodIds.Contains(x.PeriodId))
                            .Distinct(z => z.PeriodId)
                            .Count();

                    var sumPaidTotalRegion = operationsByRegion.Sum(y => y.PaidTotal);

                    var arrearRegion = Math.Max(arrearChargedTotalRegion - sumPaidTotalRegion, 0m);

                    var averageMonthlyAccuralRegion =
                        countClosedPeriodsRegion > 0
                            ? sumChargeClosedPeriodRegion / countClosedPeriodsRegion
                            : 0;

                    var ratePaymentRegion = sumChargeClosedPeriodRegion != 0
                        ? 100 * sumPaidTotalRegion / sumChargeClosedPeriodRegion
                        : 0;

                    var data = municipalities
                        .Select(x => new
                        {
                            mo_id = x.Id,
                            mo = x.Name,
                            arrear = operationsByMunicipality.Get(x.Id).Return(z => z.arrear).RoundDecimal(2),
                            average_monthly_accural =
                                operationsByMunicipality.Get(x.Id)
                                    .Return(z => z.average_monthly_accural)
                                    .RoundDecimal(2),
                            rate_payment =
                                operationsByMunicipality.Get(x.Id).Return(z => z.rate_payment).RoundDecimal(2)
                        })
                        .Select(x => new
                        {
                            x.mo_id,
                            x.mo,
                            arrear = string.Format("{0:0.00}", x.arrear),
                            average_monthly_accural = string.Format("{0:0.00}", x.average_monthly_accural),
                            rate_payment = string.Format("{0:0.00}", x.rate_payment)
                        })
                        .OrderBy(x => x.mo)
                        .ToList();

                    if (data.Any())
                    {
                        data.Insert(0,
                            new
                            {
                                mo_id = 0L,
                                mo = regionName,
                                arrear = string.Format("{0:0.00}", decimal.Round(arrearRegion, 2)),
                                average_monthly_accural = string.Format("{0:0.00}", decimal.Round(averageMonthlyAccuralRegion, 2)),
                                rate_payment = string.Format("{0:0.00}", decimal.Round(ratePaymentRegion, 2))
                            });
                    }

                    return Success(data);
                }
                catch (Exception)
                {
                    return Error("Произошла ошибка.");
                }
            }
        }

        public ActionResult GetRatePaymentMKD(BaseParams baseParams)
        {
            var begin = baseParams.Params.GetAs<DateTime>("begin");
            var end = baseParams.Params.GetAs<DateTime>("end");
            var moId = baseParams.Params.GetAs<long>("mo_id");
            var accountOpDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var realtyDomain = Container.ResolveDomain<RealityObject>();
            var realityObjectDecisionsService = Container.Resolve<IRealityObjectDecisionsService>();
            var paymPenaltiesDomain = Container.ResolveDomain<PaymentPenalties>();

            using (Container.Using(realtyDomain, accountOpDomain, realityObjectDecisionsService, paymPenaltiesDomain))
            {
                try
                {
                    var allPeriods = GetPeriods(begin, end);
                    
                    if (!allPeriods.Any())
                    {
                        return Error("Не найдены подходящие периоды");
                    }

                    var firstPaymPenalty = paymPenaltiesDomain.GetAll()
                        .Where(x => x.DecisionType == CrFundFormationDecisionType.RegOpAccount)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd >= DateTime.Now)
                        .Where(x => x.DateStart <= DateTime.Now)
                        .OrderByDescending(x => x.DateStart)
                        .FirstOrDefault();

                    var allowedDelay = firstPaymPenalty.Return(x => x.Days);

                    var roIds = realityObjectDecisionsService.GetRobjectsFundFormation(((IQueryable<long>)null))
                        .Where(x => x.Value.FirstOrDefault() != null && x.Value.First().Item2 == CrFundFormationDecisionType.RegOpAccount)
                        .Select(x => x.Key)
                        .ToArray();

                    var realities = realtyDomain.GetAll()
                        .Where(x => roIds.Contains(x.Id))
                        .Where(x => x.Municipality.Id == moId)
                        .ToList();

                    var allPeriodsIds = allPeriods.Select(x => x.Id).ToArray();
                    var closedPeriodIds = allPeriods.Where(x => x.IsClosed).Select(x => x.Id).ToHashSet();

                    var roTotals = accountOpDomain.GetAll()
                        .Where(x => roIds.Contains(x.Account.RealityObject.Id))
                        .Where(x => allPeriodsIds.Contains(x.Period.Id))
                        .Select(x => new
                        {
                            RoId = x.Account.RealityObject.Id,
                            x.ChargedTotal,
                            x.PaidTotal,
                            x.Period
                        })
                        .ToList()
                        .Select(x => new
                        {
                            x.RoId,
                            x.ChargedTotal,
                            x.PaidTotal,
                            PeriodId = x.Period.Id,
                            PeriodStart = x.Period.StartDate,
                            PeriodEnd = x.Period.GetEndDate()
                        })
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, x =>
                        {
                            var arrearChargedTotal =
                                x.Where(z => z.PeriodEnd <= end.AddDays(-allowedDelay))
                                    .Sum(z => z.ChargedTotal);

                            var sumChargeClosePeriods = x.Where(z => closedPeriodIds.Contains(z.PeriodId)).Sum(z => z.ChargedTotal);
                            var sumPaidTotal = x.Sum(z => z.PaidTotal);

                            var countClosedPeriods =
                                x.Where(z => closedPeriodIds.Contains(z.PeriodId))
                                    .Distinct(z => z.PeriodId)
                                    .Count();

                            return new
                            {
                                arrear = Math.Max(arrearChargedTotal - sumPaidTotal, 0m),
                                average_monthly_accural =
                                    countClosedPeriods > 0
                                        ? sumChargeClosePeriods / countClosedPeriods
                                        : 0,
                                rate_payment =
                                    arrearChargedTotal != 0
                                        ? 100 * sumPaidTotal / arrearChargedTotal
                                        : 0
                            };
                        });

                    var result = realities
                        .Select(x => new
                        {
                            house_id = x.Id,
                            address = x.Address,
                            arrear = string.Format("{0:0.00}", roTotals.Get(x.Id).Return(z => z.arrear).RoundDecimal(2)),
                            average_monthly_accural = string.Format("{0:0.00}", roTotals.Get(x.Id).Return(z => z.average_monthly_accural).RoundDecimal(2)),
                            rate_payment = string.Format("{0:0.00}", roTotals.Get(x.Id).Return(z => z.rate_payment).RoundDecimal(2))
                        })
                        .OrderBy(x => x.address)
                        .ToList();

                    return Success(result);
                }
                catch (Exception)
                {
                    return Error("Произошла ошибка.");
                }
            }
        }

        public ActionResult GetMkdDpkr(BaseParams baseParams)
        {
            var realtyId = baseParams.Params.GetAs<long>("house_id");

            try
            {
                var recordDomain = Container.ResolveDomain<PublishedProgramRecord>();
                var realtyDomain = Container.ResolveDomain<RealityObject>();
                var roStructElemDomain = Container.ResolveDomain<RealityObjectStructuralElement>();

                using (Container.Using(recordDomain, realtyDomain, roStructElemDomain))
                {
                    var realty = realtyDomain.FirstOrDefault(x => x.Id == realtyId);

                    if (realty == null)
                    {
                        return Error("Дом не найден");
                    }

                    var ceoStructElemDict =
                        roStructElemDomain.GetAll()
                            .Where(x => x.RealityObject.Id == realtyId)
                            .OrderBy(x => x.LastOverhaulYear)
                            .Select(x => new
                            {
                                x.StructuralElement.Group.CommonEstateObject.Id,
                                x.LastOverhaulYear
                            })
                            .ToList()
                            .GroupBy(x => x.Id)
                            .ToDictionary(x => x.Key, x => x.Select(y => y.LastOverhaulYear).First());

                    var works = recordDomain.GetAll()
                        .Where(x => x.Stage2.Stage3Version.RealityObject.Id == realtyId)
                        .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                        .Select(x => new
                        {
                            ceoId = x.Stage2.CommonEstateObject.Id,
                            name = x.CommonEstateobject,
                            sum = string.Format("{0:0.00}", decimal.Round(x.Sum, 2)),
                            plan_year = x.PublishedYear
                        })
                        .ToList()
                        .Select(x => new
                        {
                            x.ceoId,
                            x.name,
                            x.sum,
                            x.plan_year,
                            last_year = ceoStructElemDict.ContainsKey(x.ceoId)
                                ? ceoStructElemDict[x.ceoId]
                                : 0
                        })
                        .ToList();

                    var config = Container.GetGkhConfig<OverhaulHmaoConfig>();

                    return Success(new
                    {
                        address = string.Format("{0}, {1}", realty.Municipality.Name, realty.Address),
                        begin = config.ProgrammPeriodStart,
                        end = config.ProgrammPeriodEnd,
                        works
                    });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetMoRatePayment(BaseParams baseParams)
        {
            var accountOpDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var moDomain = Container.ResolveDomain<Municipality>();
            var accOwnerDecisionDomain = Container.ResolveDomain<AccountOwnerDecision>();
            var realityObjectDecisionsService = Container.Resolve<IRealityObjectDecisionsService>();

            try
            {
                using (Container.Using(accountOpDomain, moDomain, accOwnerDecisionDomain, realityObjectDecisionsService))
                {
                    var municipalities = moDomain.GetAll()
                        .Select(x => new { x.Id, x.Name })
                        .ToDictionary(x => x.Id, y => y.Name);

                    var muIds = municipalities.Keys.ToList();
                    if (!muIds.Any())
                    {
                        return Error("Ни одного МО не существует");
                    }

                    var roIds =
                        realityObjectDecisionsService.GetRobjectsFundFormation(((IQueryable<long>)null))
                            .Where(
                                x =>
                                x.Value.FirstOrDefault() != null
                                && x.Value.FirstOrDefault().Item2 == CrFundFormationDecisionType.RegOpAccount)
                            .Select(x => x.Key)
                            .ToArray();

                    var muInfo =
                        accountOpDomain.GetAll()
                            .Select(
                                x =>
                                new
                                {
                                    MuId = x.Account.RealityObject.Municipality.Id,
                                    RoId = x.Account.RealityObject.Id,
                                    x.ChargedTotal,
                                    x.PaidTotal
                                })
                            .ToList()
                            .Where(x => roIds.Contains(x.RoId))
                            .GroupBy(x => x.MuId)
                            .ToDictionary(
                                x => x.Key,
                                y =>
                                    {
                                        var chargedTotal = y.Sum(x => x.ChargedTotal);
                                        var paidTotal = y.Sum(x => x.PaidTotal);
                                        var rating = chargedTotal != 0
                                                         ? decimal.Round(100 * paidTotal / chargedTotal, 2)
                                                         : 0;

                                        return
                                            new { ChargedTotal = chargedTotal, PaidTotal = paidTotal, Rating = rating };
                                    });

                    if (!muInfo.Any())
                    {
                        return Error("Нет информации по начислениям и оплатам");
                    }

                    var regionCharged = muInfo.Values.SafeSum(x => x.ChargedTotal);
                    var regionPaid = muInfo.Values.SafeSum(x => x.PaidTotal);
                    var regionRating = regionCharged != 0 ? decimal.Round(100 * regionPaid / regionCharged, 2) : 0;

                    var leader = muInfo.OrderByDescending(x => x.Value.Rating).FirstOrDefault();
                    var outsider = muInfo.OrderBy(x => x.Value.Rating).FirstOrDefault();

                    return
                        Success(
                            new
                            {
                                rate_payment_region = string.Format("{0:0.00}", regionRating),
                                leader =
                                    new
                                    {
                                        mo_id = leader.Key,
                                        mo = municipalities.Get(leader.Key),
                                        rate_payment = string.Format("{0:0.00}", leader.Value.Rating)
                                    },
                                outsider =
                                    new
                                    {
                                        mo_id = outsider.Key,
                                        mo = municipalities.Get(outsider.Key),
                                        rate_payment = string.Format("{0:0.00}", outsider.Value.Rating)
                                    }
                            });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetCurrentProgram(BaseParams baseParams)
        {
            var programCrDomain = Container.ResolveDomain<ProgramCr>();
            var typeWorkCr = Container.ResolveDomain<TypeWorkCr>();

            try
            {
                using (Container.Using(programCrDomain, typeWorkCr))
                {
                    var now = DateTime.Now;
                    var program = programCrDomain.FirstOrDefault(x => x.Period.DateStart < now &&
                        x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full &&
                        (x.TypeProgramStateCr == TypeProgramStateCr.Active ||
                        x.TypeProgramStateCr == TypeProgramStateCr.New ||
                        x.TypeProgramStateCr == TypeProgramStateCr.Open));
                    if (program == null)
                    {
                        return Error("Нет действующей программы");
                    }

                    var objectCrs = typeWorkCr.GetAll().Where(x => x.ObjectCr.ProgramCr.Id == program.Id).Select(x => new
                    {
                        x.ObjectCr.RealityObject.Id,
                        x.Sum
                    }).ToList();

                    return Success(new
                    {
                        id = program.Id,
                        name = program.Name,
                        sum_kr = Math.Ceiling(objectCrs.SafeSum(x => x.Sum.ToDecimal())),
                        objects_kr = objectCrs.Select(x => x.Id).Distinct().Count()
                    });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetCurrentProgramOOI(BaseParams baseParams)
        {
            try
            {
                var publishedDomain = Container.ResolveDomain<PublishedProgramRecord>();
                var versionRecordDomain = Container.ResolveDomain<VersionRecord>();

                using (Container.Using(publishedDomain, versionRecordDomain))
                {
                    var dataPublished = publishedDomain.GetAll()
                        .Select(x => new
                        {
                            x.Stage2.Stage3Version.Id,
                            x.Sum
                        })
                        .ToList()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => new
                        {
                            Sum = y.Select(z => z.Sum).Sum()
                        });

                    var newData =
                        versionRecordDomain.GetAll()
                            .Where(x => x.ProgramVersion.IsMain)
                            .Where(x => publishedDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                            .Select(x => new
                            {
                                x.Id,
                                Municipality = x.RealityObject.Municipality.Name,
                                RealityObject = x.RealityObject.Id,
                                CommonEstateobject = x.CommonEstateObjects
                            }).ToList().Select(x => new
                            {
                                x.RealityObject,
                                Sum = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id].Sum : 0m
                            })
                            .ToList();

                    var summary = Math.Ceiling(newData.Sum(x => x.Sum));
                    var cnt = newData.Count();
                    var mkd = newData.Select(x => x.RealityObject).Distinct().Count();

                    var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
                    return Success(new
                    {
                        begin = config.ProgrammPeriodStart,
                        end = config.ProgrammPeriodEnd,
                        sum_kr = summary,
                        mkd = mkd,
                        ooi = cnt
                    });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        public ActionResult GetTroubleMkd(BaseParams baseParams)
        {
            try
            {
                var realtyDomain = Container.ResolveDomain<RealityObject>();
                var roDecisionProtocolDomainService = Container.ResolveDomain<RealityObjectDecisionProtocol>();
                var crFundDecisionDomainService = Container.ResolveDomain<CrFundFormationDecision>();
                var accountOpDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
                var realityObjectDecisionsService = Container.Resolve<IRealityObjectDecisionsService>();
                var paymPenaltiesDomain = Container.ResolveDomain<PaymentPenalties>();


                using (Container.Using(realtyDomain, roDecisionProtocolDomainService,
                    crFundDecisionDomainService, accountOpDomain, realityObjectDecisionsService, paymPenaltiesDomain))
                {
                    var roIds = realityObjectDecisionsService.GetRobjectsFundFormation(((IQueryable<long>)null))
                        .Where(x => x.Value.FirstOrDefault() != null && x.Value.FirstOrDefault().Item2 == CrFundFormationDecisionType.RegOpAccount)
                        .Select(x => x.Key)
                        .ToArray();

                    var firstPaymPenalty = paymPenaltiesDomain.GetAll()
                        .Where(x => x.DecisionType == CrFundFormationDecisionType.RegOpAccount)
                        .Where(x => !x.DateEnd.HasValue || x.DateEnd >= DateTime.Now)
                        .Where(x => x.DateStart <= DateTime.Now)
                        .OrderByDescending(x => x.DateStart)
                        .FirstOrDefault();

                    var allowedDelay = firstPaymPenalty != null ? firstPaymPenalty.Days : 0;

                    var operationsByRegion = accountOpDomain.GetAll()
                        //.Where(x => x.Period != null && periodsIds.Contains(x.Period.Id))
                        .Where(x => x.Account != null)
                        .Where(x => x.Account.RealityObject != null)
                        .Where(x => roIds.Contains(x.Account.RealityObject.Id))
                        .ToList()
                        .Select(
                            x => new
                            {
                                RealityObjectId = x.Account.RealityObject.Id,
                                MunicipalityId = x.Account.RealityObject.Municipality.Id,
                                x.ChargedTotal,
                                x.PaidTotal,
                                PeriodId = x.Period.Id,
                                PeriodStart = x.Period.StartDate,
                                PeriodEnd = x.Period.EndDate ?? x.Period.StartDate.AddMonths(1).AddDays(-1)
                            })
                        .ToList();
                    var end = DateTime.Now;
                    var chargedTotalRegion = operationsByRegion
                        .Where(y => y.PeriodEnd >= DateTime.MinValue && y.PeriodEnd <= end.AddDays(-allowedDelay))
                        .Sum(y => y.ChargedTotal);
                    var paidTotalRegion = operationsByRegion
                        .Where(y => y.PeriodEnd >= DateTime.MinValue && y.PeriodEnd <= end)
                        .Sum(y => y.PaidTotal);

                    var arrearRegion = chargedTotalRegion - paidTotalRegion;

                    return Success(new
                    {
                        mkd = realtyDomain.GetAll().Count(x => x.ConditionHouse != ConditionHouse.Razed 
                            && x.TypeHouse != TypeHouse.Individual
                            && x.TypeHouse != TypeHouse.BlockedBuilding),
                        trouble_mkd = realtyDomain.GetAll().Count(x => x.ConditionHouse == ConditionHouse.Emergency
                            && x.TypeHouse != TypeHouse.Individual
                            && x.TypeHouse != TypeHouse.BlockedBuilding),
                        arrear = string.Format("{0:0.00}", decimal.Round(arrearRegion, 2))
                    });
                }
            }
            catch (Exception)
            {
                return Error("Произошла ошибка");
            }
        }

        private JsonNetResult Error(string message)
        {
            return new JsonNetResult(new { success = false, data = message });
        }

        private JsonNetResult Success(object data)
        {
            return new JsonNetResult(new { success = true, data });
        }

        private object GetCurrentCr(string title, IQueryable<TypeWorkCr> typeWorkCrsQuery,
            IQueryable<ArchiveSmr> archiveSmrsQuery)
        {
            var typeWorkCrs =
                typeWorkCrsQuery.Select(
                    x => new { name = x.Work.Name, id = x.Work.Id, complete = x.PercentOfCompletion })
                    .ToList()
                    .GroupBy(x => x.id)
                    .Select(
                        x =>
                        new
                        {
                            name = x.First().name,
                            complete = string.Format("{0:0.00}", decimal.Round(x.Average(y => y.complete.HasValue ? y.complete.Value : 0), 2))
                        })
                    .ToArray();

            var typeWorksCount = typeWorkCrsQuery.Count();

            var archSmrs =
                archiveSmrsQuery.Select(
                    x =>
                    new
                    {
                        date = x.DateChangeRec.Value,
                        month = new DateTime(x.DateChangeRec.Value.Year, x.DateChangeRec.Value.Month, 1),
                        workId = x.TypeWorkCr.Work.Id,
                        type_work_id = x.TypeWorkCr.Id,
                        complete = x.PercentOfCompletion.HasValue ? x.PercentOfCompletion.Value : 0
                    })
                    .ToList()
                    .GroupBy(x => new { x.month, x.type_work_id })
                    .Select(
                        x =>
                        new
                        {
                            month = x.Key.month,
                            typeWork = x.Key.type_work_id,
                            workId = x.Select(y => y.workId).First(),
                            total_complete = x.OrderByDescending(y => y.date).First().complete
                        })
                    .GroupBy(x => x.month)
                    .Select(
                        x =>
                        new
                        {
                            date = x.Key,
                            total_complete = string.Format("{0:0.00}", typeWorksCount != 0
                                ? decimal.Round(x.Sum(y => y.total_complete) / typeWorksCount, 2)
                                : 0)
                        });

            return new { mo = title, chart_data = archSmrs, work = typeWorkCrs };
        }

        private string GetRegionName()
        {
            var gkhParams = Container.Resolve<IGkhParams>().GetParams();
            return gkhParams.ContainsKey("RegionName") ? gkhParams["RegionName"].ToString() : string.Empty;
        }

        private ChargePeriod[] GetPeriods(DateTime begin, DateTime end)
        {
            var domain = Container.ResolveDomain<ChargePeriod>();
            using (Container.Using(domain))
            {
                return domain.GetAll()
                    .Where(x => x.StartDate <= end && (!x.EndDate.HasValue || x.EndDate >= begin))
                    .ToArray();
            }
        }
    }
}