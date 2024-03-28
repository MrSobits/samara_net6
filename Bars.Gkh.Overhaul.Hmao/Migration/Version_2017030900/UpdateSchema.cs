namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2017030900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция 2017030900
    /// </summary>
    [Migration("2017030900")]
    [MigrationDependsOn(typeof(Version_2017030200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("OVRHL_PUBLISH_PRG_REC", new RefColumn("RO_ID", "OVRHL_PUBLISH_PRG_REC_RO_ID", "GKH_REALITY_OBJECT", "ID"));

            this.Database.AlterColumnSetNullable("OVRHL_PUBLISH_PRG_REC", "STAGE2_ID", true);

            this.Database.AddColumn("OVRHL_PUBLISH_PRG", "TOTAL_RO_COUNT", DbType.Int32);
            this.Database.AddColumn("OVRHL_PUBLISH_PRG", "INCLUDED_RO_COUNT", DbType.Int32);
            this.Database.AddColumn("OVRHL_PUBLISH_PRG", "EXCLUDED_RO_COUNT", DbType.Int32);
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PUBLISH_PRG_REC", "RO_ID");

            this.Database.RemoveColumn("OVRHL_PUBLISH_PRG", "TOTAL_RO_COUNT");
            this.Database.RemoveColumn("OVRHL_PUBLISH_PRG", "INCLUDED_RO_COUNT");
            this.Database.RemoveColumn("OVRHL_PUBLISH_PRG", "EXCLUDED_RO_COUNT");
        }
    }
}