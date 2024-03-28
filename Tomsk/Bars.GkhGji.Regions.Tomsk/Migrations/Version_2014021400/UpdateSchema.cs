namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_TOMSK_FRAME_VERIFICATION",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 300));

            Database.AddRefColumn("GJI_ACTVISUAL", new RefColumn("FRAME_VERIFICATION_ID", "FRAME_VERIF_ACTVISUAL", "GJI_TOMSK_FRAME_VERIFICATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_FRAME_VERIFICATION");
        }
    }
}
