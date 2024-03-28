namespace Bars.GisIntegration.CapitalRepair.Migrations.Version_2016070300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Extensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016070300")]
    [MigrationDependsOn(typeof(Bars.GisIntegration.Base.Migrations.Version_2016062800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable("GI_CR_PLAN",
            //     new Column("NAME", DbType.String, 500),
            //     new Column("MUNICIPALITY_CODE", DbType.String, 11),
            //     new Column("MUNICIPALITY_NAME", DbType.String, 500),
            //     new Column("START_MONTH_YEAR", DbType.Date),
            //     new Column("END_MONTH_YEAR", DbType.Date));

            //this.Database.AddRisEntityTable("GI_CR_PLANWORK",
            //    new Column("PLAN_GUID", DbType.String, 50),
            //    new Column("BUILDING_FIAS_GUID", DbType.String, 50),
            //    new Column("WORK_KIND_CODE", DbType.String, 10),
            //    new Column("WORK_KIND_GUID", DbType.String, 50),
            //    new Column("END_MONTH_YEAR", DbType.Date),
            //    new Column("MUNICIPALITY_CODE", DbType.String, 11),
            //    new Column("MUNICIPALITY_NAME", DbType.String, 500));

            //this.Database.AddRisEntityTable(
            //    "GI_CR_CONTRACT",
            //    new Column("PLAN_GUID", DbType.String, 40, ColumnProperty.NotNull),
            //    new Column("NUMBER", DbType.String, 40),
            //    new Column("DATE", DbType.DateTime),
            //    new Column("START_DATE", DbType.DateTime),
            //    new Column("END_DATE", DbType.DateTime),
            //    new Column("SUM", DbType.Decimal),
            //    new Column("CUSTOMER", DbType.String, 100, ColumnProperty.NotNull),
            //    new Column("PERFORMER", DbType.String, 100, ColumnProperty.NotNull),
            //    new Column("OUTLAY_MISSING", DbType.Boolean)
            //    );

            //this.Database.AddRisEntityTable(
            //    "GI_CR_ATTACHMENT_OUTLAY",
            //    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "FK_GI_CR_ATTACHMENT_OUTLAY_C", "GI_CR_CONTRACT", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "FK_GI_CR_ATTACHMENT_OUTLAY_A", "GI_ATTACHMENT", "ID")
            //    );

            //this.Database.AddRisEntityTable(
            //    "GI_CR_ATTACHMENT_CONTRACT",
            //    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "FK_GI_CR_ATTACHMENT_CONTRACT_C", "GI_CR_CONTRACT", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "FK_GI_CR_ATTACHMENT_CONTRACT_A", "GI_ATTACHMENT", "ID")
            //    );

            //this.Database.AddRisEntityTable(
            //    "GI_CR_WORK",
            //    new Column("PLANWORK_GUID", DbType.String, 50),
            //    new Column("BUILDING_FIAS_GUID", DbType.String, 50),
            //    new Column("WORK_KIND_CODE", DbType.String, 10),
            //    new Column("WORK_KIND_GUID", DbType.String, 50),
            //    new Column("END_MONTH_YEAR", DbType.String, 50),
            //    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "FK_GI_CR_WORK_CONTRACT", "GI_CR_CONTRACT", "ID"),
            //    new Column("START_DATE", DbType.Date),
            //    new Column("END_DATE", DbType.Date),
            //    new Column("COST", DbType.Decimal),
            //    new Column("COST_PLAN", DbType.Decimal),
            //    new Column("VOLUME", DbType.Decimal),
            //    new Column("OTHER_UNIT", DbType.String, 50),
            //    new Column("ADDITIONAL_INFO", DbType.String, 250),
            //    new Column("OKEI", DbType.String, 50)
            //    );
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_CR_WORK");

            //this.Database.RemoveTable("RIS_CR_ATTACHMENT_CONTRACT");

            //this.Database.RemoveTable("RIS_CR_ATTACHMENT_OUTLAY");

            //this.Database.RemoveTable("RIS_CR_CONTRACT");

            //this.Database.RemoveTable("GI_CR_PLANWORK");
            //this.Database.RemoveTable("GI_CR_PLAN");
        }
    }
}
