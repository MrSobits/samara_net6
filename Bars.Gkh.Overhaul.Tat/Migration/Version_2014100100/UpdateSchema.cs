namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014100100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2014032800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PUBLISH_PRG",
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_PUBLISH_PRG_V", "OVRHL_PRG_VERSION", "ID"),
                new RefColumn("STATE_ID", "OVRHL_PUBLISH_PRG_ST", "B4_STATE", "ID"));

            Database.AddEntityTable("OVRHL_PUBLISH_PRG_REC",
                new RefColumn("PUBLISH_PRG_ID", ColumnProperty.NotNull, "OVRHL_PUBLISH_PRG_REC_PP", "OVRHL_PUBLISH_PRG", "ID"),
                new RefColumn("STAGE2_ID", ColumnProperty.NotNull, "OVRHL_PUBLISH_PRG_REC_ST2", "OVRHL_STAGE2_VERSION", "ID"),
                new Column("INDEX_NUMBER", DbType.Int32, ColumnProperty.NotNull, 0),
                new Column("LOCALITY", DbType.String, 500),
                new Column("STREET", DbType.String, 255),
                new Column("HOUSE", DbType.String, 16),
                new Column("HOUSING", DbType.String, 16),
                new Column("ADDRESS", DbType.String, 500),
                new Column("YEAR_COMMISSIONING", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("COMMON_ESTATE_OBJECT", DbType.String, 2000),
                new Column("WEAR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("YEAR_LAST_OVERHAUL", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("YEAR_PUBLISHED", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("SUM", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PUBLISH_PRG");
            Database.RemoveTable("OVRHL_PUBLISH_PRG_REC");
        }
    }
}