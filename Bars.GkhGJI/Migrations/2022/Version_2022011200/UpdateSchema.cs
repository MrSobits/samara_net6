namespace Bars.GkhGji.Migrations._2022.Version_2022011200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022011200")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2021.Version_2021122900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {          
            //GJI_INSPECTION_VIOL_STAGE
            Database.AddColumn("GJI_INSPECTION_VIOL_STAGE", new Column("DATE_NOTIFICATION", DbType.DateTime, ColumnProperty.None));

        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_INSPECTION_VIOL_STAGE", "DATE_NOTIFICATION");
        }
    }
}