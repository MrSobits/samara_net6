namespace Bars.GkhRf.Migrations.Version_2015112300
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015112300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhRf.Migrations.Version_2014061100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("RF_CONTRACT", new Column("TERMINATION_CONTRACT_NUM", DbType.String));
            Database.AddColumn("RF_CONTRACT", new Column("TERMINATION_CONTRACT_DATE", DbType.DateTime));
            Database.AddColumn("RF_CONTRACT", new RefColumn("TERMINATION_CONTRACT_FILE_ID", "RF_TERMINATION_CONTRACT_FILE_ID", "B4_FILE_INFO", "ID"));           
        }

        public override void Down()
        {
            Database.RemoveColumn("RF_TRANSFER_CTR", "TERMINATION_CONTRACT_NUM");
            Database.RemoveColumn("RF_TRANSFER_CTR", "TERMINATION_CONTRACT_DATE");
            Database.RemoveColumn("RF_TRANSFER_CTR", "TERMINATION_CONTRACT_FILE_ID");
        }
    }
}