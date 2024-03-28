namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015012200
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015020300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015012701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("DISTR_PENALTY", DbType.Int32, ColumnProperty.NotNull, 20));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "DISTR_PENALTY");
        }
    }
}
