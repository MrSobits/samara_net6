namespace Bars.GkhGji.Migrations._2022.Version_2022102400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022102400")]
    [MigrationDependsOn(typeof(Version_2022092600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
              "GJI_RESOLUTION_DECISION_LT",
              new RefColumn("DECISION_ID", ColumnProperty.NotNull, "GJI_RESOLUTION_DECISION_LT_RD", "GJI_RESOLUTION_DECISION", "ID"),
              new Column("DECIDED", DbType.Binary),
              new Column("ESTABLISHED", DbType.Binary));

            Database.AddColumn("GJI_RESOLUTION_DECISION", new Column("APELLANT_PLACE_WORK", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION_DECISION", "APELLANT_PLACE_WORK");
            this.Database.RemoveTable("GJI_RESOLUTION_DECISION_LT");
        }
    }
}