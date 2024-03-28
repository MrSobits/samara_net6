namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;

    public class RenameDictMultipurposePermissionAction : BaseExecutionAction
    {
        public ISessionProvider SessionProvider { get; set; }

        public override string Name => "Переименовать ограничения для Универсальных Справочников в связи с переносом в общий модуль Gkh из Gkh1468";

        public override string Description => @"Универсальные Справочники перенесены в общий модуль Gkh из Gkh1468. В связи с этим ограничения 
'Gkh1468.Dictionaries.Multipurpose.X' надо переименовать в 'Gkh.Dictionaries.Multipurpose.X'";

        public override Func<IDataResult> Action => this.MovePermission;

        public BaseDataResult MovePermission()
        {
            try
            {
                var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

                session.CreateSQLQuery(
                    @"
                update b4_role_permission set permission_id = 'Gkh.Dictionaries.Multipurpose' where permission_id = 'Gkh1468.Dictionaries.Multipurpose';
                update b4_role_permission set permission_id = 'Gkh.Dictionaries.Multipurpose.Create' where permission_id = 'Gkh1468.Dictionaries.Multipurpose.Create';
                update b4_role_permission set permission_id = 'Gkh.Dictionaries.Multipurpose.View' where permission_id = 'Gkh1468.Dictionaries.Multipurpose.View';
                update b4_role_permission set permission_id = 'Gkh.Dictionaries.Multipurpose.Edit' where permission_id = 'Gkh1468.Dictionaries.Multipurpose.Edit';
                update b4_role_permission set permission_id = 'Gkh.Dictionaries.Multipurpose.Delete' where permission_id = 'Gkh1468.Dictionaries.Multipurpose.Delete';")
                    .ExecuteUpdate();
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }

            return new BaseDataResult();
        }
    }
}