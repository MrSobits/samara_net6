using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2021.Version_2021060800
{
    [Migration("2021060800")]
    [MigrationDependsOn(typeof(Version_2021051200.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_HOUSEKEEPER_REPORT", new Column("ANSWER", DbType.String, 1500, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_HOUSEKEEPER_REPORT", new Column("CKECK_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_HOUSEKEEPER_REPORT", new Column("CHECK_TIME", DbType.String, 50, ColumnProperty.None));

           
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_HOUSEKEEPER_REPORT", "ANSWER");
            Database.RemoveColumn("CR_OBJ_HOUSEKEEPER_REPORT", "CKECK_DATE");
            Database.RemoveColumn("CR_OBJ_HOUSEKEEPER_REPORT", "CHECK_TIME");
        }
    }
}