namespace Bars.Gkh.Controllers.Dict
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.Dict;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Контроллер для <see cref="BuilderDocumentType"/>
    /// </summary>
    public class BuilderDocumentTypeController : B4.Alt.DataController<BuilderDocumentType>
    {
        /// <summary>
        /// Вернуть все типы документов без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var documentTypeService = this.Container.Resolve<IBuilderDocumentTypeService>();
            try
            {
                var result = (ListDataResult)documentTypeService.ListWithoutPaging(baseParams);
                return result.ToJsonResult();
            }
            finally
            {
                this.Container.Release(documentTypeService);
            }
        }
    }
}
