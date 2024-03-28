namespace Bars.GkhGji.Regions.Khakasia.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;

    public class ClearFinalStateDocRemindersAction : BaseExecutionAction
    {
        public override string Description => "Удаление из доски задач обращений со статусом 'Закрыто'";

        public override string Name => "Удаление из доски задач обращений со статусом 'Закрыто'";

        public override Func<IDataResult> Action => this.ClearFinalStateDocReminders;

        private BaseDataResult ClearFinalStateDocReminders()
        {
            var servReminder = this.Container.Resolve<IDomainService<Reminder>>();

            var listToDel = servReminder.GetAll()
                .Where(x => x.AppealCits.State.FinalState)
                .Select(x => x.Id)
                .ToList();

            listToDel.ForEach(x => servReminder.Delete(x));

            return new BaseDataResult();
        }
    }
}