namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для обработки данных по Объему привлеченных средств на ремонт и благоустройство
    /// </summary>
    public class FinActivityRepairSourceService : IFinActivityRepairSourceService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddWorkMode(BaseParams baseParams)
        {
            var service = Container.ResolveDomain<FinActivityRepairSource>();
            var disclosureInfoDomain = Container.ResolveDomain<DisclosureInfo>();

            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var records = baseParams.Params["records"]
                    .As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>().ReadClass<FinActivityRepairSource>())
                    .ToList();

                var existingSources = service.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .AsEnumerable()
                    .GroupBy(x => x.TypeSourceFundsDi)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var disclosureInfo = disclosureInfoDomain.GetAll().FirstOrDefault(x => x.Id == disclosureInfoId);

                foreach (var rec in records)
                {
                    FinActivityRepairSource existingSource = null;

                    if (existingSources.ContainsKey(rec.TypeSourceFundsDi))
                        existingSource = existingSources[rec.TypeSourceFundsDi];

                    if (existingSource != null)
                    {
                        existingSource.Sum = rec.Sum;

                        service.Update(existingSource);
                    }
                    else
                    {
                        var newFinActivityRepairSource = new FinActivityRepairSource
                        {
                            DisclosureInfo = disclosureInfo,
                            TypeSourceFundsDi = rec.TypeSourceFundsDi,
                            Sum = rec.Sum,
                        };

                        service.Save(newFinActivityRepairSource);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                Container.Release(service);
                Container.Release(disclosureInfoDomain);
            }
        }

        /// <inheritdoc />
        public IDataResult AddDataByRealityObj(BaseParams baseParams)
        {
            var discolsureInfoDomain = this.Container.ResolveDomain<DisclosureInfo>();
            var disclosureInfoRelationDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var service = this.Container.Resolve<IDomainService<FinActivityRepairSource>>();
            try
            {
                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");
                var disclosureInfo = discolsureInfoDomain.Get(disclosureInfoId);

                var disclosureInfoRealityObjList = disclosureInfoRelationDomain
                    .GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Select(x => x.DisclosureInfoRealityObj)
                    .ToList();

                var subsidies =
                    service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.Subsidy) ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.Subsidy,
                        DisclosureInfo = disclosureInfo
                    };
                var credit =
                    service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.Credit) ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.Credit,
                        DisclosureInfo = disclosureInfo
                    };
                var financeLeasingContract =
                    service.GetAll()
                        .FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.FinanceByContractLeasing)
                    ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.FinanceByContractLeasing,
                        DisclosureInfo = disclosureInfo
                    };
                var financeEnergServContract =
                    service.GetAll()
                        .FirstOrDefault(
                            x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.FinanceByContractEnergyService) ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.FinanceByContractEnergyService,
                        DisclosureInfo = disclosureInfo
                    };
                var occupantContribution =
                    service.GetAll()
                        .FirstOrDefault(
                            x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.PurposeContributionInhabitant) ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.PurposeContributionInhabitant,
                        DisclosureInfo = disclosureInfo
                    };
                var otherSource =
                    service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.OtherSource)
                    ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.OtherSource,
                        DisclosureInfo = disclosureInfo
                    };
                var summary =
                    service.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfoId && x.TypeSourceFundsDi == TypeSourceFundsDi.Summury)
                    ??
                    new FinActivityRepairSource
                    {
                        Id = 0,
                        TypeSourceFundsDi = TypeSourceFundsDi.Summury,
                        DisclosureInfo = disclosureInfo
                    };

                var firstIterationSubsidies = true;
                var firstIterationCredit = true;
                var firstIterationFinanceLeasingContract = true;
                var firstIterationFinanceEnergServContract = true;
                var firstIterationOccupantContr = true;
                var firstIterationOtherSource = true;
                foreach (var disclosureInfoRealityObj in disclosureInfoRealityObjList)
                {
                    if (disclosureInfoRealityObj.Subsidies.HasValue)
                    {
                        if (firstIterationSubsidies)
                        {
                            subsidies.Sum = 0m;
                            firstIterationSubsidies = false;
                        }

                        subsidies.Sum += disclosureInfoRealityObj.Subsidies.Value;
                    }

                    if (disclosureInfoRealityObj.Credit.HasValue)
                    {
                        if (firstIterationCredit)
                        {
                            credit.Sum = 0m;
                            firstIterationCredit = false;
                        }

                        credit.Sum += disclosureInfoRealityObj.Credit.Value;
                    }

                    if (disclosureInfoRealityObj.FinanceLeasingContract.HasValue)
                    {
                        if (firstIterationFinanceLeasingContract)
                        {
                            financeLeasingContract.Sum = 0m;
                            firstIterationFinanceLeasingContract = false;
                        }

                        financeLeasingContract.Sum += disclosureInfoRealityObj.FinanceLeasingContract.Value;
                    }

                    if (disclosureInfoRealityObj.FinanceEnergServContract.HasValue)
                    {
                        if (firstIterationFinanceEnergServContract)
                        {
                            financeEnergServContract.Sum = 0m;
                            firstIterationFinanceEnergServContract = false;
                        }

                        financeEnergServContract.Sum += disclosureInfoRealityObj.FinanceEnergServContract.Value;
                    }

                    if (disclosureInfoRealityObj.OccupantContribution.HasValue)
                    {
                        if (firstIterationOccupantContr)
                        {
                            occupantContribution.Sum = 0m;
                            firstIterationOccupantContr = false;
                        }

                        occupantContribution.Sum += disclosureInfoRealityObj.OccupantContribution.Value;
                    }

                    if (disclosureInfoRealityObj.OtherSource.HasValue)
                    {
                        if (firstIterationOtherSource)
                        {
                            otherSource.Sum = 0m;
                            firstIterationOtherSource = false;
                        }

                        otherSource.Sum += disclosureInfoRealityObj.OtherSource.Value;
                    }
                }

                var finActivityRepairSourceList = new List<FinActivityRepairSource>
                {
                    subsidies,
                    credit,
                    financeLeasingContract,
                    financeEnergServContract,
                    occupantContribution,
                    otherSource
                };

                summary.Sum = finActivityRepairSourceList.Any(x => x.Sum.HasValue) ? finActivityRepairSourceList.Sum(x => x.Sum ?? 0) : (decimal?)null;
                finActivityRepairSourceList.Add(summary);

                foreach (var finActivityRepairSource in finActivityRepairSourceList)
                {
                    if (finActivityRepairSource.Sum.HasValue)
                    {
                        if (finActivityRepairSource.Id == 0)
                        {
                            service.Save(finActivityRepairSource);
                        }
                        else
                        {
                            service.Update(finActivityRepairSource);
                        }
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(discolsureInfoDomain);
                this.Container.Release(disclosureInfoRelationDomain);
                this.Container.Release(service);
            }
        }
    }
}
