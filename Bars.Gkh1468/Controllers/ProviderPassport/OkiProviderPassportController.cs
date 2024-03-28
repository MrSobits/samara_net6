namespace Bars.Gkh1468.Controllers
{
    using System.Collections;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4.Modules.FileStorage;
    using Bars.B4;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;

    public class OkiProviderPassportController : BaseProviderPassportController<OkiProviderPassport>
    {
        public OkiProviderPassportController(IFileManager fileManager, IDomainService<FileInfo> fileDomain) : base(fileManager, fileDomain)
        {
        }

        public IOkiPassportProviderService Service { get; set; }

        public ActionResult ListByPassport(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListByPassport(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }

        public ActionResult GetActivePassportStruct(BaseParams baseParams)
        {
            var result = Service.GetActivePassportStruct(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult MakeNewPassport(BaseParams baseParams)
        {
            var result = Service.MakeNewPassport(baseParams);
            return new JsonNetResult(result);
        }

        public override bool CheckPermissions(long id)
        {
            var SignPermission = "Gkh1468.Passport.MyOki.Sign";
            var authorization = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
            var obj = Container.Resolve<IDomainService<OkiProviderPassport>>().Get(id);

            if (authorization != null && obj != null)
            {
                return authorization.Grant(Container.Resolve<IUserIdentity>(), SignPermission, obj);
            }

            return base.CheckPermissions(id);
        }

        public ActionResult MunicipalityForOki(BaseParams baseParams)
        {
            var result = Service.MunicipalityForOki(baseParams);
            return new JsonNetResult(result);
        }
    }
}