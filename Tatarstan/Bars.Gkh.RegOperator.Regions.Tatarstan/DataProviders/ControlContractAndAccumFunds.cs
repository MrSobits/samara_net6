using System;
using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Analytics.Data;
using Bars.B4.Modules.Analytics.Enums;
using Bars.B4.Utils;
using Bars.Gkh.Domain;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.DomainService;
using Bars.Gkh.RegOperator.DataProviders.Meta;
using Bars.Gkh.RegOperator.Entities;
using Bars.GkhRf.Entities;
using Castle.Windsor;

namespace Bars.Gkh.RegOperator.Regions.Tatarstan.DataProviders
{
    public class ControlContractAndAccumFunds : BaseCollectionDataProvider<ИнформацияПоДоговорамИНакопленнымСредствам>
    {
        public ControlContractAndAccumFunds(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<ИнформацияПоДоговорамИНакопленнымСредствам> GetDataInternal(BaseParams baseParams)
        {
            var realObjProgramVersion = Container.Resolve<IRealityObjectsProgramVersion>();
            var manOrgContractRoDomain = Container.ResolveDomain<ManOrgContractRealityObject>();
            var contractRfDomain = Container.ResolveDomain<ContractRf>();
            var contractRfObjectDomain = Container.ResolveDomain<ContractRfObject>();
            var chargePeriodDomain = Container.ResolveDomain<ChargePeriod>();
            var roChargeAccOperDomain = Container.ResolveDomain<RealityObjectChargeAccountOperation>();
            var muDomain = Container.ResolveDomain<Municipality>();

            var muIds = baseParams.Params[string.Format("{0}_muIds", Key)].ToStr().ToLongArray();
            var periods = baseParams.Params[string.Format("{0}_periods", Key)].ToStr().ToLongArray();
            var typeContractManOrg = baseParams.Params.GetAs<TypeContractManOrg>(string.Format("{0}_typeContract", Key));

            var chargePeriods = chargePeriodDomain.GetAll().WhereIf(periods.Any(), x => periods.Contains(x.Id)).ToList();
            var roInDpkrQuery = realObjProgramVersion.GetMainVersionRealityObjects();
            var roQuery = roInDpkrQuery
                .WhereIf(muIds.Any(), x => muIds.Contains(x.Municipality.Id));

            var currentDate = DateTime.Now;

            var municipalities = muDomain.GetAll().WhereIf(muIds.Any(), x => muIds.Contains(x.Id)).OrderBy(x => x.Name).ToList();

            var manOrgContractRealityObjectQuery = manOrgContractRoDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Where(x => x.ManOrgContract.StartDate <= currentDate)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= currentDate)
                    .WhereIf(typeContractManOrg != 0 , x => x.ManOrgContract.TypeContractManOrgRealObj == typeContractManOrg)
                    .WhereIf(typeContractManOrg == TypeContractManOrg.DirectManag, x => x.ManOrgContract.ManagingOrganization == null);

            var roAreaByMu = manOrgContractRealityObjectQuery
                    .Select(x => new
                    {  
                        x.RealityObject.AreaLivingNotLivingMkd,
                        MunicipalityId = x.RealityObject.Municipality.Id,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.MunicipalityId)
                    .ToDictionary(x => x.Key, y => y.SafeSum(x => x.AreaLivingNotLivingMkd.ToDecimal()));

            var manOrgRealObjByMu = manOrgContractRealityObjectQuery
                .Select(x => new
                {
                    ManOrgId = (long?)x.ManOrgContract.ManagingOrganization.Id,
                    RoId = x.RealityObject.Id,
                    MuId = x.RealityObject.Municipality.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, y => new
                {
                    ManOrgCount = y.Where(x => x.ManOrgId.HasValue).Select(x => x.ManOrgId).Distinct().Count(),
                    RoCount = y.Select(x => x.RoId).Distinct().Count()
                });

            var moWithContract = manOrgContractRealityObjectQuery
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Join(contractRfDomain.GetAll(), x => x.ManOrgContract.ManagingOrganization.Id,
                    y => y.ManagingOrganization.Id,
                    (x, y) => new
                    {
                        ManOrgId = y.ManagingOrganization.Id,
                        y.DocumentDate,
                        DateBegin = y.DateBegin.HasValue ? y.DateBegin.Value : DateTime.MinValue,
                        DateEnd = y.DateEnd.HasValue ? y.DateEnd.Value : DateTime.MaxValue,
                        MuId = x.RealityObject.Municipality.Id
                    })
                .AsEnumerable()
                .Where(
                    x =>
                        chargePeriods.Any(
                            y => (y.StartDate > x.DateBegin && x.DateEnd == DateTime.MaxValue) /* Если дата начала началась раньше открытия периода и договор продолжал действовать во время периода*/
                            || (y.StartDate <= x.DateBegin && (!y.EndDate.HasValue || y.EndDate >= x.DateBegin)) /*Если дата начала дейтствия попадает в период начислений*/
                            || (y.StartDate <= x.DateEnd && (!y.EndDate.HasValue || y.EndDate >= x.DateEnd)) /*Если дата окончания дейтствия попадает в период начислений*/
                            ))
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.ManOrgId).Distinct().Count());


            var roWithContract = contractRfObjectDomain.GetAll()
                .Where(x => manOrgContractRealityObjectQuery.Any(y => y.RealityObject.Id == x.RealityObject.Id
                    && y.ManOrgContract.ManagingOrganization.Id == x.ContractRf.ManagingOrganization.Id))
                .Select(x => 
                    new {
                          RoId = x.RealityObject.Id,
                          MuId = x.RealityObject.Municipality.Id,
                          x.IncludeDate,
                          x.ExcludeDate,
                          DateBegin = x.ContractRf.DateBegin.HasValue ? x.ContractRf.DateBegin.Value : DateTime.MinValue,
                          DateEnd = x.ContractRf.DateEnd.HasValue ? x.ContractRf.DateEnd.Value : DateTime.MaxValue,
                        })
                .AsEnumerable()
                .Where(
                    x => // нам необходимо получить только те дома, которые попали под условия для столбца КоличествоУОДоговор
                        chargePeriods.Any(
                            y => (y.StartDate > x.DateBegin && x.DateEnd == DateTime.MaxValue) /* Если дата начала началась раньше открытия периода и договор продолжал действовать во время периода*/
                            || (y.StartDate <= x.DateBegin && (!y.EndDate.HasValue || y.EndDate >= x.DateBegin)) /*Если дата начала дейтствия попадает в период начислений*/
                            || (y.StartDate <= x.DateEnd && (!y.EndDate.HasValue || y.EndDate >= x.DateEnd)) /*Если дата окончания дейтствия попадает в период начислений*/
                            ) 
                        &&
                        // Также накладываем условия на даты включения потомучто так требвоалось по требваонию 50636
                        chargePeriods.Any(y => (x.IncludeDate <= y.EndDate && (!x.ExcludeDate.HasValue || x.ExcludeDate >= y.EndDate))
                            || (x.IncludeDate <= y.StartDate && (!x.ExcludeDate.HasValue || x.ExcludeDate >= y.StartDate))))
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.RoId).Distinct().Count());

            var chargeInfoByMu = roChargeAccOperDomain.GetAll()
                .Where(x => manOrgContractRealityObjectQuery.Any(y => y.RealityObject.Id == x.Account.RealityObject.Id))
                .WhereIf(periods.Any(), x => periods.Contains(x.Period.Id))
                .Select(x => new
                {
                    MuId = x.Account.RealityObject.Municipality.Id,
                    x.ChargedTotal,
                    x.PaidTotal
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key, y => new
                {
                    ChargedTotal = y.SafeSum(x => x.ChargedTotal),
                    PaidTotal = y.SafeSum(x => x.PaidTotal)
                });

            var result = new List<ИнформацияПоДоговорамИНакопленнымСредствам>();
            foreach (var mu in municipalities)
            {
                var rec = new ИнформацияПоДоговорамИНакопленнымСредствам
                {
                    МуниципальноеОбразование = mu.Name,
                    СуммаСобираемости = (roAreaByMu.Get(mu.Id) * 5 * chargePeriods.Count).RoundDecimal(2)
                };

                if (manOrgRealObjByMu.ContainsKey(mu.Id))
                {
                    rec.КоличествоДомов = manOrgRealObjByMu[mu.Id].RoCount;
                    rec.КоличествоУО = manOrgRealObjByMu[mu.Id].ManOrgCount;
                }

                rec.КоличествоУОДоговор = moWithContract.Get(mu.Id);
                rec.КоличествоДомовДоговор = roWithContract.Get(mu.Id);

                if (chargeInfoByMu.ContainsKey(mu.Id))
                {
                    rec.Начисления = chargeInfoByMu[mu.Id].ChargedTotal.RoundDecimal(2);
                    rec.Сумма = chargeInfoByMu[mu.Id].PaidTotal.RoundDecimal(2);
                }

                result.Add(rec);
            }

            return result.AsQueryable();
        }

        public override string Name
        {
            get { return "Контроль заключенных договоров и накопленных средств"; }
        }

        public override string Key
        {
            get { return typeof(ControlContractAndAccumFunds).Name; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get
            {
                return new List<DataProviderParam>
                {
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_muIds", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Муниципальные образования",
                        Additional = "MunicipalityMulti"
                    },
                     new DataProviderParam
                    {
                        Name = string.Format("{0}_periods", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Период",
                        Additional = "ChargePeriodMulti"
                    },
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_typeContract", Key),
                        ParamType = ParamType.Enum,
                        Label = "Вид управления",
                        Additional = "TypeContractManOrg",
                        Required = true
                    }
                };
            }
        }
    }
}
