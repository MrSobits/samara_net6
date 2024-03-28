using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Migrations.Version_2015012000
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014122500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            Database.AddEntityTable(
                    "CLW_CLAIM_WORK",
                    new RefColumn("STATE_ID", "CLW_CLAIM_WORK_ST", "B4_STATE", "ID"),
                    new Column("TYPE_BASE", DbType.Int16, ColumnProperty.NotNull, 10),
                    new Column("START_DATE", DbType.DateTime),
                    new Column("COUNT_DAYS_DELAY", DbType.Int64));
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_CLAIM_WORK");
        }
    }
}