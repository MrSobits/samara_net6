namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012505
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012505")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012503.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_UNACCEPT_CHARGE", new RefColumn("ACC_ID", ColumnProperty.NotNull, "ROP_UNACC_CH_ACC", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_UNACCEPT_CHARGE", "ACC_ID");
        }
    }
}
