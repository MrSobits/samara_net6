namespace Bars.Gkh1468.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh1468.DomainService.Passport;
    using Bars.Gkh1468.Entities;

    using NHibernate.Hql.Ast.ANTLR;
    using NHibernate.Linq.Visitors;

    public class PartController : BaseDataController<Part>
    {
        public ActionResult Get(BaseParams baseParams)
        {
            //var muId = baseParams.Params.Get("muId", (long)0);
            //var month = baseParams.Params.Get("month", (long)0);
            //var year = baseParams.Params.Get("year", (long)0);

            //var res = Container.Resolve<IHousePassportService>().FillPassport(muId, year, month);

            return JsSuccess();
        }

        public ActionResult Create(BaseParams baseParams)
        {
            var domain = Container.ResolveDomain<Part>();
            var result = domain.Save(baseParams);
            return JsSuccess(result.Data);
        }

        public ActionResult Update(BaseParams baseParams)
        {
            var domain = Container.ResolveDomain<Part>();
            var result = domain.Update(baseParams);
            return JsSuccess(result.Data);
        }

        public ActionResult Delete(long id)
        {
            var result = Container.Resolve<IStructPartService>().RemovePart(id);

            return new JsonNetResult(result);
        }

        public ActionResult GetTree(BaseParams baseParams)
        {
            return JsSuccess();
        }
    }
}
