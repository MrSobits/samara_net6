namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Entities;

    public class MotivationConclusionController : MotivationConclusionController<MotivationConclusion>
    {
        /// <summary>
        /// Список документов
        /// </summary>
        /// <param name="baseParams">
        /// dateStart - Необходимо получить документы больше даты начала
        /// dateEnd - Необходимо получить документы меньше даты окончания
        /// realityObjectId - Необходимо получить документы по дому
        /// </param>
        /// <returns></returns>
        public ActionResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var viewDomain = this.Container.Resolve<IDomainService<ViewMotivationConclusion>>();

            using (this.Container.Using(userManager, viewDomain))
            {
                var municipalityList = userManager.GetMunicipalityIds();
                return viewDomain.GetAll()
                    .WhereIf(municipalityList.Count > 0, x => x.MunicipalityId.HasValue
                        && municipalityList.Contains(x.MunicipalityId.Value))
                    .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                    .ToListDataResult(loadParam)
                    .ToJsonResult();
            }
        }

        /// <summary>
        /// Список документов для создания проверки по обращению граждан ¯\_(ツ)_/¯
        /// </summary>
        public ActionResult ListForBaseStetement(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<string>("Id").ToLongArray();

            return this.DomainService.GetAll()
                .WhereIfContains(ids.IsNotEmpty(), x => x.Id, ids)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentNum,
                    x.DocumentDate,
                    ManagingOrganization = x.Inspection.Contragent.Name,
                    x.Inspection.Contragent
                })
                .ToListDataResult(baseParams.GetLoadParam())
                .ToJsonResult();
        }

        public ActionResult ListForStage(BaseParams baseParams)
        {
            var motivationConclusionDomain = this.Container.Resolve<IDomainService<MotivationConclusion>>();
            var warningDocRealObjDomain = this.Container.Resolve<IDomainService<WarningDocRealObj>>();

            try
            {
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress = warningDocRealObjDomain.GetAll()
                    .Where(x => x.RealityObject != null)
                    .Select(x =>
                        new
                        {
                            WarningDocId = x.WarningDoc.Id,
                            x.RealityObject.Address
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.WarningDocId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Address).FirstOrDefault());

                return motivationConclusionDomain.GetAll()
                    .Where(x => x.Stage.Id == stageId)
                    .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber, x.State, BaseDocumentId = x.BaseDocument.Id })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        DocumentId = x.Id,
                        x.TypeDocumentGji,
                        x.DocumentDate,
                        x.DocumentNumber,
                        Address = dictRoAddress.Get(x.BaseDocumentId),
                        x.State
                    })
                    .ToListDataResult(baseParams.GetLoadParam())
                    .ToJsonResult();
            }
            finally
            {
                this.Container.Release(motivationConclusionDomain);
                this.Container.Release(warningDocRealObjDomain);
            }
        }
    }
}