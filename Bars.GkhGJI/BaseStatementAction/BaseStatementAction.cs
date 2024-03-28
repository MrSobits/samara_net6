namespace Bars.GkhGji.InspectionAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class BaseStatementAction : IBaseStatementAction
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionAppealCits> InspectionAppealCitsDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IStateProvider StateProvider { get; set; }

        public void Create(IEntity entity)
        {
            var appealCitsInfo = this.InspectionAppealCitsDomain.GetAll()
                .Where(x => x.Id == entity.Id.ToLong())
                .Where(x => (long?) x.AppealCits.Id != null)
                .Select(x => new
                {
                    x.AppealCits.Id,
                    StartState = (bool?)x.AppealCits.State.StartState,
                })
                .FirstOrDefault();

            if (appealCitsInfo == null)
            {
                return;
            }

            if (appealCitsInfo.StartState ?? true)
            {
                var typeId = this.StateProvider.GetStatefulEntityInfo(typeof(AppealCits))?.TypeId;
                var newState = this.StateDomain.GetAll()
                    .FirstOrDefault(x => x.TypeId == typeId && x.Code == "В работе");

                if (newState != null)
                {
                    this.StateProvider.ChangeState(appealCitsInfo.Id, typeId, newState, string.Empty, true);
                }
            }
        }
    }
}
