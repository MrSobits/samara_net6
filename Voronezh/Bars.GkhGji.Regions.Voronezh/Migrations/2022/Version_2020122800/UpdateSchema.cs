namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2020122800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020122800")]
    [MigrationDependsOn(typeof(Version_2020122600.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GJI_CH_SMEV_STAYING_PLACE", "DOC_TYPE");
            Database.AddColumn("GJI_CH_SMEV_STAYING_PLACE", new Column("DOC_TYPE", DbType.Int32, ColumnProperty.Null));
        }

        public override void Down()
        {

        }

    }
}
