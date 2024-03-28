namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.Overhaul.Hmao.DomainService;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class PublishedProgramController : B4.Alt.DataController<Entities.PublishedProgram>
    {
        public IFileManager FileManager { get; set; }
        public IDomainService<FileInfo> FileInfoDomain { get; set; }

        public IPublishProgramService Service { get; set; }

        public ActionResult GetPublishedProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetPublishedProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetValidationForCreatePublishProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetValidationForCreatePublishProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Message) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetValidationForSignEcp(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetValidationForSignEcp(baseParams);
            return result.Success ? new JsonNetResult(result.Message) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetDataToSignEcp(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetDataToSignEcp(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetPdf(BaseParams baseParams)
        {
            var pdfId = baseParams.Params.GetAs<long>("pdfId");
            
            if (pdfId != 0)
            {
                return B4FileStreamResult.Inline(new MemoryStream(new byte [0]), "PublishedProgram.pdf", "application/pdf");
            }
            
            using (var file = FileManager.GetFile(FileInfoDomain.Get(pdfId)))
            {
                using (var tmpStream = new MemoryStream())
                {
                    file.CopyTo(tmpStream);
                    return B4FileStreamResult.Inline(tmpStream, "PublishedProgram.pdf", "application/pdf");
                }
            }
        }

        public ActionResult SaveSignedResult(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.SaveSignedResult(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult DeletePublishedProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.DeletePublishedProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}