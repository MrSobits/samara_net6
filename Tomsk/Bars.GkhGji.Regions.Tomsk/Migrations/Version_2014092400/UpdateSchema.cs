namespace Bars.GkhGji.Regions.Tomsk.Migration.Version_2014092400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014091500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Указание к устранению нарушений входе проверки заносятся в нарушения предписания
            Database.AddTable(
                "GJI_ADMINCASE_VIOLAT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJECT_VERSION", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("OBJECT_CREATE_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("OBJECT_EDIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddForeignKey("FK_GJI_ADMINCASE_VIOLAT_ID", "GJI_ADMINCASE_VIOLAT", "ID", "GJI_INSPECTION_VIOL_STAGE", "ID");
            //-----
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_ADMINCASE_VIOLAT", "FK_GJI_ADMINCASE_VIOLAT_ID");
            Database.RemoveTable("GJI_ADMINCASE_VIOLAT");
        }
    }
}