namespace Bars.Gkh.Migrations._2017.Version_2017080100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.TechnicalPassport.Impl;

    [Migration("2017080100")]
    [MigrationDependsOn(typeof(Version_2017072100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        { 
            this.Database.AddEntityTable(
                TechnicalPassportTransformer.TechnicalPassportTableName, 
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "TECHNICAL_PASSPORT_REALITY_OBJECT_ID", "GKH_REALITY_OBJECT", "ID"));

            this.Database.AddEntityTable(
                "TP_SECTION",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("TITLE", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("ORDER", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("PARENT_ID", ColumnProperty.NotNull, "TP_SECTION_PARENT_ID", "TP_SECTION", "ID"));

            this.Database.AddEntityTable(
                "TP_FORM",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("TITLE", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("ORDER", DbType.Int32, ColumnProperty.NotNull),
                new Column("TABLE_NAME", DbType.String, ColumnProperty.NotNull),
                new RefColumn("SECTION_ID", ColumnProperty.NotNull, "TP_FORM_SECTION_ID", "TP_SECTION", "ID"),
                new Column("TYPE", DbType.Int32, ColumnProperty.NotNull));

            this.Database.AddEntityTable(
                "TP_EDITOR",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("EDITOR_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("REFERENCE_TABLE_NAME", DbType.String),
                new Column("DISPLAY_VALUE", DbType.String),
                new Column("AVALIABLE_CONSTRAINTS", DbType.Binary));

            this.Database.AddEntityTable(
                "TP_FORM_ATTRIBUTE",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, ColumnProperty.NotNull),
                new Column("DISPLAY_TEXT", DbType.String, ColumnProperty.NotNull),
                new Column("COLUMN_NAME", DbType.String, ColumnProperty.NotNull),
                new Column("REQUIRED", DbType.Boolean, ColumnProperty.NotNull, false),
                new RefColumn("FORM_ID", ColumnProperty.NotNull, "TP_FORM_ATTRIBUTE_FORM_ID", "TP_FORM", "ID"),
                new RefColumn("EDITOR_ID", ColumnProperty.NotNull, "TP_FORM_ATTRIBUTE_EDITOR_ID", "TP_EDITOR", "ID"),
                new Column("CONTSTRAINTS", DbType.Binary));
        }

        public override void Down()
        {
            this.Database.RemoveTable("TP_FORM_ATTRIBUTE");
            this.Database.RemoveTable("TP_EDITOR");
            this.Database.RemoveTable("TP_FORM");
            this.Database.RemoveTable("TP_SECTION");
            this.Database.RemoveTable(TechnicalPassportTransformer.TechnicalPassportTableName);
        }
    }
}