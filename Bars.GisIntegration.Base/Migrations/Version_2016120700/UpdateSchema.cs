namespace Bars.GisIntegration.Base.Migrations.Version_2016120700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016120700")]
    [MigrationDependsOn(typeof(Version_2016112100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("RIS_OPERATOR_TICKET",
              new Column("TICKET", DbType.String, 255, ColumnProperty.NotNull),
              new RefColumn("GKH_OPERATOR_ID", "RIS_OPERATOR_TOKEN_GKH_OPERATOR", "GKH_OPERATOR", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("RIS_OPERATOR_TICKET");
        }
    }
}