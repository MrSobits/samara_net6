namespace Bars.GkhGji.Migrations._2023.Version_2023080200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023080200")]
    [MigrationDependsOn(typeof(_2023.Version_2023071000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PREVENTIVE_VISIT", new Column("PHYSICAL_PERSON_INN", DbType.String, 15));
            Database.AddColumn("GJI_PREVENTIVE_VISIT", new Column("ACCESSGUID", DbType.String, 100));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PREVENTIVE_VISIT", "ACCESSGUID");
            Database.RemoveColumn("GJI_PREVENTIVE_VISIT", "PHYSICAL_PERSON_INN");
        }
    }
}