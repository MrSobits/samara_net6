namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013101602
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013101602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013101601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_PROP_OWN_DECISION_BASE",
                new RefColumn("OBJECT_ID", "OVRHL_BASEPROPOWNDEC_OBJ", "OVRHL_LONGTERM_PR_OBJECT", "ID"));

            Database.AddRefColumn("OVRHL_PROP_OWN_DECISION",
                new RefColumn("ACCOUNT_ID", "OVRHL_PROPOWNDEC_ACCOUNT", "OVRHL_ACCOUNT", "ID"));

            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "ACCOUNT_NUMBER");
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "ACCOUNT_CREATE_DATE");
            
            // перенесен в модуль overhaul
            //Database.AddColumn("OVRHL_ACCOUNT", new Column("ACCOUNT_TYPE", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION_BASE", "OBJECT_ID");
            Database.RemoveColumn("OVRHL_PROP_OWN_DECISION", "ACCOUNT_ID");
            Database.AddColumn("OVRHL_PROP_OWN_DECISION", new Column("ACCOUNT_NUMBER", DbType.String));
            Database.AddColumn("OVRHL_PROP_OWN_DECISION", new Column("ACCOUNT_CREATE_DATE", DbType.DateTime));
            //Database.RemoveColumn("OVRHL_ACCOUNT", "ACCOUNT_TYPE");
        }
    }
}
