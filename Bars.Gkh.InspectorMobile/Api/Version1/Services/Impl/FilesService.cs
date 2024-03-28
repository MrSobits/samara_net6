namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.General;

    using System.Linq;
    using System.Web;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.File;
    using Bars.Gkh.PrintForm.Entities;
    using Bars.Gkh.PrintForm.Services;
    using Bars.Gkh.PrintForm.Utils;
    using Bars.Gkh.Report;
    using Bars.Gkh.Services.ServiceContracts;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    using Castle.Windsor;

    using NHibernate.Linq;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для работы с файлами
    /// </summary>
    public class FilesService : IFilesService
    {
        #region DependencyInjection
        private readonly IWindsorContainer _container;
        private readonly IFileManager _fileManager;
        private readonly IDomainService<FileInfo> _fileInfoDomain;
        private readonly IDomainService<PdfSignInfo> _pdfSignInfoDomain;
        private readonly IPdfTunerService _pdfTuner;
        private readonly ICryptoService _cryptoService;

        /// <summary>
        /// Сервис для работы с файлами
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="fileManager">Менеджер файлов</param>
        /// <param name="fileInfoDomain">Домен-сервис для <see cref="FileInfo"/></param>
        /// <param name="pdfSignInfoDomain">Домен-сервис для <see cref="PdfSignInfo"/></param>
        /// <param name="pdfTuner">Сервис для внесения изменений в PDF</param>
        /// <param name="cryptoService">Сервис для работы с криптографией</param>
        public FilesService(
            IWindsorContainer container,
            IFileManager fileManager,
            IDomainService<FileInfo> fileInfoDomain,
            IDomainService<PdfSignInfo> pdfSignInfoDomain,
            IPdfTunerService pdfTuner,
            ICryptoService cryptoService)
        {
            this._container = container;
            this._fileManager = fileManager;
            this._fileInfoDomain = fileInfoDomain;
            this._pdfSignInfoDomain = pdfSignInfoDomain;
            this._pdfTuner = pdfTuner;
            this._cryptoService = cryptoService;
        }
        #endregion

        /// <inheritdoc />
        public long Upload(HttpFileCollection files)
        {
            if (files.Count != 1)
            {
                var msg = files.Count == 0
                    ? "В запросе нет файла"
                    : "Передано более одного файла";

                throw new ApiServiceException(msg);
            }

            var gkhConfigProvider = this._container.Resolve<IGkhConfigProvider>();
            using (this._container.Using(gkhConfigProvider, this._fileManager))
            {
                var file = files[0];
                var maxUploadFileSize = gkhConfigProvider.Get<GeneralConfig>().MaxUploadFileSize;

                if (file.ContentLength > maxUploadFileSize * Math.Pow(1024, 2))
                {
                    throw new ApiServiceException("Превышен максимально допустимый размер файла");
                }

                var res = this._fileManager.SaveFile(file.InputStream, file.FileName);
                return res.Id;
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetAsync(long fileId)
        {
            var fileInfo = this._fileInfoDomain.Get(fileId);
            if (fileInfo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var fileStream = this._fileManager.GetFile(fileInfo);

            return await this.CreateHttpResponseWithFile(fileInfo.FullName, fileStream);
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> GetPrintedFormAsync(long documentId, PrintedFormDocumentType documentType)
        {
            var reportId = await this.GetReportId(documentId, documentType);

            var baseParams = new BaseParams
            {
                Params = new DynamicDictionary
                {
                    { "userParams", new DynamicDictionary { { "DocumentId", documentId } } },
                    { "reportId", reportId }
                }
            };

            var gkhReportService = this._container.Resolve<IGkhReportService>();

            using (this._container.Using(gkhReportService))
            {
                var file = gkhReportService.GetReport(baseParams);

                return await this.CreateHttpResponseWithFile(file.FileDownloadName, file.FileStream);
            }
        }

        /// <inheritdoc />
        public async Task<HttpResponseMessage> AddSignatureStamp(SigningFileInfo signingFileInfo)
        {
            var pdfInfo = this._fileInfoDomain.Get(signingFileInfo.FileId);
                
            var contextGuid = Guid.NewGuid().ToString();
            var stampStrategy = new AfterTextCentrStampStrategy
            {
                PersonName = signingFileInfo.CertificateInfo.PersonName,
                SerialNumber = signingFileInfo.CertificateInfo.SerialNumber,
                ValidFromDate = signingFileInfo.CertificateInfo.ValidFromDate,
                ValidToDate = signingFileInfo.CertificateInfo.ValidToDate,
                ContextGuid = contextGuid,
                Reason = $"Подписание документа пользователем {signingFileInfo.CertificateInfo.PersonName}"
            };

            var pdfSignInfo = await this._pdfTuner.AddSignatureStampAsync(pdfInfo, stampStrategy);
            pdfSignInfo.ContextGuid = contextGuid;

            using (var transaction = this._container.Resolve<IDataTransaction>())
            {
                try
                {
                    await this.SaveSignInfoAsync(pdfSignInfo);
                    await this.CreateDocumentSignInfoLinkAsync(signingFileInfo.DocumentId, pdfSignInfo);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                
                transaction.Commit();
            }

            return await CreateHttpResponseWithFile(pdfSignInfo.StampedPdf.FullName, this._fileManager.GetFile(pdfSignInfo.ForHasFile));
        }
        
        /// <inheritdoc />
        public async Task<HttpResponseMessage> EmbedSignature(FileSignature signatureInfo)
        {
            var pdfSignInfo = this._pdfSignInfoDomain.GetAll()
                .FirstOrDefault(x => x.OriginalFile.Id == signatureInfo.FileId);

            var enhancedSignature = await this._cryptoService.EnhanceSignatureAsync(signatureInfo.Signature, pdfSignInfo.ForHasFile, CadesType.CADESCOM_CADES_BES, CadesType.CADESCOM_CADES_T);
            var signedPdfInfo = await this._pdfTuner.EmbedSignatureAsync(pdfSignInfo.StampedPdf, pdfSignInfo.OriginalFile, enhancedSignature, pdfSignInfo.ContextGuid);

            pdfSignInfo.SignedPdf = signedPdfInfo;
            await this.SaveSignInfoAsync(pdfSignInfo);
            
            return await this.CreateHttpResponseWithFile(pdfSignInfo.SignedPdf.FullName, this._fileManager.GetFile(signedPdfInfo));
        }

        /// <summary>
        /// Сохранить информацию о подписании
        /// </summary>
        private async Task SaveSignInfoAsync(PdfSignInfo signInfo)
        {
            if (signInfo.Id == 0)
            {
                var signInfoId = await _pdfSignInfoDomain.GetAll()
                    .Where(x => x.OriginalFile.Id == signInfo.OriginalFile.Id)
                    .Select(x => x.Id)
                    .SingleOrDefaultAsync();
                signInfo.Id = signInfoId;
            }

            _pdfSignInfoDomain.SaveOrUpdate(signInfo);
        }

        /// <summary>
        /// Создать связь документа и информации о подписании
        /// </summary>
        private async Task CreateDocumentSignInfoLinkAsync(long documentId, PdfSignInfo signInfo)
        {
            var documentSignInfoLinkDomain = this._container.ResolveDomain<DocumentGjiPdfSignInfo>();

            using (this._container.Using(documentSignInfoLinkDomain))
            {
                if (!await documentSignInfoLinkDomain.GetAll().AnyAsync(x => x.DocumentGji.Id == documentId && x.PdfSignInfo.Id == signInfo.Id))
                {
                    var link = new DocumentGjiPdfSignInfo
                    {
                        DocumentGji = new DocumentGji { Id = documentId },
                        PdfSignInfo = signInfo
                    };
                    
                    documentSignInfoLinkDomain.Save(link);
                }
            }
        }

        /// <summary>
        /// Сформировать http-response с приложенным файлом
        /// </summary>
        /// <param name="fileName">Наименование файла</param>
        /// <param name="fileStream">Содержимое файла</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> CreateHttpResponseWithFile(string fileName, Stream fileStream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);

                var result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(memoryStream.GetBuffer())
                };

                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = HttpUtility.UrlEncode(fileName)
                };

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return result;
            }
        }

        /// <summary>
        /// Получить идентификатор отчета (печатной формы)
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="documentType">Сопоставляемый тип документа</param>
        /// <returns></returns>
        /// <exception cref="ApiServiceException">Ошибка, возникающая при некорректной передаче тпа документа</exception>
        private async Task<string> GetReportId(long documentId, PrintedFormDocumentType documentType)
        {
            if (documentType == default)
                throw new ApiServiceException("Указанный тип документа не поддерживается");

            bool? entityExists = null;

            switch (documentType)
            {
                case PrintedFormDocumentType.ProtocolActAction:
                    var actCheckActionDomain = this._container.ResolveDomain<ActCheckAction>();

                    using (this._container.Using(actCheckActionDomain))
                    {
                        entityExists = await actCheckActionDomain.GetAll()
                            .AnyAsync(x => x.Id == documentId);
                    }
                    break;
            }

            if (!entityExists.HasValue)
            {
                var documentGjiDomain = this._container.ResolveDomain<DocumentGji>();

                using (this._container.Using(documentGjiDomain))
                {
                    var customValueAttribute = documentType.GetCustomValueAttribute<CustomValueAttribute>(nameof(TypeDocumentGji));

                    entityExists = await documentGjiDomain.GetAll()
                        .AnyAsync(x => x.Id == documentId &&
                            x.TypeDocumentGji == (TypeDocumentGji)customValueAttribute.Value);
                }
            }

            if (!entityExists.Value)
                throw new ApiServiceException("Не удалось найти документ для формирования печатной формы");

            return documentType.ToString();
        }
    }
}