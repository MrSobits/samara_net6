namespace Bars.GisIntegration.Base.Migrations.Version_2016071100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016071100")]
    [MigrationDependsOn(typeof(Version_2016062800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GI_CONTRAGENT", new Column("ORGTYPE", DbType.Int16));
            this.Database.AddColumn("GI_CONTRAGENT", new Column("ORGPPAGUID", DbType.String, 50));
            this.Database.RemoveColumn("GI_CONTRAGENT", "IS_INDIVIDUAL");
            this.Database.RemoveColumn("GI_CONTRAGENT", "SENDERID");
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.AddColumn("GI_CONTRAGENT", new Column("SENDERID", DbType.String, 50));
            this.Database.AddColumn("GI_CONTRAGENT", new Column("IS_INDIVIDUAL", DbType.Boolean));
            this.Database.RemoveColumn("GI_CONTRAGENT", "ORGPPAGUID");
            this.Database.RemoveColumn("GI_CONTRAGENT", "ORGTYPE");
        }
    }
}
