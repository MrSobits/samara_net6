namespace Bars.Gkh.Controllers.RealityObj
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Пересчитывает площадь из сведений о помещениях МКД
    /// </summary>
    public class RealityObjectFieldsController : BaseController
    {
        /// <summary>
        /// Получает значение поля для заполнения
        /// </summary>
        /// <param name="baseParams">Id - id дома, FieldName - поле для заполнения</param>
        public ActionResult GetFieldValue(BaseParams baseParams)
        {
            IDataResult result = null;
            this.Container.UsingForResolved<IRealityObjectFieldsService>((container, fildService) =>
            {
                result = fildService.GetFieldValue(baseParams);
            });
            return new JsonNetResult(result);
        }
    }
}