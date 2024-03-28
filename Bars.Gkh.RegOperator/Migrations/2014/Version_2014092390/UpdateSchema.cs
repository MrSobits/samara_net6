namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014092390
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014092390")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014092389.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERS_ACC_NUM_VALUE",
                new Column("HVALUE", DbType.Int32, ColumnProperty.NotNull));

            /*Database.For<PostgreSQLDialect>()
                .ExecuteNonQuery(
                "insert into REGOP_PERS_ACC_NUM_VALUE" +
                " (object_version, object_create_date, object_edit_date, hvalue)" +
                " values " +
                "(0, now(), now(), (select max(substring(acc_num from 3)::integer) as mx from regop_pers_acc where length(acc_num)=9))");

            Database.For<OracleDialect>()
                .ExecuteNonQuery(
                "insert into REGOP_PERS_ACC_NUM_VALUE" +
                " (object_version, object_create_date, object_edit_date, hvalue)" +
                " values " +
                "(0, now(), now(), (select max(substring(acc_num from 3)::integer) as mx from regop_pers_acc where char_length(acc_num)=9))");*/
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_NUM_VALUE");
        }
    }
}
