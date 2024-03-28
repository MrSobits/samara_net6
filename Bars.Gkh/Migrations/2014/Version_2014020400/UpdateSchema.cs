namespace Bars.Gkh.Migrations.Version_2014020400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014013000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_ENTITY_LOG_LIGHT",
                new Column("ENTITY_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("CCLASS_NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("CPROP_NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("CPROP_VALUE", DbType.String, 1000, ColumnProperty.NotNull),
                new Column("CDATE_APPLIED", DbType.DateTime, ColumnProperty.NotNull),
                new Column("CDATE_END", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("FILE_ID", "G_ENT_LOG_LIGHT_FILE", "B4_FILE_INFO", "ID"),
                new Column("USED_IN_RECALC", DbType.Boolean, ColumnProperty.NotNull),
                new Column("DATE_ACTUAL", DbType.DateTime, ColumnProperty.Null),
                new Column("CCLASS_DESC", DbType.String, 1000, ColumnProperty.Null),
                new Column("CPROP_DESCR", DbType.String, 1000, ColumnProperty.Null),
                new Column("PARAM_NAME", DbType.String, 100, ColumnProperty.NotNull),
                new Column("CUSER_NAME", DbType.String, 300, ColumnProperty.Null)
                );

            Database.AddIndex("IND_ENTITY_LOG_LIGHT_PN", false, "GKH_ENTITY_LOG_LIGHT", "PARAM_NAME");
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_ENTITY_LOG_LIGHT");
        }
    }
}