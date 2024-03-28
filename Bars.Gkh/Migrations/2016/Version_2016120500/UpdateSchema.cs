namespace Bars.Gkh.Migrations._2016.Version_2016120500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016120500
    /// </summary>
    [Migration("2016120500")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2016.Version_2016111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_EF_ANALITICS_GRAPH",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("LEVEL", DbType.Int32, ColumnProperty.NotNull),
                new Column("CATEGORY", DbType.Int32, ColumnProperty.NotNull),
                new Column("TYPE_DIAGRAM", DbType.Int32, ColumnProperty.NotNull),
                new Column("VIEW_PARAM", DbType.Int32, ColumnProperty.NotNull),
                new Column("FACTOR_CODE", DbType.String, ColumnProperty.Null),
                new Column("DATA", DbType.Binary, ColumnProperty.Null));

            this.Database.AddPersistentObjectTable(
                "GKH_EF_ANALITICS_MU_REL",
                new RefColumn("ANALITICS_ID", "EF_ANALITICS_MU_REL_ANALITICS_ID", "GKH_EF_ANALITICS_GRAPH", "ID"),
                new RefColumn("MUNICIPALITY_ID", "EF_ANALITICS_MU_REL_MUN_ID", "GKH_DICT_MUNICIPALITY", "ID"));

            this.Database.AddPersistentObjectTable(
                "GKH_EF_ANALITICS_PERIOD_REL",
                new RefColumn("ANALITICS_ID", "EF_ANALITICS_PERIOD_REL_ANALITICS_ID", "GKH_EF_ANALITICS_GRAPH", "ID"),
                new RefColumn("PERIOD_ID", "EF_ANALITICS_PERIOD_REL_PERIOD_ID", "GKH_DICT_EF_PERIOD", "ID"));

            this.Database.AddPersistentObjectTable(
                "GKH_EF_ANALITICS_MANORG_REL",
                new RefColumn("ANALITICS_ID", "EF_ANALITICS_MANORG_REL_ANALITICS_ID", "GKH_EF_ANALITICS_GRAPH", "ID"),
                new RefColumn("MANORG_ID", "EF_ANALITICS_MANORG_REL_MANORG_ID", "GKH_MANAGING_ORGANIZATION", "ID"));

            this.Database.AddEntityTable("GKH_PUB_SERV_ORG_CONTR_REAL_OBJ",
                new RefColumn("RSO_CONTR_ID", ColumnProperty.NotNull, "GKH_PUB_SERV_ORG_CONTR_REAL_OBJ_RSO_CONTR_ID", "GKH_RO_PUB_SERVORG", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_PUB_SERV_ORG_CONTR_REAL_OBJ_RSO_RO_ID", "GKH_REALITY_OBJECT", "ID"));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("GKH_EF_ANALITICS_MANORG_REL");
            this.Database.RemoveTable("GKH_EF_ANALITICS_PERIOD_REL");
            this.Database.RemoveTable("GKH_EF_ANALITICS_MU_REL");
            this.Database.RemoveTable("GKH_EF_ANALITICS_GRAPH");

            this.Database.RemoveTable("GKH_PUB_SERV_ORG_CONTR_REAL_OBJ");
        }
    }
}