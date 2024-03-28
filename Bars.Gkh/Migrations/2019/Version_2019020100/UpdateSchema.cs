using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Migrations._2019.Version_2019020100
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019020100")]
    [MigrationDependsOn(typeof(_2018.Version_2018120700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_REALITY_ANTENNA",
                new Column("AVAILABILITY", DbType.Int32, ColumnProperty.NotNull),
                new Column("WORKABILITY", DbType.Int32),
                new Column("FREQUENCY_FROM", DbType.Decimal),
                new Column("FREQUENCY_TO", DbType.Decimal),
                new Column("RANGE", DbType.Int32),
                new Column("NUMBER_APARTMENTS", DbType.Int32),
                new Column("REASON", DbType.Int32),
                new RefColumn("RO_ID", "FK_REAL_OBJECT", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("FILE_ID", "FK_FILEINFO", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_REALITY_ANTENNA");
        }
    }
}