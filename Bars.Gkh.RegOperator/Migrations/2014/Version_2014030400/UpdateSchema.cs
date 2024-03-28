namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014030400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014030400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014030301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERS_ACC_CHANGE",
                new Column("DESCRIPTION", DbType.String, 1000, ColumnProperty.Null),
                new Column("OPERATOR", DbType.String, 100, ColumnProperty.Null),
                new Column("CHANGE_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("DATE", DbType.DateTime, ColumnProperty.NotNull),
                new RefColumn("ACC_ID", ColumnProperty.NotNull, "REGOP_PERS_ACC_CH_PA", "REGOP_PERS_ACC", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERS_ACC_CHANGE");
        }
    }
}
