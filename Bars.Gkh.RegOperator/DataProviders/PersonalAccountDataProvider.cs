namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Castle.Windsor;

    using System.Collections.Generic;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Gkh.Entities.RealEstateType;


    class PersonalAccountDataProvider : BaseCollectionDataProvider<PersonalAccountInfo>
    {
        public IDomainService<BasePersonalAccount> DomainService { get; set; }
        public IDomainService<ViewPersonalAccount> ViewAccountDomain  { get; set; }
        public IDomainService<PersonalAccountPeriodSummary> SummariesDomain { get; set; }
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        public IDomainService<RealityObject> RoDomain { get; set; }

        internal GkhCache Cache;
        private ChargePeriod period;

        private Dictionary<long, Dictionary<long, List<PaysizeRealEstateType>>> paysizeRetCache = new Dictionary<long, Dictionary<long, List<PaysizeRealEstateType>>>();
        private Dictionary<long, MonthlyFeeAmountDecision> monthlyFeeDecisions = new Dictionary<long, MonthlyFeeAmountDecision>();
        private Dictionary<long, List<long>> realEstTypesByRo = new Dictionary<long, List<long>>();
        private Dictionary<long, List<PaysizeRecord>> paysizeRecCache = new Dictionary<long, List<PaysizeRecord>>();

        private void WarmCache()
        {
            monthlyFeeDecisions = Cache.GetCache<MonthlyFeeAmountDecision>().GetEntities()
           .GroupBy(x => x.Protocol.RealityObject.Id)
           .ToDictionary(x => x.Key, y => y.OrderByDescending(x => x.Protocol.ProtocolDate).First());

            realEstTypesByRo = Cache.GetCache<RealEstateTypeRealityObject>().GetEntities()
                .GroupBy(x => x.RealityObject.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.RealEstateType.Id).ToList());

            paysizeRetCache = Cache.GetCache<PaysizeRealEstateType>().GetEntities()
                   .GroupBy(x => x.Record.Municipality.Id)
                   .ToDictionary(x => x.Key, y => y
                       .GroupBy(x => x.RealEstateType.Id)
                       .ToDictionary(x => x.Key, z => z.ToList()));

            paysizeRecCache = Cache.GetCache<PaysizeRecord>().GetEntities()
                  .GroupBy(x => x.Municipality.Id)
                  .ToDictionary(x => x.Key, y => y.ToList());
        }

        public PersonalAccountDataProvider(IWindsorContainer container) : base(container)
        {
            WarmCache();
        }

        protected override IQueryable<PersonalAccountInfo> GetDataInternal(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs(string.Format("{0}_municipalities", Key), string.Empty);
            var muIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            var periodId = baseParams.Params.GetAs<long>(string.Format("{0}_period", Key));

            var accNum = baseParams.Params.GetAs(string.Format("{0}_accountNum", Key), "");

            period = ChargePeriodDomain.GetAll().FirstOrDefault(x => x.Id == periodId);

            var accountsQuery = DomainService.GetAll()
                .WhereIf(accNum.Length > 0, x => x.PersonalAccountNum == accNum || x.PersAccNumExternalSystems == accNum)
                .WhereIf(muIds.Any(), x => muIds.Contains(x.Room.RealityObject.Municipality.Id));

            var accIdQuery = accountsQuery.Select(x => x.Id);

            var accView = ViewAccountDomain.GetAll().Where(x => accIdQuery.Any(z => z == x.Id)).ToList();

            var summaries = SummariesDomain
                .GetAll()
                .Where(x => x.Period.StartDate <= period.StartDate)
                .Where(x => accIdQuery.Any(z => z == x.PersonalAccount.Id))
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, y => y.OrderBy(z => z.Period.StartDate).ToList());

            var roAddressDict = RoDomain
                .GetAll()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.First().FiasAddress);

            //var casPayCenter = _cashPayCenterPersAccDomain.GetAll()
            //    .Where(x => x.DateStart <= DateTime.Now && (!x.DateEnd.HasValue || DateTime.Now >= x.DateEnd))
            //    .Select(x => x.CashPaymentCenter.Contragent.Name)
            //    .FirstOrDefault();


            var result = accView.Select(x =>
            {
                var chargedBaseTariff = summaries[x.Id].SafeSum(z => z.GetChargedByBaseTariff()).RegopRoundDecimal(2);
                var chargedDecisionTariff = summaries[x.Id].SafeSum(z => z.GetChargedByDecisionTariff()).RegopRoundDecimal(2);
                var chargedPenalty = summaries[x.Id].SafeSum(z => z.Penalty).RegopRoundDecimal(2);

                var paymentBaseTariff = summaries[x.Id].SafeSum(z => z.TariffPayment).RegopRoundDecimal(2);
                var paymentDecisionTariff = summaries[x.Id].SafeSum(z => z.TariffDecisionPayment).RegopRoundDecimal(2);
                var paymentPenalty = summaries[x.Id].SafeSum(z => z.PenaltyPayment).RegopRoundDecimal(2);

                return new PersonalAccountInfo
                {
                    ИдентификаторПлательщика = !string.IsNullOrEmpty(accNum) ? accNum : x.PersonalAccountNum,
                    ТипПлательщика = x.OwnerType == PersonalAccountOwnerType.Individual ? "1" : "2",
                    ФИО = x.AccountOwner,
                    НаименованиеРайона = x.Municipality,
                    НаселенныйПункт = x.PlaceName,
                    Улица = x.StreetName,
                    Дом = roAddressDict[x.RoId].House,
                    БукваДома = roAddressDict[x.RoId].Letter,
                    КорпусДома = roAddressDict[x.RoId].Building,
                    Секция = roAddressDict[x.RoId].Housing,
                    НомерКвартиры = x.RoomNum,
                    БукваКвартиры = "",
                    ПлощадьПомещения = x.Area,
                    Тариф = GetRobjectTariff(x.RoId, x.MuId, x.SettleId),
                    СальдоНаНачало = summaries[x.Id].Last().SaldoIn,
                    КрНачисленоЗаПериод = chargedBaseTariff + chargedDecisionTariff,
                    КрПерерасчет = summaries[x.Id].Last().RecalcByBaseTariff,
                    КрОплаченоЗаПериод = paymentBaseTariff + paymentDecisionTariff,
                    ПеняНачисленоЗаПериод = chargedPenalty,
                    ПеняПерерасчет  = 0,
                    ПеняОплаченоЗаПериод = paymentPenalty,
                    ОтчетныйПериод =  String.Format("{0}{1}",period.StartDate.Year,period.StartDate.Month),
                    ИнформационноеСообщение = ""
                };
            }).AsQueryable();

            return result;
        }

        protected decimal GetRobjectTariff(long roId, long muId, long? settlId)
        {
            var settlementId = settlId ?? 0L;

            var roDecision = monthlyFeeDecisions.Get(roId);

            if (roDecision != null && roDecision.Decision != null)
            {
                var current = roDecision.Decision
                    .Where(x => !x.To.HasValue || x.To >= DateTime.Today)
                    .FirstOrDefault(x => x.From <= DateTime.Today);

                if (current != null)
                {
                    return current.Value;
                }
            }

            var roTypes = realEstTypesByRo.Get(roId);

            var date = period.EndDate.HasValue ? period.EndDate.Value : DateTime.Now;

            return (GetPaysizeByType(roTypes, paysizeRetCache.Get(settlementId), date)
                    ?? GetPaysizeByType(roTypes, paysizeRetCache.Get(muId), date)
                    ?? GetPaysizeByMu(paysizeRecCache.Get(settlementId), date)
                    ?? GetPaysizeByMu(paysizeRecCache.Get(muId), date)
                    ?? 0).ToDecimal();
        }

        private decimal? GetPaysizeByType(IEnumerable<long> roTypes, IDictionary<long, List<PaysizeRealEstateType>> dict, DateTime date)
        {
            if (dict == null)
            {
                return null;
            }

            decimal? value = null;

            //получаем максимальный тариф по типу дома
            foreach (var roType in roTypes)
            {
                if (dict.ContainsKey(roType))
                {
                    if (dict[roType]
                        .Where(x => x.Record.Paysize.DateStart <= date)
                        .Any(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date))
                    {
                        value = Math.Max(value ?? 0,
                            dict[roType]
                                .Where(x => x.Record.Paysize.DateStart <= date)
                                .Where(x => !x.Record.Paysize.DateEnd.HasValue || x.Record.Paysize.DateEnd >= date)
                                .Select(x => x.Value)
                                .Max() ?? 0);
                    }
                }
            }

            return value;
        }

        private decimal? GetPaysizeByMu(IEnumerable<PaysizeRecord> list, DateTime date)
        {
            if (list == null)
            {
                return null;
            }

            return list
                .OrderByDescending(x => x.Paysize.DateStart)
                .Where(x => !x.Paysize.DateEnd.HasValue || x.Paysize.DateEnd.Value >= date)
                .FirstOrDefault(x => x.Paysize.DateStart <= date)
                .Return(x => x.Value);
        }


        public override IEnumerable<DataProviderParam> Params
        {
            get
            {
                return new[]
                {
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_municipality", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Муниципальное образование",
                        Additional = "Municipality",
                        Required = true,
                    },

                    new DataProviderParam
                    {
                        Name = string.Format("{0}_period", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Период",
                        Additional = "ChargePeriod",
                        Required = true,
                    },

                    new DataProviderParam
                    {
                        Name = string.Format("{0}_acccountNum", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Лицевой счет",
                        Additional = "AccountNum",
                        Required = true,
                    }
                };
            }
        }

        public override string Name
        {
            get { return "Реестр начислений"; }
        }

        public override string Description
        {
            get { return Name; }
        }
    }
}
