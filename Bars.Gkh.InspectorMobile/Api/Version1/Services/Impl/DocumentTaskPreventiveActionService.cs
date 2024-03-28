namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// API сервис для <see cref="PreventiveActionTask"/>
    /// </summary>
    public class DocumentTaskPreventiveActionService : DocumentWithParentService<PreventiveActionTask, DocumentTaskPreventiveActionGet, object,
        DocumentTaskPreventiveActionUpdate, TaskPreventiveActionQueryParams>,
        IDocumentTaskPreventiveActionService
    {
        #region DomainServices
        private readonly IDomainService<PreventiveActionTask> _preventiveActionTaskDomain;
        private readonly IDomainService<PreventiveAction> _preventiveActionDomain;
        private readonly IDomainService<PreventiveActionTaskPlannedAction> _preventiveActionTaskPlannedActionDomain;
        private readonly IDomainService<PreventiveActionTaskConsultingQuestion> _preventiveActionTaskConsultingQuestionDomain;
        private readonly IDomainService<DocumentGjiChildren> _documentLinkDomain;
        #endregion

        /// <inheritdoc cref="DocumentTaskPreventiveActionService" />
        public DocumentTaskPreventiveActionService(
            IDomainService<PreventiveActionTask> preventiveActionTaskDomain,
            IDomainService<PreventiveAction> preventiveActionDomain,
            IDomainService<PreventiveActionTaskPlannedAction> preventiveActionTaskPlannedActionDomain,
            IDomainService<PreventiveActionTaskConsultingQuestion> preventiveActionTaskConsultingQuestionDomain,
            IDomainService<DocumentGjiChildren> documentLinkDomain)
        {
            _preventiveActionTaskDomain = preventiveActionTaskDomain;
            _preventiveActionTaskPlannedActionDomain = preventiveActionTaskPlannedActionDomain;
            _preventiveActionTaskConsultingQuestionDomain = preventiveActionTaskConsultingQuestionDomain;
            _documentLinkDomain = documentLinkDomain;
            _preventiveActionDomain = preventiveActionDomain;
        }

        /// <inheritdoc />
        protected override IEnumerable<DocumentTaskPreventiveActionGet> GetDocumentList(
            long? documentId = null,
            TaskPreventiveActionQueryParams queryParams = null,
            params long[] parentDocumentIds)
        {
            var baseQuery = _preventiveActionTaskDomain.GetAll()
                .Join(this._documentLinkDomain.GetAll(),
                    x => x.Id,
                    x => x.Children.Id,
                    (x, y) => new
                    {
                        Task = x,
                        y.Parent
                    })
                .Join(this._preventiveActionDomain.GetAll(),
                    x => x.Parent.Id,
                    y => y.Id,
                    (x, y) => new
                    {
                        x.Task,
                        PreventiveActionData = new
                        {
                            y.ControlledPersonType,
                            OrganizationId = y.ControlledOrganization != null ? y.ControlledOrganization.Id : (long?)null,
                            Individual = y.FullName,
                            PersonAddress = y.ControlledPersonAddress != null ? y.ControlledPersonAddress.AddressName : null,
                            Telephone = y.PhoneNumber,
                            MunicipalityId = y.Municipality != null ? y.Municipality.Id : (long?)null
                        }
                    })
                .SelectMany(x => this._preventiveActionTaskPlannedActionDomain.GetAll()
                        .Where(y => y.Task.Id == x.Task.Id).DefaultIfEmpty(),
                    (x, y) => new
                    {
                        x.Task,
                        x.PreventiveActionData,
                        Activity = y != null
                            ? new PlannedActivity
                            {
                                Action = y.Action,
                                Comment = y.Commentary
                            }
                            : null
                    }
                )
                .SelectMany(x => _preventiveActionTaskConsultingQuestionDomain.GetAll()
                        .Where(y => y.Task.Id == x.Task.Id).DefaultIfEmpty(),
                    (x, y) => new
                    {
                        x.Task,
                        x.PreventiveActionData,
                        x.Activity,
                        Question = y != null
                            ? new QuestionConsultationGet
                            {
                                Id = y.Id,
                                Answer = y.Answer,
                                Question = y.Question,
                                ControlPerson = y.ControlledPerson
                            }
                            : null
                    })
                .SelectMany(x => this._documentLinkDomain.GetAll()
                        .Where(y => y.Parent.Id == x.Task.Id)
                        .DefaultIfEmpty(),
                    (x, y) => new
                    {
                        x.Task,
                        x.PreventiveActionData,
                        x.Activity,
                        x.Question,
                        RelatedDocument = y != null ? new RelatedDocumentInfo { Id = y.Children.Id, DocumentType = y.Children.TypeDocumentGji } : null,
                        RelatedDoc = y != null && !y.Children.State.FinalState
                    });

            if (queryParams != null)
            {
                if (queryParams.PeriodParameter == PeriodParameter.OutOfPeriod &&
                    (!queryParams.TypeCheckId.HasValue || !queryParams.DocumentDate.HasValue))
                    throw new ApiServiceException($"При значении параметра \"periodParameter\"=4 " +
                        $"также должны быть заданы параметры \"typeCheckId\" и \"documentDate\"");

                const int periodOffset = 4;
                var userManager = this.Container.Resolve<IGkhUserManager>();
                var userMunicipalityIds = userManager.GetMunicipalityIds();
                var userRoles = userManager.GetActiveOperatorRoles().Select(x => x.Name);
                var userInspector = userRoles.Contains("ГЖИ руководство")
                    ? null
                    : userManager.GetActiveOperator()?.Inspector;

                if (!userMunicipalityIds.Any())
                {
                    throw new ApiServiceException("У пользователя не определено муниципальное образование");
                }

                baseQuery = baseQuery
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.Current,
                        x =>
                            !x.Task.State.FinalState &&
                            x.Task.ActionStartDate <= DateTime.Today &&
                            x.Task.ActionEndDate >= DateTime.Today)
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.Upcoming,
                        x =>
                            !x.Task.State.FinalState &&
                            DateTime.Today < x.Task.ActionStartDate &&
                            x.Task.ActionStartDate <= DateTime.Today.AddDays(periodOffset))
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.Completed,
                        x =>
                            DateTime.Today > x.Task.ActionEndDate &&
                            x.Task.ActionEndDate >= DateTime.Today.AddDays(-1 * periodOffset))
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.OutOfPeriod,
                        x =>
                            x.Task.ActionStartDate > DateTime.Today.AddDays(periodOffset) ||
                            x.Task.ActionEndDate < DateTime.Today.AddDays(-1 * periodOffset))
                    .WhereIf(queryParams.TypeCheckId.HasValue, x => x.Task.ActionType == queryParams.TypeCheckId)
                    .WhereIf(queryParams.TypeVisit.HasValue, x => x.Task.VisitType == queryParams.TypeVisit)
                    .WhereIf(queryParams.DocumentNumber != null, x => x.Task.DocumentNumber.Contains(queryParams.DocumentNumber))
                    .WhereIf(queryParams.DocumentDate.HasValue, x => x.Task.DocumentDate == queryParams.DocumentDate)
                    .WhereIf(queryParams.OrganizationId.HasValue, x => x.PreventiveActionData.OrganizationId == queryParams.OrganizationId)
                    .WhereIf(queryParams.Address != null, x => x.Task.ActionLocation.HouseGuid.ToString() == queryParams.Address)
                    .WhereIf(userInspector != null, x => x.Task.Executor.Id == userInspector.Id)
                    .Where(x => x.PreventiveActionData.MunicipalityId.HasValue &&
                        userMunicipalityIds.Contains(x.PreventiveActionData.MunicipalityId.Value));
            }

            return baseQuery
                .WhereIf(documentId.HasValue, x => x.Task.Id == documentId)
                .AsEnumerable()
                .GroupBy(x => new
                    {
                        x.Task,
                        x.PreventiveActionData
                    },
                    (x, y) => new
                    {
                        x.Task,
                        x.PreventiveActionData,
                        Activities = y.Where(z => z.Activity != null).DistinctBy(z => z.Activity.Action).Select(z => z.Activity).ToArray(),
                        Questions = y.Where(z => z.Question != null).DistinctBy(z => z.Question.Id).Select(z => z.Question).ToArray(),
                        RelatedDocuments = y.Where(z => z.RelatedDocument != null).DistinctBy(z => z.RelatedDocument.Id).Select(z => z.RelatedDocument).ToArray(),
                        y.First().RelatedDoc
                    })
                .Select(x => new DocumentTaskPreventiveActionGet
                {
                    Id = x.Task.Id,
                    Inspection = new InspectionInfo { InspectionId = x.Task.Inspection.Id, RelatedDocuments = x.RelatedDocuments },
                    DocumentNumber = x.Task.DocumentNumber,
                    DocumentDate = x.Task.DocumentDate,
                    StartDate = x.Task.ActionStartDate,
                    EndDate = x.Task.ActionEndDate,
                    Time = x.Task.ActionStartTime,
                    Address = x.Task.ActionLocation?.AddressName,
                    InspectorId = x.Task.Executor?.Id,
                    TypeCheckId = x.Task.ActionType,
                    TypeVisit = x.Task.VisitType,
                    ControlledPersonType = x.PreventiveActionData.ControlledPersonType?.GetDisplayName(),
                    OrganizationId = x.PreventiveActionData.OrganizationId,
                    Individual = x.PreventiveActionData.Individual,
                    PersonAddress = x.PreventiveActionData.PersonAddress,
                    Telephone = x.PreventiveActionData.Telephone,
                    RelatedDocuments = x.RelatedDoc,
                    PlannedActivities = x.Activities,
                    QuestionsConsultation = x.Questions
                });
        }

        /// <inheritdoc />
        protected override long UpdateEntity(long documentId, DocumentTaskPreventiveActionUpdate updateDocument)
        {
            var preventiveActionTask = this._preventiveActionTaskDomain.Get(documentId);

            if (preventiveActionTask.IsNull())
                throw new ApiServiceException("Не найден документ для обновления");

            this.UpdateNestedEntities(
                updateDocument.QuestionsConsultation,
                x => x.Task.Id == preventiveActionTask.Id,
                this.ConsultingQuestionTransfer<QuestionConsultationUpdate>(),
                preventiveActionTask);

            return preventiveActionTask.Id;
        }

        /// <summary>
        /// Перенос информации для <see cref="PreventiveActionTaskConsultingQuestion"/>
        /// </summary>
        /// <typeparam name="TModel">Тип модели со значениями для переноса</typeparam>
        private TransferValues<TModel, PreventiveActionTaskConsultingQuestion> ConsultingQuestionTransfer<TModel>()
            where TModel : BaseQuestionConsultation =>
            (TModel model, ref PreventiveActionTaskConsultingQuestion preventiveActionTaskConsultingQuestion, object mainEntity) =>
            {
                if (preventiveActionTaskConsultingQuestion.Id.IsDefault())
                {
                    this.EntityRefCheck(mainEntity);
                    preventiveActionTaskConsultingQuestion.Task = (PreventiveActionTask)mainEntity;
                }

                preventiveActionTaskConsultingQuestion.Question = model.Question;
                preventiveActionTaskConsultingQuestion.Answer = model.Answer;
                preventiveActionTaskConsultingQuestion.ControlledPerson = model.ControlPerson;
            };
    }
}