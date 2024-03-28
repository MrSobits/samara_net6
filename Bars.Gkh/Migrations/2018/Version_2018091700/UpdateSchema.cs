using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.Gkh.Migrations._2018.Version_2018091700
{
    [Migration("2018091700")]
    [MigrationDependsOn(typeof(Version_2018080200.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "GKH_PRINT_CERT_HISTORY",
               new Column("ACC_NUM", DbType.String, 20),
               new Column("ADDRESS", DbType.String, 255),
               new Column("TYPE", DbType.String, 50),
               new Column("NAME", DbType.String, 255),
               new Column("PRINT_DATE", DbType.DateTime),
               new Column("USERNAME", DbType.String, 255),
               new Column("ROLE", DbType.String, 255)
               );
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_PRINT_CERT_HISTORY");
        }
    }
}