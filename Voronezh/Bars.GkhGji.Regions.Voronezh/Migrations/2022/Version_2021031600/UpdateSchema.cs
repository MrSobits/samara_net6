namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021031600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021031600")]
    [MigrationDependsOn(typeof(Version_2021031100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddColumn("GJI_CH_SMEV_STAYING_PLACE", new Column("DOC_COUNTRY", DbType.String, 3));
            Database.AddColumn("GJI_CH_SMEV_LIVING_PLACE", new Column("DOC_COUNTRY", DbType.String, 3));
            Database.RemoveColumn("GJI_CH_SMEV_STAYING_PLACE", "DOC_TYPE");
            Database.RemoveColumn("GJI_CH_SMEV_LIVING_PLACE", "DOC_TYPE");
            Database.AddColumn("GJI_CH_SMEV_STAYING_PLACE", new Column("DOC_TYPE", DbType.Int32, ColumnProperty.Null));
            Database.AddColumn("GJI_CH_SMEV_LIVING_PLACE", new Column("DOC_TYPE", DbType.Int32, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_STAYING_PLACE", "DOC_COUNTRY");
            Database.RemoveColumn("GJI_CH_SMEV_LIVING_PLACE", "DOC_COUNTRY");
        }


    }
}
