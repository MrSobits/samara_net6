namespace Bars.GkhGji.Migrations._2019.Version_2019121600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121600")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019121300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESCRIPTION", new Column("RENEWAL_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_PRESCRIPTION", new Column("RENEWAL_NUMBER", DbType.String));
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_PRESCRIPTION", "RENEWAL_NUMBER");
            Database.RemoveColumn("GJI_PRESCRIPTION", "RENEWAL_DATE");
        }
    }
}