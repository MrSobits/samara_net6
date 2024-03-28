namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018042301
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using Bars.Gkh.Utils;

    [Migration("2018042301")]
    [MigrationDependsOn(typeof(Version_2018032100.UpdateSchema))]
    [MigrationDependsOn(typeof(Migrations._2018.Version_2018042300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhGjiTat");

            // Исправление предыдущей миграции
            this.Database.RemoveIndex("GJI_WARNING_DOC_ANNEX_DOC", "GJI_WARNING_DOC_VIOLATIONS");
            this.Database.AddIndex("GJI_WARNING_DOC_ANNEX_DOC", false, "GJI_WARNING_DOC_ANNEX", "WARNING_DOC_ID");

            this.Database.AddColumn("GJI_WARNING_DOC_VIOLATIONS",
                new RefColumn("REALITY_OBJECT_ID", "GJI_WARNING_DOC_VIOLATIONS_REALITY_OBJECT", "GKH_REALITY_OBJECT", "ID"));

            this.Database.RemoveColumn("GJI_WARNING_INSPECTION", "IS_PRELIMENTARY_CHECK");

            this.Database.AddEntityTable("GJI_WARNING_DOC_VIOLATIONS_DETAIL",
                new RefColumn("WARNING_DOC_VIOL_ID",
                    ColumnProperty.NotNull,
                    "GJI_WARNING_DOC_VIOLATIONS_DETAIL_WARNING",
                    "GJI_WARNING_DOC_VIOLATIONS",
                    "ID"),
                new RefColumn("VIOLATION_ID",
                    ColumnProperty.NotNull,
                    "GJI_WARNING_DOC_VIOLATIONS_DETAIL_VIOLATION",
                    "GJI_DICT_VIOLATION",
                    "ID"));

            this.Database.AddJoinedSubclassTable("GJI_MOTIVATION_CONCLUSION", "GJI_DOCUMENT", "GJI_MOTIVATION_CONCLUSION",
                new RefColumn("BASE_DOC_ID", "GJI_MOTIVATION_CONCLUSION_BASE", "GJI_DOCUMENT", "ID"),
                new RefColumn("AUTOR_ID", "GJI_MOTIVATION_CONCLUSION_AUTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("EXECUTANT_ID", "GJI_MOTIVATION_CONCLUSION_EXECUTANT", "GKH_DICT_INSPECTOR", "ID")
            );

            this.Database.AddEntityTable("GJI_MOTIVATION_CONCLUSION_ANNEX",
                new Column("NAME", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new RefColumn("DOC_ID", ColumnProperty.NotNull, "GJI_MOTIVATION_CONCLUSION_DOC", "GJI_MOTIVATION_CONCLUSION", "ID"),
                new FileColumn("FILE_ID", "GJI_MOTIVATION_CONCLUSION_ANNEX_FILE"));
            this.Database.AddIndex("GJI_MOTIVATION_CONCLUSION_ANNEX_DOC", false, "GJI_MOTIVATION_CONCLUSION_ANNEX", "DOC_ID");

            ViewManager.Create(this.Database, "GkhGjiTat");
        }

        /// <inheritdoc />
        public override void Down()
        {
            ViewManager.Drop(this.Database, "GkhGjiTat");

            this.Database.RemoveColumn("GJI_WARNING_DOC_VIOLATIONS", "REALITY_OBJECT_ID");

            this.Database.AddColumn("GJI_WARNING_INSPECTION",
                new Column("IS_PRELIMENTARY_CHECK", DbType.Boolean, ColumnProperty.NotNull, false));

            this.Database.RemoveTable("GJI_WARNING_DOC_VIOLATIONS_DETAIL");
            this.Database.RemoveTable("GJI_MOTIVATION_CONCLUSION_ANNEX");
            this.Database.RemoveTable("GJI_MOTIVATION_CONCLUSION");
        }
    }
}