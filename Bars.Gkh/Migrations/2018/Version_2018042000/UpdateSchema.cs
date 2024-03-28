namespace Bars.Gkh.Migrations._2018.Version_2018042000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018042000")]
    [MigrationDependsOn(typeof(Version_2018041700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("INCLUDE_TO_SUB_PROGRAM", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "INCLUDE_TO_SUB_PROGRAM");
        }
    }
}