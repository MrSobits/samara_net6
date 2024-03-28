namespace Bars.GkhCr.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class GkhCrAllowRenegRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get
            {
                return "GkhCrAllowRenegRule";
            }
        }

        public string Name
        {
            get
            {
                return "Проверка возможности перевода статуса при повторном согласовании";
            }
        }

        public string TypeId
        {
            get
            {
                return "cr_object";
            }
        }

        public string Description
        {
            get
            {
                return "Блокирует смену статуса, если объект уже был в статусе 55 и для него явно указан запрет на повторное согласование.";
            }
        }

        public Bars.B4.Modules.States.ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var entity = statefulEntity as ObjectCr;

            if (entity == null)
            {
                return B4.Modules.States.ValidateResult.No("Внутренняя ошибка.");
            }

            var history =
                this.Container.Resolve<IDomainService<StateHistory>>()
                         .GetAll().Any(x => x.TypeId == "cr_object" && x.EntityId == int.Parse(statefulEntity.Id.ToString())
                                            && (x.StartState.Code == "55" || x.FinalState.Code == "55"));

            if (!entity.AllowReneg && history)
            {
                return B4.Modules.States.ValidateResult.No("Уполномоченный орган пока не разрешил повторное согласование объекта.");
            }

            return B4.Modules.States.ValidateResult.Yes();
        }
    }
}