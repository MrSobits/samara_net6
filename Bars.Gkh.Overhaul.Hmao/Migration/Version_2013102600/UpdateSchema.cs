namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013102600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013102600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013102400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "FINANCE_NEED_BEFORE");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "RECOMMEND_TARIF");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "DEFICIT_BEFORE");
            Database.RemoveColumn("OVRHL_SUBSIDY_MU_REC", "RECOM_TARIF_COLL");

            Database.AddEntityTable("OVRHL_STAGE2_VERSION",
                new Column("CE_WEIGHT", DbType.Int32, ColumnProperty.NotNull, 0),
                new RefColumn("ST3_VERSION_ID", "ST2VERSION_ST3V", "OVRHL_VERSION_REC", "ID"));

            Database.RemoveColumn("OVHL_DPKR_CORRECT_ST2", "STAGE2_ID");
            Database.AddRefColumn("OVHL_DPKR_CORRECT_ST2",
                new RefColumn("ST2_VERSION_ID", ColumnProperty.NotNull, "DPKRCORR_ST2V", "OVRHL_STAGE2_VERSION", "ID"));

            Database.AddEntityTable("OVRHL_STAGE1_VERSION", 
                new Column("YEAR", DbType.Int32, ColumnProperty.NotNull, 0),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "ST3VERSION_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("STAGE2_VERSION_ID", "ST3VERSION_ST2V", "OVRHL_STAGE2_VERSION", "ID"),
                new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "ST3VERSION_SE", "OVRHL_RO_STRUCT_EL", "ID"),
                new RefColumn("COMMON_ESTATE_ID", ColumnProperty.NotNull, "ST3VERSION_CE", "OVRHL_COMMON_ESTATE_OBJECT", "ID"));

            Database.AddEntityTable("OVRHL_SM_RECORD_VERSION",
                new Column("RECOMMEND_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("RECOM_TARIF_COLL", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("FINANCE_NEED_BEFORE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("DEFICIT_BEFORE", DbType.Decimal, ColumnProperty.NotNull, 0),
                new RefColumn("SM_RECORD_ID", ColumnProperty.NotNull, "SM_REC_VERSION_MUREC", "OVRHL_SUBSIDY_MU_REC", "ID"),
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "SM_REC_VERSION_VER", "OVRHL_PRG_VERSION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_SM_RECORD_VERSION");
            Database.RemoveTable("OVRHL_STAGE1_VERSION");
            Database.RemoveTable("OVRHL_STAGE2_VERSION");

            Database.RemoveColumn("OVHL_DPKR_CORRECT_ST2", "ST2_VERSION_ID");

            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("FINANCE_NEED_BEFORE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("RECOMMEND_TARIF", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("DEFICIT_BEFORE", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("OVRHL_SUBSIDY_MU_REC", new Column("RECOM_TARIF_COLL", DbType.Decimal, ColumnProperty.NotNull, 0));
        }
    }
}