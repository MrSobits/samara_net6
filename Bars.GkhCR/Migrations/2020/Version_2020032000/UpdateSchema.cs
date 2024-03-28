using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh.Utils;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020032000
{
    [Migration("2020032000")]
    [MigrationDependsOn(typeof(Version_2020031000.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_TYPE_WORK", new Column("GIS_GKH_GUID", DbType.String, 36));
            Database.AddColumn("CR_OBJ_TYPE_WORK", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("CR_OBJ_TYPE_WORK", "GIS_GKH_GUID");
        }
    }
}