namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021031100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [MigrationDependsOn(typeof(_2020.Version_2020081400.UpdateSchema))]
    [Migration("2021031100")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_OBJ_INTERCOM",
                new Column("INTERCOM_COUNT", DbType.Int32, ColumnProperty.NotNull),
                new RefColumn("REALITY_OBJECT_ID", ColumnProperty.NotNull, "FK_GKH_OBJ_INTERCOM_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("INTERCOM_TYPE", DbType.Int32, ColumnProperty.NotNull),
                new Column("RECORDING", DbType.Int32),
                new Column("ARCHIVE_DEPTH", DbType.Int32),
                new Column("ARCHIVE_ACCESS", DbType.Int32),
                new Column("TARIFF", DbType.Decimal)
            );
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_OBJ_INTERCOM");
        }
    }
}
