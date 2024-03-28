using Bars.Gkh.Enums;

namespace Bars.Gkh.Migrations._2015.Version_2015102200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_MORG_CONTRACT", "CONTRACT_STOP_REASON", DbType.Int32, ColumnProperty.NotNull, (object)0);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MORG_CONTRACT", "CONTRACT_STOP_REASON");
        }
    }
}