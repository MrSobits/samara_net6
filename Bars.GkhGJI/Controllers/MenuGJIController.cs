namespace Bars.GkhGji.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Controllers;
    using Bars.GkhGji.DomainService;

    public class MenuGjiController : MenuController
    {
        /// <summary>
        /// Меню для карточки Деятельность ТСЖ 
        /// </summary>
        public ActionResult GetActivityTsjMenu(StoreLoadParams storeParams)
        {
            var id = storeParams.Params.GetAs<long>("objectId");
            return id > 0 ? new JsonNetResult(GetMenuItems("ActivityTsj")) : new JsonNetResult(null);
        }

        /// <summary>
        /// Меню для реестра документов ГЖИ
        /// </summary>
        public ActionResult GetDocumentsGjiRegisterMenu()
        {
            return new JsonNetResult(GetMenuItems("DocumentsGjiRegister"));
        }

        /// <summary>
        /// Меню для карточек проверок
        /// </summary>
        public ActionResult GetInspectionMenu(StoreLoadParams storeParams)
        {
            var service = Container.Resolve<IInspectionMenuService>();
            try
            {
                var inspectionId = storeParams.Params.GetAs<long>("inspectionId");
                long? documentId = storeParams.Params.GetAs<long?>("documentId");

                var result = service.GetMenu(inspectionId, documentId);

                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
            
        }
    }
}