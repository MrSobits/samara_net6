namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2023080700
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023080700")]
    [MigrationDependsOn(typeof(Version_2023040400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
            "GJI_CH_APPCIT_ANSWER_LTEXT",
            new RefColumn("APPCIT_ANSWER_ID", ColumnProperty.NotNull, "GJI_CH_APPCIT_ANSWER_LT_ANSWER", "GJI_APPCIT_ANSWER", "ID"),
            new Column("DESCRIPTION", DbType.Binary));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_APPCIT_ANSWER_LTEXT");
        }
    }
}