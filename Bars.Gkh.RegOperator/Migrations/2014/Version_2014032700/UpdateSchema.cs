namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014032700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014032700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014032500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("REGOP_PERS_ACC_OWNER", new Column("NAME", DbType.String, 500, ColumnProperty.Null));
        }

        public override void Down()
        {
            
        }
    }
}
