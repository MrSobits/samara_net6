namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019051300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019051300")]
    [MigrationDependsOn(typeof(Version_2019031500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_EMAIL_LIST", new Column("EXECUTOR", DbType.String, 500));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_EMAIL_LIST", "EXECUTOR");
        }
    }
}