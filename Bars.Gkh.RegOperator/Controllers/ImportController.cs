namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Utils;
    using Domain.ImportExport.Mapping;

    public class ImportController : BaseController
    {
        public ActionResult GetImportProviders(BaseParams baseParams)
        {
            var service = Container.ResolveAll<IAuthorizationService>().FirstOrDefault();
            var userIdentity = Container.Resolve<IUserIdentity>();

            var typeName = baseParams.Params.GetAs<string>("typeName");
            var names = typeName.Split(",");

            var providers = Container.ResolveAll<IImportMap>()
                .Where(x => string.IsNullOrEmpty(x.PermissionKey) || service.Grant(userIdentity, x.PermissionKey));

            var providerNames = providers
                .Select(x => new Proxy
                {
                    Code = "{0} ({1} | {2})".FormatUsing(x.ProviderCode, x.Format, x.Direction),
                    Name = x.GetName(),
                    Key = x.GetKey(),
                    Serializer = x.ProviderCode
                })
                .Where(x => names.Any(n => x.Key.StartsWith("{0}|".FormatUsing(n))))
                .GroupBy(x => x.Code)
                .Select(x => x.FirstOrDefault())
                .OrderBy(x => x.Name)
                .Where(x => x.IsNotNull())
                .ToList();

            providers.ForEach(Container.Release);

            return new JsonListResult(providerNames, providerNames.Count());
        }

        public class Proxy
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Key { get; set; }
            public string Serializer { get; set; }
        }
    }
}