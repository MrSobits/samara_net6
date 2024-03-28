namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022042600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;
    using System.Data;

    [Migration("2022042600")]
    [MigrationDependsOn(typeof(Version_2022042500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string KnmActionTable = "GJI_DICT_KNM_ACTION";
        private const string KnmActionKnmTypeTable = "GJI_DICT_KNM_ACTION_KNM_TYPE";
        private const string KnmActionControlTypeTable = "GJI_DICT_KNM_ACTION_CONTROL_TYPE";

        public override void Up()
        {
            this.Database.AddEntityTable(KnmActionTable,
                new Column("ACT_CHECK_ACTION_TYPE", DbType.Int32),
                new Column("ERVK_ID", DbType.String, 36));

            this.Database.AddEntityTable(KnmActionKnmTypeTable,
                new RefColumn("KNM_ACTION_ID", $"{KnmActionKnmTypeTable}_{KnmActionTable}", KnmActionTable, "ID"),
                new RefColumn("KNM_TYPE_ID", $"{KnmActionKnmTypeTable}_GJI_DICT_KNM_TYPES", "GJI_DICT_KNM_TYPES", "ID"));

            this.Database.AddEntityTable(KnmActionControlTypeTable,
                new RefColumn("KNM_ACTION_ID", $"{KnmActionControlTypeTable}_{KnmActionTable}", KnmActionTable, "ID"),
                new RefColumn("CONTROL_TYPE_ID", $"{KnmActionControlTypeTable}_GJI_DICT_CONTROL_TYPES", "GJI_DICT_CONTROL_TYPES", "ID"));
        }

        public override void Down()
        {
            var tables = new[] { KnmActionKnmTypeTable, KnmActionControlTypeTable, KnmActionTable };

            tables.ForEach(table => this.Database.RemoveTable(table));
        }
    }
}
