using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2018.Version_2018092100
{
    [Migration("2018092100")]
    [MigrationDependsOn(typeof(_2018.Version_2018061800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_PSD_WORK", "COST", DbType.Decimal, ColumnProperty.None);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_PSD_WORK", "COST");
        }
    }
}