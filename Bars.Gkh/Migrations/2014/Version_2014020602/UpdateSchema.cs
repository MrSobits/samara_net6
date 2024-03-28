namespace Bars.Gkh.Migrations.Version_2014020602
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //перенесено в модуль Decisions

            //Database.RemoveRefColumn("GKH_GENERIC_DECISION", "PROTOCOL_ID");
            //Database.AddRefColumn("GKH_GENERIC_DECISION", new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "GEN_DECISION_PROT", "GKH_OBJ_D_PROTOCOL", "ID"));
        }

        public override void Down()
        {
        }
    }
}