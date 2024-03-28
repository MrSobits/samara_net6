namespace Bars.GkhCr.Migrations.Version_2013071800
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013071800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013062900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddUniqueConstraint("UNQ_MONITORING_CR_OBJECT", "CR_OBJ_MONITORING_CMP", "OBJECT_ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("CR_OBJ_MONITORING_CMP", "UNQ_MONITORING_CR_OBJECT");
        }
    }
}