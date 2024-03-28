namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018041600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018041600")]
    [MigrationDependsOn(typeof(Version_2018041300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveTable("OVHL_SUBPROGRAM");

            Database.AddEntityTable(
               "OVRHL_SUBPROGRAM",
               new Column("IS_SUB_PROGRAM", DbType.Boolean, ColumnProperty.NotNull),
               new RefColumn("RO_ID", ColumnProperty.Unique, "OVRHL_SUBPROGRAM_GKH_REALITY_OBJECT_RO_ID_ID", "GKH_REALITY_OBJECT", "ID")
               );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SUBPROGRAM");
        }
    }
}