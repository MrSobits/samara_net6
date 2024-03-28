namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014082500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014082500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014082000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // делаю int_number = 0 где он null, чтобы можно было сделать столбец notNullable
            Database.ExecuteNonQuery(@"update regop_pers_acc set int_number = 0 where int_number is null");
            Database.ChangeColumn("REGOP_PERS_ACC", new Column("INT_NUMBER", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddColumn("REGOP_PERS_ACC", new Column("REGOP_PERS_ACC_EXTSYST", DbType.AnsiString));
        }

        public override void Down()
        {
            Database.ChangeColumn("REGOP_PERS_ACC", new Column("INT_NUMBER", DbType.Int32));
            Database.RemoveColumn("REGOP_PERS_ACC", "REGOP_PERS_ACC_EXTSYST");
        }
    }
}
