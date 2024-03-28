namespace Bars.GkhGji.Migrations._2024.Version_2024030103
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using System.Data;

    [Migration("2024030103")]
    [MigrationDependsOn(typeof(Version_2024030102.UpdateSchema))]
    /// Является Version_2018060100 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_INSPECTION_STATEMENT",
                new Column("REQUEST_TYPE", DbType.Int32, ColumnProperty.None));

            this.Database.AddEntityTable("GJI_BASESTAT_DOCUMENT",
                new RefColumn("INSPECTION_ID", ColumnProperty.None, "GJI_BASESTAT_DOCUMENT_INSPECTION", "GJI_INSPECTION", "ID"),
                new RefColumn("DOCUMENT_ID", ColumnProperty.None, "GJI_BASESTAT_DOCUMENT_DOCUMENT", "GJI_DOCUMENT", "ID"));

            //ViewManager.Drop(this.Database, "GkhGji", "DeleteViewInsStatement");
            //ViewManager.Create(this.Database, "GkhGji", "CreateViewInsStatement");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_BASESTAT_DOCUMENT");
            this.Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "REQUEST_TYPE");
        }
    }
}