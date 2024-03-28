namespace Bars.GkhGji.Controllers
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Ionic.Zip;
    using Ionic.Zlib;

    /// <summary>
    /// Контроллер <see cref="AppealCits"/>
    /// </summary>
    /// <typeparam name="T">Обращение</typeparam>
    public class AppealCitsController : AppealCitsController<AppealCits>
    {
    }

    /// <summary>
    /// Контроллер <see cref="AppealCits"/>
    /// </summary>
    /// <typeparam name="T">Обращение</typeparam>
    public class AppealCitsController<T> : FileStorageDataController<T>
        where T : AppealCits
    {
        public IBlobPropertyService<AppealCitsAnswer, AppealAnswerLongText> AnswerLongTextService { get; set; }
        public IBlobPropertyService<AppealCitsDefinition, AppealCitsDefinitionLongText> LongTextService { get; set; }

        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult GetAnswerDescription(BaseParams baseParams)
        {
            var result = this.AnswerLongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public virtual ActionResult SaveAnswerDescription(BaseParams baseParams)
        {
            //194149
            var result = this.AnswerLongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Добавить обращение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult AddAppealCitizens(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();
            try
            {
                var result = service.AddAppealCitizens(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Удалить связанные обращения
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="parentId">Родительский идентификатор</param>
        /// <returns>Результат</returns>
        public ActionResult RemoveRelated(long id, long parentId)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();
            try
            {
                var result = service.RemoveRelated(id, parentId);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Удалить связанные обращения
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="parentId">Родительский идентификатор</param>
        /// <returns>Результат</returns>
        public ActionResult RemoveAllRelated(long parentId)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();
            try
            {
                var result = service.RemoveAllRelated(parentId);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить информацию
        /// </summary>
        /// <param name="appealCitizensId">Идентификатор</param>
        /// <returns>Результат</returns>
        public ActionResult GetInfo(long? appealCitizensId)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();
            try
            {
                var result = service.GetInfo(appealCitizensId);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Экспорт обращения
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var exportService = this.Container.Resolve<IAppealCitsDataExport>();
            using (this.Container.Using(exportService))
            {
                return exportService.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Получить зонального инспектора
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult GetDefaultZji(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();
            try
            {
                var result = service.GetDefaultZji(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Добавить обращение граждан
        /// </summary>
        /// <param name="baseParams">The base Params.</param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult AddCitizenSuggestion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExportSuggestionService>();
            using (this.Container.Using(service))
            {
                return service.ExportCitizenSuggestionToGji(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Проверяет наличие созданного обращения
        /// </summary>
        /// <param name="baseParams">
        /// The base Params.
        /// </param>
        /// <returns>
        /// The <see cref="IDataResult"/>.
        /// </returns>
        public ActionResult CheckSuggestionExists(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IExportSuggestionService>();
            using (this.Container.Using(service))
            {
                return service.CheckSuggestionExists(baseParams).ToJsonResult();
            }
        }

        //Создание архива с приложениями к обращению
        public ActionResult GetAttachmentArchive(BaseParams baseParams)
        {
            var fileManager = this.Container.Resolve<IFileManager>();

            //var fileInfoDomainService = this.Container.ResolveDomain<B4.Modules.FileStorage.FileInfo>();
            var AppealCitsAttachmentDomain = this.Container.ResolveDomain<AppealCitsAttachment>();
            var AppealCitsDomain = this.Container.ResolveDomain<AppealCits>();

            try
            {
                var appealCitsId = baseParams.Params.GetAs<long>("appealCitsId");
                var appealCits = AppealCitsDomain.Get(appealCitsId);

                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866"),
                    AlternateEncodingUsage = ZipOption.AsNecessary
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
                bool skip = false;
                var appealCitsAttachmentFiles = AppealCitsAttachmentDomain.GetAll().Where(x => x.AppealCits.Id == appealCitsId).ToList();

                foreach (var file in appealCitsAttachmentFiles)
                {
                    System.IO.File.WriteAllBytes(
                        Path.Combine(tempDir.FullName, $"{file.FileInfo.Name}.{file.FileInfo.Extention}"),
                        fileManager.GetFile(file.FileInfo).ReadAllBytes());
                }

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);

                    var file = fileManager.SaveFile(ms,
                        $"Обращение{(appealCits.DateFrom.HasValue ? $" от {appealCits.DateFrom.Value.ToShortDateString()}" : "")}.zip");
                    var res = new JsonNetResult(new BaseDataResult(file.Id));
                    return res;
                }
                /*
                var contentDisposition = new ContentDisposition();
                contentDisposition.Inline = false;
                this.Response.Headers.Add("Content-Disposition", $@"attachment; filename={citizenSuggestion.Number} - {citizenSuggestion.ApplicantFio}.zip");
                var result = new FileStreamResult(ms, "application/zip");*/
            }
            finally
            {
                this.Container.Release(fileManager);

                //this.Container.Release(fileInfoDomainService);
                this.Container.Release(AppealCitsAttachmentDomain);
                this.Container.Release(AppealCitsDomain);
            }
        }

        /// <summary>
        /// Доступность работы с СОПР для указанного обращения 
        /// </summary>
        public ActionResult WorkWithSoprAvailable(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IAppealCitsService<ViewAppealCitizens>>();

            using (this.Container.Using(service))
            {
                return service.WorkWithSoprAvailable(baseParams).ToJsonResult();
            }
        }
    }
}