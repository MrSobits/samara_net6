namespace Bars.Gkh.Reforma.Migrations.Version_2016061000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Reforma.Enums;

    /// <summary>
    /// Миграция [2016061000]
    /// </summary>
    [Migration("2016061000")]
    [MigrationDependsOn(typeof(Bars.Gkh.Reforma.Migrations.Version_2016022600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("RFRM_SESSION_LOG", "TYPE_INTEGRATION", DbType.Int32, ColumnProperty.NotNull, (int)TypeIntegration.Automatic);
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("RFRM_SESSION_LOG", "TYPE_INTEGRATION");
        }
    }
}
