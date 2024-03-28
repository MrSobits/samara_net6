namespace Bars.GkhGji.Migrations._2017.Version_2017080800
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Enums;

    [Migration("2017080800")]
    [MigrationDependsOn(typeof(Version_2017080200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_DISPOSAL", new Column("DOCUMENT_NUMBER_WITH_RESULT_AGREEMENT", DbType.String.WithSize(20)));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("DOCUMENT_DATE_WITH_RESULT_AGREEMENT", DbType.DateTime, ColumnProperty.Null));

            this.Database.AddColumn("GJI_INSPECTION", new Column("REASON_ERP_CHECKING", DbType.Int32, ColumnProperty.NotNull, (int)ReasonErpChecking.NotSet));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DISPOSAL", "DOCUMENT_NUMBER_WITH_RESULT_AGREEMENT");
            this.Database.RemoveColumn("GJI_DISPOSAL", "DOCUMENT_DATE_WITH_RESULT_AGREEMENT");

            this.Database.RemoveColumn("GJI_INSPECTION", "REASON_ERP_CHECKING");
        }
    }
}