namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121201
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121201")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013121200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять эту миграцию

            /*Database.AddColumn("OVRHL_STRUCT_EL", new Column("CALCULATE_BY", DbType.Int32, ColumnProperty.NotNull, 0));

            Database.For<PostgreSQLDialect>().ExecuteNonQuery(@"update OVRHL_STRUCT_EL set CALCULATE_BY=10 where IS_CALC_LIVEAREA;");
            Database.For<OracleDialect>().ExecuteNonQuery(@"update OVRHL_STRUCT_EL set CALCULATE_BY=10 where IS_CALC_LIVEAREA=1");

            Database.RemoveColumn("OVRHL_STRUCT_EL", "IS_CALC_LIVEAREA");*/
        }

        public override void Down()
        {
            /*Database.RemoveColumn("OVRHL_STRUCT_EL", "CALCULATE_BY");
            Database.AddColumn("OVRHL_STRUCT_EL", new Column("IS_CALC_LIVEAREA", DbType.Boolean, ColumnProperty.NotNull, false));*/
        }
    }
}