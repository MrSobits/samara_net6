using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2021.Version_2021051200
{
    [Migration("2021051200")]
    [MigrationDependsOn(typeof(Version_2021042200.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {          

            this.Database.AddEntityTable("CR_OBJ_HOUSEKEEPER_REPORT",
             new Column("DESCRIPTION", DbType.String, 5000),
             new Column("IS_ARRANGED", DbType.Boolean, false),
             new Column("REPORT_DATE", DbType.DateTime, ColumnProperty.NotNull),
             new Column("REPORT_NUMBER", DbType.String, 50),
             new RefColumn("OBJECT_ID", "HOUSEKEEPER_REPORT_CR_OBJ", "CR_OBJECT", "ID"),
             new RefColumn("STATE_ID", "HOUSEKEEPER_REPORT_STATE", "B4_STATE", "ID"),
             new RefColumn("HOUSEKEEPER_ID", "HOUSEKEEPER_REPORT_HOUSEKEEPER", "GKH_REALITY_HOUSEKEEPER", "ID"));

            this.Database.AddEntityTable("CR_OBJ_HOUSEKEEPER_REPORT_FILE",
            new Column("DESCRIPTION", DbType.String, 5000),         
            new RefColumn("REPORT_ID", "HOUSEKEEPER_REPORT_FILE_REPORT", "CR_OBJ_HOUSEKEEPER_REPORT", "ID"),
            new RefColumn("FILE_INFO_ID", "HOUSEKEEPER_REPORT_FILE_FILEINFO", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_OBJ_HOUSEKEEPER_REPORT_FILE");
            this.Database.RemoveTable("CR_OBJ_HOUSEKEEPER_REPORT");
        }
    }
}