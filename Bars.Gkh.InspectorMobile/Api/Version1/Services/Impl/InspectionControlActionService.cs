namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// API-сервис для получения типов документов "Задание", "Решение"
    /// </summary>
    public class InspectionControlActionService : IInspectionControlActionService
    {
        private readonly IGkhUserManager userManager;
        private readonly IDomainService<TaskActionIsolatedRealityObject> taskRealityObjectDomain;
        private readonly IDomainService<InspectionGjiRealityObject> inspectionGjiRealityObjectDomain;
        private readonly IDomainService<Decision> decisionDomain;
        private readonly IDomainService<TaskActionIsolated> taskActionIsolatedDomain;
        private readonly IDomainService<DocumentGjiInspector> docInspectorDomain;

        /// <inheritdoc cref="InspectionControlActionService" />
        public InspectionControlActionService(IGkhUserManager userManager, 
            IDomainService<TaskActionIsolatedRealityObject> taskRealityObjectDomain,
            IDomainService<InspectionGjiRealityObject> inspectionGjiRealityObjectDomain,
            IDomainService<Decision> decisionDomain,
            IDomainService<TaskActionIsolated> taskActionIsolatedDomain,
            IDomainService<DocumentGjiInspector> docInspectorDomain)
        {
            this.userManager = userManager;
            this.taskRealityObjectDomain = taskRealityObjectDomain;
            this.inspectionGjiRealityObjectDomain = inspectionGjiRealityObjectDomain;
            this.decisionDomain = decisionDomain;
            this.taskActionIsolatedDomain = taskActionIsolatedDomain;
            this.docInspectorDomain = docInspectorDomain;
        }

        /// <inheritdoc />
        public IEnumerable<InspectionControlAction> GetControlActionDocuments()
        {
            var userMunicipalityIds = this.userManager.GetMunicipalityIds();
                
            if (!userMunicipalityIds.Any())
                throw new ApiServiceException("У пользователя не определено муниципальное образование");
            
            var userRoles = this.userManager.GetActiveOperatorRoles().Select(x => x.Name);
            var userInspectorIds = userRoles.Contains("ГЖИ руководство")
                ? new List<long>() 
                : this.userManager.GetInspectorIds();
            
            var documentList = new List<InspectionControlAction>();
            documentList.AddRange(this.GetDecisionDocuments(userInspectorIds, userMunicipalityIds));
            documentList.AddRange(this.GetTaskDocuments(userInspectorIds, userMunicipalityIds));

            return documentList;
        }

        /// <summary>
        /// Получить документы "Задание" отфильтрованные по инспекторам и муниципалитету
        /// </summary>
        /// <param name="userInspectorIds">Список идентификаторов инспекторов пользователя</param>
        /// <param name="userMunicipalityIds">Список идентификаторов муниципалитетов пользователя</param>
        private IEnumerable<InspectionControlAction> GetTaskDocuments(ICollection<long> userInspectorIds, ICollection<long> userMunicipalityIds)
        {
            return this.taskActionIsolatedDomain.GetAll()
                .Where(x => x.DateStart >= DateTime.Today && x.DateStart <= DateTime.Today.AddDays(4))
                .Join(this.docInspectorDomain.GetAll(),
                    x => x.Id,
                    y => y.DocumentGji.Id,
                    (x, y) => new
                    {
                        Task = x,
                        InspectorId = y.Inspector.Id
                    })
                .Join(this.taskRealityObjectDomain.GetAll(),
                    x => x.Task.Id,
                    y => y.Task.Id,
                    (x, y) => new
                    {
                        x.Task,
                        x.InspectorId,
                        RealityObject = new
                        {
                            y.RealityObject.Id,
                            MunicipalityId = y.RealityObject.Municipality.Id
                        }
                    })
                .Where(x => userMunicipalityIds.Contains(x.RealityObject.MunicipalityId))
                .WhereIf(userInspectorIds.Any(), x => userInspectorIds.Contains(x.InspectorId))
                .AsEnumerable()
                .GroupBy(x => new
                    {
                        x.Task
                    },
                    (x, y) => new
                    {
                        x.Task,
                        RealityObjectIds = y.DistinctBy(z => z.RealityObject.Id).Select(z => z.RealityObject.Id).ToArray()
                    })
                .Select(x => new InspectionControlAction
                {
                    Id = x.Task.Id,
                    DocumentType = x.Task.TypeDocumentGji,
                    DocumentNumber = x.Task.DocumentNumber,
                    StartDate = x.Task.DateStart,
                    OrganizationId = x.Task.TypeObject == TypeDocObject.Official || x.Task.TypeObject == TypeDocObject.Legal
                        ? x.Task.Contragent?.Id
                        : null,
                    Addresses = x.RealityObjectIds,
                })
                .ToList();
        }

        /// <summary>
        /// Получить документы "Решение" отфильтрованные по инспекторам и муниципалитету
        /// </summary>
        /// <param name="userInspectorIds">Список идентификаторов инспекторов пользователя</param>
        /// <param name="userMunicipalityIds">Список идентификаторов муниципалитетов пользователя</param>
        private IEnumerable<InspectionControlAction> GetDecisionDocuments(ICollection<long> userInspectorIds, ICollection<long> userMunicipalityIds)
        {
            return this.decisionDomain.GetAll()
                .Where(x => x.DateStart >= DateTime.Today && x.DateStart <= DateTime.Today.AddDays(4))
                .Join(this.docInspectorDomain.GetAll(),
                    x => x.Id,
                    y => y.DocumentGji.Id,
                    (x, y) => new
                    {
                        Decision = x,
                        InspectorId = y.Inspector.Id
                    })
                .Join(this.inspectionGjiRealityObjectDomain.GetAll(),
                    x => x.Decision.Inspection.Id,
                    y => y.Inspection.Id,
                    (x, y) => new
                    {
                        x.Decision,
                        x.InspectorId,
                        RealityObject = new
                        {
                            y.RealityObject.Id,
                            MunicipalityId = y.RealityObject.Municipality.Id,
                        }
                    })
                .Where(x => userMunicipalityIds.Contains(x.RealityObject.MunicipalityId))
                .WhereIf(userInspectorIds.Any(), x => userInspectorIds.Contains(x.InspectorId))
                .AsEnumerable()
                .GroupBy(x => new
                    {
                        x.Decision
                    },
                    (x, y) => new
                    {
                        x.Decision,
                        RealityObjectIds = y.DistinctBy(z => z.RealityObject.Id).Select(z => z.RealityObject.Id).ToArray()
                    })
                .Select(x => new InspectionControlAction
                {
                    Id = x.Decision.Id,
                    DocumentType = x.Decision.TypeDocumentGji,
                    DocumentNumber = x.Decision.DocumentNumber,
                    StartDate = x.Decision.DateStart,
                    EndDate = x.Decision.DateEnd,
                    OrganizationId = x.Decision.Inspection.PersonInspection == PersonInspection.Organization ||
                        x.Decision.Inspection.PersonInspection == PersonInspection.Official
                            ? x.Decision.Inspection.Contragent?.Id
                            : null,
                    Addresses = x.RealityObjectIds,
                })
                .ToList();
        }
    }
}