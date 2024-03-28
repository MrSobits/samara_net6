namespace Bars.GkhDi.Migrations.Version_2015082500
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015082500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015072901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("DI_DISINFO_LICENSE",
                new RefColumn("DISINFO_ID", ColumnProperty.NotNull, "DI_DISINFO_LICENSE", "DI_DISINFO", "ID"),
                new RefColumn("FILE_ID", "DI_DI_LIC_FILE", "B4_FILE_INFO", "ID"),
                new Column("LICENSE_NUMBER", DbType.String, 200, ColumnProperty.NotNull, "''"),
                new Column("DATE_RECEIVED", DbType.DateTime, ColumnProperty.NotNull),
                new Column("LICENSE_ORG", DbType.String, 300 ,ColumnProperty.NotNull));

            Database.AddColumn("DI_DISINFO", "HAS_LICENSE", DbType.Int16, ColumnProperty.NotNull, 30);
        }

        public override void Down()
        {
            Database.RemoveTable("DI_DISINFO_LICENSE");

            Database.RemoveColumn("DI_DISINFO", "HAS_LICENSE");
        }
    }
}
