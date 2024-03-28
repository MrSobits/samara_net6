namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015121100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015100700.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddTable(
                "GJI_TOMSK_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DATE_OF_VIOLATION", DbType.DateTime),
                new Column("HOUR_OF_VIOLATION", DbType.String, 100),
                new Column("MINUTE_OF_VIOLATION", DbType.String, 1000));

            Database.ExecuteNonQuery(@"insert into GJI_TOMSK_PROTOCOL (id) select id from GJI_PROTOCOL");
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_PROTOCOL");
        }
    }
}