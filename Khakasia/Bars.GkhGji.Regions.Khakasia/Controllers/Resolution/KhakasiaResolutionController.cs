﻿namespace Bars.GkhGji.Regions.Khakasia.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    /// <summary>
    /// Контроллер постановления для Хакасии.
    /// </summary>
    public class KhakasiaResolutionController : ResolutionController
    {
        /// <summary>
        /// Сервис текстов в байтах.
        /// </summary>
        public IBlobPropertyService<Resolution, ResolutionLongDescription> LongTextService { get; set; }

        /// <summary>
        /// Получить описание.
        /// </summary>
        /// <param name="baseParams">
        /// Параметры запроса.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public virtual ActionResult GetDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Сохранить описание.
        /// </summary>
        /// <param name="baseParams">
        /// Параметры запроса.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public virtual ActionResult SaveDescription(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);
            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
    }
}
