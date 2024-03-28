namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014020400
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014020300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERS_ACC_CALC_PARAM",
                new RefColumn("LOGGED_ENTITY_ID", ColumnProperty.NotNull, "ROP_P_ACC_C_P_LE", "GKH_ENTITY_LOG_LIGHT", "ID"),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "ROP_P_ACC_C_P_PA", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_CALC_PARAM");
        }
    }
}
