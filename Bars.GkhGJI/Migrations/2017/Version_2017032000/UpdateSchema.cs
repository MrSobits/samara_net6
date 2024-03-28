namespace Bars.GkhGji.Migrations._2017.Version_2017032000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017032000
    /// </summary>
    [Migration("2017032000")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2017.Version_2017022100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_INSPECTION_ZONAL_INSPECTION",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("ZONAL_INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_INSPECT_ZINS_INSN", false, "GJI_INSPECTION_ZONAL_INSPECTION", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_INSPECT_ZINS_INSR", false, "GJI_INSPECTION_ZONAL_INSPECTION", "ZONAL_INSPECTION_ID");
            Database.AddForeignKey("FK_GJI_INSPECT_ZINS_INSR", "GJI_INSPECTION_ZONAL_INSPECTION", "ZONAL_INSPECTION_ID", "GKH_DICT_ZONAINSP", "ID");
            Database.AddForeignKey("FK_GJI_INSPECT_ZINS_INSN", "GJI_INSPECTION_ZONAL_INSPECTION", "INSPECTION_ID", "GJI_INSPECTION", "ID");

            //ViewManager.Drop(Database, "GkhGji", "DeleteViewInsDisphead");
            //ViewManager.Drop(Database, "GkhGji", "DeleteViewInsInschek");
            //ViewManager.Drop(Database, "GkhGji", "DeleteViewInsJurpers");
            //ViewManager.Drop(Database, "GkhGji", "DeleteViewInsProsclaim");
            //ViewManager.Drop(Database, "GkhGji", "DeleteFunctionGetInspectionZonalInspections");

            //ViewManager.Create(Database, "GkhGji", "CreateFunctionGetInspectionZonalInspections");
            //ViewManager.Create(Database, "GkhGji", "CreateViewInsDisphead");
            //ViewManager.Create(Database, "GkhGji", "CreateViewInsInschek");
            //ViewManager.Create(Database, "GkhGji", "CreateViewInsJurpers");
            //ViewManager.Create(Database, "GkhGji", "CreateViewInsProsclaim");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_INSPECTION_ZONAL_INSPECTION", "FK_GJI_INSPECT_ZINS_INSR");
            Database.RemoveConstraint("GJI_INSPECTION_ZONAL_INSPECTION", "FK_GJI_INSPECT_ZINS_INSN");
            Database.RemoveTable("GJI_INSPECTION_ZONAL_INSPECTION");
        }
    }
}