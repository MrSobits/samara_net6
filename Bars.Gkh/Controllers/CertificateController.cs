namespace Bars.Gkh.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Utils;
    using Domain;

    public class CertificateController : BaseController
    {
        public ActionResult Validate(BaseParams baseParams)
        {
            var certificate = baseParams.Params.GetAs<string>("certificate");

            if (certificate.IsEmpty())
            {
                return JsFailure("Сертификат пуст!");
            }

            try
            {
                var certX509 = new X509Certificate2(Encoding.UTF8.GetBytes(certificate));

                var valid = Container.ResolveAll<IX509CertificateValidator>().All(x => x.Validate(certX509));

                return valid ? JsSuccess() : JsFailure("Сертификат не корректен!");
            }
            catch (Exception e)
            {
                return JsFailure(e.Message);
            }
        }
    }
}