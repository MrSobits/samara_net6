namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2023090600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023090600")]
    [MigrationDependsOn(typeof(Version_2023082800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_KPKR_COST_LIMITS", new Column("COST_FOR_CAP_GROUP", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_KPKR_COST_LIMITS", new Column("UNIT_COST_FOR_CAP_GROUP", DbType.Decimal, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_KPKR_COST_LIMITS", "UNIT_COST_FOR_CAP_GROUP");
            Database.RemoveColumn("OVRHL_KPKR_COST_LIMITS", "COST_FOR_CAP_GROUP");
        }
    }
}