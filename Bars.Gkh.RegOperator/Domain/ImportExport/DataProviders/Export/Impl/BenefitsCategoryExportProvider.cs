namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    using NHibernate.Linq;

    class BenefitsCategoryExportProvider : IExportDataProvider
    {
        private readonly IImportMap mapper = new BenefitsCategoryProxyMap();

        private Dictionary<long, PeriodSummaryProxy> allClosedPeriodSummary = new Dictionary<long, PeriodSummaryProxy>();

        private Dictionary<long, decimal> saldoChangeInfo = new Dictionary<long, decimal>();

        private Dictionary<long, decimal> periodSummaryInfo = new Dictionary<long, decimal>();

        public IWindsorContainer Container { get; set; }

        public ImportExportDataProvider ImportExportProvider { get; set; }

        public IImportMap Mapper
        {
            get
            {
                return this.mapper;
            }
        }

        public IDataResult<ExportOutput> GetData(BaseParams baseParams)
        {
            var periodDomain = this.Container.ResolveDomain<ChargePeriod>();
            var chargePeriodService = this.Container.Resolve<IChargePeriodRepository>();
            var basePersonalAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var persAccFilterService = this.Container.Resolve<IPersonalAccountFilterService>();

            try
            {
                var periodId = baseParams.Params.GetAsId("periodId");
                var date = baseParams.Params.GetAs<DateTime>("date");

                ChargePeriod period;

                if (periodId > 0)
                {
                    period = periodDomain.Get(periodId);
                }
                else
                {
                    period = chargePeriodService.GetPeriodByDate(date) ?? chargePeriodService.GetFirstPeriod();
                }

                if (period == null)
                {
                    return new ExportOutputResult("Не удалось получить период") { Success = false };
                }

                var result = new List<BenefitsCategoryInfoProxy>();

                var queryFilters = persAccFilterService.GetQueryableByFilters(baseParams, basePersonalAccDomain.GetAll())
                    .Where(x => x.OwnerType == PersonalAccountOwnerType.Individual)
                    .Select(x => x.Id).Distinct().ToList();

                var basePersonalAcc = basePersonalAccDomain.GetAll()
                    .Where(x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                    .Fetch(x => x.AccountOwner)
                    .ThenFetch(x => x.PrivilegedCategory)
                    .Fetch(x => x.Room)
                    .ThenFetch(x => x.RealityObject)
                    .ThenFetch(x => x.Municipality)
                    .Fetch(x => x.Room)
                    .ThenFetch(x => x.RealityObject)
                    .ThenFetch(x => x.FiasAddress);

                // выгружаем данные по частям
                const int MaxCountGet = 2000;
                
                // общее количество ЛС физических лиц
                var count = basePersonalAccDomain.GetAll().Count(x => x.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual);

                // количество обработанных записей
                var taked = 0;
                while (taked < count)
                {
                    var bpList = basePersonalAcc.Skip(taked).Take(MaxCountGet).ToList();
                    taked += MaxCountGet;

                    var accountIds = bpList.Select(x => x.AccountOwner.Id).Distinct().ToList();
                    this.InitDict(accountIds, period);

                    foreach (var perAccount in bpList)
                    {
                        if (!queryFilters.Contains(perAccount.Id))
                        {
                            continue;
                        }

                        var names = perAccount.AccountOwner.Name.Split(' ');
                        var surname = names.Length > 0 ? names[0] : string.Empty;
                        var name = names.Length > 1 ? names[1] : string.Empty;
                        var otch = names.Length > 2 ? names[2] : string.Empty;
                    
                        var ro = perAccount.Room.RealityObject;

                        var placename = ro.FiasAddress.PlaceName.Split(new[] { ' ' }, 2);
                        var typeCity = placename.Length > 0 ? placename[0] : string.Empty;
                        var city = placename.Length > 1 ? placename[1] : string.Empty;

                        var street = ro.FiasAddress.StreetName.Split(new[] { ' ' }, 2);
                        var typeStreet = street.Length > 0 ? street[0] : string.Empty;
                        var streetName = street.Length > 1 ? street[1] : string.Empty;

                        decimal? percent = perAccount.AccountOwner.PrivilegedCategory != null
                                          ? perAccount.AccountOwner.PrivilegedCategory.Percent
                                          : null;

                        var summs = this.CalcSumms(perAccount, percent);

                        var benefitInfo = new BenefitsCategoryInfoProxy
                                              {
                                                  Building = ro != null ? ro.FiasAddress.Building : string.Empty,
                                                  City = city,
                                                  Dats = period.Name,
                                                  HasDebt = summs.TotalDebt - summs.SumPaid > 0 ? "1" : "0",
                                                  FlatNum = perAccount.Room.RoomNum,
                                                  House = ro != null ? ro.FiasAddress.House : string.Empty,
                                                  HouseCode = ro != null ? ro.Id : 0L,
                                                  Housing = ro != null ? ro.FiasAddress.Housing : string.Empty,
                                                  Letter = ro != null ? ro.FiasAddress.Letter : string.Empty,
                                                  LiveArea = perAccount.Area.RegopRoundDecimal(2),
                                                  Lsa = perAccount.PersonalAccountNum,
                                                  MunicipalityName = ro != null ? ro.Municipality.Name : string.Empty,
                                                  Name = name,
                                                  Otch = otch,
                                                  Share = perAccount.AreaShare.RegopRoundDecimal(2),
                                                  Street = streetName,
                                                  Sum = summs.SumPaid,
                                                  SummaLg = summs.Credit.RegopRoundDecimal(2),
                                                  Surname = surname,
                                                  TypeCity = typeCity,
                                                  TypeStreet = typeStreet
                                              };

                        result.Add(benefitInfo);
                    }

                    bpList.Clear();
                    this.ClearDict();
                }
                
                var resultCharge = this.ImportExportProvider.Serialize(result, this.Mapper);
                var zipStream = new MemoryStream();

                var year = (period.StartDate.Year - 2000).ToString();
                var month = period.StartDate.Month.ToString().PadLeft(2, '0');
                
                using (var fileZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866")
                })
                {
                    fileZip.AddEntry(
                        "{0}{1}.dbf".FormatUsing(year, month),
                        resultCharge);

                    fileZip.Save(zipStream);

                    return new ExportOutputResult
                    {
                        Data = new ExportOutput
                        {
                            Data = zipStream,
                            OutputName = "{0}{1}.zip".FormatUsing(year, month)
                        },
                        Success = true
                    };
                }
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return null;
            }
            finally
            {
                this.Container.Release(persAccFilterService);
                this.Container.Release(periodDomain);
                this.Container.Release(chargePeriodService);
                this.Container.Release(basePersonalAccDomain);
            }
        }

        private void InitDict(List<long> accountIds, ChargePeriod period)
        {
            // Resolve domains
            var periodSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var saldoChangeDomain = this.Container.ResolveDomain<PeriodSummaryBalanceChange>();
            
            // Init dictionaries
            this.allClosedPeriodSummary = periodSummaryDomain.GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.IsClosed)
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(
                    x => x.Key,
                    x => new PeriodSummaryProxy
                                {
                                    ChargeTariff = x.Sum(y => y.ChargeTariff),
                                    ChargedByBaseTariff = x.Sum(y => y.ChargedByBaseTariff),
                                    Recalc = x.Sum(y => y.RecalcByBaseTariff + y.RecalcByDecisionTariff), // TODO fix recalc
                                    Penalty = x.Sum(y => y.Penalty)
                                });

            this.saldoChangeInfo = saldoChangeDomain.GetAll()
                .Where(x => accountIds.Contains(x.PeriodSummary.PersonalAccount.Id))
                .GroupBy(x => x.PeriodSummary.PersonalAccount.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.Sum(y => y.NewValue - y.CurrentValue));

            this.periodSummaryInfo = periodSummaryDomain.GetAll()
                .Where(x => accountIds.Contains(x.PersonalAccount.Id))
                .Where(x => x.Period.Id == period.Id)
                .GroupBy(x => x.PersonalAccount.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.ChargedByBaseTariff));

            // Release domains
            this.Container.Release(periodSummaryDomain);
            this.Container.Release(saldoChangeDomain);
        }

        private void ClearDict()
        {
            this.allClosedPeriodSummary.Clear();
            this.saldoChangeInfo.Clear();
            this.periodSummaryInfo.Clear();
        }

        private Summs CalcSumms(BasePersonalAccount account, decimal? percent)
        {
            // Получаем начиления по Закрытому периоду
            var cByBaseTariff = 0m;
            var cRecalc = 0m;
            var cTariff = 0m;
            var cPenalty = 0m;

            if (this.allClosedPeriodSummary.ContainsKey(account.Id))
            {
                var closedPeriodSum = this.allClosedPeriodSummary[account.Id];

                cByBaseTariff = closedPeriodSum.ChargedByBaseTariff;
                cRecalc = closedPeriodSum.Recalc;
                cTariff = closedPeriodSum.ChargeTariff;
                cPenalty = closedPeriodSum.Penalty;
            }
            
            var chargedTariff = cByBaseTariff + cRecalc;
            var chargedOwnerDecision = (cTariff - cByBaseTariff).ZeroIfBelowZero();

            // получаем оплаты из кошельков
            var pBaseTariff = account.BaseTariffWallet.InTransfers.Sum(x => x.Amount);
            var pSocialSupport = account.SocialSupportWallet.InTransfers.Sum(x => x.Amount);
            var pPenalty = account.PenaltyWallet.InTransfers.Sum(x => x.Amount);
            var pPreviousWork = account.PreviosWorkPaymentWallet.InTransfers.Sum(x => x.Amount);

            decimal saldoChange;
            this.saldoChangeInfo.TryGetValue(account.Id, out saldoChange);
            
            var totalDebt = (chargedTariff + cPenalty + chargedOwnerDecision)
                            - (pBaseTariff + pSocialSupport + pPenalty + pPreviousWork) + saldoChange;

            var credit = 0m;

            if (this.periodSummaryInfo.ContainsKey(account.Id))
            {
                credit += this.periodSummaryInfo[account.Id];
            }

            var sumPaid = percent.HasValue ? credit * percent.Value / 100 : 0M;

            return new Summs { Credit = credit, TotalDebt = totalDebt, SumPaid = sumPaid };
        }

        private class Summs
        {
            /// <summary>
            /// Сумма начисления
            /// </summary>
            public decimal Credit { get; set; }

            /// <summary>
            /// Итого задолженность
            /// </summary>
            public decimal TotalDebt { get; set; }

            /// <summary>
            /// Сумма к выплате
            /// </summary>
            public decimal SumPaid { get; set; }
        }

        private class PeriodSummaryProxy
        {
            public decimal ChargeTariff { get; set; }
            public decimal ChargedByBaseTariff { get; set; }
            public decimal Recalc { get; set; }
            public decimal Penalty { get; set; }
        }
    }
}
