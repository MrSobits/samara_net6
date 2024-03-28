using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020070700
{
    [Migration("2020070700")]
    [MigrationDependsOn(typeof(Version_2020052100.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_CONTRACT", new Column("START_SUM", DbType.Decimal, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_BUILD_CONTRACT", new Column("START_SUM", DbType.Decimal, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_MASS_BUILD_CONTRACT", new Column("START_SUM", DbType.Decimal, ColumnProperty.None));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.AddColumn("CR_OBJ_MASS_BUILD_CONTRACT", new Column("START_SUM", DbType.Decimal, ColumnProperty.None));
            Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "START_SUM");
            Database.RemoveColumn("CR_OBJ_CONTRACT", "START_SUM");
        }
    }
}