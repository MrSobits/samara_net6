using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2019.Version_2019061300
{
    [Migration("2019061300")]
    [MigrationDependsOn(typeof(_2019.Version_2019060500.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PROPOSE_TYPE_WORK", "BY_AREA", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PROPOSE_TYPE_WORK", "BY_AREA");
        }
    }
}