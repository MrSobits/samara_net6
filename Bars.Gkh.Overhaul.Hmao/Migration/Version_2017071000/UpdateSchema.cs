namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2017071000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    using Bars.Gkh.Utils;

    [Migration("2017071000")]
    [MigrationDependsOn(typeof(Version_2017042000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "OVRHL_CHANGE_YEAR_OWNER_DECISION",
                new Column("DOCUMENT_BASE", DbType.String),
                new Column("DOCUMENT_NUMBER", DbType.String),
                new Column("DATE", DbType.DateTime),
                new Column("REMARK", DbType.String),
                new Column("OLD_YEAR", DbType.Int32),
                new Column("NEW_YEAR", DbType.Int32),
                new FileColumn("FILE_ID", "OVRHL_CHANGE_YEAR_OWNER_DECISION_FILE_ID"),
                new RefColumn("STAGE1_ID", ColumnProperty.NotNull, "OVRHL_CHANGE_YEAR_OWNER_DECISION_STAGE1_ID", "OVRHL_STAGE1_VERSION", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_CHANGE_YEAR_OWNER_DECISION");
        }
    }
}