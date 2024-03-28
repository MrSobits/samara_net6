namespace Bars.Gkh.Migrations.Version_2013040200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013040200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013032400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GKH_INSTRUCTIONS",
                new Column("DISPLAY_NAME", DbType.String, ColumnProperty.NotNull),
                new RefColumn("FILE_INFO_ID", "GKH_INSTRUCTIONS_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_INSTRUCTIONS");
        }
    }
}