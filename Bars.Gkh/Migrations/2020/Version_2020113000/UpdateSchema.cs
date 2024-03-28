namespace Bars.Gkh.Migrations._2020.Version_2020113000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020113000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020112700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_CIT_SUG", new RefColumn("CORRESPONDENT_CONTRAGENT_ID", "FK_CIT_SUG_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_GKH_ANSWER_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_GKH_ANSWER_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_GKH_PARENT_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_GKH_CONTRAGENT_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG", new Column("GIS_WORK", DbType.Boolean, false));

            Database.AddColumn("GKH_CIT_SUG_FILES", new Column("NAME", DbType.String.WithSize(500), ColumnProperty.Null));
            Database.AddColumn("GKH_CIT_SUG_FILES", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("GKH_CIT_SUG_FILES", new Column("HASH", DbType.String));

            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("GIS_GKH_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "GIS_GKH_GUID");

            Database.RemoveColumn("GKH_CIT_SUG_FILES", "HASH");
            Database.RemoveColumn("GKH_CIT_SUG_FILES", "GIS_GKH_GUID");
            Database.RemoveColumn("GKH_CIT_SUG_FILES", "NAME");

            Database.RemoveColumn("GKH_CIT_SUG", "GIS_WORK");
            Database.RemoveColumn("GKH_CIT_SUG", "GIS_GKH_CONTRAGENT_GUID");
            Database.RemoveColumn("GKH_CIT_SUG", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("GKH_CIT_SUG", "GIS_GKH_PARENT_GUID");
            Database.RemoveColumn("GKH_CIT_SUG", "GIS_GKH_GUID");
            Database.RemoveColumn("GKH_CIT_SUG", "GIS_GKH_ANSWER_TRANSPORT_GUID");
            Database.RemoveColumn("GKH_CIT_SUG", "GIS_GKH_ANSWER_GUID");
            Database.RemoveColumn("GKH_CIT_SUG", "CORRESPONDENT_CONTRAGENT_ID");
        }
    }
}