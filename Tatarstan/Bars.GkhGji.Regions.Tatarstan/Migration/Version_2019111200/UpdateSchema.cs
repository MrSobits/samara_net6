namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019111200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Migrations;
    using System.Data;

    [Migration("2019111200")]
    [MigrationDependsOn(typeof(Version_2019111103.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            // Изменение столбца guid на tor_id таблицы Обязательных требований
            this.Database.RenameColumn("GJI_DICT_MANDATORY_REQS", "guid", "tor_id");
            this.Database.ExecuteNonQuery("ALTER TABLE GJI_DICT_MANDATORY_REQS ALTER COLUMN tor_id type uuid using tor_id::uuid");
        }

        public override void Down()
        {
            this.Database.RenameColumn("GJI_DICT_MANDATORY_REQS", "tor_id", "guid");
            this.Database.ChangeColumn("GJI_DICT_MANDATORY_REQS", new Column("guid", DbType.String, 300));
        }
    }
}
