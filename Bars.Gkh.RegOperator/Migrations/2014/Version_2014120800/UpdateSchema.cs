namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014120800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014120800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014120500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("REGOP_PERS_ACC_CHANGE", "NEW_VALUE"))
            {
                Database.AddColumn("REGOP_PERS_ACC_CHANGE", new Column("NEW_VALUE", DbType.String, 300));
            }

            if (!Database.ColumnExists("REGOP_PERS_ACC_CHANGE", "OLD_VALUE"))
            {
                Database.AddColumn("REGOP_PERS_ACC_CHANGE", new Column("OLD_VALUE", DbType.String, 300));
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC_CHANGE", "NEW_VALUE");
            Database.RemoveColumn("REGOP_PERS_ACC_CHANGE", "OLD_VALUE");
        }
    }
}
