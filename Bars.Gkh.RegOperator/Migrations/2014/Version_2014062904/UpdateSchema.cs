namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062904
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062904")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062903.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_ACC_STMNT", new Column("P_ACC_NUM", DbType.String, 100));

            Database.AddEntityTable("REGOP_TR_ACC_CRED",
                new Column("CR_ORG", DbType.String, 200),
                new Column("D_DATE", DbType.DateTime),
                new Column("CALC_ACC", DbType.String, 100),
                new Column("D_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("CONF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("DIVERGENCE", DbType.Decimal, ColumnProperty.NotNull, 0)
                );


            Database.AddEntityTable("REGOP_TR_ACC_DEB",
                new Column("ACC_NUM", DbType.String, 100),
                new Column("D_DATE", DbType.DateTime),
                new Column("P_AGENT_NAME", DbType.String, 200),
                new Column("P_AGENT_CODE", DbType.String, 200),
                new Column("D_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("CONF_SUM", DbType.Decimal, ColumnProperty.NotNull, 0),
                new Column("DIVERGENCE", DbType.Decimal, ColumnProperty.NotNull, 0)
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_TR_ACC_CRED");
            Database.RemoveTable("REGOP_TR_ACC_DEB");

            Database.RemoveColumn("REGOP_BANK_ACC_STMNT", "P_ACC_NUM");
        }
    }
}
