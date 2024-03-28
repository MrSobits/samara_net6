namespace Bars.GkhGji.Migrations._2022.Version_2022103100
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022103100")]
    [MigrationDependsOn(typeof(Version_2022102700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// <summary>
        public override void Up()
        {
            //-----
            this.Database.AddEntityTable(
            "GJI_RESOLUTION_LT",
            new RefColumn("RESOLUTION_ID", ColumnProperty.NotNull, "GJI_RESOLUTION_LT_RES", "GJI_RESOLUTION", "ID"),
            new Column("ESTABLISHED", DbType.Binary));

            this.Database.AddRefColumn("GJI_RESOLUTION_DECISION", new RefColumn("SIGNER_ID", ColumnProperty.None, "GJI_RESOLUTION_DECISION_SIGNER", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_RESOLUTION_DECISION", "SIGNER_ID");
            this.Database.RemoveTable("GJI_RESOLUTION_LT");

        }
    }
}