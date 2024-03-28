namespace Bars.GkhGji.Migrations.Version_2014043000
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014043000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2014042800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_BOILER_ROOM",
                new RefColumn("ADDRESS_ID", ColumnProperty.NotNull, "BOILER_ADDRESS", "B4_FIAS_ADDRESS", "ID"));

            Database.AddEntityTable("GJI_BOILER_UNACTIVE",
                new RefColumn("BOILER_ID", ColumnProperty.NotNull, "UNACTIVE_BOILER", "GJI_BOILER_ROOM", "ID"),
                new Column("START_TIME", DbType.DateTime, ColumnProperty.Null),
                new Column("END_TIME", DbType.DateTime, ColumnProperty.Null));

            Database.AddEntityTable("GJI_BOILER_HEATING",
                new RefColumn("BOILER_ID", ColumnProperty.NotNull, "HEATING_BOILER", "GJI_BOILER_ROOM", "ID"),
                new Column("START_TIME", DbType.DateTime, ColumnProperty.NotNull),
                new Column("END_TIME", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_BOILER_UNACTIVE");
            Database.RemoveTable("GJI_BOILER_HEATING");
            Database.RemoveTable("GJI_BOILER_ROOM");
        }
    }
}