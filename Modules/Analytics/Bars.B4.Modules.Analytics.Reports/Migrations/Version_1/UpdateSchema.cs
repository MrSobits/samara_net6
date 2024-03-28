namespace Bars.B4.Modules.Analytics.Reports.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("AL_STORED_REPORT",
                new Column("NAME", DbType.String, 200),
                new Column("CODE", DbType.String, 200),
                new RefColumn("TPL_FILE_ID", "STORED_REPORT_TPL", "B4_FILE_INFO", "ID")
                );

            Database.AddEntityTable("AL_REPORT_CUSTOM",
                new Column("CODED_REPORT_KEY", DbType.AnsiString),
                new RefColumn("TEMPLATE_ID", "REPORT_CUSTOM_TPL", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable("AL_REPORT_DATASOURCE",
                new RefColumn("DATA_SOURCE_ID", "REPORT_DATASOURCE_DATASRC", "AL_DATA_SOURCE", "ID"),
                new RefColumn("STORED_REPORT_ID", "REPORT_DATASOURCE_REPORT", "AL_STORED_REPORT", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("AL_REPORT_DATASOURCE");
            Database.RemoveTable("AL_REPORT_CUSTOM");
            Database.RemoveTable("AL_STORED_REPORT");
        }
    }
}
