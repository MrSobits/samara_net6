namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019092300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019092300")]
    [MigrationDependsOn(typeof(Version_2019092000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddRefColumn("GJI_VR_COURT_PRACTICE", new RefColumn("INSTANCE_ID", ColumnProperty.None, "FK_GJI_VR_COURT_PRACTICE_INSTANCE_ID", "GJI_DICT_INSTANCE", "ID"));
            Database.AddColumn("GJI_VR_COURT_PRACTICE", new Column("IS_DISPUTE", DbType.Boolean));
            Database.AddColumn("GJI_VR_COURT_PRACTICE", new Column("PAUSED_COMMENT", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "PAUSED_COMMENT");
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "IS_DISPUTE");
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE", "INSTANCE_ID");
        }
    }
}