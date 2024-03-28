using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    // TODO: Расскоментировать после реализации
    //using Bars.GisIntegration.Base.Entities;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Utils;

    /*public class RisTaskViewModel : BaseViewModel<RisTask>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RisTask> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long?>("documentId");

            if (!documentId.HasValue)
            {
                return new ListDataResult();
            }

            var loadParams = this.GetLoadParam(baseParams);

            var risTaskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(risTaskTriggerDomain, userManager))
            {
                var data = this.GetUserTasks(userManager, domainService, documentId.Value);
                var triggerDict = risTaskTriggerDomain.GetAll()
                    .Where(x => data.Any(y => y.Id == x.Task.Id))
                    .GroupBy(x => x.Task.Id, x => x.Trigger.Id)
                    .ToDictionary(x => x.Key, x => x.Max());

                return data
                    .OrderBy(x => x.StartTime)
                    .AsEnumerable()
                    .Select(x => new 
                    {
                        x.Id,
                        MethodName = x.Description,
                        x.StartTime,
                        EndTime = x.EndTime == default(DateTime) ? null : (DateTime?)x.EndTime,
                        x.UserName,
                        x.TaskState,
                        TriggerId = triggerDict.Get(x.Id),
                        RequestXmlFileId = x.RequestXmlFile?.Id ?? 0,
                        ResponseXmlFileId = x.ResponseXmlFile?.Id ?? 0
                    }).ToListDataResult(loadParams, this.Container);
            }
        }

        /// <summary>
        /// Получает список задач, доступных пользователю.
        /// </summary>
        /// <param name="userManager">Менеджер пользователей.</param>
        /// <param name="domainService">Домен задач</param>
        /// <param name="documentId">Идентификатор распоряжения.</param>
        private IQueryable<RisTask> GetUserTasks(IGkhUserManager userManager, IDomainService<RisTask> domainService, long documentId)
        {
            var user = userManager.GetActiveUser();
            var isAdministrator = user.Roles.Any(x => string.Equals(x.Role.Name, "Администратор"));

            var tasks = domainService.GetAll().Where(x => x.DocumentGji.Id == documentId);

            return isAdministrator ? tasks : tasks.Where(x => x.UserName == user.Name);
        }
    }*/
}
