namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.RegOperator.DomainService.Period;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using ImportMaps;
    using Ionic.Zip;
    using Ionic.Zlib;
    using Mapping;
    using ProxyEntity;
    using RegOperator.Enums;
    using Repository;
    using System.Collections.Generic;
    using Bars.Gkh.Overhaul.Utils;
    using Bars.Gkh.RegOperator.Domain.ParametersVersioning;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    class SberbankExportTambovProvider : IExportDataProvider
    {
        private readonly IImportMap _mapper = new PersonalAccountSberProxyMap();

        public IWindsorContainer Container { get; set; }

        public IDomainService<CalculationParameterTrace> CalcParamTraceDomain { get; set; }

        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        public IDomainService<IndividualAccountOwner> IndividualAccountAccountOwnerDomain { get; set; }

        public ImportExportDataProvider ImportExportProvider { get; set; }

        public virtual IImportMap Mapper { get { return _mapper; } }

        public IDataResult<ExportOutput> GetData(BaseParams @params)
        {
            var persAccFilterService = Container.Resolve<IPersonalAccountFilterService>();
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var accDomain = Container.ResolveDomain<BasePersonalAccount>();
            var indownerDomain = Container.ResolveDomain<IndividualAccountOwner>();
            var accSummaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var paymentAgentDomain = Container.ResolveDomain<PaymentAgent>();
            var chargeDomain = Container.ResolveDomain<PersonalAccountCharge>();
            var chargePeriodRepo = Container.Resolve<IChargePeriodRepository>();

            try
            {

                // Фильтры в реестре вынесены в отдельный метод поскольку Выгрузки, и все действия делаются по отфильтрованному реестру через этот метод
                var queryByFilters = persAccFilterService.GetQueryableByFilters(@params, accDomain.GetAll());

                var periodId = @params.Params.GetAsId("periodId");
                var date = @params.Params.GetAs<DateTime>("date");

                // Сказали что все выгрузки в реестре ЛС должны работат ьодинаково неправильно
                // Поэтому сделал также как сделано для выгрузки в json и ФС_город

                ChargePeriod period;

                if (periodId > 0)
                {
                    period = periodDomain.Get(periodId);
                }
                else
                {
                    period =
                        chargePeriodRepo.GetPeriodByDate(date)
                        ?? chargePeriodRepo.GetFirstPeriod();
                }

                if (period == null)
                {
                    return new ExportOutputResult("Не удалось получить период") {Success = false};
                }
                
                var periodEndDate =
                    period.EndDate.HasValue
                        ? period.EndDate.Value
                        : period.StartDate.AddMonths(1).AddDays(-1);

                var daysInPeriod = (periodEndDate - period.StartDate).Days + 1;

                var accQuery = queryByFilters.Select(x => x.Id);

                var accounts = queryByFilters.Select(acc => new
                        {
                            AccountId = acc.Id,
                            acc.PersonalAccountNum,
                            acc.RoomNum,
                            acc.Address,
                            acc.PersAccNumExternalSystems,
                            acc.AreaShare,
                            acc.RoomArea,
                            acc.OwnerType,
                            acc.OpenDate,
                            acc.CloseDate,
                            acc.OwnerId
                        })
                    .ToList()
                    .GroupBy(x => x.AccountId)
                    .ToDictionary(x => x.Key, y => y.First());

                var persOwnerDict = IndividualAccountAccountOwnerDomain.GetAll()
                  .Select(x => new
                  {
                      x.Id,
                      x.Name,
                      x.SecondName,
                      x.Surname,
                      x.FirstName,
                  }).AsEnumerable()
                  .GroupBy(x => x.Id)
                  .ToDictionary(x => x.Key, y => y.First());

                var legalOwnerDict = LegalAccountOwnerDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Contragent.Inn,
                        x.Contragent.Kpp,
                        x.Contragent.Name
                    }).ToList()
                    .Select(x => new
                    {
                        x.Id,
                        Name = x.Name ?? persOwnerDict[x.Id].Name,
                        Kpp = x.Kpp ?? "",
                        Inn = x.Inn ?? ""
                    })
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var a = chargeDomain.GetAll()
                       // .Where(x => accounts.Keys.Contains(x.BasePersonalAccount.Id))
                       .Where(x => accQuery.Any(y => y == x.BasePersonalAccount.Id))
                        .Join(CalcParamTraceDomain.GetAll(),
                        y => y.Guid,
                        x => x.CalculationGuid,
                        (c, p) => new
                        {
                            p.ParameterValues,
                            p.DateStart,
                            p.DateEnd,
                            p.CalculationGuid,
                            p.CalculationType,
                            c.ChargeDate,
                            c.BasePersonalAccount.Id,
                            c.BasePersonalAccount.OpenDate
                        }).AsEnumerable()
                    .Where(x => x.CalculationType == CalculationTraceType.Charge)
                    .Where(x => x.ChargeDate >= x.OpenDate && x.ChargeDate >= period.StartDate.Date)
                    .Where(x => x.ChargeDate <= periodEndDate)
                    .Select(x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        Tariff = x.ParameterValues.Get(VersionedParameters.BaseTariff).ToDecimal(),
                        Share = x.ParameterValues.Get(VersionedParameters.AreaShare).ToDecimal(),
                        RoomArea = x.ParameterValues.Get(VersionedParameters.RoomArea).ToDecimal(),
                        accId = x.Id
                    }).ToList();

                var chargeList = a
                    .GroupBy(x => x.accId)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(z => new {z.DateStart, DateEnd = z.DateEnd ?? DateTime.MinValue})
                            .Select(r =>
                            {
                                var countDays = (r.Key.DateEnd - r.Key.DateStart).Days + 1;

                                var first = r.First();

                                var summary = r.Sum(q => q.RoomArea*q.Share*q.Tariff)*((decimal) countDays/daysInPeriod);

                                return new
                                {
                                    id = y.Key,
                                    Period = "{0} - {1}".FormatUsing(r.Key.DateStart.ToShortDateString(),
                                        r.Key.DateEnd.ToShortDateString()),
                                    first.Tariff,
                                    first.Share,
                                    first.RoomArea,
                                    Summary = summary,
                                    CountDays = countDays
                                };

                            }));

                const decimal penaltyConstant = (1M/300);

                var penalties = chargeDomain.GetAll()
                    //.Where(x => accounts.Keys.Contains(x.BasePersonalAccount.Id))
                  .Where(x => accQuery.Any(y => y == x.BasePersonalAccount.Id))
                     .Join(CalcParamTraceDomain.GetAll(),
                        y => y.Guid,
                        x => x.CalculationGuid,
                        (c, p) => new
                        {
                            p.ParameterValues,
                            p.DateStart,
                            p.DateEnd,
                            p.CalculationGuid,
                            p.CalculationType,
                            c.ChargeDate,
                            c.BasePersonalAccount.Id,
                            c.BasePersonalAccount.OpenDate
                        }).AsEnumerable()
                    .Where(x => x.CalculationType == CalculationTraceType.Penalty)
                    .Where(x => x.ChargeDate >= x.OpenDate && x.ChargeDate >= period.StartDate.Date)
                    .Where(x => x.ChargeDate <= periodEndDate)
                    .Select(x => new
                    {
                        x.DateStart,
                        x.DateEnd,
                        Percentage = x.ParameterValues.Get("payment_penalty_percentage").ToDecimal(),
                        PenaltyDebt = x.ParameterValues.Get("penalty_debt").ToDecimal().RegopRoundDecimal(2),
                        accId = x.Id
                    })
                    .Where(x => x.PenaltyDebt > 0)
                    .GroupBy(x => x.accId)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(z => new {z.DateStart, DateEnd = z.DateEnd ?? DateTime.MinValue})
                            .Select(r =>
                            {
                                var countDays = (r.Key.DateEnd - r.Key.DateStart).Days + 1;

                                var first = r.First();

                                var summary = r.Sum(q => countDays*q.PenaltyDebt)*penaltyConstant*
                                              first.Percentage.ToDivisional();

                                return new
                                {
                                    Period =
                                        "{0} - {1}".FormatUsing(r.Key.DateStart.ToShortDateString(),
                                            r.Key.DateEnd.ToShortDateString()),
                                    first.PenaltyDebt,
                                    first.Percentage,
                                    Summary = summary,
                                    CountDays = countDays
                                };
                            }).First());

                var result = new List<PersonalAccountInfoProxy>();

                foreach (var accObj in accounts)
                {
                    var accId = accObj.Key;

                    var acc = accObj.Value;

                    var penalty = penalties.ContainsKey(accId) ? penalties[accId] : null;
                    var charge = chargeList.ContainsKey(accId) ? chargeList[accId] : null;

                    if (charge != null && charge.Any())
                    {
                        result.AddRange(charge.Select(chargeData => new PersonalAccountInfoProxy
                        {
                            AccountNumber = acc.Return(x => x.PersonalAccountNum, string.Empty),
                            PersAccNumExternalSystems = acc.Return(x => x.PersAccNumExternalSystems, string.Empty),
                            Address = "{0}, кв. {1}".FormatUsing(acc.Return(x => x.Address, string.Empty), acc.Return(x => x.RoomNum, string.Empty)),
                            Surname = acc != null ?
                                acc.OwnerType == PersonalAccountOwnerType.Individual
                                    ? persOwnerDict.Get(acc.OwnerId).Return(x => x.Surname, string.Empty)
                                    : legalOwnerDict.Get(acc.OwnerId).Return(x => x.Inn, string.Empty) : string.Empty, 
                            Name = acc != null ?
                                acc.OwnerType == PersonalAccountOwnerType.Individual
                                    ? persOwnerDict.Get(acc.OwnerId).Return(x => x.FirstName, string.Empty)
                                    : legalOwnerDict.Get(acc.OwnerId).Return(x => x.Name, string.Empty) : string.Empty,
                            SecondName = acc != null ?
                                acc.OwnerType == PersonalAccountOwnerType.Individual
                                    ? persOwnerDict.Get(acc.OwnerId).Return(x => x.SecondName, string.Empty)
                                    : legalOwnerDict.Get(acc.OwnerId).Return(x => x.Kpp, string.Empty) : string.Empty,
                            ChargeTotal = chargeData.Return(x => x.Summary).RegopRoundDecimal(2),
                            Tariff = chargeData.Return(x => x.Tariff).RegopRoundDecimal(2),
                            Area = chargeData.Return(x => x.RoomArea).RegopRoundDecimal(2),
                            AreaShare = chargeData.Return(x => x.Share).RegopRoundDecimal(2),
                            CountDays = chargeData.Return(x => x.CountDays).ToString(),
                            PenaltyDebt = penalty != null ? penalty.Summary.RegopRoundDecimal(2) : 0,
                            DebtCountDays = penalty != null ? penalty.CountDays.ToString() : "0",
                            OwnerType = acc != null ? acc.OwnerType == PersonalAccountOwnerType.Individual ? "1" : "2" : string.Empty,
                            OpenDate = acc.Return(x => x.OpenDate, DateTime.MinValue),
                            CloseDate = acc !=null && acc.CloseDate.HasValue && acc.CloseDate.Value.Date != DateTime.MinValue ? acc.CloseDate.Value.ToShortDateString() : ""
                        }));
                    }
                    else
                    {
                        result.Add(new PersonalAccountInfoProxy
                        {
                            AccountNumber = acc.Return(x => x.PersonalAccountNum, string.Empty),
                            PersAccNumExternalSystems = acc.Return(x => x.PersAccNumExternalSystems, string.Empty),
                            Address = "{0}, кв. {1}".FormatUsing(acc.Return(x => x.Address, string.Empty), acc.Return(x => x.RoomNum, string.Empty)),
                            Surname = acc != null ?
                                acc.OwnerType == PersonalAccountOwnerType.Individual
                                    ? persOwnerDict[acc.OwnerId].Surname
                                    : legalOwnerDict[acc.OwnerId].Kpp : string.Empty,
                            Name = acc != null ?
                                acc.OwnerType == PersonalAccountOwnerType.Individual
                                    ? persOwnerDict[acc.OwnerId].FirstName
                                    : legalOwnerDict[acc.OwnerId].Name : string.Empty,
                            SecondName = acc != null ?
                                acc.OwnerType == PersonalAccountOwnerType.Individual
                                    ? persOwnerDict[acc.OwnerId].SecondName
                                    : legalOwnerDict[acc.OwnerId].Inn : string.Empty,
                            ChargeTotal = 0,
                            Tariff = 0,
                            Area = 0,
                            AreaShare = 0,
                            CountDays = "0",
                            PenaltyDebt = penalty != null ? penalty.Summary.RegopRoundDecimal(2) : 0,
                            DebtCountDays = penalty != null ? penalty.CountDays.ToString() : "0",
                            OwnerType =  acc != null ? acc.OwnerType == PersonalAccountOwnerType.Individual ? "1" : "2" : string.Empty,
                            OpenDate = acc.Return(x => x.OpenDate, DateTime.MinValue),
                            CloseDate = acc != null && acc.CloseDate.HasValue && acc.CloseDate.Value.Date != DateTime.MinValue ? acc.CloseDate.Value.ToShortDateString() : ""
                        });
                    }
                }

                var resultCharge = ImportExportProvider.Serialize(result, Mapper);
                var zipStream = new MemoryStream();


                var year = (period.StartDate.Year - 2000).ToString();
                var month = period.StartDate.Month.ToString();

                month = month.Length == 1 ? "0" + month : month;

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

                    return new ExportOutputResult()
                    {
                        Data = new ExportOutput()
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
                Container.Release(persAccFilterService);
                Container.Release(periodDomain);
                Container.Release(accDomain);
                Container.Release(indownerDomain);
                Container.Release(accSummaryDomain);
                Container.Release(paymentAgentDomain);
                Container.Release(chargeDomain);
                Container.Release(chargePeriodRepo);
            }
        }
    }
}
