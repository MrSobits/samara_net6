namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013120300
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Удаляем не нужные колонки
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "DPKR_CORRECT_ID");
            Database.RemoveColumn("OVHL_DPKR_CORRECT_ST2", "PUBLICATION_YEAR");

            Database.AddColumn("OVRHL_SHORT_PROG_REC", new Column("YEAR", DbType.Int16, ColumnProperty.NotNull, 0));
            Database.AddRefColumn("OVRHL_SHORT_PROG_REC", new RefColumn("STAGE2_ID", "OVRHL_SHORT_PROG_REC_V", "OVRHL_STAGE2_VERSION", "ID"));
            Database.AddRefColumn("OVRHL_SHORT_PROG_REC", new RefColumn("REALITY_OBJECT_ID", "OVRHL_SHORT_PROG_REC_RO", "GKH_REALITY_OBJECT", "ID"));

            Database.AddEntityTable("OVRHL_PUBLISH_PRG",
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_PUBLISH_PRG_V", "OVRHL_PRG_VERSION", "ID"),
                new RefColumn("STATE_ID", "OVRHL_PUBLISH_PRG_ST", "B4_STATE", "ID"),
                new RefColumn("FILE_ID", "OVRHL_PUBLISH_PRG_F", "B4_FILE_INFO", "ID"),
                new Column("ECP_SIGNED", DbType.Boolean, ColumnProperty.NotNull, false));

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
                new Column("COMMON_ESTATE_OBJECT", DbType.String, 255),
                new Column("WEAR", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("YEAR_LAST_OVERHAUL", DbType.Int16, ColumnProperty.NotNull, 0),
                new Column("YEAR_PUBLISHED", DbType.Int16, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PUBLISH_PRG_REC");
            Database.RemoveTable("OVRHL_PUBLISH_PRG");

            // Возвращаем удаленные колонки
            //Database.AddColumn("OVHL_DPKR_CORRECT_ST2", new Column("PUBLICATION_YEAR", DbType.Int16, ColumnProperty.NotNull, 0));

            //Database.AddRefColumn("OVRHL_SHORT_PROG_REC", new RefColumn("DPKR_CORRECT_ID", "OVRHL_SHORT_PROG_REC_DC", "OVRHL_DPKR_CORRECT_ST2", "ID"));
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "YEAR");
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "STAGE2_ID");
            Database.RemoveColumn("OVRHL_SHORT_PROG_REC", "REALITY_OBJECT_ID");
        }
    }
}