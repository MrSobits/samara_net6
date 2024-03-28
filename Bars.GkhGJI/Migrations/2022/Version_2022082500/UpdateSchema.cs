namespace Bars.GkhGji.Migrations._2022.Version_2022082500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022082500")]
    [MigrationDependsOn(typeof(Version_2022082200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable(
                    "GJI_DECISION_VERIFSUBJ_NORM_DOC",
                    new RefColumn("DECISION_VERIFSUBJ_ID", "GJI_DECISION_VERIFSUBJ_NORM_DOC_VERIFSUBJ_ID", "GJI_DECISION_VERIFSUBJ", "ID"),
                    new RefColumn("DOC_ITEM_ID", "GJI_DECISION_VERIFSUBJ_NORM_DOC_ITEM_ID", "GKH_DICT_NORMATIVE_DOC_ITEM", "ID"));

            Database.AddColumn("GJI_DICT_PROSECUTOR_OFFICE", new Column("ERKNM_CODE", DbType.String, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_PROSECUTOR_OFFICE", "ERKNM_CODE");
            Database.RemoveTable("GJI_DECISION_VERIFSUBJ_NORM_DOC");
        }
    }
}