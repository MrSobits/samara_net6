namespace Bars.Gkh.Migrations._2022.Version_2022053000
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022053000")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022032400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_REALITY_VIDECAM",
                new Column("INSTALLPLACE", DbType.String, 1500),
                new Column("WORKABILITY", DbType.Int32, ColumnProperty.NotNull,30),
                new Column("DESCRIPTION", DbType.String, 1500),
                new Column("UNIQ_NUMBER", DbType.String, 250),
                new Column("VIDECAM_URL", DbType.String, 350),
                new RefColumn("RO_ID", "GKH_REALITY_VIDECAM_REAL_OBJECT", "GKH_REALITY_OBJECT", "ID"));

            this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("HAS_VIDECAM", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "HAS_VIDECAM");
            this.Database.RemoveTable("GKH_REALITY_VIDECAM");
        }
    }
}