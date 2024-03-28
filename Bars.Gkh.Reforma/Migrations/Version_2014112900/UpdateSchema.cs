namespace Bars.Gkh.Reforma.Migrations.Version_2014112900
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014112900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Reforma.Migrations.Version_2014112800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddPersistentObjectTable(
               "RFRM_CHANGED_ROBJECT",
               new RefColumn("ROBJECT_ID", ColumnProperty.NotNull, "RFRM_CHNGD_ROBJ_ROBJ", "GKH_REALITY_OBJECT", "ID"),
               new RefColumn("PERIOD_DI_ID", ColumnProperty.Null, "RFRM_CHNGD_ROBJ_PER", "DI_DICT_PERIOD", "ID"));

            Database.AddRefColumn("RFRM_REALITY_OBJECT", new RefColumn("REF_MAN_ORG_ID", ColumnProperty.Null, "RFRM_ROBJ_RFRM_MO", "RFRM_MANAGING_ORG", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("RFRM_REALITY_OBJECT", "REF_MAN_ORG_ID");
            Database.RemoveTable("RFRM_CHANGED_ROBJECT");
        }
    }
}