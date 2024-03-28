namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023031400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023031400")]

    [MigrationDependsOn(typeof(_2023.Version_2023022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("CLW_LEGAL_CLAIM_WORK", new RefColumn("OPER_MANAGEMENT_ID", "OPER_MANAGEMENT", "GKH_CONTRAGENT", "ID"));
            this.Database.AddColumn("CLW_LEGAL_CLAIM_WORK", new Column("OPER_MAN_REASON", DbType.String, ColumnProperty.None));
            this.Database.AddColumn("CLW_LEGAL_CLAIM_WORK", new Column("OPER_MAN_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CLW_LEGAL_CLAIM_WORK", "OPER_MAN_DATE");
            this.Database.RemoveColumn("CLW_LEGAL_CLAIM_WORK", "OPER_MAN_REASON");
            this.Database.RemoveColumn("CLW_LEGAL_CLAIM_WORK", "OPER_MANAGEMENT_ID");
        }
    }
}
