namespace Bars.GkhGji.Migrations._2023.Version_2023050500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023050500")]
    [MigrationDependsOn(typeof(_2023.Version_2023042500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
  

            Database.AddColumn("GJI_EMAIL", new Column("DECLINE_REASON", DbType.String, 1500));
            Database.AddColumn("GJI_EMAIL", new Column("DECLINE_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0));

        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_EMAIL", "DECLINE_TYPE");
            this.Database.RemoveColumn("GJI_EMAIL", "DECLINE_REASON");
        }
    }
}