namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012702.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_COMP_PROC", "NAME", DbType.String, 255, ColumnProperty.Null);
            Database.AddColumn("REGOP_COMP_PROC", "TASK_ID", DbType.Int32, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_COMP_PROC", "NAME");
            Database.RemoveColumn("REGOP_COMP_PROC", "TASK_ID");
        }
    }
}
