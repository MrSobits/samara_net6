namespace Bars.Gkh.ClaimWork.Controllers.Document
{
    using System;
    using System.Linq;
   using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using Bars.Gkh.Controllers.Document;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Domain;

    public class PetitionController : LawsuitController<Petition>
    {
        public ActionResult CheckChildDocs(BaseParams baseParams)
        {
            var lawsuitClwDocDomain = Container.ResolveDomain<LawsuitClwDocument>();
            var lawsuitClwCourtDomain = Container.ResolveDomain<LawsuitClwCourt>();
            var lawsuitId = baseParams.Params.GetAsId();

            try
            {
                if (lawsuitClwCourtDomain.GetAll().Any(x => x.DocumentClw.Id == lawsuitId)
                    || lawsuitClwDocDomain.GetAll().Any(x => x.DocumentClw.Id == lawsuitId))
                {
                    return JsSuccess("У документа существуют дочерние элементы. Вы уверены, что хотите его удалить?");
                }

                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsFailure(e.Message);
            }
            finally
            {
                Container.Release(lawsuitClwDocDomain);
                Container.Release(lawsuitClwCourtDomain);
            }
        }
    }
}