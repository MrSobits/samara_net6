namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013092501
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013092501")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013092500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенесен в Overhaul
            //Database.AddEntityTable("OVRHL_CREDIT_ORG",
            //        new RefColumn("FIAS_ID", "OVRHL_CREDIT_ORG_FIAS", "B4_FIAS_ADDRESS", "ID"),
            //        new RefColumn("PARENT_ID", "OVRHL_CREDIT_ORG_CRORG", "OVRHL_CREDIT_ORG", "ID"),
            //        new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
            //        new Column("IS_FILIAL", DbType.Boolean, ColumnProperty.NotNull, false),
            //        new Column("ADDRESS", DbType.String, 500),
            //        new Column("ADDRESS_OUT_SUBJECT", DbType.String, 500),
            //        new Column("IS_ADDRESS_OUT", DbType.Boolean, ColumnProperty.NotNull, false),
            //        new Column("INN", DbType.String, 20),
            //        new Column("KPP", DbType.String, 20),
            //        new Column("BIK", DbType.String, 20),
            //        new Column("OKPO", DbType.String, 20),
            //        new Column("CORR_ACCOUNT", DbType.String, 50)
            //    );
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_CREDIT_ORG");
        }
    }
}