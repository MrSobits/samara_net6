namespace Bars.Gkh.MigrationsVersion_2014052400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014052400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.MigrationsVersion_2014052301.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_MAN_ORG_REGISTRY",
                new Column("EGRUL_DATE", DbType.DateTime, ColumnProperty.Null),
                new Column("INFO_DATE", DbType.DateTime, ColumnProperty.NotNull),
                new Column("INFO_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("REG_NUM", DbType.String, 100, ColumnProperty.NotNull),
                new RefColumn("FILE_ID", "GKH_MAN_ORG_REG_FILE", "B4_FILE_INFO", "ID"),
                new RefColumn("MAN_ORG_ID", ColumnProperty.NotNull, "GKH_MAN_ORG_REG_MAN_ORG", "GKH_MANAGING_ORGANIZATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_MAN_ORG_REGISTRY");
        }
    }
}