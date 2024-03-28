namespace Bars.Gkh.Controllers.RealityObj
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Контроллер для фотографий жилого дома
    /// </summary>
    public class RealityObjectImageController : FileStorageDataController<RealityObjectImage>
    {
        public IFileManager FileManager { get; set; }
        
        /// <summary>
        /// Получение файла
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Файл в формате Base64String</returns>
        public ActionResult GetFileUrl(BaseParams baseParams)
        {
            var objectId = baseParams.Params.GetAs<long>("id");

            var objectImage = this.DomainService.GetAll()
                .FirstOrDefault(x => x.RealityObject.Id == objectId && x.ImagesGroup == ImagesGroup.Avatar);

            try
            {
                if (objectImage?.File == null || !this.FileManager.CheckFile(objectImage.File.Id).Success)
                {
                    return new JsonNetResult(new {success = false});
                }

                return new JsonNetResult(
                    new
                    {
                        success = true,
                        src = this.FileManager.GetBase64String(objectImage.File),
                        imageId = objectImage.Id
                    });
            }
            catch (Exception exception)
            {
                return JsonNetResult.Failure(exception.Message);
            }
        }
    }
}