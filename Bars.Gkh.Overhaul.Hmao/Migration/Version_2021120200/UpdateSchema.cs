namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2021120200
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021120200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021120100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PROP_OWN_PROTOCOLS_ANNEX",
           new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "OVRHL_OWNPROTOCOLS_ANNEX_PR", "OVRHL_PROP_OWN_PROTOCOLS", "ID"),
           new FileColumn("FILE_ID", "OVRHL_PROP_OWN_PROTOCOLS_ANNX_FIL"),
           new Column("DOCUMENT_DATE", DbType.DateTime, ColumnProperty.None),
           new Column("NAME", DbType.String, 300),
           new Column("DESCRIPTION", DbType.String, 500)
           );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PROP_OWN_PROTOCOLS_ANNEX");
        }
    }
}