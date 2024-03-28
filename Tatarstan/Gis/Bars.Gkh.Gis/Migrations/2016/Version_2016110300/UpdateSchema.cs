namespace Bars.Gkh.Gis.Migrations._2016.Version_2016110300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016110300")]
    [MigrationDependsOn(typeof(Version_2016092200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "BIL_CONNECTION",
                new Column("CONNECTION", DbType.String),
                new Column("APP_URL", DbType.String),
                new Column("CONNECTION_TYPE", DbType.Int16));
        }

        public override void Down()
        {
            this.Database.RemoveTable("BIL_CONNECTION");
        }
    }
}
