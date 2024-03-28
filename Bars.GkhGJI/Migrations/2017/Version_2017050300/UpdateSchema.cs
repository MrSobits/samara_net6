namespace Bars.GkhGji.Migrations._2017.Version_2017050300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017050300")]
    [MigrationDependsOn(typeof(Version_2017042600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_INSPECTION", "REGISTR_NUMBER", DbType.String);
            this.Database.AddColumn("GJI_INSPECTION", "REGISTR_NUMBER_DATE", DbType.DateTime);
            this.Database.AddColumn("GJI_INSPECTION", "CHECK_DAY_COUNT", DbType.Int32);
            this.Database.AddColumn("GJI_INSPECTION", "CHECK_DATE", DbType.DateTime);

            this.Database.AddEntityTable(
                "GJI_INSPECTION_BASE_CONTRAGENT",
                new RefColumn("INSPECTION_ID", ColumnProperty.NotNull, "GJI_INSPECTION_BASE_CONTRAGENT_INSPECTION", "GJI_INSPECTION", "ID"),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "GJI_INSPECTION_BASE_CONTRAGENT_CONTRAGENT", "GKH_CONTRAGENT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION", "REGISTR_NUMBER");
            this.Database.RemoveColumn("GJI_INSPECTION", "REGISTR_NUMBER_DATE");
            this.Database.RemoveColumn("GJI_INSPECTION", "CHECK_DAY_COUNT");
            this.Database.RemoveColumn("GJI_INSPECTION", "CHECK_DATE");

            this.Database.RemoveTable("GJI_INSPECTION_BASE_CONTRAGENT");
        }
    }
}