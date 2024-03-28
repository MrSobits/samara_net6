using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2019.Version_2019020400
{
    [Migration("2019020400")]
    [MigrationDependsOn(typeof(_2018.Version_2018092100.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_TYPE_WORK", "IS_EMERGENCY", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_TYPE_WORK", "IS_EMERGENCY");
        }
    }
}