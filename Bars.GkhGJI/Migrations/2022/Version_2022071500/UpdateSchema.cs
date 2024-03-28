namespace Bars.GkhGji.Migrations._2022.Version_2022071500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022071500")]
    [MigrationDependsOn(typeof(Version_2022070600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("CONTROL_DATE_GIS_GKH", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "CONTROL_DATE_GIS_GKH");
        }
    }
}