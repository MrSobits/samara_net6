namespace Bars.Gkh.Gis.Controllers.Register.HouseRegisterRegister
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Gis.DomainService.Register.HouseRegisterRegister;

    public class GisHouseRegisterController : BaseController
    {
        /// <summary>
        /// Скопировать параметры домов РЖД в дома ГИС
        /// </summary>        
        public ActionResult CopyParams()
        {
            var result = (ListDataResult) Container.Resolve<IHouseRegisterService>().CopyHouseParams();
            if (!result.Success)
            {
                return JsonNetResult.Failure(result.Message);
            }
            return Redirect(Url.Content("~/"));
        }        
    }
}
