namespace Bars.Gkh.Overhaul.Nso.Migration.Version_2013122001
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013122001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Nso.Migration.Version_2013122000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_CONTRIBUT_COLLECT",
                new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_CON_COL_OBJ", "OVRHL_LONGTERM_PR_OBJECT", "ID"),
                new Column("COL_DATE", DbType.DateTime),
                new Column("PERSONAL_ACCOUNT", DbType.String),
                new Column("AREA_OWNER_ACCOUNT", DbType.Decimal)
               );
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_CONTRIBUT_COLLECT");
        }
    }
}