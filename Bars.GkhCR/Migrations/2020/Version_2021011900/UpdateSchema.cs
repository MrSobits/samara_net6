using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2021011900
{
    [Migration("2021011900")]
    [MigrationDependsOn(typeof(Version_2020071700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
             "CR_FILE_REGISTER",
             new RefColumn("RO_ID", ColumnProperty.NotNull, "CR_REG_RO_ID", "GKH_REALITY_OBJECT", "ID"),
             new RefColumn("FILE_ID", ColumnProperty.Null, "CR_REG_FILE_ID", "B4_FILE_INFO", "ID"),
             new Column("DATE_FROM", DbType.DateTime, ColumnProperty.Null),
             new Column("DATE_TO", DbType.DateTime, ColumnProperty.Null)
             );
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveTable("CR_FILE_REGISTER");
        }
    }
}