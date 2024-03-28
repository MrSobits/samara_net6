namespace Bars.GkhRf.Migrations.Version_2016052100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016052100")]
    [MigrationDependsOn(typeof(Version_2015121100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("RF_CONTRACT_OBJECT", new Column("TOTAL_AREA", DbType.Decimal));
            this.Database.AddColumn("RF_CONTRACT_OBJECT", new Column("AREA_LIV", DbType.Decimal));
            this.Database.AddColumn("RF_CONTRACT_OBJECT", new Column("AREA_NOT_LIV", DbType.Decimal));
            this.Database.AddColumn("RF_CONTRACT_OBJECT", new Column("AREA_LIV_OWN", DbType.Decimal));
        }

        /// <summary>
        /// Отменить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("RF_CONTRACT_OBJECT", "TOTAL_AREA");
            this.Database.RemoveColumn("RF_CONTRACT_OBJECT", "AREA_LIV");
            this.Database.RemoveColumn("RF_CONTRACT_OBJECT", "AREA_NOT_LIV");
            this.Database.RemoveColumn("RF_CONTRACT_OBJECT", "AREA_LIV_OWN");
        }
    }
}