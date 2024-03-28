namespace Bars.Gkh.Migration.Version_2015050800
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015050800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015050600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CLW_RESTR_DEBT_AMIC_AGR",
                new Column("DOCUMENT", DbType.String, 200),
                new Column("DOC_NUM", DbType.String, 100),
                new Column("DOC_DATE", DbType.DateTime),
                new Column("SUM_PERCENT", DbType.Decimal),
                new RefColumn("LAWSUIT_ID", "CLW_RESTRDEBTAA_LWST", "CLW_LAWSUIT", "ID"),
                new RefColumn("DOC_FILE_ID", "CLW_RESTRDEBTAA_DOCF", "B4_FILE_INFO", "ID"),
                new RefColumn("SCHEDULER_FILE_ID", "CLW_RESTRDEBTAA_SCHF", "B4_FILE_INFO", "ID"));

            Database.AddEntityTable("CLW_RESTR_DEBT_AMIC_AGR_SCHED",
                new Column("PAYM_DEADLINE", DbType.DateTime),
                new Column("SUM", DbType.Decimal),
                new RefColumn("RECTR_DEBT_AMIC_AGR_ID", "CLW_RESTRDEBTAA_SCHD", "CLW_RESTR_DEBT_AMIC_AGR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_RESTR_DEBT_AMIC_AGR_SCHED");

            Database.RemoveTable("CLW_RESTR_DEBT_AMIC_AGR");
        }
    }
}