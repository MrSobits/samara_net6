namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Enums;

    public class BankDocumentResolverController : BaseController
    {
        public BankDocumentResolver Resolver { get; set; }

        public ActionResult GetDocsForResolve(BaseParams @params)
        {
            var data = Resolver.GetUnresolvedDocs(@params);

            return new JsonNetResult(data);
        }

        public ActionResult GetSuspects(BaseParams @params)
        {
            var type = @params.Params.GetAs<ImportedPaymentState>("type", ignoreCase: true);

            return new JsonNetResult(Resolver.GetSuspects(type, @params));
        }

        public ActionResult ResolveDocs(BaseParams @params)
        {
            var dtos = @params.Params.GetAs<List<DocumentForResolveDto>>("records");

            Resolver.Resolve(dtos);

            return JsSuccess();
        }

        public ActionResult ResolveUnacceptedPayments()
        {
            Resolver.ResolveUnacceptedPayments();

            return JsSuccess();
        }
    }
}