using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Tyumen.Migration.Version_1
{
    [Migration("1")]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
               "REGOP_REQUESTSTATE",
               new Column("USERID", DbType.Int64, ColumnProperty.NotNull),
               new Column("USERNAME", DbType.String, 200, ColumnProperty.NotNull),
               new Column("DESCRIPTION", DbType.String, 300, ColumnProperty.Null),
               new RefColumn("RO_ID", ColumnProperty.NotNull, "REGOP_REQUESTSTATE_GKH_REALITY_OBJECT_RO_ID_ID", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("FILE_ID", ColumnProperty.Null, "REGOP_REQUESTSTATE_B4_FILE_INFO_FILE_ID_ID", "B4_FILE_INFO", "ID")
               );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_REQUESTSTATE");
        }
    }
}