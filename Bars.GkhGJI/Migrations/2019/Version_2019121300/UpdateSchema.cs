namespace Bars.GkhGji.Migrations._2019.Version_2019121300
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121300")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019121100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("ANNEX_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0));
            Database.AddColumn("GJI_PRESCRIPTION_ANNEX", new Column("NUMBER", DbType.String));
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "NUMBER");
            Database.RemoveColumn("GJI_PRESCRIPTION_ANNEX", "ANNEX_TYPE");
        }
    }
}