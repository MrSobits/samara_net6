namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.File;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API-контроллер для работы с файлами
    /// </summary>
    [RoutePrefix("v1/files")]
    public class FilesController : BaseApiController
    {
        /// <summary>
        /// Загрузить файл на сервер
        /// </summary>
        [HttpPost]
        [Route("upload")]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Upload() =>
            await this.ExecuteApiServiceMethodAsync<IFilesService>(nameof(IFilesService.Upload), HttpContext.Current.Request.Files);

        /// <summary>
        /// Скачать файл с сервера
        /// </summary>
        /// <param name="fileId">Идентификатор файла</param>
        [HttpGet]
        [Route("download/{fileId:long}")]
        public async Task<IHttpActionResult> GetAsync(long fileId) =>
            await this.ExecuteApiServiceMethodWithResultCastingAsync<IFilesService>(obj => this.ResponseMessage((HttpResponseMessage)obj), nameof(IFilesService.GetAsync), fileId);

        /// <summary>
        /// Сформировать и получить печатную форму документа
        /// </summary>
        [HttpGet]
        [Route("printedForms")] 
        public async Task<IHttpActionResult> GetPrintedFormAsync(long documentId, PrintedFormDocumentType documentType) =>
            await this.ExecuteApiServiceMethodWithResultCastingAsync<IFilesService>(obj => this.ResponseMessage((HttpResponseMessage)obj), nameof(IFilesService.GetPrintedFormAsync), documentId, documentType);

        /// <summary>
        /// Добавить штамп в Pdf файл
        /// </summary>
        [HttpPost]
        [Route("addSignatureStamp")]
        public async Task<IHttpActionResult> AddSignatureStamp([FromBody] SigningFileInfo info) =>
            await this.ExecuteApiServiceMethodWithResultCastingAsync<IFilesService>(obj => this.ResponseMessage((HttpResponseMessage)obj), nameof(IFilesService.AddSignatureStamp), info);
        
        /// <summary>
        /// Внедрить подпись в Pdf файл
        /// </summary>
        [HttpPost]
        [Route("embedSignature")]
        public async Task<IHttpActionResult> EmbedSignature([FromBody] FileSignature signatureInfo) =>
            await this.ExecuteApiServiceMethodWithResultCastingAsync<IFilesService>(obj => this.ResponseMessage((HttpResponseMessage)obj), nameof(IFilesService.EmbedSignature), signatureInfo);
    }
}