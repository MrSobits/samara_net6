namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014020900
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014020402.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PAYMENT_PENALTIES",
                new Column("DAYS", DbType.Int16),
                new Column("PERCENTAGE", DbType.Decimal),
                new Column("DATE_START", DbType.DateTime),
                new Column("DATE_END", DbType.DateTime)
                );
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PAYMENT_PENALTIES");
        }
    }
}
