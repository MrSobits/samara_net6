namespace Bars.GkhGji.Migrations._2020.Version_2020050600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020050600")]
    [MigrationDependsOn(typeof(Version_2020042800.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {         
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("TYPE_ANNEX", DbType.Int32, ColumnProperty.NotNull, 0));          
        }

        public override void Down()
        {           
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "TYPE_ANNEX");     
        }
    }
}