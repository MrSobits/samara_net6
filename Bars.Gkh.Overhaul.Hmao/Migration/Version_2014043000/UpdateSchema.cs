namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2014043000
{
    using System.Data;

    using Bars.Gkh.Utils;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014043000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2014041700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_PRG_VERSION", new Column("ACTUALIZE_DATE", DbType.DateTime));
            Database.ExecuteNonQuery("UPDATE OVRHL_PRG_VERSION SET ACTUALIZE_DATE = VERSION_DATE");
            Database.AlterColumnSetNullable("OVRHL_PRG_VERSION", "ACTUALIZE_DATE", false);

            Database.AddColumn("OVRHL_VERSION_REC", new Column("IS_MANUALLY_CORRECT", DbType.Boolean, ColumnProperty.NotNull, false));

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery("UPDATE OVRHL_VERSION_REC SET IS_MANUALLY_CORRECT = TRUE WHERE OBJECT_EDIT_DATE > OBJECT_CREATE_DATE");
            }
            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery("UPDATE OVRHL_VERSION_REC SET IS_MANUALLY_CORRECT = 1 WHERE OBJECT_EDIT_DATE > OBJECT_CREATE_DATE");
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_VERSION_REC", "IS_MANUALLY_CORRECT");
            Database.RemoveColumn("OVRHL_PRG_VERSION", "ACTUALIZE_DATE");
        }
    }
}