namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public class TaskActionIsolatedService : ITaskActionIsolatedService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult ListForCitizenAppeal(BaseParams baseParams)
        {
            var appealId = baseParams.Params.GetAsId("appealCitizensId");
            var actionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();

            using (this.Container.Using(actionIsolatedDomain))
            {
                return actionIsolatedDomain
                    .GetAll()
                    .Where(x => x.TypeBase == TypeBaseAction.Appeal && x.AppealCits != null)
                    .Where(x => x.AppealCits.Id == appealId)
                    .Select(x => new
                    {
                        x.DocumentNumber,
                        x.DocumentDate,
                        x.TypeObject,
                        x.KindAction
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}