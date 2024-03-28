namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2014011600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014011600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2013122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Перенесено в базовый модуль Overhaul, не удалять миграцию

            /*Database.AddColumn("OVRHL_COMMON_ESTATE_OBJECT", new Column("IS_MAIN", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.AddColumn("OVRHL_STRUCT_EL", new Column("CALCULATE_BY", DbType.Int32, ColumnProperty.NotNull, 0));

            Database.For<PostgreSQLDialect>().ExecuteNonQuery(@"update OVRHL_STRUCT_EL set CALCULATE_BY=10 where IS_CALC_LIVEAREA;");
            Database.For<OracleDialect>().ExecuteNonQuery(@"update OVRHL_STRUCT_EL set CALCULATE_BY=10 where IS_CALC_LIVEAREA=1");

            Database.RemoveColumn("OVRHL_STRUCT_EL", "IS_CALC_LIVEAREA");

            Database.AddColumn("OVRHL_STRUCT_EL_GROUP", new Column("USE_IN_CALC", DbType.Boolean, ColumnProperty.NotNull, true));*/
        }

        public override void Down()
        {
            /*Database.AddColumn("OVRHL_STRUCT_EL", new Column("IS_CALC_LIVEAREA", DbType.Boolean, ColumnProperty.NotNull, false));

            Database.For<PostgreSQLDialect>().ExecuteNonQuery(@"update OVRHL_STRUCT_EL set IS_CALC_LIVEAREA = true  where CALCULATE_BY <> 0;");
            Database.For<OracleDialect>().ExecuteNonQuery(@"update OVRHL_STRUCT_EL set IS_CALC_LIVEAREA = 1 where CALCULATE_BY <> 0");

            Database.RemoveColumn("OVRHL_STRUCT_EL", "CALCULATE_BY");
            Database.RemoveColumn("OVRHL_COMMON_ESTATE_OBJECT", "IS_MAIN");

            Database.RemoveColumn("OVRHL_STRUCT_EL_GROUP", "USE_IN_CALC");*/
        }
    }
}