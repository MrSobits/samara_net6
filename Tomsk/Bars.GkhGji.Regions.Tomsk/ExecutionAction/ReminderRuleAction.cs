namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ReminderRuleAction : BaseExecutionAction
    {
        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<Reminder> ReminderDomain { get; set; }

        public override string Description => @"Томск - Актуализация задач в панели руководителя для Типа Обращения";

        public override string Name => "Томск - Актуализация задач в панели руководителя для Типа Обращения";

        public override Func<IDataResult> Action => this.ActualizeReminders;

        public BaseDataResult ActualizeReminders()
        {
            var listForDelete = this.ReminderDomain.GetAll()
                .Where(x => x.AppealCits != null && x.AppealCits.State.FinalState)
                .Select(x => x.Id)
                .ToList();

            var appCits = this.AppealCitsDomain.GetAll().Where(x => !x.State.FinalState);

            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    listForDelete.ForEach(x => this.ReminderDomain.Delete(x));

                    appCits.ForEach(x => this.AppealCitsDomain.Update(x));

                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }
    }
}