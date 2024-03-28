namespace Bars.Gkh.RegOperator.DomainService.Import.Ches
{
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.File;
    using Bars.Gkh.RegOperator.Imports.Ches.PreImport;
    using Bars.Gkh.RegOperator.Report;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Dapper;

    using NHibernate.Linq;

    using ChesImport = Bars.Gkh.RegOperator.Entities.Import.Ches.ChesImport;

    /// <summary>
    /// Сервис для работы с данными Импорта ЧЭС
    /// </summary>
    public class ChesImportService : IChesImportService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<ChargePeriod> PeriodDomain { get; set; }

        public IDomainService<ChesImport> ChesImportDomain { get; set; }

        public IDomainService<ChesImportFile> ChesImportFileDomain { get; set; }

        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IChesTempDataProviderBuilder ProviderBuilder { get; set; }

        public IBillingFilterService BillingFilterService { get; set; }

        public IFileManager FileManager { get; set; }

        private readonly bool IsBufferedQuery = false;
        
        /// <inheritdoc />
        public IDataResult ListChargeInfo(BaseParams baseParams)
        {
            var summaryData = this.GetData(baseParams, FileType.Calc);

            return new ListSummaryResult(
                new[]
                {
                    new ChargeSummaryProxy
                    {
                        WalletType = WalletType.BaseTariffWallet,
                        SaldoIn = summaryData.Get("SaldoInBase").ToDecimal(),
                        SaldoOut = summaryData.Get("SaldoOuthBase").ToDecimal(),
                        Paid = summaryData.Get("PaymentBase").ToDecimal(),
                        SaldoChange = summaryData.Get("ChangeBase").ToDecimal(),
                        Charged = summaryData.Get("ChargeBase").ToDecimal(),
                        Recalc = summaryData.Get("RecalcBase").ToDecimal()
                    },
                    new ChargeSummaryProxy {
                        WalletType = WalletType.DecisionTariffWallet,
                        SaldoIn = summaryData.Get("SaldoInTr").ToDecimal(),
                        SaldoOut = summaryData.Get("SaldoOuthTr").ToDecimal(),
                        Paid = summaryData.Get("PaymentTr").ToDecimal(),
                        SaldoChange = summaryData.Get("ChangeTr").ToDecimal(),
                        Charged = summaryData.Get("ChargeTr").ToDecimal(),
                        Recalc = summaryData.Get("RecalcTr").ToDecimal()
                    },
                    new ChargeSummaryProxy {
                        WalletType = WalletType.PenaltyWallet,
                        SaldoIn = summaryData.Get("SaldoInPeni").ToDecimal(),
                        SaldoOut = summaryData.Get("SaldoOuthPeni").ToDecimal(),
                        Paid = summaryData.Get("PaymentPeni").ToDecimal(),
                        SaldoChange = summaryData.Get("ChangePeni").ToDecimal(),
                        Charged = summaryData.Get("ChargePeni").ToDecimal(),
                        Recalc = summaryData.Get("RecalcPeni").ToDecimal()
                    }
                }
                .AsQueryable()
                .Order(baseParams.GetLoadParam())
                .ToList(),
                3);
        }

        /// <inheritdoc />
        public IDataResult ListPaymentInfo(BaseParams baseParams)
        {
            var summaryData = this.GetData(baseParams, FileType.Pay);

            var countData = new PaymentSummaryProxy
            {
                Name = "Количество",
                WalletType = WalletType.Unknown,
                Paid = summaryData.Get("PaymentCount").ToInt(),
                Cancelled = summaryData.Get("CancelCount").ToInt(),
                Refund = summaryData.Get("ReturnCount").ToInt()
            };

            var data = new[]
            {
                new PaymentSummaryProxy
                {
                    Name = WalletType.BaseTariffWallet.GetDisplayName(),
                    WalletType = WalletType.BaseTariffWallet,
                    Paid = summaryData.Get("PaidBase").ToDecimal(),
                    Cancelled = summaryData.Get("CancelBase").ToDecimal(),
                    Refund = summaryData.Get("RefundBase").ToDecimal(),
                    Count = summaryData.Get("BaseCount").ToInt()
                },
                new PaymentSummaryProxy
                {
                    Name = WalletType.DecisionTariffWallet.GetDisplayName(),
                    WalletType = WalletType.DecisionTariffWallet,
                    Paid = summaryData.Get("PaidTr").ToDecimal(),
                    Cancelled = summaryData.Get("CancelTr").ToDecimal(),
                    Refund = summaryData.Get("RefundTr").ToDecimal(),
                    Count = summaryData.Get("DecisionCount").ToInt()
                },
                new PaymentSummaryProxy
                {
                    Name = WalletType.PenaltyWallet.GetDisplayName(),
                    WalletType = WalletType.PenaltyWallet,
                    Paid = summaryData.Get("PaidPeni").ToDecimal(),
                    Cancelled = summaryData.Get("CancelPeni").ToDecimal(),
                    Refund = summaryData.Get("RefundPeni").ToDecimal(),
                    Count = summaryData.Get("PenaltyCount").ToInt()
                }
            };

            var summary = data.Aggregate(new PaymentSummaryProxy
                {
                    Name = "Итого",
                    WalletType = WalletType.Unknown,
                    Summary = 0
                },
                (res, proxy) =>
                {
                    res.Paid += proxy.Paid;
                    res.Cancelled += proxy.Cancelled;
                    res.Refund += proxy.Refund;
                    res.Summary += proxy.Summary;
                    res.Count += proxy.Count;
                    return res;
                });

            var result = data.AsQueryable().Order(baseParams.GetLoadParam()).ToList();

            result.Add(summary);
            result.Add(countData);

            return new ListDataResult(result, result.Count);
        }

        /// <inheritdoc />
        public IDataResult ListSaldoChangeInfo(BaseParams baseParams)
        {
            var summaryData = this.GetData(baseParams, FileType.SaldoChange);

            return new ListSummaryResult(
                new[]
                {
                    new SaldoChangeSummaryProxy { WalletType = WalletType.BaseTariffWallet , Change = summaryData.Get("SaldoChangeBase").ToDecimal() },
                    new SaldoChangeSummaryProxy { WalletType = WalletType.DecisionTariffWallet , Change = summaryData.Get("SaldoChangeTr").ToDecimal() },
                    new SaldoChangeSummaryProxy { WalletType = WalletType.PenaltyWallet , Change = summaryData.Get("SaldoChangePeni").ToDecimal() }
                }
                .AsQueryable()
                .Order(baseParams.GetLoadParam())
                .ToList(),
                3);
        }

        /// <inheritdoc />
        public IDataResult ListRecalcInfo(BaseParams baseParams)
        {
            var summaryData = this.GetData(baseParams, FileType.Recalc);

            return new ListSummaryResult(
                new[]
                {
                    new RecalcSummaryProxy { WalletType = WalletType.BaseTariffWallet, Recalc = summaryData.Get("BaseRecalc").ToDecimal() },
                    new RecalcSummaryProxy { WalletType = WalletType.DecisionTariffWallet, Recalc = summaryData.Get("TariffDecisionRecalc").ToDecimal() },
                    new RecalcSummaryProxy { WalletType = WalletType.PenaltyWallet, Recalc = summaryData.Get("PenaltyRecalcRecalc").ToDecimal() }
                }
                .AsQueryable()
                .Order(baseParams.GetLoadParam())
                .ToList(),
                3);
        }

        /// <inheritdoc />
        public IDataResult Export(BaseParams baseParams)
        {
            var generator = this.Container.Resolve<ICodedReportManager>();
            using (this.Container.Using(generator))
            {
                var period = this.PeriodDomain.Get(baseParams.Params.GetAsId("periodId"));
                var report = new ChesImportReport(this.Container, this);

                return new ReportResult
                {
                    ReportStream = generator.GenerateReport(report, baseParams, ReportPrintFormat.xlsx),
                    FileName = period.Name + ".xlsx"
                };
            }
        }

        /// <inheritdoc />
        public IChesTempDataProvider GetImporter(BaseParams baseParams, FileType fileType)
        {
            var periodId = baseParams.Params.GetAsId("periodId");
            var period = this.PeriodDomain.Get(periodId);
            var fileInfo = ChesFileInfoProvider.GetFileInfo(fileType, period?.StartDate.Year ?? 0, period?.StartDate.Month ?? 0);

            return this.ProviderBuilder
                .SetParams(baseParams)
                .Build(fileInfo as IPeriodImportFileInfo);
        }

        /// <inheritdoc />
        public IDataResult DeleteSection(BaseParams baseParams)
        {
            var fileType = baseParams.Params.GetAs<FileType>("type");
            var periodId = baseParams.Params.GetAsId("periodId");

            this.Container.InTransaction(() =>
            {
                var chesImport = this.ChesImportDomain.GetAll().FirstOrDefault(x => x.Period.Id == periodId);
                var chesImportFileId = this.ChesImportFileDomain.GetAll().FirstOrDefault(x => x.ChesImport.Id == chesImport.Id)?.Id;

                if (chesImport.IsNotNull())
                {
                    chesImport.LoadedFiles.Remove(fileType);
                    this.ChesImportFileDomain.Delete(chesImportFileId);
                    this.ChesImportDomain.SaveOrUpdate(chesImport);
                }

                this.GetImporter(baseParams, fileType).DropData();
            });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IList<int> GetPaymentDays(BaseParams baseParams)
        {
            var result = new List<int>();
            var provider = this.GetImporter(baseParams, FileType.Pay);
            if (provider != null)
            {
                this.Container.InStatelessConnectionTransaction((connection, transaction) =>
                {
                    try
                    {
                        var transformProvider = MigratorUtils.GetTransformProvider(connection);
                        if (transformProvider.TableExists(provider.TableName))
                        {
                            result = connection
                                .Query<int>($"SELECT paymentday FROM {provider.TableName} "
                                    + "WHERE paymentday is not null GROUP BY paymentday")
                                .ToList();
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                });
            }

            return result;
        }

        /// <inheritdoc />
        public IDataResult PaymentsList(BaseParams baseParams)
        {
            var result = new ListDataResult();
            var paymentDay = baseParams.Params.GetAs("paymentDay", 0);
            var isValid = baseParams.Params.GetAs<bool?>("isValid");
            var provider = this.GetImporter(baseParams, FileType.Pay);
            if (provider != null)
            {
                this.Container.InStatelessConnectionTransaction((connection, transaction) =>
                {
                    var validFilter = isValid.HasValue
                        ? isValid.Value
                            ? "AND isvalid is true"
                            : "AND isvalid is not true"
                        : string.Empty;
                    var sql = $"SELECT t.*, b.report_date as ReportDate FROM {provider.TableName} as t "
                        + "LEFT JOIN REGOP_IMPORTED_PAYMENT p"
                        + "  ON (CASE WHEN t.registrynum ~ E'^\\\\d+$' THEN CAST(t.registrynum as bigint) ELSE 0 END) = p.id "
                        + "LEFT JOIN REGOP_BANK_DOC_IMPORT b ON b.id = p.BANK_DOC_ID "
                        + $"WHERE paymentday={paymentDay} {validFilter}";

                    result = connection
                        .Query<PayFileInfo.Row>(sql, buffered: this.IsBufferedQuery)
                        .AsQueryable()
                        .ToListDataResult(baseParams.GetLoadParam());
                });
            }

            return result;
        }

        /// <inheritdoc />
        public IDataResult ListSaldoCheck(BaseParams baseParams)
        {
            var result = new ListDataResult();
            var provider = this.GetImporter(baseParams, FileType.Calc);
            if (provider != null)
            {
                var loadParam = baseParams.GetLoadParam();

                this.Container.InStatelessConnectionTransaction((connection, transaction) =>
                {
                    var data = connection.Query<CalcFileInfo.SaldoCheckDto>(
                            $"SELECT * FROM {provider.CheckViewName}", buffered: this.IsBufferedQuery)
                        .AsQueryable()
                        .Filter(loadParam, this.Container);

                    var summaryData = new SaldoSummaryDto();
                    var count = 0;
                    data.ForEach(x =>
                    {
                        summaryData.Saldo += x.Saldo;
                        summaryData.ChesSaldo += x.ChesSaldo;
                        summaryData.DiffSaldo += x.DiffSaldo;
                        count++;
                    });

                    var resultData = data.Order(loadParam)
                        .Paging(loadParam)
                        .ToList();

                    result = new ListSummaryResult(resultData, count, summaryData);
                });
            }

            return result;
        }

        /// <inheritdoc />
        public IDataResult ImportSaldo(BaseParams baseParams)
        {
            var provider = this.GetImporter(baseParams, FileType.Calc);
            var updatedRows = 0;
            if (provider != null)
            {
                var isImportAll = baseParams.Params.GetAs("importAll", false);
                var calcIds = baseParams.Params.GetAs("ids", new long[0]);

                if (!isImportAll && calcIds.Length == 0)
                {
                    return BaseDataResult.Error("Не переданы параметры импорта");
                }

                this.Container.InStatelessConnectionTransaction((connection, transaction) =>
                {
                    var importSaldoSql = "UPDATE REGOP_PERS_ACC_PERIOD_SUMM s SET "
                        + $"saldo_out = v.chessaldo FROM {provider.CheckViewName} v "
                        + $"WHERE s.period_id = {provider.Period.Id} AND s.id = v.periodsummaryid ";

                    var setImportedSql = $"UPDATE {provider.TableName} SET isimported = true ";
                    if (calcIds.Length > 0)
                    {
                        var ids = string.Join(",", calcIds);
                        importSaldoSql += $"AND v.id in ({ids})";
                        setImportedSql += $"WHERE id in ({ids})";
                    }

                    updatedRows = connection.Execute(importSaldoSql, transaction: transaction);
                    connection.Execute(setImportedSql, transaction: transaction);
                });

                provider.UpdateCheckView();
            }

            return new BaseDataResult(updatedRows);
        }

        /// <inheritdoc />
        public IDataResult RunCheck(BaseParams baseParams)
        {
            var types = baseParams.Params.GetAs<FileType[]>("fileTypes");
            var periodId = baseParams.Params.GetAsId("periodId");
            var period = this.PeriodDomain.Get(periodId);
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var fields = new[] { "id", "lsnum", "houseid" };

            using (this.Container.Using(sessionProvider))
            {
                this.BillingFilterService.Initialize(period.GetEndDate());

                var chesImportFileDict = this.ChesImportFileDomain.GetAll()
                    .Where(x => x.ChesImport.Period.Id == periodId)
                    .ToDictionary(x => x.FileType);

                this.Container.InStatelessConnectionTransaction(
                    (con, tr) =>
                    {
                        foreach (var type in types)
                        {
                            var chesImportFile = chesImportFileDict.Get(type);
                            var provider = this.GetImporter(baseParams, type);
                            var columns = provider.FileInfo.GetColumns().Values;
                            var sqlQuery = $@"SELECT {string.Join(",", columns.Where(x => fields.Contains(x.Name.ToLower())).Select(x => x.Name))}
                                      FROM {provider.TableName}";
                            var rows = con.Query<CheckDto>(sqlQuery, transaction: tr)
                                .ToList();

                            StringBuilder log = new StringBuilder("Номер ЛС;Муниципальный район;Адрес;Причина\r\n");
                            var countErrors = 0;

                            if (type == FileType.Account)
                            {
                                var take = 30000;
                                for (int skip = 0; skip < rows.Count; skip += take)
                                {
                                    var portion = rows.Skip(skip).Take(take);
                                    var roIds = portion.Select(x => x.HouseId.ToLong()).Distinct().ToList();

                                    var realityObjects = this.RealityObjectDomain.GetAll()
                                        .WhereContains(x => x.Id, roIds)
                                        .Select(x => new
                                        {
                                            x.Id,
                                            x.Address,
                                            MunicipalityName = x.Municipality.Name
                                        })
                                        .AsEnumerable()
                                        .ToDictionary(x => x.Id);

                                    foreach (var row in portion)
                                    {
                                        var realityObject = realityObjects.Get(row.HouseId.ToLong());
                                        if (realityObject == null)
                                        {
                                            log.AppendFormat($"{row.LsNum};;;Не найден дом\r\n");
                                            countErrors++;
                                            continue;
                                        }

                                        string errorMessage;
                                        row.Isvalid = this.BillingFilterService.CheckByRealityObject(new RealityObject { Id = row.HouseId.ToLong() },
                                            out errorMessage);

                                        if (!row.Isvalid)
                                        {
                                            var address = $"{realityObject.Address}";
                                            log.AppendFormat($"{row.LsNum};{realityObject.MunicipalityName};{address};{errorMessage}\r\n");
                                            countErrors++;
                                        }
                                    }

                                    this.IsChecked(provider, con, tr, portion.Where(x => x.Isvalid).ToList());
                                }
                            }
                            else
                            {
                                var take = 30000;
                                for (int skip = 0; skip < rows.Count; skip += take)
                                {
                                    var portion = rows.Skip(skip).Take(take);

                                    var lsNums = portion.Select(x => $"'{x.LsNum}'").Distinct();

                                    var sql = $@"select ac.acc_num as PersonalAccountNum, ro.address as Address,
                                                 rm.croom_num as RoomNum, mu.name as MunicipalityName
                                                 from regop_pers_acc ac
                                                 join gkh_room rm on rm.id = ac.room_id
                                                 join gkh_reality_object ro on ro.id = rm.ro_id
                                                 join gkh_dict_municipality mu on mu.id = ro.municipality_id
                                                 where ac.acc_num IN({string.Join(",", lsNums)})";

                                    var accounts = con.Query<AccountDto>(sql).ToDictionary(x => x.PersonalAccountNum);

                                    foreach (var row in portion)
                                    {
                                        var account = accounts.Get(row.LsNum);

                                        if (account == null)
                                        {
                                            log.AppendFormat($"{row.LsNum};;;Не найден ЛС\r\n");
                                            countErrors++;
                                            continue;
                                        }

                                        string errorMessage;
                                        row.Isvalid = this.BillingFilterService.CheckByAccountNumber(row.LsNum, out errorMessage);

                                        if (!row.Isvalid)
                                        {
                                            var address = $"{account.Address}, кв. {account.RoomNum}";
                                            log.AppendFormat($"{row.LsNum};{account.MunicipalityName};{address};{errorMessage}\r\n");
                                            countErrors++;
                                        }
                                    }

                                    this.IsChecked(provider, con, tr, portion.Where(x => x.Isvalid).ToList());
                                }
                            }

                            var fileInfo = chesImportFile.File;

                            if (countErrors > 0)
                            {
                                using (var memoryStream = new MemoryStream())
                                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                                {
                                    writer.Write(log);
                                    writer.Flush();
                                    chesImportFile.File = this.FileManager.SaveFile(memoryStream, "Отчет проверки.csv");
                                }

                                chesImportFile.CheckState = CheckState.NotCheked;
                            }
                            else
                            {
                                chesImportFile.CheckState = CheckState.Checked;
                                chesImportFile.File = null;
                            }

                            this.ChesImportFileDomain.SaveOrUpdate(chesImportFile);
                            if (fileInfo != null)
                            {
                                this.FileManager.Delete(fileInfo);
                            }
                        }
                    });
            }

            return new BaseDataResult();
        }

        private void IsChecked(IChesTempDataProvider provider, IDbConnection conn, IDbTransaction transaction, List<CheckDto> rows)
        {
            if (rows.Count > 0)
            {
                var sql = $@"UPDATE {provider.TableName}
                         SET 
                            isvalid=true
                            WHERE id in ({string.Join(",", rows.Select(x => x.Id))})";
                conn.Execute(sql, transaction: transaction);
            }
        }

        private IDictionary<string, object> GetData(BaseParams baseParams, FileType type)
        {
            return this.GetImporter(baseParams, type).GetSummaryData(baseParams);
        }

        internal class ChargeSummaryProxy
        {
            public WalletType WalletType { get; set; }
            public decimal SaldoIn { get; set; }
            public decimal Charged { get; set; }
            public decimal Paid { get; set; }
            public decimal SaldoChange { get; set; }
            public decimal Recalc { get; set; }
            public decimal SaldoOut { get; set; }
        }

        internal class PaymentSummaryProxy
        {
            public string Name { get; set; }
            public WalletType WalletType { get; set; }
            public decimal Paid { get; set; }
            public decimal Cancelled { get; set; }
            public decimal Refund { get; set; }
            public decimal Sum => (int) this.WalletType == -1 ? 0 : this.Summary ?? this.Paid - this.Cancelled - this.Refund;
            public decimal? Summary { get; set; }
            public int Count { get; set; }
        }

        internal class SaldoChangeSummaryProxy
        {
            public WalletType WalletType { get; set; }
            public decimal Change { get; set; }
        }

        internal class RecalcSummaryProxy
        {
            public WalletType WalletType { get; set; }
            public decimal Recalc { get; set; }
        }

        private class SaldoSummaryDto
        {
            public decimal Saldo { get; set; }
            public decimal ChesSaldo { get; set; }
            public decimal DiffSaldo { get; set; }
        }

        private class CheckDto
        {
            public long Id { get; set; }
            public string LsNum { get; set; }
            public string HouseId { get; set; }
            public bool Isvalid { get; set; }
        }

        private class AccountDto
        {
            public string PersonalAccountNum { get; set; }
            public string Address { get; set; }
            public string RoomNum { get; set; }
            public string MunicipalityName { get; set; }
        }
    }
}