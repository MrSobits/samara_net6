namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022041600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022041600")]
    [MigrationDependsOn(typeof(Version_2022041501.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Table = "GJI_DICT_TAT_RISK_CATEGORY";

        public override void Up()
        {
            this.Database.AddJoinedSubclassTable(Table,
                "GKH_DICT_RISK_CATEGORY",
                "RISK_CATEGORY_TAT",
                new Column("ERVK_GUID", DbType.String, 36));
        }

        public override void Down()
        {
            this.Database.RemoveTable(Table);
        }
    }
}