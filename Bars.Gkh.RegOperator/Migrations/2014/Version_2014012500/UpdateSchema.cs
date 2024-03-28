namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012500
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("STATE_ID", ColumnProperty.NotNull, "STATECONTRACT_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "STATE_ID");
        }
    }
}
