namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016032900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016032900
    /// </summary>
    [Migration("2016032900")]
    [MigrationDependsOn(typeof(Version_2016030100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn(
                "GKH_CONSTRUCTION_OBJECT",
                new Column("SUM_SMR", DbType.Decimal.WithSize(12, 2)));
            this.Database.ChangeColumn(
                "GKH_CONSTRUCTION_OBJECT",
                new Column("SUM_DEV_PSD", DbType.Decimal.WithSize(12, 2)));
            this.Database.ChangeColumn(
                "GKH_CONSTRUCTION_OBJECT",
                new Column("LIMIT_ON_HOUSE", DbType.Decimal.WithSize(12, 2)));
            this.Database.ChangeColumn(
                "GKH_CONSTRUCTION_OBJECT",
                new Column("TOTAL_AREA", DbType.Decimal.WithSize(12, 2)));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
        }
    }
}
