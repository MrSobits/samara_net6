namespace Bars.Gkh.Overhaul.Regions.Kamchatka.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;

    public class UpdateVersionSumAction : BaseExecutionAction
    {
        public override string Name
        {
            get { return "Обновление сумм в версии ДПКР (КАМЧАТКА)"; }
        }

        public override string Description
        {
            get { return "Обновление сумм в версии ДПКР (КАМЧАТКА)"; }
        }

        public override Func<IDataResult> Action
        {
            get { return this.Execute; }
        }

        private BaseDataResult Execute()
        {
            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.OpenStatelessSession();

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // обновляем суммы от  этапа к этапу, т.к. после удаления дублей может быть ситуация с неправильными суммами
                        session.CreateSQLQuery(@"
                update OVRHL_STAGE2_VERSION st2 set sum = ( select sum(st1.sum + st1.sum_service) from OVRHL_STAGE1_VERSION st1 where st1.stage2_version_id = st2.id);
                update ovrhl_version_rec st3 set sum = ( select sum(st2.sum) from OVRHL_STAGE2_VERSION st2 where st2.st3_version_id = st3.id);")
                            .ExecuteUpdate();

                        transaction.Commit();
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        throw exc;
                    }
                }
            }
            finally
            {
                sessionProvider.CloseCurrentSession();
            }

            return new BaseDataResult();
        }
    }
}