using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2023.Version_2023013100
{
    [Migration("2023013100")]
    [MigrationDependsOn(typeof(_2023.Version_2023011200.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddEntityTable("AGENT_PIR_DEBTOR_CREDITED",
                    new RefColumn("AGENT_PIR_DEBTOR_ID", "AGENT_PIR_DEBTOR_CR", "AGENT_PIR_DEBTOR", "ID"),
                    new Column("CREDIT", DbType.Decimal),
                    new Column("DOC_DATE", DbType.DateTime),
                    new Column("ACTIVE_USER", DbType.String),
                    new RefColumn("CREDITED_FILE_INFO_ID", "CREDITED_FILE_INFO", "B4_FILE_INFO", "ID"));

            Database.AddColumn("AGENT_PIR_DOCUMENT", new Column("REPAID", DbType.Decimal, ColumnProperty.NotNull, 0));
            Database.AddColumn("AGENT_PIR_DOCUMENT", new Column("YES_NO", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            Database.RemoveColumn("AGENT_PIR_DOCUMENT", "YES_NO");
            Database.RemoveColumn("AGENT_PIR_DOCUMENT", "REPAID");

            Database.RemoveTable("AGENT_PIR_DEBTOR_CREDITED");
        }
    }
}