namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014110600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014110600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014110300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("OVERHAUL_PAYMENT", DbType.Decimal));
            Database.AddColumn("REGOP_PERS_ACC_PERIOD_SUMM", new Column("RECRUITMENT_PAYMENT", DbType.Decimal));
            Database.RemoveColumn("REGOP_PERS_ACC", "SERV_TYPE_ID");
            Database.RemoveTable("REGOP_PERSACC_SERVTYPE");
            Database.AddColumn("REGOP_PERS_ACC", new Column("PERSACC_SERV_TYPE", DbType.Int16, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "PERSACC_SERV_TYPE");
            Database.AddEntityTable("REGOP_PERSACC_SERVTYPE",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String));
            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("SERV_TYPE_ID", "PERS_ACC_SERV_TYPE", "REGOP_PERSACC_SERVTYPE", "ID"));
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "RECRUITMENT_PAYMENT");
            Database.RemoveColumn("REGOP_PERS_ACC_PERIOD_SUMM", "OVERHAUL_PAYMENT");
        }
    }
}
