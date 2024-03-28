namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015093001
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015093000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("PAD_STATE", DbType.Int32, ColumnProperty.NotNull, 10));
            Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("PC_STATE", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "PAD_STATE");
            Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "PC_STATE");
        }
    }
}
