namespace Bars.GkhGji.Migrations._2024.Version_2024022000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024022000")]
    [MigrationDependsOn(typeof(Version_2024021400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_EMAIL", new Column("ANSWER_AT", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_EMAIL", "ANSWER_AT");
        }
    }
}