namespace Bars.Gkh1468.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.StimulReport;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Report;

    using B4.IoC;

    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Microsoft.AspNetCore.Mvc.ActionConstraints;

    public class FiasController : Gkh.Controllers.FiasController
    {
        public ActionResult Print()
        {
            var report = new OkiPassportReport { Container = Container };
            
            report.SetExportFormat(new PrintFormExportFormat { Id = (int)StiExportFormat.Pdf });

            var result = report.GetGeneratedReport();

            var mimeType = report.GetMimeType();
            var fileName = report.GetFileName();

            // Хак для отображения русских имен файлов
            fileName = UserAgentInfo.GetBrowserName(ControllerContext.HttpContext.Request).StartsWith("IE")
                ?  WebUtility.UrlEncode(fileName).Replace("+", "%20")
                : fileName.Replace("\"", "'");

            return new FileStreamResult(result, mimeType) { FileDownloadName = fileName };
        }

        public override ActionResult List(StoreLoadParams storeParams)
        {
            /*
             Тут мы получаем все дочерние элементы по уровню
            */

            var parentGuid = storeParams.Params.ContainsKey("parentGuid")
                                   ? storeParams.Params["parentGuid"].ToString()
                                   : "";

            var level = storeParams.Params.ContainsKey("level")
                                   ? storeParams.Params["level"].ToInt()
                                   : 0;

            var service = Container.Resolve<IDomainService<Fias>>();

            var data = service.GetAll()
                .Where(x => x.ParentGuid == parentGuid)
                .WhereIf(level > 0, x => x.AOLevel == (FiasLevelEnum)level)
                .Select(x => new
                {
                    x.Id,
                    Name = x.OffName+" "+x.ShortName,
                    x.CodeRecord,
                    x.AOLevel,
                    x.AOGuid,
                    x.TypeRecord,
                    x.ActStatus,
                    x.MirrorGuid
                 })
                .Filter(storeParams, Container);

            int totalCount = data.Count();

            data = data.Order(storeParams).Paging(storeParams);

            return new JsonListResult(data.ToList(), totalCount);
        }

        public ActionResult GetRegionList()
        {
            var repository = Container.Resolve<IFiasRepository>();
            var regData = repository.GetAll()
                .Where(x => x.AOLevel == FiasLevelEnum.Region && x.ActStatus == FiasActualStatusEnum.Actual)
                .Select(x => new { GuidId = x.AOGuid, PostCode = x.PostalCode, x.ShortName, x.OffName, x.CodeRegion })
                .ToArray();

            var data = regData.Select(x => new
            {
                x.GuidId,
                x.PostCode,
                Name = x.CodeRegion == "41" ? x.OffName + " " + x.ShortName : x.ShortName + ". " + x.OffName,
            }).ToArray();

            return new JsonListResult(data, data.Length);
        }

        /// <summary>
        /// метод получения динамических адресов уровней населенных пунктов (без улиц) по переданной строке фильтра
        /// </summary>
        [ParamsAttribute("parentguid")]
        public ActionResult GetPlacesList(string filter, string parentguid)
        {
            var repository = Container.Resolve<IFiasRepository>();
            var adrs = repository.GetPlacesDinamicAddress(filter, parentguid);
            return new JsonListResult(adrs, adrs.Count);
        }

        /// <summary>
        /// Геокодирует адреса (получает координаты)
        /// </summary>
        /// <param name="forceUpdate">Обновить все адреса</param>
        public ActionResult RefreshCoordinates(bool? forceUpdate)
        {
            var domain = Container.Resolve<IDomainService<FiasAddress>>();

            var addresses =
                domain.GetAll()
                      .WhereIf(!(forceUpdate.HasValue && forceUpdate.Value), x => x.Coordinate == null || x.Coordinate == "")
                      .Select(x => new { x.Id, x.AddressName })
                      .ToArray();

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                Parallel.ForEach(
                                 addresses,
                    a =>
                    {
                        var r = Yandex.GetCoordinates(a.AddressName);
                        if (r.IsNotEmpty())
                        {
                            var addr = domain.Load(a.Id);
                            addr.Coordinate = r;
                            domain.Update(addr);
                        }
                    });

                try
                {
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Получить список типов помещений
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPlacementTypeList()
        {
            var placementTypeDomain = this.Container.ResolveDomain<PlacementType>();
            PlacementType[] data;
            using (Container.Using(placementTypeDomain))
            {
                data = placementTypeDomain.GetAll().ToArray();
            }

            return new JsonListResult(data, data.Length);
        }
    }

    class ParamsAttribute : ActionMethodSelectorAttribute
    {
        private string ParamName { get; set; }

        public ParamsAttribute(string paramName)
        {
            this.ParamName = paramName;
        }

        public override bool IsValidForRequest(Microsoft.AspNetCore.Routing.RouteContext routeContext, Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor action)
        {
            return !string.IsNullOrEmpty(routeContext.HttpContext.Request.Form[this.ParamName]);
        }
    }
}