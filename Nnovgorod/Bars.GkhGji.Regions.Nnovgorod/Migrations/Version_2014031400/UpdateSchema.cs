namespace Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014031400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014021100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----Мероприятия по контролю распоряжения
            Database.AddEntityTable(
                "GJI_NNOV_DISP_CON_MEASURE",
                new Column("DISPOSAL_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("CONTROL_MEASURES_NAME", DbType.String, 2000));

            Database.AddForeignKey("FK_CONTROL_MEASURES_NAME", "GJI_NNOV_DISP_CON_MEASURE", "DISPOSAL_ID", "GJI_DISPOSAL", "ID");
            
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_NNOV_DISP_CON_MEASURE", "FK_CONTROL_MEASURES_NAME");
            Database.RemoveTable("GJI_NNOV_DISP_CON_MEASURE");
        }
    }
}