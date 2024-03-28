namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014012501
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014012500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERIOD", new Column("PERIOD_NAME", DbType.String, 200, ColumnProperty.NotNull));

            Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("BIRTH_DATE", DbType.DateTime, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("ID_TYPE", DbType.Int64, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("ID_NUM", DbType.String, 200, ColumnProperty.NotNull));
            Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("ID_SERIAL", DbType.String, 200, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERIOD", "PERIOD_NAME");

            Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "BIRTH_DATE");
            Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "ID_TYPE");
            Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "ID_NUM");
            Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "ID_SERIAL");
        }
    }
}
