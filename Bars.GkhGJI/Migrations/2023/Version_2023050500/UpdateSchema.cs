namespace Bars.GkhGji.Migrations._2023.Version_2023051500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023051500")]
    [MigrationDependsOn(typeof(_2023.Version_2023050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        { 
            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("ANSWER_FINAL_TYPE", DbType.Int32, 2, ColumnProperty.NotNull, 2));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPCIT_ANSWER", "ANSWER_FINAL_TYPE");
           
        }
    }
}