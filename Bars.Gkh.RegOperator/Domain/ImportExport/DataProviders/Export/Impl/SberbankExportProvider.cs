namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.Gkh.Utils;

    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    using DomainService.PersonalAccount;

    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using ImportMaps;
    using Ionic.Zip;
    using Ionic.Zlib;
    using Mapping;
    using ProxyEntity;
    using Repository;

    public class SberbankExportProvider : IExportDataProvider
    {
        private readonly IImportMap _mapper = new PersonalAccountInfoSberProxyMap();
        //36-ричная система счисления
        private readonly string[] _notation36 =
        {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b",
            "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n",
            "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"
        };

        public IWindsorContainer Container { get; set; }

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
            var bankAccountProvider = Container.Resolve<IBankAccountDataProvider>();
            var roDomain = Container.ResolveDomain<RealityObject>();

            try
            {

                // Фильтры в реестре вынесены в отдельный метод поскольку Выгрузки, и все действия делаются по отфильтрованному реестру через этот метод
                var queryByFilters = persAccFilterService.GetQueryableByFilters(@params, accDomain.GetAll());
                
                var payAgentId = @params.Params.GetAsId("payAgentId");
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
                    return new ExportOutputResult("Не удалось получить период") { Success = false };
                }

                /*var dateStart = period.StartDate;
                var dateEnd = period.EndDate.HasValue
                    ? period.EndDate.Value
                    : period.StartDate.AddMonths(1).AddDays(-1);*/

                var paymentAgent = paymentAgentDomain.Get(payAgentId);

                if (paymentAgent.IsNull())
                {
                    return new ExportOutputResult()
                    {
                        Message = "Не найден платежный агент",
                        Success = false
                    };
                }

                var paIds = queryByFilters.Select(x => x.Id).ToArray();

                var summaries = accSummaryDomain.GetAll()
                    .Where(x => x.Period.Id == periodId)
                    .ToList()
                    .Where(x => paIds.Contains(x.PersonalAccount.Id))
                    .Select(x => new
                    {
                        x.PersonalAccount.Id,
                        x.ChargeTariff,
                        x.Penalty
                    })
                    .ToDictionary(x => x.Id, y => new { y.Penalty, y.ChargeTariff });

                var accounts = queryByFilters
                    .Where(x => chargeDomain.GetAll().Any(y => y.ChargeDate >= period.StartDate && y.BasePersonalAccount.Id == x.Id))
                    // поскольку сказали что экспорты должны работат ьодинаково неправильно
                    // то делаю также как в json И ФС_город
                    //.Where(x => x.OpenDate <= dateEnd)
                    //.Where(x => x.CloseDate <= DateTime.MinValue || x.CloseDate >= dateStart)
                    .Join(indownerDomain.GetAll(),
                        x => x.OwnerId,
                        y => y.Id,
                        (acc, own) => new
                        {
                            AccountId = acc.Id,
                            acc.PersonalAccountNum,
                            acc.RoomNum,
                            acc.Address,
                            own.Surname,
                            Name = own.FirstName,
                            own.SecondName,
                            acc.RoId
                        })
                    .ToArray();

                var calcAccountDict =
                    accounts.Select(x => x.RoId)
                            .Distinct()
                            .Split(999)
                            .SelectMany(
                                x =>
                                bankAccountProvider.GetBankNumbersForCollection(
                                    x.Select(y => new RealityObject { Id = y })))
                            .ToDictionary(x => x.Key, x => x.Value);

                var chargeData = accounts
                    .Select(x => new PersonalAccountInfoProxy
                    {
                        AccountNumber = x.PersonalAccountNum,
                        Address = "{0}, кв. {1}".FormatUsing(x.Address, x.RoomNum),
                        Surname = x.Surname,
                        Name = x.Name,
                        SecondName = x.SecondName,
                        ChargeTotal = summaries.Get(x.AccountId).Return(y => y.ChargeTariff).RegopRoundDecimal(2), // Задача округления до 2х знаков 44906
                        TypePayment = "01",
                        AddInfo2 = calcAccountDict.ContainsKey(x.RoId) ? calcAccountDict[x.RoId] : null
                    })
                    .ToList();

                var penaltyData = accounts
                    .Select(x => new PersonalAccountInfoProxy
                    {
                        AccountNumber = x.PersonalAccountNum,
                        Address = "{0}, кв. {1}".FormatUsing(x.Address, x.RoomNum),
                        Surname = x.Surname,
                        Name = x.Name,
                        SecondName = x.SecondName,
                        ChargeTotal = summaries.Get(x.AccountId).Return(y => y.Penalty).RegopRoundDecimal(2), // Задача округления до 2х знаков 44906
                        TypePayment = "02",
                        AddInfo2 = calcAccountDict.ContainsKey(x.RoId) ? calcAccountDict[x.RoId] : null
                    })
                    .ToList();

                //сие есть костыль, будет переделано
                var resultCharge = ImportExportProvider.Serialize(chargeData, Mapper);

                var resultPenalty = ImportExportProvider.Serialize(penaltyData, Mapper);

                var g = _notation36[DateTime.Today.Year - 2000];
                var m = _notation36[DateTime.Today.Month];
                var d = _notation36[DateTime.Today.Day];

                var zipStream = new MemoryStream();

                using (var fileZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866")
                })
                {
                    if (!paymentAgent.SumContractId.IsEmpty())
                    {
                        fileZip.AddEntry(
                            "{0}{1}{2}{3}00.dbf".FormatUsing(paymentAgent.SumContractId.PadLeft(3, '0'), g, m, d),
                            resultCharge);
                    }

                    if (!paymentAgent.PenaltyContractId.IsEmpty())
                    {
                        fileZip.AddEntry(
                            "{0}{1}{2}{3}00.dbf".FormatUsing(paymentAgent.PenaltyContractId.PadLeft(3, '0'), g, m, d),
                            resultPenalty);
                    }

                    fileZip.Save(zipStream);

                    return new ExportOutputResult()
                    {
                        Data = new ExportOutput()
                        {
                            Data = zipStream,
                            OutputName = "{0}{1}{2}00.zip".FormatUsing(g, m, d)
                        },
                        Success = true
                    };
                }
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
                Container.Release(bankAccountProvider);
                Container.Release(roDomain);
            }
        }
    }
}