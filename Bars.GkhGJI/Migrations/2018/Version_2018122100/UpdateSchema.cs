namespace Bars.GkhGji.Migrations._2018.Version_2018122100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018122100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018120400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
       
            this.Database.AddColumn("GJI_RESOLPROS", new Column("UIN", DbType.String, ColumnProperty.None));
           
        }

        public override void Down()
        {
        
            this.Database.RemoveColumn("GJI_RESOLPROS", "UIN");
        }

    }
}