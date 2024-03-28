namespace Bars.GkhGji.Regions.Tula.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class ClearFinalStateDocRemindersAction : IExecutionAction
    {
        public IWindsorContainer Container { get; set; }

        public static string Code
        {
            get
            {
                return "ClearFinalStateDocReminders";
            }
        }

        string IExecutionAction.Code
        {
            get
            {
                return Code;
            }
        }

        public string Description
        {
            get
            {
                return "Удаление из доски задач обращений со статусом 'Закрыто'";
            }
        }

        public string Name
        {
            get
            {
                return "Удаление из доски задач обращений со статусом 'Закрыто'";
            }
        }

        public Func<BaseDataResult> Action
        {
            get
            {
                return ClearFinalStateDocReminders;
            }
        }

        private BaseDataResult ClearFinalStateDocReminders()
        {
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            var listToDel = servReminder.GetAll()
                .Where(x => x.AppealCits.State.FinalState)
                .Select(x => x.Id)
                .ToList();

            listToDel.ForEach(x => servReminder.Delete(x));

            return new BaseDataResult();
        }
    }
}