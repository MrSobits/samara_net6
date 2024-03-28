namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015050701
{
    using B4.Application;
    using B4.Modules.States;
    using Gkh.Modules.ClaimWork.Contracts;
    using Entities;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050701")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015050700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            var oldState = ClaimWorkStates.PetitionNeeded;
            var newState = ClaimWorkStates.LawsuitNeeded;
            var info = ApplicationContext.Current.Container.Resolve<IStateProvider>().GetStatefulEntityInfo(typeof(DebtorClaimWork));

            var sql = string.Format(
                "update CLW_CLAIM_WORK " +
                "  set state_id = (select id from b4_state where name='{0}' and type_id='{1}')" +
                "  where id in (select clw.id from CLW_CLAIM_WORK clw" +
                "               join b4_state st on st.id = state_id and st.name='{2}' and st.type_id='{1}')"
                ,
                newState,
                info.TypeId,
                oldState);

            Database.ExecuteNonQuery(sql);
        }

        public override void Down()
        {
        }

        #endregion
    }
}
