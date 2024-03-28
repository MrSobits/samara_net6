namespace Bars.GkhCr.Migrations.Version_2015011200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015011200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014102100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_COMPETITION",
                new FileColumn("FILE_ID", "CR_COMPETITION_FILE"),
                new StateColumn("STATE_ID", "CR_COMPETITION_STATE"),
                new Column("NOTIF_NUMBER", DbType.String, 100),
                new Column("NOTIF_DATE", DbType.Date, ColumnProperty.NotNull),

                new Column("REVIEW_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("REVIEW_TIME", DbType.String, 100),
                new Column("REVIEW_PLACE", DbType.String, 300),

                new Column("EXEC_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("EXEC_TIME", DbType.String, 100),
                new Column("EXEC_PLACE", DbType.String, 300));

            Database.AddEntityTable("CR_COMPETITION_LOT",
                new RefColumn("COMPETITION_ID", ColumnProperty.NotNull, "CR_COMPETITION_LOT_CMP", "CR_COMPETITION", "ID"),
                new Column("LOT_NUMBER", DbType.Int32, ColumnProperty.NotNull),
                new Column("STARTING_PRICE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("SUBJECT", DbType.String, 500),

                new Column("CONTRACT_NUMBER", DbType.String, 100),
                new Column("CONTRACT_DATE", DbType.Date),
                new Column("CONTRACT_FACT_PRICE", DbType.Decimal),
                new FileColumn("CONTRACT_FILE_ID", "CR_COMPETITION_LOT_CTR_FIL"));

            Database.AddEntityTable("CR_COMPETITION_LOT_TW",
                new RefColumn("LOT_ID", ColumnProperty.NotNull, "CR_COMPETITION_LOT_TW_LOT", "CR_COMPETITION_LOT", "ID"),
                new RefColumn("TYPE_WORK_ID", ColumnProperty.NotNull, "CR_COMPETITION_LOT_TW_TW", "CR_OBJ_TYPE_WORK", "ID"));

            Database.AddEntityTable("CR_COMPETITION_LOT_BID",
                new RefColumn("LOT_ID", ColumnProperty.NotNull, "CR_COMPETITION_LOT_BID_LOT", "CR_COMPETITION_LOT", "ID"),
                new RefColumn("BUILDER_ID", ColumnProperty.NotNull, "CR_COMP_LB_BLDR", "GKH_BUILDER", "ID"),
                new Column("BDATE", DbType.Date, ColumnProperty.NotNull),
                new Column("POINTS", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PRICE", DbType.Decimal, ColumnProperty.NotNull),
                new Column("PRICE_NDS", DbType.Decimal, ColumnProperty.NotNull),
                new Column("IS_WINNER", DbType.Boolean, ColumnProperty.NotNull));

            Database.AddEntityTable("CR_COMPETITION_DOCUMENT",
                new RefColumn("COMPETITION_ID", ColumnProperty.NotNull, "CR_COMPETITION_DOC_CMP", "CR_COMPETITION", "ID"),
                new FileColumn("FILE_ID", "CR_COMPETITION_DOC_FILE"),
                new Column("DOCUMENT_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("DOCUMENT_NAME", DbType.String, 300),
                new Column("DOCUMENT_NUMBER", DbType.String, 100));

            Database.AddEntityTable("CR_COMPETITION_PROTOCOL",
                new RefColumn("COMPETITION_ID", ColumnProperty.NotNull, "CR_COMPETITION_PROT_CMP", "CR_COMPETITION", "ID"),
                new Column("EXEC_DATE", DbType.Date),
                new Column("EXEC_TIME", DbType.String),
                new Column("IS_CANCELLED", DbType.Boolean, ColumnProperty.NotNull),
                new Column("CNOTE", DbType.String, 500),
                new Column("SIGN_DATE", DbType.Date, ColumnProperty.NotNull),
                new Column("TYPE_PROTOCOL", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_COMPETITION_PROTOCOL");
            Database.RemoveTable("CR_COMPETITION_DOCUMENT");

            Database.RemoveTable("CR_COMPETITION_LOT_BID");
            Database.RemoveTable("CR_COMPETITION_LOT_TW");
            Database.RemoveTable("CR_COMPETITION_LOT");
            Database.RemoveTable("CR_COMPETITION");
        }
    }
}