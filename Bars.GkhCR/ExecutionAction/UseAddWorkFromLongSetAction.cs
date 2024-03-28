namespace Bars.GkhCR.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class UseAddWorkFromLongSetAction : BaseExecutionAction
    {
        public IRepository<Contragent> ContragentRepository { get; set; }

        public override string Description => "Проставление свойства Добавление видов работ из ДПКР для существующих КПКР";

        public override string Name => "Проставление свойства Добавление видов работ из ДПКР для существующих КПКР";

        public override Func<IDataResult> Action => this.SetAddWork;

        public BaseDataResult SetAddWork()
        {
            var service = this.Container.Resolve<IDomainService<ProgramCrChangeJournal>>();

            using (this.Container.Using(service))
            {
                var programs = service.GetAll()
                    .Where(x => x.TypeChange == TypeChangeProgramCr.FromDpkr)
                    .Select(x => x.ProgramCr)
                    .ToList();

                programs.ForEach(x => x.AddWorkFromLongProgram = AddWorkFromLongProgram.Use);

                TransactionHelper.InsertInManyTransactions(this.Container, programs, programs.Count, true, true);
            }

            return new BaseDataResult();
        }
    }
}