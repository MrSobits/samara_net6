using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023022000
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023022000")]
    [MigrationDependsOn(typeof(_2022.Version_2022091500.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            this.Database.AddEntityTable("SBERBANK_PAYMENT_DOC",
                new RefColumn("PERIOD", "PERIOD_ID", "REGOP_PERIOD", "ID"),
                new RefColumn("ACCOUNT", "ACCOUNT_ID", "REGOP_PERS_ACC", "ID"),
                new Column("LAST_DATE", DbType.DateTime),
                new Column("COUNT", DbType.Int16),
                new Column("GUID", DbType.String),
                new RefColumn("FILE", "SBERBANK_PAYMENT_DOC_FILE_ID", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("SBERBANK_PAYMENT_DOC");
        }
    }
}