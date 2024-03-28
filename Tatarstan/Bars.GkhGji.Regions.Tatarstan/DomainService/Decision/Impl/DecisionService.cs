namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Decision.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Disposal;
    using Bars.GkhGji.Regions.Tatarstan.Dto;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    public class DecisionService : DisposalService, IDecisionService
    {
        private readonly IDomainService<DecisionKnmAction> decisionKnmActionDomain;

        /// <inheritdoc />
        public DecisionService(IDomainService<DecisionKnmAction> decisionKnmActionDomain)
        {
            this.decisionKnmActionDomain = decisionKnmActionDomain;
        }

        /// <inheritdoc />
        public override IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            var decisionDomain = this.Container.ResolveDomain<TatarstanDecision>();
            using (this.Container.Using(decisionDomain))
            {
                var data = this.GetListViewResult(baseParams)
                .SelectMany(x => decisionDomain.GetAll()
                    .Where(y => y.Id == x.Id)
                    .DefaultIfEmpty(),
                    (x, y) => new
                    {
                        DisposalDto = x,
                        Decision = y
                    })
                .Select(x => new DisposalDto
                {
                    Id = x.DisposalDto.Id,
                    State = x.DisposalDto.State,
                    DateEnd = x.DisposalDto.DateEnd,
                    DateStart = x.DisposalDto.DateStart,
                    DocumentDate = x.DisposalDto.DocumentDate,
                    DocumentNumber = x.DisposalDto.DocumentNumber,
                    DocumentNum = x.DisposalDto.DocumentNum,
                    TypeBase = x.DisposalDto.TypeBase,
                    KindCheck = x.DisposalDto.KindCheck,
                    ContragentName = x.DisposalDto.ContragentName,
                    MunicipalityNames = x.DisposalDto.MunicipalityNames,
                    MunicipalityId = x.DisposalDto.MunicipalityId,
                    PersonInspectionAddress = x.DisposalDto.PersonInspectionAddress,
                    IsActCheckExist = x.DisposalDto.IsActCheckExist,
                    RealityObjectCount = x.DisposalDto.RealityObjectCount,
                    TypeSurveyNames = x.DisposalDto.TypeSurveyNames,
                    InspectorNames = x.DisposalDto.InspectorNames,
                    InspectionId = x.DisposalDto.InspectionId,
                    TypeDocumentGji = x.DisposalDto.TypeDocumentGji,
                    TypeAgreementProsecutor = x.DisposalDto.TypeAgreementProsecutor,
                    ControlType = x.DisposalDto.ControlType,
                    HasActSurvey = x.DisposalDto.HasActSurvey,
                    LicenseNumber = x.DisposalDto.LicenseNumber,
                    ErpRegistrationNumber = x.DisposalDto.ErpRegistrationNumber,
                    ErknmRegistrationNumber = (isExport ? "'" : "") + x.Decision.ErknmRegistrationNumber
                });

                return data.ToListDataResultWithPaging(loadParam, this.Container, true, !isExport);
            }
        }

        /// <inheritdoc />
        public override IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var result = base.GetInfo(documentId);
            var plannedActions = this.decisionKnmActionDomain.GetAll()
                .Where(x => x.MainEntity.Id == documentId)
                .ToList();

            return new BaseDataResult(new
            {
                inspectorNames = result.InspectorNames, 
                inspectorIds = result.InspectorIds, 
                baseName = result.BaseName, 
                planName = result.PlanName,
                plannedActionNames = string.Join(", ", plannedActions.Select(x => x.KnmAction.ActCheckActionType.GetDisplayName())),
                plannedActionIds = string.Join(",", plannedActions.Select(x => x.KnmAction.Id)),
            });
        }
    }
}