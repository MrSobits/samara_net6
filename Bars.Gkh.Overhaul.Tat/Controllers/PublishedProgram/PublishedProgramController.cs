namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Tat.DomainService;

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

        public ActionResult GetPdf(BaseParams baseParams)
        {
            var pdfId = baseParams.Params.GetAs<long>("pdfId");

            byte[] data;
            if (pdfId == 0)
            {
                data = new byte[0];
            }
            else
            {
                using (var file = FileManager.GetFile(FileInfoDomain.Get(pdfId)))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        file.CopyTo(tmpStream);
                        data = tmpStream.ToArray();
                    }
                }
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=PublishedProgram.pdf");
            Response.Headers.Add("Content-Length", data.Length.ToString());
            return File(data, "application/pdf");
        }

        public ActionResult DeletePublishedProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.DeletePublishedProgram(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}