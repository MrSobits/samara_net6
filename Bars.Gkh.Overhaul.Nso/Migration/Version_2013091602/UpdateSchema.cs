namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013091602
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013091602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013091601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RenameColumn("OVRHL_SUBSIDY_MU_REC", "PLAN_COLLECTION", "OWNERS_LIMIT");
            Database.RenameColumn("OVRHL_SUBSIDY_MU_REC", "CALC_TARIF", "ESTABLISHED_TARIF");

            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("START_RECOM_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("RECOM_TARIF_COLL", DbType.Decimal, ColumnProperty.NotNull, 0));

            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("COEF_AVG_INFL", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU", new Column("CONSIDER_INFL", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "CONSIDER_INFL");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU", "COEF_AVG_INFL");

            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "RECOM_TARIF_COLL");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "START_RECOM_TARIF");

            Database.RenameColumn("OVRHL_SUBSIDY_MU_REC", "OWNERS_LIMIT", "PLAN_COLLECTION");
            Database.RenameColumn("OVRHL_SUBSIDY_MU_REC", "ESTABLISHED_TARIF", "CALC_TARIF");
        }
    }
}