namespace Bars.GkhGji.Regions.Tomsk.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;

    public class MigrateAppealCitsExecutantInformationAction : BaseExecutionAction
    {
        public override string Description => this.Name;

        public override string Name => "ГЖИ Томск - Перенос сведений об исполнителях из старой формы в новую";

        public override Func<IDataResult> Action => this.Execute;

        public IDomainService<AppealCits> AppealCitsDomainService { get; set; }

        public IStateProvider StateProvider { get; set; }

        private BaseDataResult Execute()
        {
            var tmpExecutant = new AppealCitsExecutant();
            this.StateProvider.SetDefaultState(tmpExecutant);
            var state = tmpExecutant.State;
            if (state == null)
            {
                return new BaseDataResult(false, "Не задан начальный статус для Исполнителя обращения");
            }

            var executants = new List<AppealCitsExecutant>();
            this.AppealCitsDomainService.GetAll().Where(x => x.Executant != null).ForEach(
                x =>
                {
                    executants.Add(
                        new AppealCitsExecutant
                        {
                            AppealCits = x,
                            Executant = x.Executant,
                            PerformanceDate = x.ExecuteDate ?? DateTime.Now,
                            Controller = x.Tester,
                            OrderDate = x.SuretyDate ?? DateTime.Now,
                            Author = x.Surety,
                            State = state
                        });
                });

            TransactionHelper.InsertInManyTransactions(this.Container, executants, useStatelessSession: true);

            return new BaseDataResult();
        }
    }
}