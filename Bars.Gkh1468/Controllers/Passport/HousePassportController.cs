namespace Bars.Gkh1468.Controllers.Passport
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Import;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities.Passport;
    using Newtonsoft.Json;
    using NHibernate.Util;

    public class HousePassportController : B4.Alt.DataController<HousePassport>
    {
        public IHousePassportService Service { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; } 

        public ActionResult Import1468Passports(BaseParams baseParams)
        {
            var service = Container.Resolve<IGkhImportService>();
            var result = service.Import(baseParams);

            var anyFile = baseParams.Files.Any();

            if (anyFile)
            {
                var res = new
                {
                    success = result.Success,
                    result.Message
                };

                return Content(JsonConvert.SerializeObject(res), "text/html");
            }

            return new JsonNetResult(result);
        }
    }
}