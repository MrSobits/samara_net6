namespace Bars.Gkh.Migrations.Version_2013041001
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "CONVERTER_HEATS_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("NEW_EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveTable("CONVERTER_HEATS_EXTERNAL");
        }
    }
}