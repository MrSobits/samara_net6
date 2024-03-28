using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014062200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("SUBCIDY_YEAR", DbType.Int64, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("BUDGET_REGION", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("BUDGET_MU", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("BUDGET_FSR", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("BUDGET_OTHER_SRC", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("PLAN_OWN_COLLECTION", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("PLAN_OWN_PRC", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("NOT_REDUCE_SIZE_PRC", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("OWNER_SUM_CR", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_REC_VERSION", new Column("DATE_CALC_OWN", DbType.DateTime));

            // обновляю сразу добавленные колонки по данным субсидирования по мо (чтобы не парились выполнением действий)
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set subcidy_year = (select subcidy_year from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set budget_region = (select budget_region from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set budget_mu = (select budget_mu from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set budget_fsr = (select budget_fsr from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set budget_other_src = (select budget_other_src from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set plan_own_collection = (select plan_own_collection from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set plan_own_prc = (select plan_own_prc from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set not_reduce_size_prc = (select not_reduce_size_prc from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set owner_sum_cr = (select owner_sum_cr from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
            Database.ExecuteNonQuery(@"update ovrhl_subsidy_rec_version v set date_calc_own = (select date_calc_own from ovrhl_subsidy_rec s where s.id = v.subsidy_rec_id)");
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "SUBCIDY_YEAR");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "BUDGET_REGION");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "BUDGET_MU");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "BUDGET_FSR");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "BUDGET_OTHER_SRC");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "PLAN_OWN_COLLECTION");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "NOT_REDUCE_SIZE_PRC");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "OWNER_SUM_CR");
            Database.RemoveColumn("OVRHL_SUBSIDY_REC_VERSION", "DATE_CALC_OWN");
        }
    }
}