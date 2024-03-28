namespace Bars.Gkh.Migrations.Version_2014012300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014012200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_ROOM",
                new Column("CAREA", DbType.Decimal, ColumnProperty.NotNull),
                new Column("LAREA", DbType.Decimal, ColumnProperty.NotNull),
                new Column("CDESCRIPTION", DbType.String, 1000, ColumnProperty.Null),
                new Column("CROOM_NUM", DbType.String, 10, ColumnProperty.Null),
                new Column("ROOMS_COUNT", DbType.Int16, ColumnProperty.Null),
                new Column("ENTRANCE_NUM", DbType.Int16, ColumnProperty.Null),
                new Column("TYPE", DbType.Int16, ColumnProperty.Null),
                new Column("OWNERSHIP_TYPE", DbType.Int16, ColumnProperty.Null),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "ROOM_RO", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_ROOM");
        }
    }
}