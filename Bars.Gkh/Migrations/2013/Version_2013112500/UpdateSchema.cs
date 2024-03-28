namespace Bars.Gkh.Migrations.Version_2013112500
{
    using System;
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013112500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2013111800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_IMPORT_EXPORT",
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("HAS_ERRORS", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("HAS_MESSAGES", DbType.Boolean, ColumnProperty.NotNull, false),
                new Column("IE_TYPE", DbType.Int16, ColumnProperty.NotNull, 0),
                new RefColumn("FILE_ID", "IE_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_IMPORT_EXPORT");
        }
    }
}