namespace Bars.Gkh.Migrations.Version_2013082800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013082800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013081600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "CONVERTER_STHIST_EXTERNAL",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new Column("STATE_HISTORY_ID", DbType.Int64));

            Database.AddEntityTable(
                "CONVERTER_STB3B4_EXTERNAL",
                new Column("STATE_B3", DbType.Int64),
                new Column("STATE_ID", DbType.Int64));
        }

        public override void Down()
        {
            Database.RemoveTable("CONVERTER_STHIST_EXTERNAL");
            Database.RemoveTable("CONVERTER_STB3B4_EXTERNAL");
        }
    }
}