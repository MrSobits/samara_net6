namespace Bars.GkhGji.Regions.Tula.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Regions.Tula.DomainService;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class DocumentViolGroupController : B4.Alt.DataController<DocumentViolGroup>
    {
        public IBlobPropertyService<DocumentViolGroup, DocumentViolGroupLongText> LongTextService { get; set; }

        public IViolationGroupService ViolationGroupoService { get; set; }

        /// <summary>
        /// Метод сохранения Пунктов нарушений для Описания
        /// </summary>
        public virtual ActionResult SavePoints(BaseParams baseParams)
        {
            var result = this.ViolationGroupoService.SavePoints(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод сохранения Blob поля Описание
        /// </summary>
        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод получения Blob поля Описание
        /// </summary>
        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод сохранения Blob поля Мероприятие
        /// </summary>
        public virtual ActionResult SaveAction(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод получения Blob поля Мероприятие
        /// </summary>
        public virtual ActionResult GetAction(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}