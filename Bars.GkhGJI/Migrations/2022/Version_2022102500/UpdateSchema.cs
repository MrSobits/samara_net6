namespace Bars.GkhGji.Migrations._2022.Version_2022102500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022102500")]
    [MigrationDependsOn(typeof(Version_2022102400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_DICT_CON_ACTIVITY", new Column("ERKNM_GUID", DbType.String, 50));
            Database.AddColumn("GJI_DICT_CONTROL_LIST", new Column("ERKNM_GUID", DbType.String, 50));
            Database.AddColumn("GJI_DICT_CONTROL_LIST_QUESTION", new Column("ERKNM_GUID", DbType.String, 50));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_CONTROL_LIST_QUESTION", "ERKNM_GUID");
            Database.RemoveColumn("GJI_DICT_CONTROL_LIST", "ERKNM_GUID");
            Database.RemoveColumn("GJI_DICT_CON_ACTIVITY", "ERKNM_GUID");
        }
    }
}