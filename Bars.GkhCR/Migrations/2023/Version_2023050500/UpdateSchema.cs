using Bars.B4.Modules.Ecm7.Framework;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023050500
{
    [Migration("2023050500")]
    [MigrationDependsOn(typeof(_2021.Version_2021082300.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_DICT_PROGRAM", new Column("IMPORT_CONTRACT", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("GIS_GKH_GUID", DbType.String,36, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("GIS_GKH_DOC_GUID", DbType.String, 36, ColumnProperty.None));

            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("GIS_GKH_GUID", DbType.String, 36, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("GIS_GKH_TRANSPORT_GUID", DbType.String, 36, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("GIS_GKH_DOC_GUID", DbType.String, 36, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "GIS_GKH_DOC_GUID");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "GIS_GKH_GUID");

            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "GIS_GKH_DOC_GUID");
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "GIS_GKH_TRANSPORT_GUID");
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "GIS_GKH_GUID");

            Database.RemoveColumn("CR_DICT_PROGRAM", "IMPORT_CONTRACT");
        }
    }
}