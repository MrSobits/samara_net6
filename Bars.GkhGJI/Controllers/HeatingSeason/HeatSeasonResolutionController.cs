namespace Bars.GkhGji.Controllers.HeatingSeason
{
    using System;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class HeatingSeasonResolutionController : FileStorageDataController<HeatingSeasonResolution>
    {
        public ActionResult ListFull(BaseParams baseParams)
        {
            var periodId = baseParams.Params.GetAs<long>("periodId");
            var userManager = Container.Resolve<IGkhUserManager>();
            var municipalityDomain = Container.Resolve<IRepository<Municipality>>();
            var resolutionDomain = Container.Resolve<IDomainService<HeatingSeasonResolution>>();

            var municipalityIds = userManager.GetMunicipalityIds();

            // Получаем список постановлений по периоду
            var resolutions = resolutionDomain.GetAll().Where(x => x.HeatSeasonPeriodGji.Id == periodId).Select(x => new
            {
                AcceptDate = x.AcceptDate,
                Doc = x.Doc != null ? x.Doc.Name : "",
                x.Municipality,
                MunicipalityName = x.Municipality.Name,
                x.HeatSeasonPeriodGji,
                x.Id,
                Phantom = false
            }).ToList();

            var resolutionMoIds = resolutions.Select(x => x.Municipality.Id).ToList();

            // Получаем МО (без МР), по которым нет постановлений
            var municipalities = municipalityDomain.GetAll().
                Where(x => x.ParentMo != null). // Фильтруем муниципальные районы
                WhereIf(municipalityIds.Count > 0,
                    x => municipalityIds.Contains(x.Id) || municipalityIds.Contains(x.ParentMo.Id))
                . // Если у оператора задан список МО, то список постановлений формируется по списку.
                WhereIf(resolutionMoIds.Count > 0, x => !resolutionMoIds.Contains(x.Id));

            // Объединяем постановления со списком МО, по которым нет постановлений
            var union = resolutions.ToList();

            var nextId = union.Count > 0 ? union.Max(x => x.Id) : 0L;

            foreach (var mo in municipalities)
            {
                nextId++;
                union.Add(new
                {
                    AcceptDate = DateTime.MinValue,
                    Doc = string.Empty,
                    Municipality = mo,
                    MunicipalityName = mo.Name,
                    HeatSeasonPeriodGji = (HeatSeasonPeriodGji)null,
                    Id = nextId,
                    Phantom = true
                });
            }

            var loadParam = GetLoadParam(baseParams);

            var result = union.OrderBy(x => x.MunicipalityName).AsQueryable().Filter(loadParam, Container).Order(loadParam);

            return new JsonListResult(result.Paging(loadParam).ToList(), result.Count());
        }
    }
}
