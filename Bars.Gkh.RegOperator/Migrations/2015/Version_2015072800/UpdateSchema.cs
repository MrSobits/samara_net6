namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015072800
{

    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015072500.UpdateSchema))]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("RF_TRANSFER_CTR", new Column("PAYMENT_PRIORITY", DbType.String, 250, ColumnProperty.Null, "5"));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("IS_EDIT_PURPOSE", DbType.Boolean, ColumnProperty.NotNull, true));
            Database.AddColumn("RF_TRANSFER_CTR", new Column("TYPE_CALC_NDS", DbType.Int32, ColumnProperty.NotNull, 0));
            Database.AddRefColumn("RF_TRANSFER_CTR", new RefColumn("DOC_FILE_ID", "RF_TRANS_DOC_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_TRANSFER_CTR", "DOC_FILE_ID");
            Database.RemoveColumn("RF_TRANSFER_CTR", "PAYMENT_PRIORITY");
            Database.RemoveColumn("RF_TRANSFER_CTR", "IS_EDIT_PURPOSE");
            Database.RemoveColumn("RF_TRANSFER_CTR", "TYPE_CALC_NDS");
        }
    }
}
