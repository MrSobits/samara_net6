namespace Bars.GkhGji.Migrations._2022.Version_2022102600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022102600")]
    [MigrationDependsOn(typeof(Version_2022102500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            //-----Определения постановления
            Database.AddEntityTable(
                "GJI_APPCIT_DEFINITION",
                new RefColumn("FILE_ID", "GJI_APPCIT_DEFINITION_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("APPEAL_ID", "GJI_APPCIT_DEFINITION_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
                new RefColumn("ISSUED_DEFINITION_ID", "GJI_APPCIT_DEFINITION_ISD", "GKH_DICT_INSPECTOR", "ID"),
                new Column("DOC_NUMBER", DbType.Int32),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 5));
            //-----
            this.Database.AddEntityTable(
            "GJI_APPCIT_DEFINITION_LT",
            new RefColumn("DEFINITION_ID", ColumnProperty.NotNull, "GJI_APPCIT_DEFINITION_LT_AD", "GJI_APPCIT_DEFINITION", "ID"),
            new Column("DECIDED", DbType.Binary),
            new Column("ESTABLISHED", DbType.Binary));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_APPCIT_DEFINITION_LT");
            this.Database.RemoveTable("GJI_APPCIT_DEFINITION");
        }
    }
}