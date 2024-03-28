namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024022100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024022100")]
    [MigrationDependsOn(typeof(Version_2023103000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", new Column("WORKS", DbType.String, 500));
            Database.AddColumn("OVRHL_PROP_OWN_PROTOCOLS", new Column("DISPLAY_ON_PORTAL", DbType.Boolean));

            Database.AddEntityTable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK",
               new Column("COST_LIMIT", DbType.Decimal, ColumnProperty.NotNull),
               new Column("TYPE_FIN_SOURCE", DbType.Int16, ColumnProperty.NotNull),
               new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
               new Column("RESPONSIBLE", DbType.String, ColumnProperty.NotNull),
               new RefColumn("WORK_ID", "OVRHL_PROP_OWN_PROTOCOLS_DEC_WORK_WORK_ID", "GKH_DICT_WORK", "ID"),
               new RefColumn("PROT_TYPE_DEC_ID", "OVRHL_PROP_OWN_PROTOCOL_DEC_WORK_DEC_ID", "OVRHL_PROP_OWN_PROTOCOLS_DEC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PROP_OWN_PROTOCOL_DEC_WORK");

            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "DISPLAY_ON_PORTAL");
            Database.RemoveColumn("OVRHL_PROP_OWN_PROTOCOLS", "WORKS");
        }
    }
}