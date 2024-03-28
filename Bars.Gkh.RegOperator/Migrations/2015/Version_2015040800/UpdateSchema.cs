namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015040800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015040800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015040300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddColumn("REGOP_SUSPENSE_ACCOUNT", new Column("d_date", DbType.DateTime, ColumnProperty.Null));
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("d_date", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "d_date");
            Database.RemoveColumn("REGOP_SUSPENSE_ACCOUNT", "d_date");
        }

        #endregion
    }
}
