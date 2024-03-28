namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062901
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_CALC_ACC_REGOP", "IS_TRANSIT", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_CALC_ACC_REGOP", "IS_TRANSIT");
        }
    }
}
