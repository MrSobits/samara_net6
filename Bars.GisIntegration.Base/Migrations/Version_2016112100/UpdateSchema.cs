namespace Bars.GisIntegration.Base.Migrations.Version_2016112100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016112100")]
    [MigrationDependsOn(typeof(Version_2016111800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.ChangeColumn("GI_ATTACHMENT", new Column("NAME", DbType.String, 500));
            this.Database.ChangeColumn("GI_ATTACHMENT", new Column("DESCRIPTION", DbType.String, 500));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
        }
    }
}