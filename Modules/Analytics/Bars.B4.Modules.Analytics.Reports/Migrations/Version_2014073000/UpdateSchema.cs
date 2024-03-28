namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_2014073000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Reports.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable("AL_REPORT_PARAM",
                new Column("PARAM_TYPE", DbType.Int16),
                new Column("REQUIRED", DbType.Boolean),
                new Column("LABEL_TEXT", DbType.String, 200),
                new Column("NAME", DbType.String, 100),
                new Column("DEFAULT_VAL_BYTES", DbType.Binary),
                new RefColumn("REPORT_ID", ColumnProperty.NotNull, "REPORT_PARAM_REPORT", "AL_STORED_REPORT", "ID"));

            Database.AddColumn("AL_STORED_REPORT", new RefColumn("CATEGORY_ID", ColumnProperty.Null, "AL_REPORT_CATEGORY", "B4_PRINT_FORM_CATEGORY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_STORED_REPORT", "CATEGORY_ID");
            Database.RemoveTable("AL_REPORT_PARAM");
        }
    }
}
