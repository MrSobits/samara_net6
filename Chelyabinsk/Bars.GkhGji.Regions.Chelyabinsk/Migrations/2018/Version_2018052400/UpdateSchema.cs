namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2018052400
{
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018052400")]
    [MigrationDependsOn(typeof(Version_2018052300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("GJI_CH_GIS_GMP", new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.ChangeColumn("GJI_CH_GIS_GMP", new Column("MESSAGEID", DbType.String, 36, ColumnProperty.Unique));
        }
    }
}