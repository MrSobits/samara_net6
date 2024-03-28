namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2019091201
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019091201")]
    [MigrationDependsOn(typeof(Version_2019091200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PROP_OWN_PROTOCOLS_DEC",
                new Column("DESCRIPTION", DbType.String, 1000),
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "OVRHL_PROP_OWN_PROTOCOLS_PID", "OVRHL_PROP_OWN_PROTOCOLS", "ID"),
                new RefColumn("DOCUMENT_FILE_ID", "OVRHL_PROP_OWN_PROTOCOLS_DEC_FL", "B4_FILE_INFO", "ID"),
                new RefColumn("PROT_TYPE_DEC_ID", ColumnProperty.NotNull, "OVRHL_PROP_OWN_PROTOCOLS_PTDID", "OVRHL_DICT_PROTTYPE_DECISION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PROP_OWN_PROTOCOLS_DEC");
        }
    }
}