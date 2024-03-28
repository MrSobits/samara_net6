namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_SPEC_ACC", "ACC_TYPE", DbType.Int32, ColumnProperty.NotNull, "-1");
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_SPEC_ACC", "ACC_TYPE");
        }
    }
}
