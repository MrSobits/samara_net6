using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.BaseApiIntegration.Controllers;
using Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Services.Impl
{
    /// <summary>
    /// Сервис для работы с документами "Профилактическое мероприятие"
    /// </summary>
    public class InspectionPreventiveActionService : IInspectionPreventiveActionService
    {
        private readonly IGkhUserManager userManager;
        private readonly IDomainService<PreventiveAction> preventiveActionDomain;
        private readonly IDomainService<PreventiveActionTask> preventiveActionTaskDomain;
        private readonly IDomainService<DocumentGjiChildren> docChildernDomain;

        /// <inheritdoc cref="InspectionPreventiveActionService" />
        public InspectionPreventiveActionService(
            IGkhUserManager userManager,
            IDomainService<PreventiveAction> preventiveActionDomain,
            IDomainService<PreventiveActionTask> preventiveActionTaskDomain,
            IDomainService<DocumentGjiChildren> docChildernDomain)
        {
            this.userManager = userManager;
            this.preventiveActionDomain = preventiveActionDomain;
            this.preventiveActionTaskDomain = preventiveActionTaskDomain;
            this.docChildernDomain = docChildernDomain;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<InspectionPreventiveActionTask>> GetListAsync()
        {
            var userMoIds = userManager.GetMunicipalityIds();

            if (!userMoIds.Any())
                throw new ApiServiceException("У пользователя не определено муниципальное образование");

            var userRoles = userManager.GetActiveOperatorRoles().Select(x => x.Name);
            var userInspectorIds = userRoles.Contains("ГЖИ руководство")
                ? new List<long>()
                : userManager.GetInspectorIds();

            return await preventiveActionTaskDomain.GetAll()
                .Join(docChildernDomain.GetAll(),
                    x => x,
                    y => y.Children,
                    (a, b) => new
                    {
                        PreventiveActionTask = a,
                        DocChildren = b
                    })
                .Join(preventiveActionDomain.GetAll(),
                    x => x.DocChildren.Parent,
                    y => y,
                    (a, b) => new
                    {
                        a.PreventiveActionTask,
                        PreventiveAction = b
                    })
                .Where(x => userMoIds.Contains(x.PreventiveAction.Municipality.Id))
                .WhereIf(userRoles.Contains("Сотрудник ГЖИ"), x => userManager.GetInspectorIds().Contains(x.PreventiveActionTask.Executor.Id))
                .Where(x => x.PreventiveActionTask.ActionStartDate != null
                            && x.PreventiveActionTask.ActionStartDate.Value.Date >= DateTime.Now.Date
                            && x.PreventiveActionTask.ActionStartDate.Value.Date <= DateTime.Now.Date.AddDays(4))
                .Select(x => new InspectionPreventiveActionTask
                {
                    Id = x.PreventiveActionTask.Id,
                    TypeCheckId = x.PreventiveActionTask.ActionType,
                    TypeVisit = x.PreventiveActionTask.VisitType,
                    DocumentNumber = x.PreventiveActionTask.DocumentNumber,
                    DocumentDate = x.PreventiveActionTask.DocumentDate,
                    StartDate = x.PreventiveActionTask.ActionStartDate,
                    OrganizationId = x.PreventiveAction.ControlledOrganization.Id,
                    Address = x.PreventiveActionTask.ActionLocation.AddressName
                })
                .ToListAsync();
        }
    }
}