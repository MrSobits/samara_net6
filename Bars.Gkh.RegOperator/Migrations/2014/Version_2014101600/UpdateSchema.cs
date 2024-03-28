namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014101600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014101500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_TRANSFER", new Column("TARGET_COEF", DbType.Int16, ColumnProperty.NotNull, 1));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_TRANSFER", "TARGET_COEF");
        }
    }
}
