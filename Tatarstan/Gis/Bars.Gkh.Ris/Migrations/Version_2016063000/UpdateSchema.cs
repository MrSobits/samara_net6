namespace Bars.Gkh.Ris.Migrations.Version_2016063000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016063000")]
    [MigrationDependsOn(typeof(Version_2016062700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisEntityTable(
            //    "RIS_CR_CONTRACT",
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
            //    "RIS_CR_ATTACHMENT_OUTLAY",
            //    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "FK_RIS_CR_ATTACHMENT_OUTLAY_C", "RIS_CR_CONTRACT", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "FK_RIS_CR_ATTACHMENT_OUTLAY_A", "RIS_ATTACHMENT", "ID")
            //    );

            //this.Database.AddRisEntityTable(
            //    "RIS_CR_ATTACHMENT_CONTRACT",
            //    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "FK_RIS_CR_ATTACHMENT_CONTRACT_C", "RIS_CR_CONTRACT", "ID"),
            //    new RefColumn("ATTACHMENT_ID", ColumnProperty.NotNull, "FK_RIS_CR_ATTACHMENT_CONTRACT_A", "RIS_ATTACHMENT", "ID")
            //    );

            //this.Database.AddRisEntityTable(
            //    "RIS_CR_WORK",
            //    new Column("PLANWORK_GUID", DbType.String, 50),
            //    new Column("BUILDING_FIAS_GUID", DbType.String, 50),
            //    new Column("WORK_KIND_CODE", DbType.String, 10),
            //    new Column("WORK_KIND_GUID", DbType.String, 50),
            //    new Column("END_MONTH_YEAR", DbType.String, 50),
            //    new RefColumn("CONTRACT_ID", ColumnProperty.NotNull, "FK_RIS_CR_WORK_CONTRACT", "RIS_CR_CONTRACT", "ID"),
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
        }
    }
}
