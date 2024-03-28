namespace Bars.Gkh.ClaimWork.Controllers
{
    using System.Collections;
   using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.ClaimWork.Services;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    public class JurInstitutionController : B4.Alt.DataController<JurInstitution>
    {
        /// <summary>
        /// Добавить жилые дома
        /// </summary>
        public ActionResult AddRealObjs(BaseParams baseParams)
        {
            var jurInstitutionService = this.Container.Resolve<IJurInstitutionService>();

            try
            {
                return new JsonNetResult(jurInstitutionService.AddRealObjs(baseParams));
            }
            finally
            {
                this.Container.Release(jurInstitutionService);
            }
        }

        /// <summary>
        /// Получить жилые дома для добавления
        /// </summary>
        public ActionResult ListRealObj(StoreLoadParams storeParams)
        {
            var jurInstitutionService = this.Container.Resolve<IJurInstitutionService>();

            try
            {
                var result = (ListDataResult) jurInstitutionService.ListRealObj(storeParams);
                return result.Success ? new JsonListResult((IList) result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(jurInstitutionService);
            }
        }

        /// <summary>
        /// Поулчить добавленные жилые дома
        /// </summary>
        public ActionResult ListRealObjByMunicipality(BaseParams baseParams)
        {
            var jurInstitutionService = this.Container.Resolve<IJurInstitutionService>();

            try
            {
                return jurInstitutionService.ListRealObjByMunicipality(baseParams).ToJsonResult();;
            }
            finally
            {
                this.Container.Release(jurInstitutionService);
            }
        }
    }
}