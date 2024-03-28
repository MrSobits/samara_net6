namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014020902.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_BANK_DOC_IMPORT",
                new Column("IMPORT_DATE", DbType.DateTime),
                new Column("DOCUMENT_TYPE", DbType.String, 300),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String, 100),
                new Column("IMPORTED_SUM", DbType.Decimal),
                new Column("STATUS", DbType.Int16),
                new RefColumn("LOG_IMPORT_ID", "REGOP_BANK_DOC_IMPORT_LOG", "GKH_LOG_IMPORT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_BANK_DOC_IMPORT");
        }
    }
}
