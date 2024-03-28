namespace Bars.Gkh.Migrations._2017.Version_2017030602
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Gkh.2017030602
    /// </summary>
    [Migration("2017030602")]
    [MigrationDependsOn(typeof(Version_2017030200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_REALITY_OBJECT", "CTP_NAME", DbType.String);
            this.Database.AddRefColumn("GKH_REALITY_OBJECT", new RefColumn("FIAS_ADDRESS_CTP_ID", "RO_CTP_FIAS", "B4_FIAS_ADDRESS", "ID"));
            this.Database.AddColumn("GKH_REALITY_OBJECT", "NUMBER_IN_CTP", DbType.Int32);
            this.Database.AddColumn("GKH_REALITY_OBJECT", "PRIORITY_CTP", DbType.Int32);
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "CTP_NAME");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "FIAS_ADDRESS_CTP_ID");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "NUMBER_IN_CTP");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "PRIORITY_CTP");
        }
    }
}