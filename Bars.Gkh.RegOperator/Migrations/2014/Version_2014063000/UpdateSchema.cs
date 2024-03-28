namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014063000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014063000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062904.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_BANKACC_STMNT_GROUP",
                new Column("IMPORT_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("USER_LOGIN", DbType.String),
                new Column("SUM", DbType.Decimal)
                );

            Database.AddRefColumn("REGOP_BANK_ACC_STMNT", new RefColumn("STMNT_GROUP", "ACC_STMNT_GROUP", "REGOP_BANKACC_STMNT_GROUP", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "STMNT_GROUP");
            Database.RemoveTable("REGOP_BANKACC_STMNT_GROUP");
        }
    }
}
