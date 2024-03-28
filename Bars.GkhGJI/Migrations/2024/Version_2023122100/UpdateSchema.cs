namespace Bars.GkhGji.Migrations._2024.Version_2024012400
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Enums;
    using System.Data;

    [Migration("2024012400")]
    [MigrationDependsOn(typeof(_2023.Version_2023122210.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DICISION", new Column("TYPE_RISK", DbType.Int16, ColumnProperty.NotNull, 0));

        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICISION", "TYPE_RISK");
           

        }
    }
}