namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014031400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014031400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2014030600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Таблица не используется
            /*
            Database.AddEntityTable("GKH_SERVICED_HOUSE",
                new RefColumn("RO_ID", ColumnProperty.NotNull, "SERV_HOUSE_REAL_OBJ", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("CALC_ACC_ID", ColumnProperty.NotNull, "SERV_HOUSE_CALC_ACC", "OVRHL_REG_OP_CALC_ACC",
                    "ID")
                );
             */
        }

        public override void Down()
        {
            // Database.RemoveEntityTable("GKH_SERVICED_HOUSE");
        }
    }
}