namespace Bars.GkhGji.Regions.BaseChelyabinsk.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Reminder;

    public class UpdateReminderAction : BaseExecutionAction
    {
        public IDomainService<ChelyabinskReminder> ReminderDomain { get; set; }

        public IDomainService<AppealCitsExecutant> AppealCitsExecutantDomain { get; set; }

        public override string Description => "Обновление напоминание по обращениям";

        public override string Name => "Обновление напоминание по обращениям";

        public override Func<IDataResult> Action => this.UpdateReminder;

        public BaseDataResult UpdateReminder()
        {
            this.Container.InTransaction(
                () =>
                {
                    this.ReminderDomain.GetAll()
                        .Where(x => !x.AppealCits.State.FinalState)
                        .Select(x => x.Id)
                        .ForEach(x => this.ReminderDomain.Delete(x));

                    // Список Идентификаторов Инспекторов данного обращения
                    var appealCitsExecutants = this.AppealCitsExecutantDomain.GetAll()
                        .Where(x => !x.AppealCits.State.FinalState).ToList();

                    foreach (var appealCitsExecutant in appealCitsExecutants)
                    {
                        var rem = new ChelyabinskReminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.Statement,
                            CategoryReminder = CategoryReminder.ExecutionStatemen,
                            Num = appealCitsExecutant.AppealCits.NumberGji,
                            CheckDate = appealCitsExecutant.AppealCits.CheckTime != DateTime.MinValue ? appealCitsExecutant.AppealCits.CheckTime : null,
                            AppealCits = appealCitsExecutant.AppealCits,
                            AppealCitsExecutant = appealCitsExecutant,
                            Inspector = appealCitsExecutant.Controller
                        };

                        this.ReminderDomain.Save(rem);
                    }
                });
            return new BaseDataResult();
        }
    }
}