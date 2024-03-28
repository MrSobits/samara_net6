namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2018032100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;

    [Migration("2018032100")]
    [MigrationDependsOn(typeof(Version_2016061001.UpdateSchema))]
    [MigrationDependsOn(typeof(Migrations._2018.Version_2018032100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            ViewManager.Drop(this.Database, "GkhGjiTat");

            this.Database.AddJoinedSubclassTable("GJI_WARNING_INSPECTION", "GJI_INSPECTION", "GJI_WARNING_INSPECTION",
                new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NAME", DbType.String),
                new Column("DOCUMENT_NUMBER", DbType.String, 20),
                new Column("SOURCE_FORM_TYPE", DbType.Int32, ColumnProperty.NotNull, (int) TypeBase.DisposalHead),
                new Column("IS_PRELIMENTARY_CHECK", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("BASIS_ID", "GJI_WARNING_INSPECTION_BASIS", "GJI_INSPECTION_BASIS", "ID"),
                new FileColumn("FILE_ID", "GJI_WARNING_INSPECTION_FILE"));

            this.Database.AddJoinedSubclassTable("GJI_WARNING_DOC", "GJI_DOCUMENT", "GJI_WARNING_DOC",
                new Column("TAKING_DATE", DbType.DateTime),
                new Column("RESULT_TEXT", DbType.String),
                new Column("BASE_WARNING", DbType.String),
                new Column("NC_OUT_DATE", DbType.DateTime),
                new Column("NC_OUT_NUM", DbType.String),
                new Column("NC_OUT_DATE_LATTER", DbType.DateTime),
                new Column("NC_OUT_NUM_LATTER", DbType.String),
                new Column("NC_OUT_SENT", DbType.Int32, ColumnProperty.NotNull, (int) YesNo.No),
                new Column("NC_IN_DATE", DbType.DateTime),
                new Column("NC_IN_NUM", DbType.String),
                new Column("NC_IN_DATE_LATTER", DbType.DateTime),
                new Column("NC_IN_NUM_LATTER", DbType.String),
                new Column("NC_IN_RECIVED", DbType.Int32, ColumnProperty.NotNull, (int) YesNo.No),
                new RefColumn("AUTOR_ID", "GJI_WARNING_DOC_AUTOR", "GKH_DICT_INSPECTOR", "ID"),
                new RefColumn("EXECUTANT_ID", "GJI_WARNING_DOC_EXECUTANT", "GKH_DICT_INSPECTOR", "ID"),
                new FileColumn("FILE_ID", "GJI_WARNING_DOC_FILE")
            );

            this.Database.AddEntityTable("GJI_WARNING_BASIS",
                new Column("CODE", DbType.String, 255, ColumnProperty.NotNull),
                new Column("NAME", DbType.String, 1000, ColumnProperty.NotNull));
            this.Database.AddIndex("GJI_WARNING_BASIS_CODE", true, "GJI_WARNING_BASIS", "CODE");

            this.Database.AddEntityTable("GJI_WARNING_DOC_BASIS",
                new RefColumn("DOC_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_BASIS_DOC", "GJI_WARNING_DOC", "ID"),
                new RefColumn("BASIS_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_BASIS", "GJI_WARNING_BASIS", "ID"));
            this.Database.AddIndex("GJI_WARNING_DOC_BASIS_DOC", false, "GJI_WARNING_DOC_BASIS", "DOC_ID");

            this.Database.AddEntityTable("GJI_WARNING_DOC_VIOLATIONS",
                new Column("DESCRIPTION", DbType.String, 10000),
                new RefColumn("WARNING_DOC_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_VIOLATIONS_DOC", "GJI_WARNING_DOC", "ID"),
                new RefColumn("NORMATIVE_DOC_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_VIOLATIONS_NORMATIVE", "GKH_DICT_NORMATIVE_DOC", "ID"));
            this.Database.AddIndex("GJI_WARNING_DOC_VIOLATIONS_DOC", false, "GJI_WARNING_DOC_VIOLATIONS", "WARNING_DOC_ID");

            this.Database.AddEntityTable("GJI_WARNING_DOC_ANNEX",
                new Column("NAME", DbType.String),
                new Column("DESCRIPTION", DbType.String),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new RefColumn("WARNING_DOC_ID", ColumnProperty.NotNull, "GJI_WARNING_DOC_ANNEX_DOC", "GJI_WARNING_DOC", "ID"),
                new FileColumn("FILE_ID", "GJI_WARNING_DOC_ANNEX_FILE"));
            this.Database.AddIndex("GJI_WARNING_DOC_ANNEX_DOC", false, "GJI_WARNING_DOC_ANNEX", "WARNING_DOC_ID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            ViewManager.Drop(this.Database, "GkhGjiTat");
            this.Database.RemoveTable("GJI_WARNING_DOC_ANNEX");
            this.Database.RemoveTable("GJI_WARNING_DOC_VIOLATIONS");
            this.Database.RemoveTable("GJI_WARNING_DOC_BASIS");
            this.Database.RemoveTable("GJI_WARNING_BASIS");
            this.Database.RemoveTable("GJI_WARNING_DOC");
            this.Database.RemoveTable("GJI_WARNING_INSPECTION");
        }
    }
}