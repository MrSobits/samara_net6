namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class GuidSuspenseAccountGeneratorAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Description => "Генерация гуидов для cчетов невыясненных сумм";

        public override string Name => "Генерация гуидов для cчетов невыясненных сумм";

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var suspenseAccRepo = this.Container.ResolveRepository<SuspenseAccount>();

            using (this.Container.Using(suspenseAccRepo))
            {
                var session = this.SessionProvider.GetCurrentSession();

                var suspenseAccounts = suspenseAccRepo.GetAll()
                    .AsEnumerable()
                    .Where(x => string.IsNullOrEmpty(x.TransferGuid))
                    .ToList();

                foreach (var suspAccount in suspenseAccounts)
                {
                    session.CreateSQLQuery(
                        string.Format("update regop_suspense_account set c_guid = '{0}' where id = {1}", Guid.NewGuid().ToString(), suspAccount.Id))
                        .ExecuteUpdate();
                }
            }

            return new BaseDataResult();
        }
    }
}