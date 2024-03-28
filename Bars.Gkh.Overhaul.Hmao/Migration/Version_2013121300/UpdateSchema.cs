namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121300
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("OVRHL_RO_COM_EST_OBJ",
                new RefColumn("REAL_OBJ_ID", ColumnProperty.NotNull, "OV_RO_EST_OBJ_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("CMN_ESTATE_OBJ_ID", ColumnProperty.NotNull, "OV_RO_EST_OBJ_CEO", "OVRHL_COMMON_ESTATE_OBJECT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("OVRHL_RO_COM_EST_OBJ");
        }
    }
}