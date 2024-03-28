using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.Utils;
using System.Data;

namespace Bars.GkhCr.Migrations._2019.Version_2019091600
{
    [Migration("2019091600")]
    [MigrationDependsOn(typeof(Version_2019083000.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_DICT_PROGRAM", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("CR_DICT_PROGRAM", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
            Database.AddColumn("CR_DICT_PROGRAM", new Column("GIS_GKH_DOC_ATTACHMENT_GUID", DbType.String, 36));
            Database.AddColumn("CR_DICT_PROGRAM", new Column("GIS_GKH_DOC_GUID", DbType.String, 36));
            Database.AddColumn("CR_DICT_PROGRAM", new Column("GIS_GKH_DOC_TRANSPORT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_DICT_PROGRAM", "GIS_GKH_DOC_TRANSPORT_GUID");
            Database.RemoveColumn("CR_DICT_PROGRAM", "GIS_GKH_DOC_GUID");
            Database.RemoveColumn("CR_DICT_PROGRAM", "GIS_GKH_DOC_ATTACHMENT_GUID");
            Database.RemoveColumn("CR_DICT_PROGRAM", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("CR_DICT_PROGRAM", "GIS_GKH_GUID");
        }
    }
}