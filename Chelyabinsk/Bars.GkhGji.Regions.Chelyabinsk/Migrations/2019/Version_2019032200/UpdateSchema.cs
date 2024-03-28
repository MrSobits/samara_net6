namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2019032200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019032200")]
    [MigrationDependsOn(typeof(Version_2019022100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_PAY_REG", new Column("IS_PAYFINE_ADDED", DbType.Boolean, false));
            Database.AddColumn("GJI_CH_GIS_GMP", new Column("RECONCILE_ANSWER", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_PAY_REG", "IS_PAYFINE_ADDED");
            Database.RemoveColumn("GJI_CH_GIS_GMP", "RECONCILE_ANSWER");
        }
    }
}