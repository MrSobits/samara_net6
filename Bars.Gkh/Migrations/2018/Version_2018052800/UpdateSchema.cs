namespace Bars.Gkh.Migrations._2018.Version_2018052800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018052800")]
    [MigrationDependsOn(typeof(Version_2018052300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        string tableCourtPretension = "CLW_LAWSUIT_COURT";
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("CB_SSP_DATE", DbType.DateTime));
            Database.AddColumn(tableCourtPretension, new Column("PretensionType", DbType.Int32));
            Database.AddColumn(tableCourtPretension, new Column("PretensionReciever", DbType.String));
            Database.AddColumn(tableCourtPretension, new Column("PretensionDate", DbType.DateTime));
            Database.AddColumn(tableCourtPretension, new Column("PretensionResult", DbType.String));
            Database.AddColumn(tableCourtPretension, new Column("PretensionReviewDate", DbType.DateTime));
            Database.AddColumn(tableCourtPretension, new Column("PretensionNote", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "CB_SSP_DATE");
            Database.RemoveColumn(tableCourtPretension, "PretensionType");
            Database.RemoveColumn(tableCourtPretension, "PretensionReciever");
            Database.RemoveColumn(tableCourtPretension, "PretensionDate");
            Database.RemoveColumn(tableCourtPretension, "PretensionResult");
            Database.RemoveColumn(tableCourtPretension, "PretensionReviewDate");
            Database.RemoveColumn(tableCourtPretension, "PretensionNote");
        }
    }
}