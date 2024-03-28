namespace Bars.GisIntegration.Smev.Tasks.SendData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;
    using Bars.GisIntegration.Smev.Tasks.SendData.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    using Fasterflect;

    public class KnmSendDataTask : ErknmSendDataTask<MessageFromErknmSetType>
    {
        /// <inheritdoc />
        protected Dictionary<Type, string[]> GuidObjectsDict => new Dictionary<Type, string[]>
        {
            // ToDo: раскомментить по мере реализации
            { typeof (InspectionDocumentsValidationResponse), new [] { "guid" } },
            { typeof (IDocumentAttachmentsValidationResponse), new [] { "guid" } },
            { typeof (IReasonDocumentsValidationResponse), new [] { "guid" } },
            { typeof (IReasonsValidationResponse), new [] { "numGuid" } },
            { typeof (IOrganizationsValidationResponse), new [] { "guid" } },
            { typeof (ISubjectActValidationResponse), new [] { "guid" } },
            { typeof (IActKnoInspectorsValidationResponse), new [] { "guid" } },
            { typeof (IActDocumentValidationResponse), new [] { "guid" } },
            { typeof (ISubjectResultDecisionsValidationResponse), new [] { "guid" } },
            { typeof (IResultDecisionDocumentValidationResponse), new [] { "guid" } },
            { typeof (IObjectsValidationResponse), new [] { "guid" } },
            { typeof (IInspectorsValidationResponse), new [] { "guid" } },
            { typeof (IExpertsValidationResponse), new [] { "guid" } },
            { typeof (IEventsValidationResponse), new [] { "guid" } },
            { typeof (IPlacesValidationResponse), new [] { "guid" } },
            //{ typeof (IRequirementsValidationResponse), new [] { "guid" } },
            //{ typeof (IChecklistsValidationResponse), new [] { "guid" } },
            { typeof (IOrganizationDocumentsValidationResponse), new [] { "guid" } },
            //{ typeof (ChecklistAnswersValidationResponse), new [] { "guid" } },
            //{ typeof (DictionaryCheckListVersionManuallyAnswersValidationResponse), new [] { "guid" } }
            { typeof(IResultDecisionInjunctionValidationResponse), new [] { "guid" } },
            { typeof(IResultDecisionInspectorsValidationResponse), new [] { "guid" } },
            { typeof(IResultDecisionResponsibleEntitiesValidationResponse), new [] { "guid" } },
            { typeof(IResponsibleSubjectStructuresNPAValidationResponse), new [] { "guid"} }
        };

        /// <inheritdoc />
        protected override PackageProcessingResult ProcessSmevResponse(MessageFromErknmSetType response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            if (!response.Items?.Any() ?? true)
            {
                throw new ArgumentException();
            }

            var hasError = response.Errors?.Any() ?? false;

            if (!hasError)
            {
                var decisionDomain = this.Container.ResolveDomain<Decision>();

                using (this.Container.Using(decisionDomain))
                {
                    var objectId = transportGuidDictByType.Get(typeof(Decision)).Get("Id");
                    var decision = decisionDomain.Get(objectId);

                    if (decision is null)
                    {
                        throw new NullReferenceException("Не найден объект в базе данных");
                    }

                    foreach (var inspectionResponse in response.Items)
                    {
                        var requestStatus = (RequestStatusType)inspectionResponse.TryGetPropertyValue("RequestStatus");

                        if (requestStatus == null || requestStatus.Value != "SUCCESS")
                        {
                            hasError = true;
                            continue;
                        }

                        switch (inspectionResponse)
                        {
                            case UpdateInspectionResponseType updateInspectionResponseType:
                                this.UpdateErknmGuids(updateInspectionResponseType.Inspection, this.GuidObjectsDict);
                                break;
                            case CreateInspectionResponseType createInspectionResponseType:
                                if (string.IsNullOrEmpty(decision.ErknmGuid))
                                {
                                    decision.ErknmRegistrationNumber = createInspectionResponseType.Inspection.erknmId;
                                    decision.ErknmGuid = createInspectionResponseType.Inspection.iGuid;
                                    decision.ErknmRegistrationDate = DateTime.Now;
                                    decision.QRCodeAccessToken = createInspectionResponseType.Inspection.accessToken;

                                    decisionDomain.Update(decision);
                                }
                                this.UpdateErknmGuids(createInspectionResponseType.Inspection, this.GuidObjectsDict);
                                break;
                            default:
                                continue;
                        }
                    }
                }
            }

            return new PackageProcessingResult
            {
                State = hasError
                    ? PackageState.ProcessedWithErrors
                    : PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };
        }
    }
}