namespace Bars.GkhGji.Migrations.Version_2013061400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013061400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013061100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveTable("GJI_STATEM_SOURS");
            Database.RemoveTable("GJI_STATEMENT_STATSUBJECT");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "STATEMENT_DATE");
            Database.RemoveColumn("GJI_INSPECTION_STATEMENT", "CHECK_TIME");
        }

        public override void Down()
        {
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("STATEMENT_DATE", DbType.DateTime));
            Database.AddColumn("GJI_INSPECTION_STATEMENT", new Column("CHECK_TIME", DbType.DateTime));

            Database.AddEntityTable(
        "GJI_STATEM_SOURS",
        new Column("EXTERNAL_ID", DbType.String, 36),
        new Column("REVENUE_DATE", DbType.DateTime),
        new Column("REVENUE_SOURCE_NUMBER", DbType.String, 50),
        new Column("REVENUE_SOURCE_ID", DbType.Int64, 22),
        new Column("REVENUE_FORM_ID", DbType.Int64, 22),
        new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull));
            Database.AddIndex("IND_GJI_STAT_SRC", false, "GJI_STATEM_SOURS", "REVENUE_SOURCE_ID");
            Database.AddIndex("IND_GJI_STAT_FORM", false, "GJI_STATEM_SOURS", "REVENUE_FORM_ID");
            Database.AddIndex("IND_GJI_STAT_INS", false, "GJI_STATEM_SOURS", "INSPECTION_ID");
            Database.AddForeignKey("FK_GJI_STAT_INS", "GJI_STATEM_SOURS", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_FORM", "GJI_STATEM_SOURS", "REVENUE_FORM_ID", "GJI_DICT_REVENUEFORM", "ID");
            Database.AddForeignKey("FK_GJI_STAT_SRC", "GJI_STATEM_SOURS", "REVENUE_SOURCE_ID", "GJI_DICT_REVENUESOURCE", "ID");

            //-----Тематика обращения проверки по обращениям граждан
            Database.AddEntityTable(
                "GJI_STATEMENT_STATSUBJECT",
                new Column("INSPECTION_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STATEMENT_SUBJECT_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("EXTERNAL_ID", DbType.String, 36));
            Database.AddIndex("IND_GJI_STAT_SUBJ_INS", false, "GJI_STATEMENT_STATSUBJECT", "INSPECTION_ID");
            Database.AddIndex("IND_GJI_STAT_SUBJ_SUBJ", false, "GJI_STATEMENT_STATSUBJECT", "STATEMENT_SUBJECT_ID");
            Database.AddForeignKey("FK_GJI_STAT_SUBJ_INS", "GJI_STATEMENT_STATSUBJECT", "INSPECTION_ID", "GJI_INSPECTION", "ID");
            Database.AddForeignKey("FK_GJI_STAT_SUBJ_SUBJ", "GJI_STATEMENT_STATSUBJECT", "STATEMENT_SUBJECT_ID", "GJI_DICT_STATEMENT_SUBJ", "ID");
        }
    }
}