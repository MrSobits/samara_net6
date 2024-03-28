namespace Bars.GisIntegration.RegOp.Migrations.Version_2016102401
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102401")]
    [MigrationDependsOn(typeof(Version_2016102400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddColumn("RIS_CAPITAL_REPAIR_CHARGE", new Column("ORG_PPAGUID_REPAIRE_CHARGE", DbType.String));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_CAPITAL_REPAIR_CHARGE", "ORG_PPAGUID_REPAIRE_CHARGE");
        }
    }
}