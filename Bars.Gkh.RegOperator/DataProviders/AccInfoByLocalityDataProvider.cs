namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Analytics.Data;
    using B4.Modules.Analytics.Enums;
    using B4.Utils;
    using Castle.Windsor;
    using ConfigSections.RegOperator;
    using Entities;
    using Entities.PersonalAccount;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Utils;
    using Meta;

    public class AccInfoByLocalityDataProvider : BaseCollectionDataProvider<РеестрКвитанций>
    {
        public AccInfoByLocalityDataProvider(IWindsorContainer container)
            : base(container)
        {
        }

        protected override IQueryable<РеестрКвитанций> GetDataInternal(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAsId(string.Format("{0}_period", Key));
            var locality = baseParams.Params.GetAs<string>(string.Format("{0}_locality", Key));

            if (periodId == 0 || string.IsNullOrEmpty(locality))
            {
                throw new Exception("Необходимо заполнить все обязательные поля.");
            }

            var roDomain = Container.ResolveDomain<RealityObject>();
            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var persAccSummaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var rentPaymentInDomain = Container.ResolveDomain<RentPaymentIn>();
            var accumulatedFundsDomain = Container.ResolveDomain<AccumulatedFunds>();
            var saldoOutChangeDomain = Container.ResolveDomain<PeriodSummaryBalanceChange>();

            var config = Container.GetGkhConfig<RegOperatorConfig>();

            try
            {
                var representativeOnPrintReceipts = config.PaymentDocumentConfigContainer.PaymentDocumentConfigIndividual.RepresentativeOnPrintReceipts;

                var period = periodDomain.Get(periodId);

                var roQuery = roDomain.GetAll()
                    .Where(x => x.FiasAddress.PlaceName == locality);

                var rentPaymentQuery = rentPaymentInDomain.GetAll()
                    .Where(x => x.OperationDate >= period.StartDate)
                    .Where(x => !period.EndDate.HasValue || x.OperationDate <= period.EndDate);

                var accumFundsQuery = accumulatedFundsDomain.GetAll()
                    .Where(x => x.OperationDate >= period.StartDate)
                    .Where(x => !period.EndDate.HasValue || x.OperationDate <= period.EndDate);

                var saldoChangeQuery = saldoOutChangeDomain.GetAll()
                    .Where(y => y.ObjectCreateDate >= period.StartDate)
                    .Where(y => y.ObjectCreateDate <= period.EndDate);

                var saldoOutByRo =
                    persAccSummaryDomain.GetAll()
                        .Where(
                            x =>
                                roQuery.Any(y => y.Id == x.PersonalAccount.Room.RealityObject.Id) &&
                                x.PersonalAccount.State.StartState)
                        .Where(x => x.Period.Id == period.Id)
                        .Select(x => new
                        {
                            RoId = x.PersonalAccount.Room.RealityObject.Id,
                            periodIsOpen = !x.Period.IsClosed,
                            x.SaldoIn,
                            ChargeTotal = x.ChargedByBaseTariff + x.RecalcByBaseTariff, // TODO fix recalc
                            TotalPayment =

                                x.TariffPayment
                                + x.PenaltyPayment
                                + (rentPaymentQuery
                                    .Where(y => y.Account == x.PersonalAccount)
                                    .Sum(y => (decimal?) y.Sum) ?? 0)
                                + (accumFundsQuery
                                    .Where(y => y.Account == x.PersonalAccount)
                                    .Sum(y => (decimal?) y.Sum) ?? 0),
                            x.Penalty,
                            saldoChange = saldoChangeQuery
                                .Where(y => y.PeriodSummary.PersonalAccount == x.PersonalAccount)
                                .Sum(y => (decimal?) y.NewValue - y.CurrentValue) ?? 0
                        })
                        .Select(x => new
                        {
                            x.RoId,
                            SaldoOut =
                                x.periodIsOpen
                                    ? 0
                                    : x.SaldoIn + x.ChargeTotal + x.Penalty + x.saldoChange - x.TotalPayment
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.RoId)
                        .ToDictionary(x => x.Key, y => y.SafeSum(x => x.SaldoOut));

                var persInfoByRo = persAccDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.Room.RealityObject.Id) && x.State.StartState)
                    .Select(x => new {x.PersonalAccountNum, RoId = x.Room.RealityObject.Id})
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => new
                    {
                        Count = y.Count(),
                        AccNums = y.Select(x => x.PersonalAccountNum).AggregateWithSeparator(", ")
                    });



                return roQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.FiasAddress.PlaceName,
                        x.FiasAddress.StreetName,
                        x.FiasAddress.House
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var rec = new РеестрКвитанций
                        {
                            НаселенныйПункт = x.PlaceName,
                            Улица = x.StreetName,
                            Дом = x.House,
                            Сумма = saldoOutByRo.Get(x.Id),
                            Месяц = period.StartDate.ToString("MMMM"),
                            Год = period.StartDate.Year,
                            ПредставительОргИсполнителя = representativeOnPrintReceipts
                        };

                        if (persInfoByRo.ContainsKey(x.Id))
                        {
                            rec.ЛицевыеСчета = persInfoByRo[x.Id].AccNums;
                            rec.КоличесвоЛицСчетов = persInfoByRo[x.Id].Count;
                        }

                        return rec;
                    })
                    .AsQueryable();
            }
            finally
            {
                Container.Release(roDomain);
                Container.Release(persAccDomain);
                Container.Release(periodDomain);
                Container.Release(persAccSummaryDomain);
                Container.Release(rentPaymentInDomain);
                Container.Release(accumulatedFundsDomain);
                Container.Release(saldoOutChangeDomain);
            }
        }

        public override string Name
        {
            get { return "Реестр квитанций"; }
        }

        public override string Key
        {
            get { return typeof(AccInfoByLocalityDataProvider).Name; }
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
                        Name = string.Format("{0}_period", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Период",
                        Additional = "ChargePeriod",
                        Required = true
                    },

                    new DataProviderParam
                    {
                        Name = string.Format("{0}_locality", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Населенный пункт",
                        Additional = "Locality",
                        Required = true,

                    }
                };
            }
        }
    }
}
