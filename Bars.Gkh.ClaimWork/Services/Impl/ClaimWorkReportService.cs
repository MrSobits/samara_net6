namespace Bars.Gkh.ClaimWork.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.BuilderContract;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Controller;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.ReportHandler;
    using Bars.Gkh.TextValues;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Ionic.Zip;
    using Ionic.Zlib;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;
    using Report;
    using RegOperator.Entities;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.Entities.Owner;

    /// <summary>
    /// Сервис для работы с претензиями неплательщиков
    /// </summary>
    public class ClaimWorkReportService : IClaimWorkReportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IMenuItemText MenuItemText { get; set; }

        public IGkhConfigProvider ConfigProv { get; set; }

        /// <summary>
        /// Вернуть список отчётов
        /// </summary>
        /// <param name="baseParams">Базовые параметры запросы</param>
        /// <returns>Результат операции</returns>
        public IList<ReportInfo> GetReportList(BaseParams baseParams)
        {
            var reports = this.Container.ResolveAll<IClaimWorkCodedReport>();

            var codeForms = baseParams.Params["codeForm"].ToString().Split(StringSplitOptions.RemoveEmptyEntries, ","," ");
            var claimworkId = baseParams.Params.GetAsId("claimWorkId");
            var type = baseParams.Params.GetAs<DebtorType?>("type");
            var configSection = this.ConfigProv.Get<DebtorClaimWorkConfig>();
            var documentTypes = new List<ClaimWorkDocumentType>();
            var allowCodeForms = codeForms.ToList();

            if (type != null)
            {
                if (claimworkId > 0)
                {
                    var docDomain = this.Container.ResolveDomain<DocumentClw>();
                    using (this.Container.Using(docDomain))
                    {
                        documentTypes = docDomain.GetAll()
                            .Where(x => x.ClaimWork.Id == claimworkId)
                            .Select(x => x.DocumentType)
                            .ToList();
                    }
                }

                var config = type == DebtorType.Legal
                    ? configSection.Legal
                    : configSection.Individual;

                var configParams = new List<string>();
                foreach (var property in config.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var codeForm = property.GetCustomAttribute<StringValueAttribute>()?.Value;
                    var documentValue = property.GetValue(config);
                    var value = documentValue.GetType().GetProperty("PrintType", BindingFlags.Instance | BindingFlags.Public)?.GetValue(documentValue);
                    var printType = value as PrintType? ?? PrintType.NoPrint;

                    if (printType == PrintType.NoPrint && !string.IsNullOrEmpty(codeForm))
                    {
                        configParams.Add(codeForm);
                    }
                }

                allowCodeForms = codeForms.Except(configParams.ToArray()).ToList();
            }

            try
            {
                return reports
                    .Where(x => allowCodeForms.Contains(x.CodeForm))
                    .WhereIf(claimworkId > 0, x => documentTypes.Contains(x.DocumentType))
                    .Select(x => new ReportInfo
                    {
                        Id = x.Id,
                        Name = this.MenuItemText.GetText(x.Name),
                        Description = this.MenuItemText.GetText(x.Description)
                    })
                    .ToList();
            }
            finally
            {
                foreach (var report in reports)
                {
                    this.Container.Release(report);
                }
            }
        }

        /// <summary>
        /// Напечатать отчёт
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Файл отчёта</returns>
        public IDataResult GetReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IClaimWorkCodedReport>();
            var exportHandlers = this.Container.ResolveAll<IClwReportExportHandler>();

            var reportId = baseParams.Params.GetAs<string>("reportId");
            var userParam = new UserParamsValues();
            var documentId = string.Empty;

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];

                documentId = userParam.GetValue("DocumentId").ToString();
            }

            try
            {
                var report = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                if (report == null)
                {
                    throw new Exception("Не найдена реализация отчета для выбранного документа");
                }

                report.DocumentId = documentId;

                report.Generate();

                var stream = report.ReportFileStream;

                var file = fileManager.SaveFile(stream, report.OutputFileName);

                this.Container.InTransaction(() =>
                {
                    exportHandlers.Where(x => x.CanHandle(report)).ForEach(x => x.HandleExport(report, file));
                });

                return new BaseDataResult(file.Id);
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
                this.Container.Release(exportHandlers);
            }
        }

        /// <summary>
        /// Печать массового отчёта
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Архив отчёта</returns>
        public IDataResult GetMassReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var documentClwDomain = this.Container.ResolveDomain<DocumentClw>();
            var claimWorkCodedReports = this.Container.ResolveAll<IClaimWorkCodedReport>();
            var exportHandlers = this.Container.ResolveAll<IClwReportExportHandler>();
            var configDebtor = this.Container.GetGkhConfig<DebtorClaimWorkConfig>().General;
            var configBuilder = this.Container.GetGkhConfig<BuilderContractClaimWorkConfig>();

            var reportId = baseParams.Params.GetAs<string>("reportId");
            var claimWorkType = baseParams.Params.GetAs<ClaimWorkTypeBase>("claimWorkType");

            var typeUsage = claimWorkType == ClaimWorkTypeBase.Debtor ? configDebtor.TypeUsage : configBuilder.TypeUsage;
            var documentClwFileName = configDebtor.DocumentClwFileName;

            var userParam = new UserParamsValues();
            long[] baseClaimWorkIds;
            var documentIds = new List<long>();

            try
            {
                var tempReport = claimWorkCodedReports.FirstOrDefault(x => x.Id == reportId);

                var reports = new List<IClaimWorkCodedReport>();

                if (tempReport != null)
                {
                    var reportName = tempReport.GetType().FullName;

                    if (baseParams.Params.ContainsKey("userParams") &&
                        baseParams.Params["userParams"] is DynamicDictionary)
                    {
                        userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];

                        baseClaimWorkIds = userParam.GetValue("DocumentIds")
                            .To<List<object>>()
                            .OfType<long>()
                            .ToArray();

                        if (tempReport.DocumentType == ClaimWorkDocumentType.RestructDebt)
                        {
                            documentIds = userParam.GetValue("DocumentIds")
                                .To<List<object>>()
                                .OfType<long>()
                                .ToList();
                        }
                        else
                        {
                            documentIds = documentClwDomain.GetAll()
                                .Where(x => baseClaimWorkIds.Contains(x.ClaimWork.Id))
                                .Where(x => x.DocumentType == tempReport.DocumentType)
                                .Select(x => x.Id)
                                .ToList();
                        }
                    }

                    foreach (var documentId in documentIds)
                    {
                        var report = this.Container.Resolve<IClaimWorkCodedReport>(reportName);

                        if (report == null)
                        {
                            continue;
                        }

                        using (this.Container.Using(report))
                        {
                            report.DocumentId = documentId.ToString();

                            report.Generate();

                            // добавляем к наименованию файла адрес, если выставлена настройка
                            if (claimWorkType == ClaimWorkTypeBase.Debtor && documentClwFileName == DocumentClwFileName.WithAddress)
                            {
                                var address = report.ReportInfo.Address;
                                report.OutputFileName = address.Append($"{report.OutputFileName}", " - ");
                            }

                            reports.Add(report);

                            exportHandlers
                                .Where(x => x.CanHandle(report))
                                .ForEach(x => x.HandleExport(report, fileManager.SaveFile(report.ReportFileStream, report.OutputFileName)));

                            report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                        }
                    }
                }

                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                if (typeUsage == TypeUsage.Used)
                {
                    var reportsByMuDict = reports.GroupBy(x => x.ReportInfo.MunicipalityName)
                        .ToDictionary(
                            x => x.Key,
                            x => x.Select(
                                    y => new
                                    {
                                        y.OutputFileName,
                                        y.ReportFileStream
                                    })
                                .ToList());

                    foreach (var municipality in reportsByMuDict)
                    {
                        var municipalityDir = tempDir.CreateSubdirectory(municipality.Key.IsEmpty() ? "Без_МО" : municipality.Key);

                        foreach (var report in municipality.Value)
                        {
                            File.WriteAllBytes(
                                Path.Combine(municipalityDir.FullName, ClaimWorkReportService.ValidateFileName(report.OutputFileName)),
                                report.ReportFileStream.ReadAllBytes());
                        }
                    }
                }
                else
                {
                    foreach (var report in reports)
                    {
                        File.WriteAllBytes(
                            Path.Combine(tempDir.FullName, ClaimWorkReportService.ValidateFileName(report.OutputFileName)),
                            report.ReportFileStream.ReadAllBytes());
                    }
                }

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);

                    var file = fileManager.SaveFile(ms, "Документы.zip");
                    return new BaseDataResult(file.Id);
                }
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(documentClwDomain);
                this.Container.Release(exportHandlers);
            }
        }

        //Печать из вкладки собственников
        public IDataResult GetLawsuitOnwerReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IClaimWorkCodedReport>();
            var lawsuitDomain = this.Container.ResolveDomain<Lawsuit>();

            var reportId = baseParams.Params.GetAs<string>("reportId");
            var recIds = baseParams.Params.GetAs<long[]>("recIds");
            var userParam = new UserParamsValues();
            var documentId = string.Empty;

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];

                documentId = userParam.GetValue("DocumentId").ToString();
            }

            if (recIds.Length < 1)
            {
                throw new Exception("Необходимо выбрать хотя бы одну запись для печати");
            }

            try
            {
                var lawSuit = lawsuitDomain.Get(documentId.ToLong());

                if (recIds.Length == 1 || reportId == "LawSuitOwnerClaimStatementReport" || reportId == "LawSuitOwnerApplicationIssuanceCourtOrderReport")
                {
                    //TODO: Унифицировать логику
                    
                    var report = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                    if (report is ExecutiveClaimReport )
                    {
                        var exClaimReport = report as ExecutiveClaimReport;

                        exClaimReport.ReportInfo = new ClaimWorkReportInfo();

                        exClaimReport.ClaimworkId = lawSuit.ClaimWork.Id.ToString();

                        exClaimReport.DocumentId = documentId;

                        exClaimReport.OwnerId = recIds.First().ToString();

                        var clwOwnerInfoContainer = this.Container.Resolve<IDomainService<LawsuitOwnerInfo>>();
                        try
                        {
                            var owner = clwOwnerInfoContainer.Get(recIds.First());
                            exClaimReport.Solidary = owner.SharedOwnership;
                        }
                        catch
                        { }

                        exClaimReport.ReportInfo.OnwerInfoIds = recIds;

                       // exClaimReport.Solidary = 

                        exClaimReport.Generate();

                        return new BaseDataResult(fileManager.SaveFile(exClaimReport.ReportFileStream, exClaimReport.OutputFileName).Id);
                    }
                    //Очень тупое решение. Время жмет - некогда придумывать что то красивее. Туду выше актуален
                    if (report  is ExecutiveClaimReportRepeat)
                    {
                        var exClaimReport = report as ExecutiveClaimReportRepeat;

                        exClaimReport.ReportInfo = new ClaimWorkReportInfo();

                        exClaimReport.ClaimworkId = lawSuit.ClaimWork.Id.ToString();

                        exClaimReport.DocumentId = documentId;

                        exClaimReport.OwnerId = recIds.First().ToString();

                        var clwOwnerInfoContainer = this.Container.Resolve<IDomainService<LawsuitOwnerInfo>>();
                        try
                        {
                            var owner = clwOwnerInfoContainer.Get(recIds.First());
                            exClaimReport.Solidary = owner.SharedOwnership;
                        }
                        catch
                        { }

                        exClaimReport.ReportInfo.OnwerInfoIds = recIds;

                        // exClaimReport.Solidary = 

                        exClaimReport.Generate();

                        return new BaseDataResult(fileManager.SaveFile(exClaimReport.ReportFileStream, exClaimReport.OutputFileName).Id);
                    }

                    if (report == null)
                    {
                        throw new Exception("Не найдена реализация отчета для выбранного документа");
                    }

                    report.ReportInfo = new ClaimWorkReportInfo();

                    report.DocumentId = documentId;

                    report.ReportInfo.OnwerInfoIds = recIds;

                    report.Generate();

                    var stream = report.ReportFileStream;

                    var file = fileManager.SaveFile(stream, report.OutputFileName);

                    return new BaseDataResult(file.Id);
                }
                else
                {
                    var reports = new List<IClaimWorkCodedReport>();

                    var tempReport = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                    if (tempReport != null)
                    {
                        var reportName = tempReport.GetType().FullName;

                        foreach (var recId in recIds)
                        {
                            var report = this.Container.Resolve<IClaimWorkCodedReport>(reportName);

                            if (report == null)
                            {
                                continue;
                            }
                            else if (report is ExecutiveClaimReport)
                            {
                                var exClaimReport = report as ExecutiveClaimReport;

                                exClaimReport.ReportInfo = new ClaimWorkReportInfo();

                                exClaimReport.ClaimworkId = lawSuit.ClaimWork.Id.ToString();

                                exClaimReport.DocumentId = documentId;

                                exClaimReport.OwnerId = recId.ToString();

                                exClaimReport.ReportInfo.OnwerInfoIds = recIds;

                                exClaimReport.Generate();

                                reports.Add(exClaimReport);

                                exClaimReport.ReportFileStream.Seek(0, SeekOrigin.Begin);
                            }
                            else
                            {
                                report.ReportInfo = new ClaimWorkReportInfo();

                                report.DocumentId = documentId;
                                report.ReportInfo.OnwerInfoIds = new[] { recId };

                                report.Generate();

                                reports.Add(report);
                                report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                            }
                        }
                    }

                    var archive = new ZipFile(Encoding.UTF8)
                    {
                        CompressionLevel = CompressionLevel.Level9,
                        AlternateEncoding = Encoding.GetEncoding("cp866")
                    };

                    var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    foreach (var report in reports)
                    {
                        File.WriteAllBytes(
                            Path.Combine(tempDir.FullName, ClaimWorkReportService.ValidateFileName(report.OutputFileName)),
                            report.ReportFileStream.ReadAllBytes());
                    }

                    archive.AddDirectory(tempDir.FullName);

                    using (var ms = new MemoryStream())
                    {
                        var docName = lawSuit.DocumentType.GetDisplayName();

                        archive.Save(ms);

                        var file = fileManager.SaveFile(
                            ms,
                            $"{docName} {(lawSuit.DocumentDate.HasValue ? $" - {lawSuit.DocumentDate.Value.ToShortDateString()}" : string.Empty)}.zip");
                        return new BaseDataResult(file.Id);
                    }
                }
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
                this.Container.Release(lawsuitDomain);
            }
        }

        //Печать из карточки и из реестра
        public IDataResult GetAccountReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IClaimWorkCodedReport>();
            var documentDomain = this.Container.ResolveDomain<DocumentClw>();          


            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>(); //Резолвер айди клеймворка при печати из карточки ПИР

            var reportId = baseParams.Params.GetAs<string>("reportId");
            var recIds = baseParams.Params.GetAs<long[]>("recIds");         
            var userParam = new UserParamsValues();

            string documentId = "";
            long? claimworkId = 0;

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];

                claimworkId = userParam.GetValue("claimWorkId").ToLong();

                documentId = userParam.GetValue("DocumentId").ToString();


            }

            //var recIds = baseParams.Params.GetAs<long[]>("recIds");

            if (documentId != "" && recIds == null)
            {
                //recIds = userParam.Values.GetAs<long[]>("DocumentId");
            }


#warning требуется переписать без костылей

            //lawsuit

            var lawsuitDomain = this.Container.ResolveDomain<Lawsuit>();
            var docClwDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var lawsuit = lawsuitDomain.Get(documentId.ToLong());

            //courtclaim

            var courtClaimDomain = this.Container.ResolveDomain<CourtOrderClaim>();
            var courtclaim = courtClaimDomain.Get(documentId.ToLong());


            if (claimworkId == 0 || claimworkId == null)
            {
                if (lawsuit.IsNotNull())
                {
                    claimworkId = lawsuit.ClaimWork.Id;
                }
                else if (courtclaim.IsNotNull())
                {
                    claimworkId = courtclaim.ClaimWork.Id;
                }
            }
            var docClw = docClwDomain.GetAll().Where(x => x.ClaimWork.Id == claimworkId).Select(x=>   x.Id ).ToArray();
            recIds = docClw;
            
            //var documentId = string.Empty;

            if (recIds.Length < 1 && documentId==null)
            {
                throw new Exception("Необходимо выбрать хотя бы одну запись для печати");
            }

            try
            {

                if (recIds.Length == 1 || documentId!=null)
                {
                    var report = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                    if (report == null)
                    {
                        throw new Exception("Не найдена реализация отчета для выбранного документа");
                    }

                    //documentId = documentDomain.GetAll()
                    //    .Where(x => x.ClaimWork.Id == claimworkId)
                    //    .Where(x => x.DocumentType == report.DocumentType)
                    //    .OrderByDescending(x => x.DocumentDate)
                    //    .Select(x => x.Id)
                    //    .FirstOrDefault()
                    //    .ToStr();

                    //****
                    IClaimWorkService claimWorkService;
                    report.ReportInfo = new ClaimWorkReportInfo();

                    if (lawsuit.IsNotNull())
                    {
                        claimWorkService = claimWorkServices.FirstOrDefault(x => x.TypeBase == lawsuit.ClaimWork.ClaimWorkTypeBase);
                        report.ReportInfo = claimWorkService != null ? claimWorkService.ReportInfoByClaimwork(lawsuit.ClaimWork.Id) : null;
                    }
                    else if (courtclaim.IsNotNull())
                    {
                        claimWorkService = claimWorkServices.FirstOrDefault(x => x.TypeBase == courtclaim.ClaimWork.ClaimWorkTypeBase);
                        report.ReportInfo = claimWorkService != null ? claimWorkService.ReportInfoByClaimwork(courtclaim.ClaimWork.Id) : null;
                    }
                    //****

                    report.DocumentId = documentId;
                    if (documentId == "0" || documentId == null)
                    {
                        report.DocumentId = claimworkId.ToString();
                    }

                    if (recIds != null)
                    {
                        report.ReportInfo.OnwerInfoIds = recIds;
                    }

                    report.Generate();

                    var stream = report.ReportFileStream;

                    var file = fileManager.SaveFile(stream, report.OutputFileName);

                    return new BaseDataResult(file.Id);
                }
                else
                {
                    var reports = new List<IClaimWorkCodedReport>();

                    var tempReport = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                    //documentId = documentDomain.GetAll()
                    //    .Where(x => x.ClaimWork.Id == claimworkId)
                    //    .Where(x => x.DocumentType == tempReport.DocumentType)
                    //    .OrderByDescending(x => x.DocumentDate)
                    //    .Select(x => x.Id)
                    //    .FirstOrDefault()
                    //    .ToStr();

                    if (tempReport != null)
                    {
                        var reportName = tempReport.GetType().FullName;

                        foreach (var recId in recIds)
                        {
                            var report = this.Container.Resolve<IClaimWorkCodedReport>(reportName);

                            if (report == null)
                            {
                                continue;
                            }

                            report.ReportInfo = new ClaimWorkReportInfo();

                            report.DocumentId = documentId;
                            report.ReportInfo.OnwerInfoIds = new[] {recId};

                            report.Generate();

                            reports.Add(report);
                            report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                        }
                    }
                    else
                    {
                        throw new Exception("Не найдена реализация отчета для выбранного документа");
                    }

                    var archive = new ZipFile(Encoding.UTF8)
                    {
                        CompressionLevel = CompressionLevel.Level9,
                        AlternateEncoding = Encoding.GetEncoding("cp866")
                    };

                    var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    foreach (var report in reports)
                    {
                        File.WriteAllBytes(
                            Path.Combine(tempDir.FullName, ClaimWorkReportService.ValidateFileName(report.OutputFileName)),
                            report.ReportFileStream.ReadAllBytes());
                    }

                    archive.AddDirectory(tempDir.FullName);

                    using (var ms = new MemoryStream())
                    {
                        var docName = tempReport.DocumentType.GetDisplayName();

                        archive.Save(ms);

                        var file = fileManager.SaveFile(
                            ms,
                            $"{docName} ({DateTime.Now.Date.ToShortDateString()}).zip");
                        return new BaseDataResult(file.Id);
                    }
                }
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
            }
        }

        /// <inheritdoc />
        public IDataResult GetMassAccountReport(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var claimWorkCodedReportDomain = this.Container.ResolveAll<IClaimWorkCodedReport>();
            var claimWorkServices = this.Container.ResolveAll<IClaimWorkService>();
            var documentDomain = this.Container.ResolveDomain<DocumentClw>();
            var claimworkDetailDomain = this.Container.ResolveDomain<ClaimWorkAccountDetail>();
            var configDebtor = this.Container.GetGkhConfig<DebtorClaimWorkConfig>().General;

            var typeUsage = configDebtor.TypeUsage;
            var documentClwFileName = configDebtor.DocumentClwFileName;

            var reportId = baseParams.Params.GetAs<string>("reportId");
            var recIds = baseParams.Params.GetAs<long[]>("recIds");
            var claimworkIds = baseParams.Params.GetAs<long[]>("claimworkIds");
            var isMassPrint = baseParams.Params.GetAs<bool>("isMassPrint");
            var claimworkId = 0L;
            DirectoryInfo tempDir = null;
            try
            {
                Dictionary<long, long> dict = new Dictionary<long, long>();

                if (isMassPrint)
                {
                    dict = claimworkDetailDomain.GetAll()
                        .Where(x => claimworkIds.Contains(x.ClaimWork.Id))
                        .Select(
                            x => new
                            {
                                ClaimWorkId = x.ClaimWork.Id,
                                DetailId = x.Id
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.DetailId)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.ClaimWorkId).FirstOrDefault());

                    recIds = dict.Keys.ToArray();
                }
                else
                {
                    dict = claimworkDetailDomain.GetAll()
                        .Where(x => recIds.Contains(x.Id))
                        .Select(
                            x => new
                            {
                                ClaimWorkId = x.ClaimWork.Id,
                                DetailId = x.Id
                            })
                        .AsEnumerable()
                        .GroupBy(x => x.DetailId)
                        .ToDictionary(x => x.Key, y => y.Select(z => z.ClaimWorkId).FirstOrDefault());
                }

                if (recIds.Length == 1)
                {
                    var report = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                    if (report == null)
                    {
                        throw new Exception("Не найдена реализация отчета для выбранного документа");
                    }

                    claimworkId = dict.Get(recIds.FirstOrDefault());

                    var documentId = documentDomain.GetAll()
                        .Where(x => x.ClaimWork.Id == claimworkId)
                        .Where(x => x.DocumentType == report.DocumentType)
                        .OrderByDescending(x => x.DocumentDate)
                        .Select(x => x.Id)
                        .FirstOrDefault()
                        .ToStr();

                    var claimWorkService =
                        claimWorkServices.FirstOrDefault(x => x.TypeBase == ClaimWorkTypeBase.Debtor);

                    report.ReportInfo = claimWorkService != null
                        ? claimWorkService.ReportInfoByClaimwork(claimworkId)
                        : new ClaimWorkReportInfo() { MunicipalityName = "" };

                    report.DocumentId = documentId;

                    report.ReportInfo.OnwerInfoIds = recIds;

                    report.Generate();

                    // добавляем к наименованию файла адрес, если выставлена настройка
                    if (documentClwFileName == DocumentClwFileName.WithAddress)
                    {
                        var address = report.ReportInfo.Address;
                        report.OutputFileName = address.Append($"{report.OutputFileName}", " - ");
                    }

                    var stream = report.ReportFileStream;

                    var file = fileManager.SaveFile(stream, report.OutputFileName);

                    return new BaseDataResult(file.Id);
                }
                else
                {
                    var reports = new List<IClaimWorkCodedReport>();

                    var tempReport = claimWorkCodedReportDomain.FirstOrDefault(x => x.Id == reportId);

                    if (tempReport != null)
                    {
                        var reportName = tempReport.GetType().FullName;

                        foreach (var recId in recIds)
                        {
                            claimworkId = dict.Get(recId);

                            var documentId = documentDomain.GetAll()
                                .Where(x => x.ClaimWork.Id == claimworkId)
                                .Where(x => x.DocumentType == tempReport.DocumentType)
                                .OrderByDescending(x => x.DocumentDate)
                                .Select(x => x.Id)
                                .FirstOrDefault();

                            var report = this.Container.Resolve<IClaimWorkCodedReport>(reportName);

                            if (documentId == 0 || report == null)
                            {
                                continue;
                            }

                            var claimWorkService =
                                claimWorkServices.FirstOrDefault(x => x.TypeBase == ClaimWorkTypeBase.Debtor);

                            report.ReportInfo = claimWorkService != null
                                ? claimWorkService.ReportInfoByClaimworkDetail(recId)
                                : new ClaimWorkReportInfo() { MunicipalityName = "" };

                            report.DocumentId = documentId.ToStr();
                            report.ReportInfo.OnwerInfoIds = new[] { recId };

                            report.Generate();

                            // добавляем к наименованию файла адрес, если выставлена настройка
                            if (documentClwFileName == DocumentClwFileName.WithAddress)
                            {
                                var address = report.ReportInfo.Address;
                                report.OutputFileName = address.Append($"{report.OutputFileName}", " - ");
                            }

                            reports.Add(report);
                            report.ReportFileStream.Seek(0, SeekOrigin.Begin);
                        }
                    }
                    else
                    {
                        throw new Exception("Не найдена реализация отчета для выбранного документа");
                    }

                    if (reports.Count == 0)
                    {
                        throw new Exception($"Документ {tempReport.DocumentType.GetDisplayName()} не найден");
                    }

                    var archive = new ZipFile(Encoding.UTF8)
                    {
                        CompressionLevel = CompressionLevel.Level9,
                        AlternateEncoding = Encoding.GetEncoding("cp866")
                    };

                    tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

                    if (typeUsage == TypeUsage.Used)
                    {
                        var reportsByMuDict = reports.GroupBy(x => x.ReportInfo.MunicipalityName)
                            .ToDictionary(
                                x => x.Key,
                                x => x.Select(
                                        y => new
                                        {
                                            y.OutputFileName,
                                            y.ReportFileStream
                                        })
                                    .ToList());

                        foreach (var municipality in reportsByMuDict)
                        {
                            var municipalityDir = tempDir.CreateSubdirectory(municipality.Key.IsEmpty() ? "Без_МО" : municipality.Key);

                            foreach (var report in municipality.Value)
                            {
                                File.WriteAllBytes(
                                    Path.Combine(municipalityDir.FullName, ClaimWorkReportService.ValidateFileName(report.OutputFileName)),
                                    report.ReportFileStream.ReadAllBytes());
                            }
                        }
                    }
                    else
                    {
                        foreach (var report in reports)
                        {
                            File.WriteAllBytes(
                                Path.Combine(tempDir.FullName, ClaimWorkReportService.ValidateFileName(report.OutputFileName)),
                                report.ReportFileStream.ReadAllBytes());
                        }
                    }

                    archive.AddDirectory(tempDir.FullName);

                    using (var ms = new MemoryStream())
                    {
                        var docName = tempReport.DocumentType.GetDisplayName();

                        archive.Save(ms);

                        var file = fileManager.SaveFile(
                            ms,
                            $"{docName} ({DateTime.Now.Date.ToShortDateString()}).zip");
                        return new BaseDataResult(file.Id);
                    }
                }
            }
            finally
            {
                this.Container.Release(fileManager);
                this.Container.Release(claimWorkCodedReportDomain);
                this.Container.Release(claimworkDetailDomain);
                tempDir?.Delete(true);
            }
        }

        private static string ValidateFileName(string fileName)
        {
            return string.Join("-",fileName.Split(Path.GetInvalidFileNameChars())).Replace("--", "-");
        }
    }
}