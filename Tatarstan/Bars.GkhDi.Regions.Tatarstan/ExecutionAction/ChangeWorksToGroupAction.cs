namespace Bars.Gkh.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhDi.Entities;

    public class ChangeWorksToGroupAction : BaseExecutionAction
    {
        public override string Description => "Работам по ТО с группой работ по ТО \"ТЕХНИЧЕСКОЕ ОБСЛУЖИВАНИЕ ВЕНТИЛЯЦИОННЫХ СИСТЕМ\" "
            + "изменяется группа работ по ТО на \"ТЕХНИЧЕСКОЕ ОБСЛУЖИВАНИЕ ЖИЛЫХ ЗДАНИЙ\"";

        public override string Name => "Смена группы у работ по ТО с группой \"Техобслуживание вентиляционных систем\"";

        public override Func<IDataResult> Action => this.ChangeWorksToGroup;

        public BaseDataResult ChangeWorksToGroup()
        {
            var workToService = this.Container.Resolve<IDomainService<WorkTo>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var oldData = workToService.GetAll().Where(x => x.GroupWorkTo.Name == "ТЕХНИЧЕСКОЕ ОБСЛУЖИВАНИЕ ВЕНТИЛЯЦИОННЫХ СИСТЕМ");

                    var newGroup =
                        this.Container.Resolve<IDomainService<GroupWorkTo>>()
                            .GetAll()
                            .FirstOrDefault(x => x.Name == "ТЕХНИЧЕСКОЕ ОБСЛУЖИВАНИЕ ЖИЛЫХ ЗДАНИЙ");

                    if (newGroup != null)
                    {
                        foreach (var data in oldData)
                        {
                            data.GroupWorkTo = newGroup;

                            workToService.Save(data);
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }

            return new BaseDataResult();
        }
    }
}