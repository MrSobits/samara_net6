namespace Bars.Gkh.RegOperator.Domain.ImportExport.DataProviders.Export.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
    using Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps;
    using Bars.Gkh.RegOperator.Domain.ImportExport.Mapping;
    using Bars.Gkh.RegOperator.Domain.ProxyEntity;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using Ionic.Zip;
    using Ionic.Zlib;

    using NHibernate.Linq;

    internal class PrivilegedCategoryExportProvider : IExportDataProvider
    {
        private const int StepSize = 2000;

        private readonly Dictionary<long, decimal> chargeDict = new Dictionary<long, decimal>(StepSize);

        private readonly Dictionary<long, decimal> debtDict = new Dictionary<long, decimal>(StepSize);

        private readonly IImportMap mapper = new PrivilegedCategoryProxyMap();

        public IWindsorContainer Container { get; set; }

        public ImportExportDataProvider ImportExportProvider { get; set; }

        public IImportMap Mapper
        {
            get
            {
                return mapper;
            }
        }

        public IDataResult<ExportOutput> GetData(BaseParams baseParams)
        {
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var chargePeriodService = Container.Resolve<IChargePeriodRepository>();
            var personalAccountPrivilegedCategoryDomain = Container.ResolveDomain<PersonalAccountPrivilegedCategory>();
            var personalAccountService = Container.Resolve<IPersonalAccountService>();

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

                var periodStart = period.StartDate;
                var periodEnd = period.EndDate ?? period.StartDate.AddMonths(1);

                var activePrivileges =
                    personalAccountPrivilegedCategoryDomain.GetAll()
                        .Fetch(x => x.PersonalAccount)
                        .ThenFetch(x => x.Room)
                        .ThenFetch(x => x.RealityObject)
                        .Where(x => x.PersonalAccount.AccountOwner.OwnerType == PersonalAccountOwnerType.Individual)
                        .Where(x => (x.DateFrom <= periodEnd) && (x.DateTo == null || x.DateTo >= periodStart))
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.DateFrom,
                            x.DateTo,
                            x.PrivilegedCategory.Percent,
                            x.PersonalAccount.PersonalAccountNum,
                            x.PersonalAccount.AccountOwner.Name,
                            Tariff = personalAccountService.GetTariff(x.PersonalAccount, periodStart),
                            x.PrivilegedCategory.LimitArea,
                            AddressCode = x.PersonalAccount.Return(y => y.Room.RealityObject.AddressCode, ""),
                            RoomNum = x.PersonalAccount.Return(y => y.Room.RoomNum, ""),
                            Area = x.PersonalAccount.Return(y => y.Room.Area),
                            x.PersonalAccount.AreaShare,
                            x.PersonalAccount.Id
                        })
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, x => x.ToArray());

                var result = new List<PrivilegedCategoryInfoProxy>();

                var dateprl = period.StartDate.AddMonths(1).ToString("dd.MM.yyyy");

                var count = activePrivileges.Count;
                var took = 0;
                while (took < count)
                {
                    var part = activePrivileges.Skip(took).Take(StepSize).ToArray();
                    took += StepSize;

                    InitDict(part.Select(x => x.Key).ToArray(), period.Id);

                    foreach (var pair in part)
                    {
                        var value = pair.Value[0];

                        var names = value.Name.Split(' ');
                        var surname = names.Length > 0 ? names[0] : string.Empty;
                        var name = names.Length > 1 ? names[1] : string.Empty;
                        var otch = names.Length > 2 ? names[2] : string.Empty;

                        var addressCode = ParseAddressCode(value.AddressCode);
                        var ropl = value.Area.RegopRoundDecimal(2);
                        var dolg = debtDict[pair.Key] > 0 ? "1" : "0";

                        foreach (var privilege in pair.Value)
                        {
                            var privilegeInfo = new PrivilegedCategoryInfoProxy
                            {
                                KOD_POST = addressCode[0],
                                KOD_NNASP = addressCode[1],
                                KOD_NYLIC = addressCode[2],
                                NDOM = addressCode[3],
                                NKORP = addressCode[4],
                                NKW = value.RoomNum,
                                NKOMN = addressCode[6],
                                NKOD = addressCode[7],
                                NKOD_PU = addressCode[8],
                                FAMIL = surname,
                                IMJA = name,
                                OTCH = otch,
                                ROPL = ropl,
                                DATLGTS1 = privilege.DateFrom.ToString("dd.MM.yyyy"),
                                DATLGTPO1 = privilege.DateTo.Return(x => x.Value.ToString("dd.MM.yyyy")),
                                DATEPRL1 = dateprl,
                                LSA = privilege.PersonalAccountNum,
                                DOLG = dolg,
                                SUML1 =
                                    CalcSuml(
                                        periodStart,
                                        periodEnd,
                                        privilege.DateFrom,
                                        privilege.DateTo,
                                        privilege.Percent ?? 0,
                                        privilege.Tariff, 
                                        privilege.LimitArea.HasValue
                                            ? Math.Min(privilege.LimitArea.Value, privilege.Area)
                                            : privilege.Area,
                                            privilege.AreaShare)
                                        .RegopRoundDecimal(2)
                            };  

                            result.Add(privilegeInfo);
                        }
                    }

                    part = null;
                    ClearDict();
                }

                var resultCharge = ImportExportProvider.Serialize(result, Mapper);
                var zipStream = new MemoryStream();

                var year = period.StartDate.Year.ToString(CultureInfo.InvariantCulture);
                var month = period.StartDate.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

                using (var fileZip = new ZipFile(Encoding.UTF8) { CompressionLevel = CompressionLevel.Level3, AlternateEncoding = Encoding.GetEncoding("cp866") })
                {
                    fileZip.AddEntry("LGT-{0}-{1}.dbf".FormatUsing(year, month), resultCharge);

                    fileZip.Save(zipStream);

                    return new ExportOutputResult { Data = new ExportOutput { Data = zipStream, OutputName = "LGT-{0}-{1}.zip".FormatUsing(year, month) }, Success = true };
                }
            }
            catch (Exception ex)
            {
                return new ExportOutputResult(ex.Message) { Success = false };
            }
            finally
            {
                Container.Release(periodDomain);
                Container.Release(chargePeriodService);
                Container.Release(personalAccountPrivilegedCategoryDomain);
            }
        }

        private void InitDict(long[] accountIds, long periodId)
        {
            var summaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            try
            {
                var summaries = summaryDomain.GetAll().Where(x => accountIds.Contains(x.PersonalAccount.Id)).GroupBy(x => x.PersonalAccount.Id).ToDictionary(x => x.Key, x => x.ToArray());
                foreach (var accountId in accountIds)
                {
                    var accountSummary = summaries.Get(accountId);
                    if (accountSummary == null)
                    {
                        debtDict[accountId] = 0;
                        chargeDict[accountId] = 0;
                        continue;
                    }

                    debtDict[accountId] = accountSummary.SafeSum(x => x.GetTotalCharge() - x.GetTotalPayment());
                    chargeDict[accountId] = accountSummary.FirstOrDefault(x => x.Period.Id == periodId).Return(x => x.GetTotalCharge());
                }
            }
            finally
            {
                Container.Release(summaryDomain);
            }
        }

        private void ClearDict()
        {
            debtDict.Clear();
            chargeDict.Clear();
        }

        private decimal CalcSuml(
            DateTime periodFrom,
            DateTime periodTo,
            DateTime dateFrom,
            DateTime? dateTo,
            decimal percent,
            decimal tariff,
            decimal area,
            decimal areaShare)
        {
            if (percent == 0)
            {
                return 0;
            }

            if (dateFrom < periodFrom)
            {
                dateFrom = periodFrom;
            }

            if (!dateTo.HasValue || dateTo > periodTo)
            {
                dateTo = periodTo;
            }

            var days = (dateTo.Value - dateFrom).Days + 1;
            var totalDays = DateTime.DaysInMonth(periodFrom.Year, periodFrom.Month);

            return (days * (tariff * area * areaShare) / totalDays) * (percent / 100);
        }

        private string[] ParseAddressCode(string addressCode)
        {
            var result = new[] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

            if (addressCode.IsEmpty())
            {
                return result;
            }

            if (addressCode.Length <= 2)
            {
                return result;
            }

            result[0] = addressCode.Substring(0, 3);

            if (addressCode.Length <= 7)
            {
                return result;
            }

            result[1] = addressCode.Substring(3, 5);

            if (addressCode.Length <= 12)
            {
                return result;
            }

            result[2] = addressCode.Substring(8, 5);

            if (addressCode.Length <= 16)
            {
                return result;
            }

            result[3] = addressCode.Substring(13, 4);

            if (addressCode.Length <= 19)
            {
                return result;
            }

            result[4] = addressCode.Substring(17, 3);

            if (addressCode.Length <= 22)
            {
                return result;
            }

            result[5] = addressCode.Substring(20, 3);

            if (addressCode.Length <= 25)
            {
                return result;
            }

            result[6] = addressCode.Substring(23, 3);

            if (addressCode.Length <= 29)
            {
                return result;
            }

            result[7] = addressCode.Substring(26, 4);

            if (addressCode.Length <= 31)
            {
                return result;
            }

            result[8] = addressCode.Substring(30, 2);

            return result;
        }
    }
}