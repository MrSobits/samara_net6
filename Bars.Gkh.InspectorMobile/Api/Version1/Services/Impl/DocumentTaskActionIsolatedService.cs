namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskActionIsolated;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <inheritdoc cref="Bars.Gkh.InspectorMobile.Api.Version1.Services.IDocumentTaskActionIsolatedService" />
    public class DocumentTaskActionIsolatedService : DocumentWithParentService<TaskActionIsolated, DocumentTaskActionIsolated, object, object, TaskActionIsolatedQueryParams>, IDocumentTaskActionIsolatedService
    {
        private readonly IDomainService<TaskActionIsolated> taskActionIsolatedDomain;
        private readonly IDomainService<TaskActionIsolatedRealityObject> taskRObjectDomain;
        private readonly IDomainService<ActActionIsolated> actActionIsolatedDomain;
        private readonly IDomainService<ActCheckRealityObject> actCheckRObjectDomain;
        private readonly IDomainService<DocumentGjiChildren> documentGjiChildrenDomain;
        private readonly IDomainService<DocumentGjiInspector> docInspectorDomain;
        private readonly IGkhUserManager userManager;

        /// <inheritdoc />
        public DocumentTaskActionIsolatedService(
            IDomainService<TaskActionIsolated> taskActionIsolatedDomain,
            IDomainService<TaskActionIsolatedRealityObject> taskRObjectDomain,
            IDomainService<ActActionIsolated> actActionIsolatedDomain,
            IDomainService<ActCheckRealityObject> actCheckRObjectDomain,
            IDomainService<DocumentGjiChildren> documentGjiChildrenDomain,
            IDomainService<DocumentGjiInspector> docInspectorDomain,
            IGkhUserManager userManager)
        {
            this.taskActionIsolatedDomain = taskActionIsolatedDomain;
            this.taskRObjectDomain = taskRObjectDomain;
            this.actActionIsolatedDomain = actActionIsolatedDomain;
            this.actCheckRObjectDomain = actCheckRObjectDomain;
            this.documentGjiChildrenDomain = documentGjiChildrenDomain;
            this.docInspectorDomain = docInspectorDomain;
            this.userManager = userManager;
        }

        /// <inheritdoc />
        protected override IEnumerable<DocumentTaskActionIsolated> GetDocumentList(long? documentId = null, 
            TaskActionIsolatedQueryParams queryParams = null, params long[] parentDocumentIds)
        {
            var userMunicipalityIds = this.userManager.GetMunicipalityIds();

            if (!userMunicipalityIds.Any())
                throw new ApiServiceException("У пользователя не определено муниципальное образование");
            
            const int dateOffset = 4;
            var currentDate = DateTime.Today;
            
            // Базовый запрос по документам "Задание"
            var tasksDocQuery = this.taskActionIsolatedDomain.GetAll()
                .WhereIf(documentId.HasValue, x => x.Id == documentId);
            
            var userRoles = this.userManager.GetActiveOperatorRoles().Select(x => x.Name);
            var userInspector = userRoles.Contains("ГЖИ руководство")
                ? null
                : this.userManager.GetActiveOperator()?.Inspector;

            // Получаем словарь с ключом: идентификатор документа родителя(Задание) и значением: ребенок(Акт) с идентификатором и статусом
            var taskActDict = this.documentGjiChildrenDomain.GetAll()
                .WhereIf(documentId.HasValue, x => x.Parent.Id == documentId)
                .Where(x =>
                    x.Parent.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated &&
                    x.Children.TypeDocumentGji == TypeDocumentGji.ActActionIsolated)
                .Join(this.actActionIsolatedDomain.GetAll(),
                    x => x.Children.Id,
                    y => y.Id,
                    (x, y) => new
                    {
                        TaskId = x.Parent.Id,
                        Act = new
                        {
                            y.Id,
                            y.State.FinalState
                        }
                    })
                .AsEnumerable()
                .GroupBy(x => x.TaskId)
                .ToDictionary(x => x.Key, y => y.Select(z => z.Act).First());

            // Получаем инспекторов по документам "Задание" и формируем словарь
            var taskInspectorsDict = this.docInspectorDomain.GetAll()
                .WhereIf(documentId.HasValue, x => x.DocumentGji.Id == documentId)
                .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)
                .Where(x => x.Inspector != null)
                .WhereIf(userInspector != null, x => x.Inspector.Id == userInspector.Id)
                .AsEnumerable()
                .GroupBy(x => x.DocumentGji.Id)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => x.Inspector.Id).ToArray());

            // Получаем дома по документам "Задание" и формируем словарь
            var taskRObjectDict = this.taskRObjectDomain.GetAll()
                .WhereIf(documentId.HasValue, x => x.Task.Id == documentId)
                .Where(x => x.Task.TypeDocumentGji == TypeDocumentGji.TaskActionIsolated)
                .Where(x => userMunicipalityIds.Contains(x.RealityObject.Municipality.Id))
                .AsEnumerable()
                .GroupBy(x => new
                    {
                        x.Task.Id
                    },
                    (x, y) => new
                    {
                        x.Id,
                        RealityObjects = y.Select(z => new
                        {
                            z.RealityObject.HouseGuid,
                            z.RealityObject.Id
                        })
                    })
                .ToDictionary(x => x.Id, y => y.RealityObjects);

            if (queryParams != null)
            {
                if (queryParams.PeriodParameter == PeriodParameter.OutOfPeriod && (!queryParams.KindAction.HasValue || !queryParams.DocumentDate.HasValue))
                    throw new ApiServiceException($"При значении параметра \"periodParameter\"=4 " + 
                        $"также должны быть заданы параметры \"kindAction\" и \"documentDate\"");
                
                // Получаем дома по дочерним актам и формируем словарь: Идентификатор документ-родителя(Задние) -- Дома дочернего акта  
                var actRObjectsList = this.documentGjiChildrenDomain.GetAll()
                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActActionIsolated)
                    .SelectMany(x => this.actCheckRObjectDomain.GetAll()
                            .Where(y => y.ActCheck.Id == x.Children.Id)
                            .DefaultIfEmpty(),
                        (x, y) => new
                        {
                            TaskId = x.Parent.Id,
                            ActRo = y
                        })
                    .AsEnumerable()
                    .GroupBy(x => new
                        {
                            x.TaskId
                        },
                        (x, y) => new
                        {
                            x.TaskId,
                            RealityObjects = y.Where(z => z.ActRo != null).Select(z => z.ActRo).ToArray()
                        })
                    .ToArray();

                var tasksFilteredByAddress = new List<long>();
                var tasksWithDefinedViolation = new List<long>();

                switch (queryParams.PeriodParameter)
                {
                    // Собираем идентификаторы документов "Задание" по определенным периодов,
                    // проверяем имеются ли у дочернего акта дом с определлёным признаком нарушения
                    case PeriodParameter.Current:
                        tasksWithDefinedViolation = actRObjectsList
                            .Where(x => x.RealityObjects.All(y => y.HaveViolation == YesNoNotSet.NotSet))
                            .Select(x => x.TaskId)
                            .ToList();
                        break;
                    case PeriodParameter.Completed:
                        tasksWithDefinedViolation = actRObjectsList
                            .Where(x => x.RealityObjects.All(y => 
                                y.HaveViolation == YesNoNotSet.Yes || y.HaveViolation == YesNoNotSet.No))
                            .Select(x => x.TaskId)
                            .ToList();
                        break;
                }

                if (taskRObjectDict.Any() && !string.IsNullOrEmpty(queryParams.Address))
                {
                    // Собираем идентификаторы документов "Задание" если был задан параметр "Address"
                    tasksFilteredByAddress = taskRObjectDict
                        .SelectMany(g => g.Value,
                            (x, y) => new
                            {
                                TaskId = x.Key,
                                Value = y
                            })
                        .Where(z => z.Value.HouseGuid?.ToLower() == queryParams.Address.ToLower())
                        .Select(z => z.TaskId)
                        .ToList();
                }

                tasksDocQuery = tasksDocQuery
                    .Where(x => taskRObjectDict.Keys.Contains(x.Id))
                    .WhereIf(userInspector != null, x => taskInspectorsDict.Keys.Contains(x.Id))
                    .WhereIf(!string.IsNullOrEmpty(queryParams.DocumentNumber),
                        x => x.DocumentNumber.Contains(queryParams.DocumentNumber))
                    .WhereIf(queryParams.OrganizationId.HasValue, x => x.Contragent.Id == queryParams.OrganizationId)
                    .WhereIf(!string.IsNullOrEmpty(queryParams.Individual),
                        x => x.PersonName.ToUpper().Contains(queryParams.Individual.ToUpper()))
                    .WhereIf(!string.IsNullOrEmpty(queryParams.Address),
                        x => tasksFilteredByAddress.Contains(x.Id))
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.OutOfPeriod || queryParams.KindAction != null,
                        x => x.KindAction == queryParams.KindAction)
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.OutOfPeriod || queryParams.DocumentDate != null,
                        x => x.DocumentDate == queryParams.DocumentDate)
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.Current,
                        x =>
                            (!x.State.FinalState && x.DateStart == currentDate &&
                                (!taskActDict.Keys.Contains(x.Id) || tasksWithDefinedViolation.Contains(x.Id)))
                            || (currentDate > x.DateStart && (!taskActDict.Keys.Contains(x.Id) || tasksWithDefinedViolation.Contains(x.Id))))
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.Upcoming,
                        x => !x.State.FinalState
                            && x.DateStart > currentDate
                            && x.DateStart <= currentDate.AddDays(dateOffset))
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.Completed,
                        x => x.DateStart < currentDate
                            && x.DateStart >= currentDate.AddDays(-dateOffset)
                            && tasksWithDefinedViolation.Contains(x.Id))
                    .WhereIf(queryParams.PeriodParameter == PeriodParameter.OutOfPeriod,
                        x => x.DateStart <= currentDate.AddDays(-dateOffset) ||
                            x.DateStart >= currentDate.AddDays(dateOffset));
            }
            
            var relatedDocuments = this.DocumentGjiDomain.GetAll()
                .Where(x => tasksDocQuery.Any(y => y.Inspection.Id == x.Inspection.Id))
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.ActActionIsolated
                    || x.TypeDocumentGji == TypeDocumentGji.MotivatedPresentation)
                .AsEnumerable()
                .GroupBy(x => x.Inspection.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => new RelatedDocumentInfo
                {
                    Id = y.Id,
                    DocumentType = y.TypeDocumentGji
                }));

            return tasksDocQuery
                .AsEnumerable()
                .Select(x => new DocumentTaskActionIsolated
                {
                    Id = x.Id,
                    Inspection = new InspectionInfo { InspectionId = x.Inspection.Id, RelatedDocuments = relatedDocuments.Get(x.Inspection.Id) },
                    DocumentNumber = x.DocumentNumber,
                    DocumentDate = x.DocumentDate,
                    OrganizationId = x.Contragent?.Id,
                    Individual = x.PersonName,
                    TypeCheckId = x.KindAction,
                    TypeControlId = x.ControlType.Id,
                    Base = x.TypeBase,
                    Appeal = x.TypeBase == TypeBaseAction.Appeal && x.AppealCits != null
                        ? $"{x.AppealCits.NumberGji} от {x.AppealCits.DateFrom.ToDateString()}"
                        : null,
                    Plan = x.TypeBase == TypeBaseAction.Plan && x.PlanAction != null ? x.PlanAction.Name : null,
                    EventDate = x.DateStart,
                    Time = x.TimeStart.Value.TimeOfDay,
                    Addresses = taskRObjectDict.Get(x.Id)?.Select(z => z.Id),
                    ExecutiveId = x.IssuedTask?.Id,
                    InspectorIds = taskInspectorsDict.Get(x.Id),
                    RelatedDocuments = !taskActDict.Get(x.Id)?.FinalState ?? false
                });
        }
    }
}