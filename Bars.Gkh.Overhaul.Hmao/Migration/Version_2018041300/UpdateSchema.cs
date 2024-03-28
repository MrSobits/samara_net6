namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018041300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018041300")]
    [MigrationDependsOn(typeof(Version_2018041100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "OVHL_SUBPROGRAM",
               new Column("IS_SUB_PROGRAM", DbType.Binary, ColumnProperty.NotNull)
               );

            Database.AddForeignKey("OVHL_SUBPROGRAM_GKH_REALITY_OBJECT_ID_ID", "OVHL_SUBPROGRAM", "ID", "GKH_REALITY_OBJECT", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("OVHL_SUBPROGRAM");
        }
    }
}