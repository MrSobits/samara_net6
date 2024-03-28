namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014013002
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014013001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("REGOP_RO_SUPP_ACC_OP", "TYPE");
            Database.AddColumn("REGOP_RO_SUPP_ACC_OP", new Column("OP_TYPE", DbType.Int32, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_SUPP_ACC_OP", "OP_TYPE");
            Database.AddColumn("REGOP_RO_SUPP_ACC_OP", new Column("TYPE", DbType.String, 500, ColumnProperty.NotNull));
        }
    }
}
