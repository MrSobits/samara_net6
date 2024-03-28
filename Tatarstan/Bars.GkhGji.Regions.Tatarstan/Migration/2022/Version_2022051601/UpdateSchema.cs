namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022051601
{

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022051601")]
    [MigrationDependsOn(typeof(Version_2022051300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Table = "GJI_TAT_DISPOSAL_ANNEX";

        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(Table,
                "GJI_DISPOSAL_ANNEX",
                "DISPOSAL_ANNEX_TAT_DISPOSAL_ANNEX",
                new RefColumn("ERKNM_TYPE_DOCUMENT_ID", "DISPOSAL_ANNEX_ERKNM_TYPE_DOCUMENT", "GJI_DICT_ERKNM_TYPE_DOCUMENT", "ID"));

            this.Database.ExecuteNonQuery($"INSERT INTO {Table} (id) SELECT id FROM GJI_DISPOSAL_ANNEX");
        }

        public override void Down()
        {
            this.Database.RemoveTable(Table);
        }
    }
}
