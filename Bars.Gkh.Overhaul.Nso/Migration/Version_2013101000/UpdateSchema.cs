namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013101000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013100901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "OVRHL_PROP_OWN_PROTOCOLS",
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new Column("NUMBER_OF_VOTES", DbType.Decimal),
                new Column("TOTAL_NUMBER_OF_VOTES", DbType.Decimal),
                new Column("PERCENT_OF_PARTICIPATE", DbType.Decimal),
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "OVRHL_PROPOWNPROTOCOL_OBJ", "OVRHL_LONGTERM_PR_OBJECT", "ID"),
                new RefColumn("DOCUMENT_FILE_ID", "OVRHL_PROP_OWN_PROTOCOL", "B4_FILE_INFO", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PROP_OWN_PROTOCOLS");
        }
    }
}