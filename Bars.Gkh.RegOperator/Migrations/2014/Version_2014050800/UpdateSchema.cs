namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014050800
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014050800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014050700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERSACCALCPARAMTMP",
                new RefColumn("LOGGED_ENTITY_ID", ColumnProperty.NotNull, "ROP_P_ACC_C_P_LE_T", "GKH_ENTITY_LOG_LIGHT", "ID"),
                new RefColumn("PERS_ACC_ID", ColumnProperty.NotNull, "ROP_P_ACC_C_P_PA_T", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERSACCALCPARAMTMP");
        }
    }
}
