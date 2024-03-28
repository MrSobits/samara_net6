namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015062200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015062200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015061700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.ColumnExists("REGOP_PERS_ACC_CHANGE", "REASON"))
            {
                Database.AddColumn("REGOP_PERS_ACC_CHANGE", new Column("REASON", DbType.String, ColumnProperty.Null));
            }
        }

        public override void Down()
        {
            if (Database.ColumnExists("REGOP_PERS_ACC_CHANGE", "REASON"))
            {
                Database.RemoveColumn("REGOP_PERS_ACC_CHANGE", "REASON");
            }
        }
    }
}
