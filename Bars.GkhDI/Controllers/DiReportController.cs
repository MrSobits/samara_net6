using Bars.B4.Utils.Web;

namespace Bars.GkhDi.Controllers
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4.Config;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Report;
    using Services;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    using System.Globalization;
    using System.Linq;
    using System.Net;

    using Bars.Gkh.Config;

    public class DiReportController : BaseController
    {
        public DiReportController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }
        
        private readonly IFileManager _fileManager;
        public IConfigProvider ConfigProvider { get; set; }
        public IGkhParams ParamManager { get; set; }
        public IDomainService<PeriodDi> PeriodDiDomain { get; set; }
        public IDomainService<ManagingOrganization> ManOrgDomain { get; set; }

        public ActionResult Report731Print(BaseParams baseParams)
        {
            var file = Container.Resolve<IGkhReportService>().GetReport(baseParams);

            // Хак для отображения русских имен файлов
            file.FileDownloadName = UserAgentInfo.GetBrowserName(ControllerContext.HttpContext.Request).StartsWith("IE")
                ? WebUtility.UrlEncode(file.FileDownloadName).Replace("+", "%20")
                : file.FileDownloadName.Replace("\"", "'");

            var appParams = ConfigProvider.GetConfig().AppSettings;
            var appParams1 = ParamManager.GetParams();

            var needSaveReportToDirecory =
                appParams1.GetAs<string>("DI_SaveReport731ToDirectory")
                ?? appParams.GetAs<string>("DI_SaveReport731ToDirectory");

            if (string.IsNullOrEmpty(needSaveReportToDirecory))
            {
                return file;
            }

            var dirPath = needSaveReportToDirecory;
            var filesDirectory = new DirectoryInfo(dirPath);

            try
            {
                if (!filesDirectory.Exists)
                {
                    filesDirectory.Create();
                }
            }
            catch (Exception)
            {
                needSaveReportToDirecory = "";
            }

            var userParams = baseParams.Params.ContainsKey("userParams") ? baseParams.Params["userParams"] as DynamicDictionary : null;

            var periodId = userParams != null && userParams.ContainsKey("periodId")
                ? userParams.GetAs<long>("periodId")
                : 0;

            var manorgId = userParams != null && userParams.ContainsKey("periodId")
                ? userParams.GetAs<long>("manorgId")
                : 0;

            var period = PeriodDiDomain.Get(periodId);
            var manorg = ManOrgDomain.Get(manorgId);

            if (!string.IsNullOrEmpty(needSaveReportToDirecory) && period != null && manorg != null )
            {
                var year = period.DateStart.GetValueOrDefault().Year;
                var inn = manorg.ReturnSafe(x => x.Contragent).ReturnSafe(x => x.Inn);

                if (year > 0 && !string.IsNullOrEmpty(inn))

                {
                    var fileName = string.Format("report731_year{0}_inn{1}", year, inn);

                    var buffer = new byte[file.FileStream.Length];

                    file.FileStream.Seek(0, SeekOrigin.Begin);
                    file.FileStream.Read(buffer, 0, buffer.Length);
                    file.FileStream.Seek(0, SeekOrigin.Begin);

                    System.IO.File.WriteAllBytes(
                        Path.Combine(filesDirectory.FullName,
                            string.Format("{0}.{1}", fileName, "xlsx")), buffer);

                    file.FileStream.Seek(0, SeekOrigin.Begin);
                }
            }

            return file;
        }

        public ActionResult GetReport731(BaseParams baseParams)
        {
            var inn = baseParams.Params.GetAs<string>("inn");
            var year = baseParams.Params.GetAs<int>("year");
            var dateActuality = DateTime.Now.Date.AddDays(-14);

            var appParams = ConfigProvider.GetConfig().AppSettings;
            var appParams1 = ParamManager.GetParams();

            var needSaveReportToDirecory =
                appParams1.GetAs<string>("DI_SaveReport731ToDirectory")
                ?? appParams.GetAs<string>("DI_SaveReport731ToDirectory");

            if (string.IsNullOrEmpty(needSaveReportToDirecory))
            {
                var fileName = string.Format("report731_year{0}_inn{1}", year, inn);
                var fileRepos = Container.ResolveRepository<Bars.B4.Modules.FileStorage.FileInfo>();

                var file =
                    fileRepos.GetAll()
                        .Where(x => x.Name == fileName)
                        .Where(x => x.ObjectCreateDate >= dateActuality)
                        .Where(x => x.Extention.Contains("xlsx"))
                        .OrderByDescending(x => x.ObjectCreateDate)
                        .FirstOrDefault();
                if (file != null)
                {
                    var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(file.FullName));

                    return new FileStreamResult(_fileManager.GetFile(file), mimeType) { FileDownloadName = file.FullName };
                }

                FileStreamResult report;

                try
                {
                    report = Container.Resolve<IGkhReportService>().GetReport(GenerateBaseParams(inn, year));
                }
                catch (ArgumentException e)
                {
                    return JsonNetResult.Failure(e.Message);
                }

                _fileManager.SaveFile(report.FileStream, fileName + ".xlsx");

                return report;
            }
            else
            {
                var dirPath = needSaveReportToDirecory;
                var filesDirectory = new DirectoryInfo(dirPath);

                if (!filesDirectory.Exists)
                {
                    filesDirectory.Create();
                }

                var fileName = string.Format("report731_year{0}_inn{1}", year, inn);

                #warning Костылизация - поиск файла отчета для реформы жкх в папке по маске, в обход IFileManager, чтобы можно было скопировать в папку отчеты, сгенерированные на другой машине
                // TODO переписать так, чтобы генерация очтета отправлялась в определенную очередь, откуда файлики ложились в нужную папку и потом доставались через fileinfo как раньше
                // refactor this shit

                var files = filesDirectory.GetFiles(fileName + "*");

                var file = files.Where(x => x.LastWriteTime >= dateActuality)
                             .Where(x => x.FullName.Contains("xlsx"))
                             .OrderByDescending(x => x.CreationTime)
                             .FirstOrDefault();

                if (file != null)
                {
                    var stream = new MemoryStream(System.IO.File.ReadAllBytes(file.FullName));

                    var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(file.FullName));

                    return new FileStreamResult(stream, mimeType) { FileDownloadName = file.Name };
                }

                FileStreamResult report;

                try
                {
                    report = Container.Resolve<IGkhReportService>().GetReport(GenerateBaseParams(inn, year));

                    var buffer = new byte[report.FileStream.Length];

                    report.FileStream.Seek(0, SeekOrigin.Begin);
                    report.FileStream.Read(buffer, 0, buffer.Length);
                    report.FileStream.Seek(0, SeekOrigin.Begin);

                    System.IO.File.WriteAllBytes(
                        Path.Combine(filesDirectory.FullName,
                            string.Format("{0}.{1}", fileName, "xlsx")), buffer);
                }
                catch (ArgumentException e)
                {
                    return JsonNetResult.Failure(e.Message);
                }

                report.FileStream.Seek(0, SeekOrigin.Begin);
                _fileManager.SaveFile(report.FileStream, fileName + ".xlsx");

                return report;
            }

        }

        public ActionResult MassGenerate(BaseParams baseParams)
        {
            var objectIds = baseParams.Params.GetAs<long[]>("objectIds");
            var periodId = baseParams.Params.GetAsId("periodId");

            var period = Container.ResolveDomain<PeriodDi>().Get(periodId);

            if (period == null)
            {
                return JsonNetResult.Failure("Не удалось получить период раскрытия");
            }

            int countSuccess = 0;

            try
            {
                var inns = Container.ResolveRepository<ManagingOrganization>().GetAll()
                .Where(x => objectIds.Contains(x.Id))
                .Where(x => x.Contragent.Inn != null)
                .Select(x => x.Contragent.Inn.Trim());

                foreach (var inn in inns)
                {
                    GetReport731(new BaseParams
                    {
                        Params = new DynamicDictionary
                        {
                            {"inn", inn},
                            {"year", period.DateStart.GetValueOrDefault().Year}
                        }
                    });

                    countSuccess++;
                }
            }
            catch (Exception e)
            {
                return JsonNetResult.Failure(e.Message);
            }

            return JsSuccess(string.Format("Успешно сгенерировано {0} отчетов", countSuccess));
        }

        private BaseParams GenerateBaseParams(string inn, int year)
        {
            if (inn.IsEmpty())
            {
                throw new ArgumentException("Не указан ИНН");
            }

            inn = inn.Trim();

            var manorg = Container.Resolve<IRepository<ManagingOrganization>>().GetAll()
                .Where(x => x.Contragent.Inn.Trim() == inn)
                .ToList();

            if (manorg.Count > 1)
            {
                throw new ArgumentException("Существует более одной управляющей организации с таким ИНН");
            }

            if (manorg.Count == 0)
            {
                throw new ArgumentException("Отсутствует информация об управляющей организации с таким ИНН");
            }

            var period = Container.ResolveDomain<PeriodDi>().GetAll()
                .OrderByDescending(x => x.DateEnd)
                .FirstOrDefault(x => x.DateStart.Value.Year <= year && x.DateEnd.Value.Year >= year);

            if (period == null)
            {
                throw new ArgumentException("Отсутствует период");
            }

            var result = new BaseParams();

            result.Params.Add("reportId", "DisclosureInfo731");

            var userParams = new DynamicDictionary
            {
                {"manorgId", manorg.First().Id}, 
                {"periodId", period.Id}
            };

            result.Params.Add("userParams", userParams);

            return result;
        }

        public ActionResult ListManOrgsNotExistsReport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var periodId = baseParams.Params.GetAsId("periodId");

            var diPeriod = Container.ResolveDomain<PeriodDi>().Get(periodId);

            if (diPeriod == null)
            {
                return JsonNetResult.Success;
            }

            var filterExists = Container.ResolveRepository<FileInfo>().GetAll()
                .Where(x => x.Name.StartsWith("report731"))
                .Where(x => x.Extention.Contains("xlsx"));

            var dictCountRo = Container.ResolveDomain<ManOrgContractRealityObject>().GetAll()
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Where(x => x.ManOrgContract.ManagingOrganization != null)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    ManOrgId = x.ManOrgContract.ManagingOrganization.Id,
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate
                })
                .Where(x => x.StartDate <= diPeriod.DateEnd)
                .Where(x => !x.EndDate.HasValue || x.EndDate >= diPeriod.DateStart)
                .AsEnumerable()
                .GroupBy(x => x.ManOrgId)
                .ToDictionary(x => x.Key, y => y.Distinct(x => x.Id).Count());

            var data = Container.ResolveRepository<ManagingOrganization>().GetAll()
                .Where(x => x.Contragent.ContragentState == ContragentState.Active || x.Contragent.DateTermination >= diPeriod.DateStart)
                .Where(y =>
                    !filterExists
                        .Any(x => x.Name ==
                                  ("report731_year" + diPeriod.DateStart.GetValueOrDefault().Year
                                   + "_inn" + y.Contragent.Inn.Trim())))
                .Where(x => x.Contragent.Inn != null)
                .Select(x => new
                {
                    ManOrgId = x.Id,
                    ManOrgName = x.Contragent.Name,
                    x.Contragent.Inn
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.ManOrgId,
                    x.ManOrgName,
                    x.Inn,
                    CountRo = dictCountRo.Get(x.ManOrgId)
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new JsonListResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());

        }
    }
}