namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Disposal
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Entities;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Dto;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Views;

    using ControlType = Bars.GkhGji.Entities.Dict.ControlType;

    public class DisposalService : GkhGji.DomainService.DisposalService
    {
        public override IDataResult ListControlType(BaseParams baseParams)
        {
            var controlTypeDomain = this.Container.ResolveDomain<ControlType>();

            using (this.Container.Using(controlTypeDomain))
            {
                return controlTypeDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .ToListDataResult(baseParams.GetLoadParam());

            }
        }

        public override IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var listResult = this.GetListViewResult(baseParams)
                .Filter(loadParam, this.Container);
            var count = listResult.Count();
            
            var orderField = loadParam.Order.FirstOrDefault(x => x.Name == "State");
            listResult = orderField != null
                ? orderField.Asc
                    ? listResult.OrderBy(x => x.State.Code)
                    : listResult.OrderByDescending(x => x.State.Code)
                : listResult.Order(loadParam);

            return new ListDataResult((!isExport ? listResult.Paging(loadParam) : listResult).ToList(), count);
        }

        protected override void GetInspectionInfo(ref string baseName, ref string planName, long inspectionId, TypeBase typeBase)
        {
            switch (typeBase)
            {
                case TypeBase.InspectionActionIsolated:
                    {
                        this.GetInfoInspectionActionIsolated(ref baseName, ref planName, inspectionId);
                    }
                    break;
            }
        }
        
        protected virtual void GetInfoInspectionActionIsolated(ref string baseName, ref string planName, long inspectionId)
        {
            var inspectionActionIsolatedDomainService = this.Container.Resolve<IDomainService<InspectionActionIsolated>>();
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();

            try
            {
                baseName = "КНМ без взаимодействия с контролируемыми лицами";

                var inspectionActionIsolated = inspectionActionIsolatedDomainService.GetAll()
                    .Where(x => x.Id == inspectionId)
                    .Select(x => x.ActionIsolated).FirstOrDefault();

                var taskActionIsolated = taskActionIsolatedDomain.GetAll()
                    .Where(x => x.Inspection == inspectionActionIsolated)
                    .Select(x => new
                    {
                        x.DocumentNumber,
                        x.DocumentDate
                    })
                    .FirstOrDefault();

                planName = $"{taskActionIsolated.DocumentNumber} " +
                    $"{(taskActionIsolated.DocumentDate.HasValue ? taskActionIsolated.DocumentDate.Value.Date.ToString("dd.MM.yyyy") : string.Empty)}";

            }
            finally
            {
                this.Container.Release(inspectionActionIsolatedDomainService);
                this.Container.Release(taskActionIsolatedDomain);
            }
        }

        protected IQueryable<DisposalDto> GetListViewResult(BaseParams baseParams)
        {
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAsId("realityObjectId");
            var typeDocumentGji = baseParams.Params.GetAs<TypeDocumentGji>("typeDocumentGji");

            return this.GetViewListWithDocType<ViewTatDisposal>(typeDocumentGji)
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId + "/"))
                .Where(x => x.TypeDocumentGji == typeDocumentGji)
                .Select(x => new DisposalDto
                {
                    Id = x.Id,
                    State = x.State,
                    DateEnd = x.DateEnd,
                    DateStart = x.DateStart,
                    DocumentDate = x.DocumentDate,
                    DocumentNumber = x.DocumentNumber,
                    DocumentNum = x.DocumentNum,
                    TypeBase = x.TypeBase,
                    KindCheck = x.KindCheckName,
                    ContragentName = x.ContragentName,
                    MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
                    MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
                    PersonInspectionAddress = x.RealityObjectAddresses.Replace(";", ","),
                    IsActCheckExist = x.IsActCheckExist,
                    RealityObjectCount = x.RealityObjectCount,
                    TypeSurveyNames = x.TypeSurveyNames,
                    InspectorNames = x.InspectorNames,
                    InspectionId = x.InspectionId,
                    TypeDocumentGji = x.TypeDocumentGji,
                    TypeAgreementProsecutor = x.TypeAgreementProsecutor,
                    ControlType = x.ControlTypeName,
                    HasActSurvey = x.HasActSurvey,
                    LicenseNumber = x.License != null && x.License.State.FinalState &&
                        (x.License.DateTermination == null || x.License.DateTermination > DateTime.Today)
                            ? x.License.LicNum.ToString()
                            : "",
                    ErpRegistrationNumber = x.ErpRegistrationNumber
                });
        }
    }
}