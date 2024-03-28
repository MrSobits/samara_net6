namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2020101400
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2020101400")]
    [MigrationDependsOn(typeof(Version_2020063000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", new Column("VOTE_FORM", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", new Column("REGISTRATION_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", new Column("REGISTRATION_NUMBER", DbType.String, 50));
            Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("INSPECTOR_ID", "OVRHL_PROP_OWN_PROTOCOLS_INSP", "GKH_DICT_INSPECTOR", "ID"));
            Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("PROTOCOL_STATE_ID", "OVRHL_PROP_OWN_PROTOCOLS_STATE", "GKH_DICT_MKDPROTOCOL_STATE", "ID"));
            Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("PROTOCOL_SOURCE_ID", "OVRHL_PROP_OWN_PROTOCOLS_SOURCE", "GKH_DICT_MKDPROTOCOL_SOURCE", "ID"));
            Database.AddRefColumn("OVRHL_PROP_OWN_PROTOCOLS", new RefColumn("PROTOCOL_INICIATOR_ID", "OVRHL_PROP_OWN_PROTOCOLS_INICIATOR", "GKH_DICT_MKDPROTOCOL_INICIATOR", "ID"));
        }
                       
        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "PROTOCOL_INICIATOR_ID");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "PROTOCOL_SOURCE_ID");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "PROTOCOL_STATE_ID");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "INSPECTOR_ID");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "REGISTRATION_NUMBER");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "REGISTRATION_DATE");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "VOTE_FORM");
        }
    }
}