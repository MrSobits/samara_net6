namespace Bars.Gkh.RegOperator.Migrations
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015051400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015050800.UpdateSchema))]
    public class V_2015051400 : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("receiver_number", DbType.String, 100, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "receiver_number");
        }

        #endregion
    }
}