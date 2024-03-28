namespace Bars.GkhGji.Migrations._2024.Version_2024020100
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2024020100")]
    [MigrationDependsOn(typeof(Version_2024012400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("SENDED_TO_EDM", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "SENDED_TO_EDM");

        }
    }
}