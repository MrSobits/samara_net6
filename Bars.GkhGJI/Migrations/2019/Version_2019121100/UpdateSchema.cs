namespace Bars.GkhGji.Migrations._2019.Version_2019121100
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121100")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2019.Version_2019120900.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_DISPOSAL", new Column("KIND_KND", DbType.Int32, 4, ColumnProperty.NotNull, 0));
        }

        public override void Down()
        {          
            Database.RemoveColumn("GJI_DISPOSAL", "KIND_KND");
        }
    }
}