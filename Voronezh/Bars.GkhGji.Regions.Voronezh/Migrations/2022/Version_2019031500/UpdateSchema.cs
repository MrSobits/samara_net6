namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019031500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019031500")]
    [MigrationDependsOn(typeof(Version_2019031400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {

            Database.AddEntityTable(
             "GJI_PRELIMINARY_CHECK",
             new Column("RESULT", DbType.String, 5000),
             new Column("CHECK_DATE", DbType.DateTime, ColumnProperty.Null),
             new Column("NUMBER", DbType.String, 20),
             new RefColumn("FILE_INFO_ID", ColumnProperty.None, "FK_GJI_PRELIMINARY_CHECK_FILE", "B4_FILE_INFO", "ID"),
             new RefColumn("APPCIT_ID", ColumnProperty.NotNull, "FK_GJI_PRELIMINARY_CHECK_APPCIT", "GJI_APPEAL_CITIZENS", "ID"),
             new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "FK_GJI_PRELIMINARY_CHECK_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
             new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "FK_GJI_PRELIMINARY_CHECK_INSPECTOR", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PRELIMINARY_CHECK");
        }
    }
}