using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2022.Version_2022112200
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022112200")]
    [MigrationDependsOn(typeof(_2022.Version_2022092800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            this.Database.RemoveTable("AGENT_PIR_DOCUMENT");

            Database.AddEntityTable("AGENT_PIR_DOCUMENT",
                   new Column("AP_DOC_NUMBER", DbType.String),
                   new Column("AP_DOC_DATE", DbType.DateTime),
                   new Column("AP_DOC_DEBT_SUM", DbType.Decimal, ColumnProperty.NotNull),
                   new Column("AP_DOC_PENI_SUM", DbType.Decimal, ColumnProperty.NotNull),
                   new Column("AP_DOC_TYPE", DbType.Int32),
                   new RefColumn("AP_DOC_FILE_INFO_ID", "AP_DOC_FILE_INFO", "B4_FILE_INFO", "ID"),
                   new RefColumn("AP_DOC_DEBTOR_ID", "AP_DOC_AP_DEBTOR", "AGENT_PIR_DEBTOR", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("AGENT_PIR_DOCUMENT");            
        }
    }
}