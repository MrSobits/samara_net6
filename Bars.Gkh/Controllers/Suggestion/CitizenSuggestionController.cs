// -----------------------------------------------------------------------
// <copyright file="ServiceOrganizationController.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Bars.Gkh.Controllers
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities.Suggestion;
    using Ionic.Zip;
    using Ionic.Zlib;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class CitizenSuggestionController : FileStorageDataController<CitizenSuggestion>
    {
        public ActionResult ListExecutor(BaseParams baseParams)
        {
            var result = (ListDataResult)this.Resolve<ICitizenSuggestionService>().ListExecutor(baseParams);
            return result.Success?new JsonListResult((IList)result.Data, result.TotalCount):JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Напечатать Обращение граждан с (портала)
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult PrintSuggestionPortalReport(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ICitizenSuggestionReportService>();
            try
            {
                var result = service.PrintSuggestionPortalReport(baseParams);
                return new ReportStreamResult(result, "Обращение.docx");
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        //Создание архива с обращением и файлами, ассоциированными с ним
        public ActionResult GetArchive(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ICitizenSuggestionReportService>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var fileInfoDomainService = this.Container.ResolveDomain<FileInfo>();
            var citizenSuggestionService = this.Container.ResolveDomain<CitizenSuggestion>();
            var citizenSuggestionFileService = this.Container.ResolveDomain<CitizenSuggestionFiles>();
            
            try
            {
                var portalReport = service.PrintSuggestionPortalReport(baseParams);
                var portalReportResult = new ReportStreamResult(portalReport, "Обращение.docx");

                var citizenSuggestionId = baseParams.Params.GetAs<long>("citizenSuggestionId");
                var citizenSuggestion = citizenSuggestionService.Get(citizenSuggestionId);

                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866"),
                    AlternateEncodingUsage = ZipOption.AsNecessary
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
                bool skip = false;
                try
                {
                    var configProvider = Container.Resolve<IConfigProvider>();
                    var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Voronezh");
                    skip = config.GetAs<bool>("SkipWordSugestion", false, true);
                }
                catch (Exception e)
                {

                }
                if (!skip)
                {
                    System.IO.File.WriteAllBytes(Path.Combine(tempDir.FullName, "Обращение.docx"), portalReportResult.FileStream.ReadAllBytes());
                }

                var citizenSuggestionFiles = citizenSuggestionFileService.GetAll().Where(x => x.CitizenSuggestion.Id == citizenSuggestionId).ToList();

                foreach (var file in citizenSuggestionFiles)
                {
                    System.IO.File.WriteAllBytes(
                        Path.Combine(tempDir.FullName,$"{file.DocumentFile.Name}.{file.DocumentFile.Extention}"),
                        fileManager.GetFile(file.DocumentFile).ReadAllBytes());
                }

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);

                    var file = fileManager.SaveFile(ms, $"{citizenSuggestion.Number} - {citizenSuggestion.ApplicantFio}.zip");
                    var res= new JsonNetResult(new BaseDataResult(file.Id));
                    return res;
                }
                /*
                var contentDisposition = new ContentDisposition();
                contentDisposition.Inline = false;
                this.Response.AddHeader("Content-Disposition", $@"attachment; filename={citizenSuggestion.Number} - {citizenSuggestion.ApplicantFio}.zip");
                var result = new FileStreamResult(ms, "application/zip");*/
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(fileManager);
                this.Container.Release(fileInfoDomainService);
                this.Container.Release(citizenSuggestionService);
                this.Container.Release(citizenSuggestionFileService);
            }
        }
        public ActionResult SendAnswerEmail(BaseParams baseParams)
        {

            var sendService = this.Container.Resolve<ISuggestionSendEmailService>();

            var resultSend = sendService.SendAnswerEmail(baseParams);

            var result = new JsonNetResult(resultSend);
            return result;
        }
    }
}