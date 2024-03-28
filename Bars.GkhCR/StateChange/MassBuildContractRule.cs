namespace Bars.GkhCr.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.GkhCr.Entities;
    using Bars.GkhCR.Tasks;
    using Castle.Windsor;

    public class MassBuildContractRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        private ITaskManager _taskManager;

        public string Id
        {
            get
            {
                return "MassBuildContractRule";
            }
        }

        public string Name
        {
            get
            {
                return "Массовое создание договоров в объектах КР при смене статуса массового договора";
            }
        }

        public string TypeId
        {
            get
            {
                return "cr_mass_build_contract";
            }
        }

        public string Description
        {
            get
            {
                return "Создает договора с подрядчиками по выбранным объектам КР с работами из выбранного списка работ, если они есть в самом объекте КР";
            }
        }

        public Bars.B4.Modules.States.ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var entity = statefulEntity as MassBuildContract;

            if (entity == null)
            {
                return B4.Modules.States.ValidateResult.No("Внутренняя ошибка.");
            }
            var taskManager = Container.Resolve<ITaskManager>();
            BaseParams baseParams = new BaseParams();
            baseParams.Params.Add("taskId", entity.Id.ToString());

            var taskInfo = taskManager.CreateTasks(new MassBuildContractTaskProvider(), baseParams);

            if (taskInfo == null)
                return B4.Modules.States.ValidateResult.No("Не удалось поставить задачу по массовому формированию договоров");
            else
                return B4.Modules.States.ValidateResult.Yes();



        }
    }
}